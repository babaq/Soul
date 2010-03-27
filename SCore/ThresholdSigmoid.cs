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
using SSolver;

namespace SCore
{
    [Serializable]
    public class ThresholdSigmoid : ThresholdHeaviside
    {
        public ThresholdSigmoid(INeuron hostneuron, double threshold)
            : base(hostneuron, threshold)
        {
        }


        public override double Fire(double hillockpotential, double currentT)
        {
            return CoreFunc.Sigmoid(hillockpotential - Threshold);
        }

    }
}
