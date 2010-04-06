using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class Rand_Gauss
    {
        static double gset;
        static int iset = 0;

        public static double Generate()
        {
            Random Randg = new Random();
            double V1, V2, scq, S;

            if (iset == 0)
            {
                do
                {
                    V1 = 2 * Randg.NextDouble() - 1.0;
                    V2 = 2 * Randg.NextDouble() - 1.0;
                    S = V1 * V1 + V2 * V2;
                } while (S >= 1.0 || S == 0.0);

                scq = Math.Sqrt(-2 * Math.Log(S) / S);
                gset = V2 * scq;
                iset = 1;
                return (V1 * scq);
            }
            else
            {
                iset = 0;
                return gset;
            }

        }
    }
}

