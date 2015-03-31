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
using System.Reflection;
using System.Windows.Media.Media3D;

namespace SCore
{
    public interface IRandomizable
    {
        void Randomize(bool isincludechildren = false, params Tuple<string, Randomizer>[] targets);
        void Randomize(string property, Randomizer randomizer, bool isincludechildren = false);
    }

    public class Randomizer
    {
        int dimxbegin, dimxend, dimybegin, dimyend, dimzbegin, dimzend;
        RNG randomsource;
        double mean, std;


        public Randomizer(RNG randomsource, int dimxbegin = 0, int dimxend = 0, int dimybegin = 0, int dimyend = 0, int dimzbegin = 0, int dimzend = 0, double mean = 0.5, double std = 0.5)
        {
            this.randomsource = randomsource;
            this.dimxbegin = dimxbegin;
            this.dimxend = dimxend;
            this.dimybegin = dimybegin;
            this.dimyend = dimyend;
            this.dimzbegin = dimzbegin;
            this.dimzend = dimzend;
            this.mean = mean;
            this.std = std;
        }


        public virtual void Randomize(object[, ,] dimensionelements, PropertyInfo propertyinfo)
        {
            if (randomsource != null)
            {
                PrepareDimensionRange(dimensionelements.GetLength(0), dimensionelements.GetLength(1), dimensionelements.GetLength(2));
                try
                {
                    bool isneedconvert = propertyinfo.PropertyType != typeof(double);
                    for (var i = dimxbegin; i <= dimxend; i++)
                    {
                        for (var j = dimybegin; j <= dimyend; j++)
                        {
                            for (var k = dimzbegin; k <= dimzend; k++)
                            {
                                var randomvalue = randomsource.NextMeanStdDouble(mean, std);
                                propertyinfo.SetValue(dimensionelements[i, j, k], Convert(isneedconvert, randomvalue, propertyinfo.PropertyType), null);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        protected virtual void PrepareDimensionRange(int dimxlength, int dimylength, int dimzlength)
        {
            dimxbegin = Math.Min(dimxbegin, dimxlength - 1);
            dimxend = Math.Min(dimxend, dimxlength - 1);
            if (dimxbegin > dimxend)
            {
                throw new ArgumentOutOfRangeException("DimXBeginValue", "ArgumentBeginValueGreaterThanEndValue");
            }
            dimybegin = Math.Min(dimybegin, dimylength - 1);
            dimyend = Math.Min(dimyend, dimylength - 1);
            if (dimybegin > dimyend)
            {
                throw new ArgumentOutOfRangeException("DimYBeginValue", "ArgumentBeginValueGreaterThanEndValue");
            }
            dimzbegin = Math.Min(dimzbegin, dimzlength - 1);
            dimzend = Math.Min(dimzend, dimzlength - 1);
            if (dimzbegin > dimzend)
            {
                throw new ArgumentOutOfRangeException("DimZBeginValue", "ArgumentBeginValueGreaterThanEndValue");
            }
        }

        protected virtual object Convert(bool isconvert, double value, Type outtype)
        {
            if (isconvert)
            {
                if (outtype == typeof(Point3D))
                {
                    return new Point3D(value, randomsource.NextMeanStdDouble(mean, std), randomsource.NextMeanStdDouble(mean, std));
                }
            }
            return value;
        }

    }

    public static class Poisson
    {
        private static Random ran;

        static Poisson()
        {
            ran = new Random();
        }

        /// <summary>
        /// 负指数分布随机数产生
        /// </summary>
        /// <param name="lam">参数</param>
        /// <returns></returns>
        public static double ngtIndex(double lam)
        {
            double dec = ran.NextDouble();
            while (dec == 0)
                dec = ran.NextDouble();
            return -Math.Log(dec) / lam;
        }

        /// <summary>
        /// 泊松分布产生
        /// </summary>
        /// <param name="lam">参数</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static double poisson(double lam, double time)
        {
            int count = 0;

            while (true)
            {
                time -= ngtIndex(lam);
                if (time > 0)
                    count++;
                else
                    break;
            }
            return count;
        }
    }

}
