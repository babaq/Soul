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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using SSolver;
using System.ComponentModel;

namespace SCore
{
    public interface INetwork : INotifyPropertyChanged,IRandomizable
    {
        Guid ID { get; }
        string Name { get; set; }
        Point3D Position { set; get; }
        INeuron[, ,] DimensionNeurons { get; }
        Dictionary<Guid, INeuron> Neurons { get; }
        Dictionary<Guid, INetwork> ChildNetworks { get; }
        INetwork ParentNetwork { get; set; }
        void Update(double deltaT, double currentT, ISolver solver);
        void Tick(double currentT);
        void RegisterUpdated(EventHandler onoutput);
        void RegisterSpike(EventHandler onspike);
        void UnRegisterUpdated(EventHandler onoutput);
        void UnRegisterSpike(EventHandler onspike);
        void RaiseUpdated();
        string Summary { get; }
        void Set(bool isincludechildnetwork = false, Point3D? position = null, double? potential = null, double? output = null, double? lastoutput = null, double? r = null, double? c = null, double? restpotential = null);
        Nullable< Point3D> Dimension { get; }
        void ReShape(Point3D newdimension,Vector3D? neurondistance=null,bool isincludechildnetwork=false);
        void NotifyPropertyChanged(string propertyname);
        void ReSet(double startT = 0.0, bool isincludechildnetwork = true);
    }
}
