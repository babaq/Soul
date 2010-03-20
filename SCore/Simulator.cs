//--------------------------------------------------------------------------------
// This file is part of The Soul, A Neural Network Simulation System.
//
// Copyright © 2010 LBE Group. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSolver;

namespace SCore
{
    public class Simulator : ISimulator
    {
        private double t0;
        private double deltaT;
        private double durationT;
        private double currentT;
        private bool isrunning;
        private bool isrunover;
        private INetwork network;
        private ISolver solver;
        private RecordType recordtype;
        private string recordfile;
        private FileStream potentialfile;
        private FileStream spikefile;
        private StreamWriter potentialwriter;
        private StreamWriter spikewriter;

        public Simulator(double deltatime, double durationtime,INetwork targetnetwork,ISolver solver,RecordType recordtype,string recordfile)
        {
            deltaT = deltatime;
            durationT = durationtime;
            currentT = 0.0;
            t0 = currentT;
            isrunning = false;
            isrunover = false;
            network = targetnetwork;
            this.solver = solver;
            this.recordtype = recordtype;
            this.recordfile = recordfile;
        }


        #region ISimulator Members

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
        }

        public ISolver Solver
        {
            get { return solver; }
            set { solver = value; }
        }

        public double DeltaT
        {
            get
            {
                return deltaT;
            }
            set
            {
                deltaT = value;
            }
        }

        public double DurationT
        {
            get
            {
                return durationT;
            }
            set
            {
                durationT = value;
            }
        }

        public double CurrentT
        {
            get { return currentT; }
        }

        public void Run()
        {
            if (network != null && solver != null)
            {
                isrunning = true;
                BeginRecord();
                do
                {
                    Step();
                } while (currentT -t0<= durationT);
                EndRecord();
                t0 = currentT;
                isrunning = false;
                isrunover = true;
            }
        }

        public virtual void BeginRecord()
        {
            if (recordtype != RecordType.None)
            {
                var file = "";
                if(string.IsNullOrEmpty(recordfile))
                {
                    file = DateTime.Now.ToShortTimeString();
                }
                else
                {
                    file = recordfile + "_" + DateTime.Now.ToShortTimeString();
                }
                switch (recordtype)
                {
                    case RecordType.Potential:
                        potentialfile = new FileStream(file+"_v.csv", FileMode.Append, FileAccess.Write);
                        potentialwriter = new StreamWriter(potentialfile,Encoding.ASCII);
                        RegisterUpdated(new EventHandler(RecordPotential));
                        RecordStep(potentialwriter, recordtype,currentT);
                        break;
                    case RecordType.Spike:
                        spikefile = new FileStream(file+"_s.csv",FileMode.Append,FileAccess.Write);
                        spikewriter = new StreamWriter(spikefile,Encoding.ASCII);
                        RegisterSpike(new EventHandler(RecordSpike));
                        break;
                    default:
                        potentialfile = new FileStream(file + "_v.csv", FileMode.Append, FileAccess.Write);
                        spikefile = new FileStream(file + "_s.csv", FileMode.Append, FileAccess.Write);
                        potentialwriter = new StreamWriter(potentialfile, Encoding.ASCII);
                        spikewriter = new StreamWriter(spikefile, Encoding.ASCII);
                        RegisterUpdated(new EventHandler(RecordPotential));
                        RegisterSpike(new EventHandler(RecordSpike));
                        RecordStep(potentialwriter, recordtype,currentT);
                        break;
                }

            }
        }

        public void RecordStep(StreamWriter potentialwriter,RecordType recordtype,double currentT)
        {
            network.RecordStep(potentialwriter,recordtype,currentT);
        }

        public void RegisterSpike(EventHandler recordspike)
        {
            network.RegisterSpike(recordspike);
        }

        public void RegisterUpdated(EventHandler recordpotential)
        {
            network.RegisterUpdated(recordpotential);
        }

        public void UnRegisterSpike(EventHandler recordspike)
        {
            network.UnRegisterSpike(recordspike);
        }

        public void UnRegisterUpdated(EventHandler recordpotential)
        {
            network.UnRegisterUpdated(recordpotential);
        }

        public virtual void RecordPotential(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            potentialwriter.WriteLine(currentT.ToString("F3")+","+neuron.Output.ToString("F3")+","+neuron.ID.ToString("N"));
        }

        public virtual void RecordSpike(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            spikewriter.WriteLine(currentT.ToString("F3") + ","+ neuron.ID.ToString("N"));
        }

        public virtual void EndRecord()
        {
            if(recordtype!=RecordType.None)
            {
                if(spikewriter!=null)
                {
                    spikewriter.Close();
                    spikefile.Close();
                    spikewriter.Dispose();
                    spikefile.Dispose();
                    UnRegisterSpike(new EventHandler(RecordSpike));
                }
                if(potentialwriter!=null)
                {
                    potentialwriter.Close();
                    potentialfile.Close();
                    potentialwriter.Dispose();
                    potentialfile.Dispose();
                    UnRegisterUpdated(new EventHandler(RecordPotential));
                }
            }
        }

        public void Step()
        {
            Step(deltaT);
        }

        public void Step(double delta)
        {
            network.Update(delta,currentT, solver);
            network.Tick();
            currentT += delta;
        }

        public bool IsRunning
        {
            get { return isrunning; }
        }

        public bool IsRunOver
        {
            get { return isrunover; }
        }

        public RecordType RecordType
        {
            get { return recordtype; }
            set { recordtype = value; }
        }

        public string RecordFile
        {
            get { return recordfile; }
            set { recordfile = value; }
        }

        #endregion
    }
}
