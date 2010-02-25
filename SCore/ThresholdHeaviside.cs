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
using SSolver;

namespace SCore
{
    public class ThresholdHeaviside : IHilllock
    {
        private double threshold;

        public ThresholdHeaviside(double threshold)
        {
            this.threshold = threshold;
        }


        #region IHilllock Members

        public double Threshold
        {
            get
            {
                return threshold;
            }
            set
            {
                threshold = value;
            }
        }

        public virtual double Fire(double hilllockpotential)
        {
            return CoreFunc.Heaviside(hilllockpotential - threshold);
        }

        public virtual double ResetPotential
        {
            get { return -65.0; }
            set {}
        }

        public virtual double RefractoryPeriod
        {
            get { return 1.5; }
            set {}
        }

        #endregion
    }
}
