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
        private Queue<double> travelingspiketrain;
        private bool isspiked;


        public ThresholdSpike(INeuron hostneuron, double threshold, double resetpotential, double refractoryperiod)
            : base(hostneuron, threshold)
        {
            this.resetpotential = resetpotential;
            this.refractoryperiod = refractoryperiod;
            this.travelingspiketrain = new Queue<double>();
            type = HillockType.Spike;
            isspiked = false;
        }


        public override double Fire(double hillockpotential, double currentT)
        {
            if (CoreFunc.Heaviside(hillockpotential-Threshold)==0.0)
            {
                return hillockpotential;
            }
            else
            {
                isspiked = true;
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

        public override Queue<double> TravelingSpikeTrain
        {
            get { return travelingspiketrain; }
        }

        public override bool IsInRefractoryPeriod(double currentT)
        {
            if (travelingspiketrain.Count == 0)
            {
                return false;
            }
            if (currentT - travelingspiketrain.Last() > refractoryperiod)
            {
                return false;
            }
            return true;
        }

        public override void Tick(double currentT)
        {
            if (isspiked)
            {
                travelingspiketrain.Enqueue(currentT);
                isspiked = false;
            }

            if (travelingspiketrain.Count > 0)
            {
                if (currentT - travelingspiketrain.Peek() > GlobleSettings.AxonDelayMax)
                {
                    travelingspiketrain.Dequeue();
                }
            }
        }

    }
}
