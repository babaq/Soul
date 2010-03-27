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
    public interface ISimulation
    {
        INetwork Network { get; set; }
        ISolver Solver { get; set; }
        IRecord Recorder { get; set; }
        double DeltaT { set; get; }
        double DurationT { set; get; }
        double CurrentT { get; set; }
        double Progress { get; }
        void Run();
        void Stop();
        void Pause();
        void Resume();
        void Step(double deltatime);
        bool IsRunning { get; }
        bool IsPaused { get; }
        string Summary { get; }
    }

}
