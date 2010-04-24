﻿//--------------------------------------------------------------------------------
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
using System.Windows.Media.Media3D;

namespace SCore
{
    public interface ISynapse : ICloneable
    {
        Guid ID { get; }
        double Weight { set; get; }
        INeuron PreSynapticNeuron { get; set; }
        double AxonDelay { set; get; }
        double Release(double deltaT, double currentT);
        SynapseType Type { get; }
    }

    public enum SynapseType
    {
        WeightSynapse,
        SpikeWeightSynapse
    }

}
