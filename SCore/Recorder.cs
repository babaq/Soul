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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace SCore
{
    public class Recorder:IRecord
    {
        private ISimulation hostsimulator;
        private RecordType recordtype;
        private string recordfile;
        private FileStream potentialfile;
        private FileStream spikefile;
        private StreamWriter potentialwriter;
        private StreamWriter spikewriter;


        public Recorder(ISimulation hostsimulator, RecordType recordtype,string recordfile)
        {
            this.hostsimulator = hostsimulator;
            this.recordtype = recordtype;
            this.recordfile = recordfile;
            potentialfile = spikefile  = null;
            potentialwriter = spikewriter = null;
        }


        #region IRecord Members

        public ISimulation HostSimulator
        {
            get { return hostsimulator; }
            set { hostsimulator = value; }
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

        public virtual void RecordBegin()
        {
            if (recordtype != RecordType.None)
            {
                var file=recordfile + "_" + DateTime.Now.ToString("yyyy-MM-d_HH-mm-ss");
                if(recordtype==RecordType.Potential || recordtype==RecordType.All)
                {
                    potentialfile = new FileStream(file + "_v.txt", FileMode.Append, FileAccess.Write);
                    potentialwriter = new StreamWriter(potentialfile, Encoding.ASCII);
                    hostsimulator.Network.RegisterUpdated(RecordOnUpdated);
                    potentialwriter.WriteLine(hostsimulator.Summary);
                    hostsimulator.Network.RaiseUpdated();
                }
                if (recordtype == RecordType.Spike || recordtype == RecordType.All)
                {
                    spikefile = new FileStream(file + "_s.txt", FileMode.Append, FileAccess.Write);
                    spikewriter = new StreamWriter(spikefile, Encoding.ASCII);
                    hostsimulator.Network.RegisterSpike(RecordOnSpike);
                    spikewriter.WriteLine(hostsimulator.Summary);
                }
            }
        }

        public virtual void RecordEnd()
        {
            if (spikewriter != null)
            {
                spikewriter.Close();
                spikefile.Close();
                spikewriter.Dispose();
                spikewriter = null;
                spikefile.Dispose();
                spikefile = null;
                hostsimulator.Network.UnRegisterSpike(RecordOnSpike);
            }
            if (potentialwriter != null)
            {
                potentialwriter.Close();
                potentialfile.Close();
                potentialwriter.Dispose();
                potentialwriter = null;
                potentialfile.Dispose();
                potentialfile = null;
                hostsimulator.Network.UnRegisterUpdated(RecordOnUpdated);
            }
        }

        public virtual void RecordOnUpdated(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            potentialwriter.WriteLine(hostsimulator.CurrentT.ToString("F3") + "\t"+ neuron.Output.ToString("F3") + "\t" + neuron.ID.ToString("N"));
        }

        public virtual void RecordOnSpike(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            spikewriter.WriteLine(hostsimulator.CurrentT.ToString("F3") + "\t" + neuron.ID.ToString("N"));
        }

        public void Save(INetwork network, string file)
        {
            var ext = file.Substring(file.LastIndexOf(".")+1);
            switch (ext)
            {
                case "network":
                    using (var networkwriter = new FileStream(file, FileMode.Create, FileAccess.Write))
                    {
                        var bf = new BinaryFormatter();
                        bf.Serialize(networkwriter, network);
                    }
                    break;
                case "xml":
                    break;
            }
        }

        public INetwork Open(string file)
        {
            var ext = file.Substring(file.LastIndexOf(".") + 1);
            INetwork network=null;
            switch (ext)
            {
                case "network":
                    using (var networkreader = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        var bf = new BinaryFormatter();
                        network = bf.Deserialize(networkreader) as INetwork;
                    }
                    break;
                case "xml":
                    break;
            }
            return network;
        }

        #endregion

    }

}
