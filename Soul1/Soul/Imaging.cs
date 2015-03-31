//--------------------------------------------------------------------------------
// This file is part of the Soul - Neural Network Simulation System.
//
// Copyright © 2010 Alex-Joyce. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using SoulCore;

namespace Soul
{
    [ValueConversion(typeof(double), typeof(Color), ParameterType = typeof(HillockType))]
    public class Imaging : IValueConverter
    {
        public Color Dye { get; set; }


        public static Color Dyes(NeuronType type)
        {
            Color color;
            switch (type)
            {
                case NeuronType.HH:
                    color = Colors.Blue;
                    break;
                case NeuronType.IF:
                    color = Colors.Red;
                    break;
                case NeuronType.LI:
                    color = Colors.Green;
                    break;
                default:
                    color = Colors.White;
                    break;
            }
            return color;
        }

        public static double Normalize(double source, HillockType type)
        {
            switch (type)
            {
                case HillockType.Spike:
                    source = (source - GlobleSettings.NeuronPotentialMin) / GlobleSettings.NeuronPotentialRange;
                    break;
            }
            return source;
        }


        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var neuronoutput = (double)value;
            var hillocktype = (HillockType)parameter;

            neuronoutput = Normalize(neuronoutput, hillocktype);
            var imagingcolor = Color.FromRgb((byte)(Dye.R * neuronoutput),
                                  (byte)(Dye.G * neuronoutput), (byte)(Dye.B * neuronoutput));
            return imagingcolor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

    }
}
