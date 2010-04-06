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
using System.Linq;
using System.Text;

namespace SCore
{
    public interface IHillock
    {
        INeuron HostNeuron { get; set; }
        double Threshold { set; get; }
        double Fire(double hillockpotential, double currentT);
        double ResetPotential { set; get; }
        double RefractoryPeriod { set; get; }
        bool IsInRefractoryPeriod(double currentT);
        Queue<double > TravelingSpikeTrain{ get;}
        void UpdateTravelingSpikeTrain(double currentT);
        event EventHandler Spike;
        void FireSpike();
    }
}
