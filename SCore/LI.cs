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
    /// <summary>
    /// Leaky Integrator Model
    /// </summary>
    public class LI : MP
    {
        private double tao;


        public LI(double threshold, double initoutput, double tao)
            : base(new Point3D(0.0, 0.0, 0.0), new ThresholdSigmoid(threshold), initoutput)
        {
            this.tao = tao;
        }

        public LI(Point3D position, IHilllock hilllock, double initoutput, double tao)
            : base(position, hilllock, initoutput)
        {
            this.tao = tao;
        }


        public override double Tao
        {
            get
            {
                return tao;
            }
            set
            {
                tao = value;
            }
        }

        public override void Update(double deltaT)
        {
            var output = 0.0;
            for (int i = 0; i < Synapses.Count; i++)
            {
                output += Synapses[i].PreSynapticNeuron.LastOutput * Synapses[i].Weight;
            }
            
            Solve.Deriv deriv = new Solve.Deriv(CoreFunc.LIDeriv_Tao_Sigma);
            
            var param = new double[] {tao, output};
            Solve.RK4By_Del(deltaT, LastOutput, param,
                            deriv);
        }

    }
}
