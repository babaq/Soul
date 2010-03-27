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
using System.Windows.Media.Media3D;

namespace SCore
{
    [Serializable]
    public class WeightSynapse : ISynapse
    {
        private Guid id;
        private double weight;
        private INeuron presynapticneuron;


        public WeightSynapse(INeuron presynapticneuron, double weight)
        {
            this.id = Guid.NewGuid();
            this.presynapticneuron = presynapticneuron;
            this.weight = weight;
        }


        #region ISynapse Members

        public Guid ID
        {
            get { return id; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public INeuron PreSynapticNeuron
        {
            get { return presynapticneuron; }
        }

        public Point3D Position
        {
            get { return new Point3D(); }
            set { }
        }

        public virtual double AxonDelay
        {
            get { return 0.0; }
            set { }
        }

        public virtual double Release(double deltaT, double currentT)
        {
            return presynapticneuron.LastOutput * weight;
        }

        #endregion
    }
}
