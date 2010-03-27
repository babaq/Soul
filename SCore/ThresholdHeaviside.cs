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
    public class ThresholdHeaviside : IHillock
    {
        private double threshold;
        private INeuron hostneuron;


        public ThresholdHeaviside(INeuron hostneuron, double threshold)
        {
            this.threshold = threshold;
            this.hostneuron = hostneuron;
        }


        #region IHillock Members

        public INeuron HostNeuron
        {
            get { return hostneuron; }
            set { hostneuron = value; }
        }

        public double Threshold
        {
            get{return threshold;}
            set{threshold = value;}
        }

        public virtual double Fire(double hillockpotential, double currentT)
        {
            return CoreFunc.Heaviside(hillockpotential - threshold);
        }

        public virtual double ResetPotential
        {
            get { return -60.0; }
            set {}
        }

        public virtual double RefractoryPeriod
        {
            get { return 1.0; }
            set {}
        }

        public virtual bool IsInRefractoryPeriod(double currentT)
        {
            return false;
        }

        public virtual Queue<double > TravalingSpikeTrain
        {
            get { return null; }
        }

        public virtual void UpdateTravalingSpikeTrain(double currentT)
        {
        }

        public void FireSpike()
        {
            if (Spike != null)
            {
                Spike(HostNeuron, EventArgs.Empty);
            }
        }

        public event EventHandler Spike;

        #endregion
    }
}
