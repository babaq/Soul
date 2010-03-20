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
        private double resistence;


        public IF(double threshold,double resetpotential, double refractoryperiod, double initoutput, double tao, double resistence)
            : this(new Point3D(0.0,0.0,0.0),new ThresholdSpike(threshold,resetpotential,refractoryperiod), initoutput, tao, resistence)
        {
        }

        public IF(Point3D position, IHillock hilllock, double initoutput, double tao, double resistence)
            : base(position, hilllock, initoutput, tao,-65.0)
        {
            this.resistence = resistence;
            DynamicRule = CoreFunc.dIF;
        }


        public override double R
        {
            get
            {
                return resistence;
            }
            set
            {
                resistence = value;
            }
        }

        public override void Update(double deltaT, double currentT, ISolver solver)
        {
            if (!Hillock.IsInRefractoryPeriod(currentT))
            {
                for (int i = 0; i < Synapses.Count; i++)
                {
                    var dirac = 0.0;
                    if (Synapses[i].PreSynapticNeuron.Hillock.TravalingSpikeTime.Count > 0)
                    {
                        for (int j = 0; j < Synapses[i].PreSynapticNeuron.Hillock.TravalingSpikeTime.Count; j++)
                        {
                            dirac += CoreFunc.rDiracDelta(
                                currentT - Synapses[i].PreSynapticNeuron.Hillock.TravalingSpikeTime.ElementAt(j) -
                                Synapses[i].AxonDelay, deltaT);
                        }
                        Synapses[i].PreSynapticNeuron.Hillock.CheckTravalingSpike(currentT);
                    }
                    Output += dirac*Synapses[i].Weight;
                }
                var dynamicruleparam = new double[] {Tao, resistence, RestPotential, Output};
                Output = solver.Solve(deltaT, currentT, LastOutput, dynamicruleparam, DynamicRule);
                Output = Hillock.Fire(Output, currentT);
                OnUpdated();
            }
        }

        public override void RegisterSpike(EventHandler recordspike)
        {
            Hillock.Spike += recordspike;
        }

        public override void UnRegisterSpike(EventHandler recordspike)
        {
            Hillock.Spike -= recordspike;
        }

    }
}
