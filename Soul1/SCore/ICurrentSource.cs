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
using System.ComponentModel;
using SSolver;

namespace SCore
{
    public interface ICurrentSource : INotifyPropertyChanged
    {
        Guid ID { get; }
        string Name { set; get; }
        Point3D Position { set; get; }
        void InjectTo(INeuron neuron);
        double Flow(double currentT);
        bool ON { get; set; }
        double Amplitude { get; set; }
        void NotifyPropertyChanged(string propertyname);
        TimeFunc TimeFunc { get; set; }
        double[] TimeFuncParams { get; set; }
        RNG Random { get; set; }
        double[] RandomParams { get; set; }
    }
}
