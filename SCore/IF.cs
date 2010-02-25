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
    public class IF : LI
    {
        public IF(double threshold,double resetpotential, double refractoryperiod, double initoutput, double tao)
            : this(new Point3D(0.0,0.0,0.0),new ThresholdFire(threshold,resetpotential,refractoryperiod), initoutput, tao)
        {
        }

        public IF(Point3D position, IHilllock hilllock, double initoutput, double tao)
            : base(position, hilllock, initoutput, tao)
        {
            DynamicRule = CoreFunc.dLI;
        }


        public override void Update(double deltaT, double currentT, ISolver solver)
        {
            for (int i = 0; i < Synapses.Count; i++)
            {
                Output += Synapses[i].PreSynapticNeuron.LastOutput * Synapses[i].Weight;
            }
            var dynamicruleparam = new double[] { Tao, Output };
            Output = solver.Solve(deltaT, currentT, LastOutput, dynamicruleparam, DynamicRule);
            Output = Hilllock.Fire(Output);
        }

    }
}
