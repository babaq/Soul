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
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Text;
using SSolver;

namespace SCore
{
    public class Simulator : ISimulation
    {
        private INetwork network;
        private ISolver solver;
        private IRecord recorder;
        private double deltaT;
        private double durationT;
        private double currentT;
        private double t0;
        private Thread runthread;
        private EventWaitHandle hpause;
        private EventWaitHandle hstop;
        private bool isrunning;
        private bool ispaused;


        public Simulator(double deltatime, double durationtime,INetwork targetnetwork,ISolver solver,IRecord recorder)
        {
            deltaT = deltatime;
            durationT = durationtime;
            currentT = 0.0;
            t0 = currentT;
            network = targetnetwork;
            this.solver = solver;
            this.recorder = recorder;
            this.recorder.HostSimulator = this;
            runthread = null;
            hpause = new ManualResetEvent(true);
            hstop = new ManualResetEvent(false);
            isrunning = false;
            ispaused = false;
        }


        #region ISimulation Members

        public INetwork Network
        {
            get { return network; }
            set
            {
                if (!IsRunning)
                {
                    network = value;
                }
            }
        }

        public ISolver Solver
        {
            get { return solver; }
            set
            {
                if (!IsRunning)
                {
                    solver = value;
                }
            }
        }

        public IRecord Recorder
        {
            get { return recorder; }
            set
            {
                if (!IsRunning)
                {
                    recorder = value;
                }
            }
        }

        public double DeltaT
        {
            get{return deltaT;}
            set{deltaT = value;}
        }

        public double DurationT
        {
            get{return durationT;}
            set{durationT = value;}
        }

        public double CurrentT
        {
            get { return currentT; }
            set
            {
                if (!IsRunning)
                {
                    currentT = value;
                    t0 = currentT;
                }
            }
        }

        public double Progress
        {
            get { return (currentT - t0)/durationT; }
        }

        public void Run()
        {
            if (!IsRunning)
            {
                if (network != null && solver != null && recorder != null)
                {
                    runthread = new Thread(run) {Name = "Run"};
                    runthread.Start();
                    isrunning = true;
                }
            }
        }

        private void run()
        {
            try
            {
                recorder.RecordBegin();
                hstop.Reset();
                t0 = currentT;
                do
                {
                    hpause.WaitOne(Timeout.Infinite);
                    if (hstop.WaitOne(0))
                    {
                        break;
                    }
                    Step(deltaT);
                } while (currentT - t0 < durationT);
            }
            catch (Exception e)
            {
            }
            finally
            {
                recorder.RecordEnd();
                isrunning = false;
                RaiseRunOver();
            }
        }

        public void Stop()
        {
            if(IsRunning)
            {
                hstop.Set();
                Resume();
            }
        }

        public void Pause()
        {
            if(IsRunning&&!IsPaused)
            {
                hpause.Reset();
                ispaused = true;
            }
        }

        public void Resume()
        {
            if (IsRunning&&IsPaused)
            {
                hpause.Set();
                ispaused = false;
            }
        }

        public void Step(double deltatime)
        {
            currentT += deltatime;
            network.Update(deltatime,currentT, solver);
            network.Tick(currentT);
            RaiseSteped();
        }

        public bool IsRunning
        {
            get{return isrunning;}
        }

        public bool IsPaused
        {
            get { return ispaused; }
        }

        public event EventHandler RunOver;

        public event EventHandler Steped;

        public void RaiseRunOver()
        {
            if (RunOver != null)
            {
                RunOver(this, EventArgs.Empty);
            }
        }

        public void RaiseSteped()
        {
            if(Steped!=null)
            {
                Steped(this, EventArgs.Empty);
            }
        }

        public string Summary
        {
            get
            {
                var s = new StringBuilder();
                s.AppendLine("############################################################");
                s.AppendLine("# The Soul Neural Network Simulation System. SCore Version: "+Assembly.GetExecutingAssembly().GetName().Version);
                s.AppendLine("# ");
                s.Append(network.Summary);
                s.AppendLine("# ");
                s.Append(solver.Summary);
                s.AppendLine("# ");
                s.AppendLine("# Simulation Summary.");
                s.AppendLine("# DeltaT=" + deltaT);
                s.AppendLine("# DurationT=" + durationT);
                s.AppendLine("############################################################");
                return s.ToString();
            }
        }

        public void NotifyPropertyChanged(string propertyname)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyname));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
