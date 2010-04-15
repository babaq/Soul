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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using SSolver;

namespace SCore
{
    public interface INeuron : ICloneable, INotifyPropertyChanged
    {
        Guid ID { get; }
        string Name { set; get; }
        Point3D Position { set; get; }
        Dictionary<Guid,ISynapse> Synapses { get; }
        IHillock Hillock { set; get; }
        double Potential { get; set; }
        double Output { get; set; }
        double LastOutput { get; set; }
        void Update(double deltaT,double currentT,ISolver solver);
        void Tick();
        void ProjectTo(INeuron targetneuron,ISynapse targetsynapse);
        void ProjectedFrom(INeuron sourceneuron,ISynapse selfsynapse);
        void DisConnect(Guid selfsynapseid);
        INetwork ParentNetwork { get; set; }
        Derivative DynamicRule { get; set; }
        event EventHandler Updated;
        void RaiseUpdated();
        double R { get; set; }
        double C { get; set; }
        double Tao { get; }
        double RestPotential { get; set; }
        void RegisterUpdated(EventHandler onoutput);
        void RegisterSpike(EventHandler onspike);
        void UnRegisterUpdated(EventHandler onoutput);
        void UnRegisterSpike(EventHandler onspike);
        NeuronType Type { get; }
        void NotifyPropertyChanged(string propertyname);
        string Summary { get; }
    }


    public enum NeuronType
    {
        MP,
        LI,
        IF,
        HH,
        MC
    }

}
