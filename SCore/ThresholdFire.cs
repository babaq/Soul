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
    public class ThresholdFire : ThresholdSigmoid
    {
        private double resetpotential;
        private double refractoryperiod;


        public ThresholdFire(double threshold, double resetpotential, double refractoryperiod)
            : base(threshold)
        {
            this.resetpotential = resetpotential;
            this.refractoryperiod = refractoryperiod;
        }


        public override double Fire(double hilllockpotential)
        {
        }

        public override double ResetPotential
        {
            get
            {
                return resetpotential;
            }
            set
            {
                resetpotential = value;
            }
        }

        public override double RefractoryPeriod
        {
            get
            {
                return refractoryperiod;
            }
            set
            {
                refractoryperiod = value;
            }
        }

    }
}
