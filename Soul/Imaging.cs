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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using SCore;

namespace Soul
{
    [ValueConversion(typeof(double), typeof(Color), ParameterType = typeof(Color))]
    public class Imaging:IValueConverter
    {
        public static Color ImagingDye(NeuronType type)
        {
            Color color;
            switch (type)
            {
                default:
                    color = Colors.White;
                    break;
            }
            return color;
        }


        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var neuronoutput = (double) value;
            var imagingdye = (Color) parameter;
            var imagingcolor = Color.FromRgb( (byte) (imagingdye.R*neuronoutput),
                                  (byte) (imagingdye.G*neuronoutput), (byte) (imagingdye.B*neuronoutput));
            return imagingcolor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

    }
}
