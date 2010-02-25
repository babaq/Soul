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
    public class Simulator : ISimulator
    {
        private double t0;
        private double deltaT;
        private double durationT;
        private double currentT;
        private bool isrunning;
        private bool isrunover;
        private INetwork network;
        private ISolver solver;

        public Simulator(double deltatime, double durationtime,INetwork targetnetwork,ISolver solver)
        {
            deltaT = deltatime;
            durationT = durationtime;
            currentT = 0.0;
            t0 = currentT;
            isrunning = false;
            isrunover = false;
            network = targetnetwork;
            this.solver = solver;
        }


        #region ISimulator Members

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
        }

        public ISolver Solver
        {
            get { return solver; }
            set { solver = value; }
        }

        public double DeltaT
        {
            get
            {
                return deltaT;
            }
            set
            {
                deltaT = value;
            }
        }

        public double DurationT
        {
            get
            {
                return durationT;
            }
            set
            {
                durationT = value;
            }
        }

        public double CurrentT
        {
            get { return currentT; }
        }

        public void Run()
        {
            if (network != null && solver != null)
            {
                isrunning = true;
                do
                {
                    Step();
                } while (currentT -t0<= durationT);
                t0 = currentT;
                isrunning = false;
                isrunover = true;
            }
        }

        public void Step()
        {
            Step(deltaT);
        }

        public void Step(double delta)
        {
            //record
            network.Update(delta,currentT, solver);
            network.Tick();
            currentT += delta;
        }

        public bool IsRunning
        {
            get { return isrunning; }
        }

        public bool IsRunOver
        {
            get { return isrunover; }
        }

        #endregion
    }
}
