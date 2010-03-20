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
using SSolver;

namespace SCore
{
    /// <summary>
    /// Leaky Integrator Model
    /// </summary>
    public class LI : MP
    {
        private double tao;
        private double restpotential;


        public LI(double threshold, double initoutput, double tao)
            : this(new Point3D(0.0, 0.0, 0.0), new ThresholdSigmoid(null, threshold), initoutput, tao,-65.0)
        {
        }

        public LI(Point3D position, IHillock hillock, double initoutput, double tao, double restpotentail)
            : base(position, hillock, initoutput)
        {
            this.tao = tao;
            this.restpotential = restpotentail;
            DynamicRule = CoreFunc.dLI;
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

        public override double RestPotential
        {
            get
            {
                return restpotential;
            }
            set
            {
                restpotential = value;
            }
        }

        public override void Update(double deltaT,double currentT,ISolver solver)
        {
            for (int i = 0; i < Synapses.Count; i++)
            {
                Output += Synapses[i].PreSynapticNeuron.LastOutput * Synapses[i].Weight;
            }
            var dynamicruleparam = new double[] {tao,RestPotential, Output};
            Output = solver.Solve(deltaT, currentT, LastOutput, dynamicruleparam, DynamicRule);
            Output = Hillock.Fire(Output,currentT);
            OnUpdated();
        }

    }
}
