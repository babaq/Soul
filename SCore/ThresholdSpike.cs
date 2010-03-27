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
    public class ThresholdSpike : ThresholdSigmoid
    {
        private double resetpotential;
        private double refractoryperiod;
        private Queue<double> travalingspiketrain;


        public ThresholdSpike(INeuron hostneuron, double threshold, double resetpotential, double refractoryperiod)
            : base(hostneuron, threshold)
        {
            this.resetpotential = resetpotential;
            this.refractoryperiod = refractoryperiod;
            this.travalingspiketrain = new Queue<double>();
        }


        public override double Fire(double hillockpotential, double currentT)
        {
            if (CoreFunc.Heaviside(hillockpotential-Threshold)==0.0)
            {
                return hillockpotential;
            }
            else
            {
                travalingspiketrain.Enqueue(currentT);
                FireSpike();
                return resetpotential;
            }
        }

        public override double ResetPotential
        {
            get{return resetpotential;}
            set{resetpotential = value;}
        }

        public override double RefractoryPeriod
        {
            get{return refractoryperiod;}
            set{refractoryperiod = value;}
        }

        public override Queue<double> TravalingSpikeTrain
        {
            get { return travalingspiketrain; }
        }

        public override bool IsInRefractoryPeriod(double currentT)
        {
            if (travalingspiketrain.Count == 0)
            {
                return false;
            }
            if (currentT - travalingspiketrain.Last() > refractoryperiod)
            {
                return false;
            }
            return true;
        }

        public override void UpdateTravalingSpikeTrain(double currentT)
        {
            if(currentT-travalingspiketrain.Peek()>SConstants.AxonDelayMax)
            {
                travalingspiketrain.Dequeue();
            }
        }

    }
}
