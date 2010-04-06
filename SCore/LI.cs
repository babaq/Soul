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
    [Serializable]
    public class LI : MP
    {
        private double r;
        private double c;
        private double restpotential;


        public LI(double threshold, double initpotential, double r, double c, double restpotential)
            : this("LI", threshold, initpotential, r, c, restpotential)
        {
        }

        public LI(string name,double threshold, double initpotential, double r, double c, double restpotential)
            : this(name, new Point3D(), new ThresholdSigmoid(null, threshold), initpotential, r, c, restpotential,0.0)
        {
        }

        public LI(string name, Point3D position, IHillock hillock, double initpotential, double r, double c, double restpotential,double currentT)
            : base(name, position, hillock, initpotential,currentT)
        {
            this.r = r;
            this.c = c;
            this.restpotential = restpotential;
            DynamicRule = CoreFunc.dLI;
            type = NeuronType.LI;
        }


        public override double R
        {
            get{return r;}
            set{r = value;}
        }

        public override double C
        {
            get{return c;}
            set{c = value;}
        }

        public override double RestPotential
        {
            get{return restpotential;}
            set{restpotential = value;}
        }

        public override void Update(double deltaT,double currentT,ISolver solver)
        {
            var sigma = 0.0;
            for (int i = 0; i < Synapses.Count; i++)
            {
                sigma += Synapses.ElementAt(i).Value.Release(deltaT, currentT);
            }
            var dynamicruleparam = new double[] {Tao,RestPotential, sigma};
            Potential = solver.Solve(deltaT, currentT, Potential, dynamicruleparam, DynamicRule);
            Output = Hillock.Fire(Potential, currentT);
            RaiseUpdated();
        }

        public override object Clone()
        {
            var clone = new LI(Hillock.Threshold, LastOutput, r,c,restpotential);
            return clone;
        }

    }
}
