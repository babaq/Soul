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

namespace SCore
{
    public interface INeuron : ICloneable
    {
        Guid ID { get; }
        string Name { set; get; }
        Point3D Position { set; get; }
        List<ISynapse> Synapses { get; }
        IHillock Hillock { set; get; }
        double Output { get; set; }
        double LastOutput { get; set; }
        void Update(double deltaT,double currentT,ISolver solver);
        void Tick();
        void ProjectTo(INeuron targetneuron,ISynapse targetsynapse);
        void ProjectedFrom(INeuron sourceneuron,ISynapse selfsynapse);
        void DisConnect(ISynapse selfsynapse);
        IPopulation Population { get; set; }
        double Tao { get; set; }
        double R { get; set; }
        double RestPotential { get; set; }
        Derivative DynamicRule { get; set; }
        event EventHandler Updated;
        void RegisterUpdated(EventHandler recordpotential);
        void RegisterSpike(EventHandler recordspike);
        void UnRegisterUpdated(EventHandler recordpotential);
        void UnRegisterSpike(EventHandler recordspike);
        void RecordStep(StreamWriter potentialwriter, RecordType recordtype,double currentT);
    }
}
