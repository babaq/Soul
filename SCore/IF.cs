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
    /// Integrate-and-Fire Model
    /// </summary>
    [Serializable]
    public class IF : LI
    {
        public IF(double threshold, double resetpotential, double refractoryperiod, double initpotential, double r, double c, double restpotential)
            : this("IF", threshold, resetpotential, refractoryperiod, initpotential, r, c, restpotential)
        {
        }

        public IF(string name,double threshold, double resetpotential, double refractoryperiod, double initpotential, double r, double c, double restpotential)
            : this(name, new Point3D(), new ThresholdSpike(null, threshold, resetpotential, refractoryperiod), initpotential, r, c, restpotential,0.0)
        {
        }

        public IF(string name, Point3D position, IHillock hillock, double initpotential, double r, double c, double restpotential, double currentT)
            : base(name, position, hillock, initpotential, r, c, restpotential,currentT)
        {
            DynamicRule = CoreFunc.dIF;
            type = NeuronType.IF;
        }


        public override void Update(double deltaT, double currentT, ISolver solver)
        {
            if (!Hillock.IsInRefractoryPeriod(currentT))
            {
                var sigma = 0.0;
                for (int i = 0; i < Synapses.Count; i++)
                {
                    sigma += Synapses.ElementAt(i).Value.Release(deltaT, currentT);
                }
                var dynamicruleparam = new double[] {Tao, R, RestPotential, sigma};
                Potential = solver.Solve(deltaT, currentT, Potential, dynamicruleparam, DynamicRule);
                Potential = Hillock.Fire(Potential, currentT);
                Output = Potential;
            }
            RaiseUpdated();
        }

        public override void RegisterSpike(EventHandler onspike)
        {
            Hillock.Spike += onspike;
        }

        public override void UnRegisterSpike(EventHandler onspike)
        {
            Hillock.Spike -= onspike;
        }

        public override object Clone()
        {
            var clone = new IF(Hillock.Threshold, Hillock.ResetPotential, Hillock.RefractoryPeriod, LastOutput, R, C,
                               RestPotential);
            return clone;
        }

    }
}
