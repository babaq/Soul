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
    /// <summary>
    /// Generate Gaussian Random Number Using Polar Form of Box-Muller Transformation
    /// </summary>
    public static class GaussRandom
    {
        private static Random uniformrandom;
        private static double gn;
        private static int ign;


        static GaussRandom()
        {
            uniformrandom = new Random();
            ign = 0;
        }


        /// <summary>
        /// Gets Standard Gaussian Distribution Number (0, 1)
        /// </summary>
        /// <returns></returns>
        public static double Sample()
        {
            double un1, un2, sqw, w;
            if (ign == 0)
            {
                do
                {
                    un1 = 2.0 * uniformrandom.NextDouble() - 1.0;
                    un2 = 2.0 * uniformrandom.NextDouble() - 1.0;
                    w = un1 * un1 + un2 * un2;
                } while (w >= 1.0 || w == 0.0);

                sqw = Math.Sqrt((-2.0 * Math.Log(w) )/ w);
                gn = un2 * sqw;
                ign = 1;
                return un1 * sqw;
            }
            else
            {
                ign = 0;
                return gn;
            }
        }

        /// <summary>
        /// Gets Custom Gaussian Distribution Number (mean, sd)
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static double Next(double mean,double sd)
        {
            return CoreFunc.gGuassRandom(Sample(), mean, sd);
        }

    }

    public static class Projection
    {
        public static void From_To(INeuron neuron, INetwork network)
        {
            
        }

        public static void From_To(INetwork network, INeuron neuron)
        {
            
        }

        public static void From_To(INetwork sourcenetwork, INetwork targetnetwork)
        {
            
        }

    }

}

