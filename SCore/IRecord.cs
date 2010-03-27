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

namespace SCore
{
    public interface IRecord
    {
        ISimulation HostSimulator { get; set; }
        RecordType RecordType { get; set; }
        string RecordFile { get; set; }
        void RecordBegin();
        void RecordEnd();
        void RecordOnUpdated(object sender, EventArgs e);
        void RecordOnSpike(object sender, EventArgs e);
        void Save(INetwork network,string file);
        INetwork Open(string file);
    }

    public enum RecordType
    {
        None,
        All,
        Potential,
        Spike
    }

}
