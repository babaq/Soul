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
    public class SpikeWeightSynapse : WeightSynapse
    {
        private double axondelay;


        public SpikeWeightSynapse(INeuron presynapticneuron, double weight) : this(presynapticneuron, weight,0.0)
        {
        }

        public SpikeWeightSynapse(INeuron presynapticneuron, double weight, double axondelay) : base(presynapticneuron, weight)
        {
            this.axondelay = axondelay;
        }


        public override double AxonDelay
        {
            get{return axondelay;}
            set{axondelay = value;}
        }

        public override double Release(double deltaT, double currentT)
        {
            var dirac = 0.0;
            if (PreSynapticNeuron.Hillock.TravelingSpikeTrain.Count > 0)
            {
                for (int i = 0; i < PreSynapticNeuron.Hillock.TravelingSpikeTrain.Count; i++)
                {
                    dirac += CoreFunc.rDiracDelta(
                        currentT - PreSynapticNeuron.Hillock.TravelingSpikeTrain.ElementAt(i) -
                        axondelay, deltaT);
                }
            }
            return dirac*Weight;
        }

    }
}
