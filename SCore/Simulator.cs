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

namespace SCore
{
    public class Simulator : ISimulator
    {
        private double deltaT;
        private double durationT;
        private double currentT;
        private bool isrunning;
        private bool isrunover;
        private INetwork network;

        public Simulator(double deltatime, double durationtime,INetwork targetnetwork)
        {
            deltaT = deltatime;
            durationT = durationtime;
            currentT = 0.0;
            isrunning = false;
            isrunover = false;
            network = targetnetwork;
        }


        #region ISimulator Members

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
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
            if (network != null)
            {
                isrunning = true;
                do
                {
                    network.Update(deltaT);
                    network.Tick();
                    currentT += deltaT;
                } while (currentT <= durationT);
                isrunning = false;
                isrunover = true;
            }
        }

        public void StepRun()
        {
            if (network != null)
            {
                if (currentT <= durationT)
                {
                    isrunning = true;
                    network.Update(deltaT);
                    network.Tick();
                    currentT += deltaT;
                }
                else
                {
                    isrunning = false;
                    isrunover = true;
                }
            }
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
