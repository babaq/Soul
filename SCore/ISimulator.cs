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
    public interface ISimulator
    {
        INetwork Network { get; set; }
        ISolver Solver { get; set; }
        double DeltaT { set; get; }
        double DurationT { set; get; }
        double CurrentT { get; }
        void Run();
        void Step(double delta);
        bool IsRunning { get; }
        bool IsRunOver { get; }
        RecordType RecordType { get; set; }
        string RecordFile { get; set; }
        void BeginRecord();
        void EndRecord();
        void RecordPotential(object sender, EventArgs e);
        void RecordSpike(object sender, EventArgs e);
        void RegisterUpdated(EventHandler recordpotential);
        void RegisterSpike(EventHandler recordspike);
        void UnRegisterUpdated(EventHandler recordpotential);
        void UnRegisterSpike(EventHandler recordspike);
        void RecordStep(StreamWriter potentialwriter, RecordType recordtype,double currentT);
    }

    public enum RecordType
    {
        None,
        All,
        Potential,
        Spike
    }

}
