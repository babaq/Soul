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
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace SCore
{
    [Serializable]
    public class Network : INetwork
    {
        private Guid id;
        private string name;
        private Point3D position;
        private INeuron[, ,] dimensionneurons;
        private Dictionary<Guid, INeuron> neurons;
        private Dictionary<Guid, INetwork> childnetworks;
        private INetwork parentnetwork;
        private Point3D? dimension;


        public Network()
            : this("Network", new Point3D())
        {
        }

        public Network(string name, Point3D position)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.position = position;
            neurons = new Dictionary<Guid, INeuron>();
            dimensionneurons = null;
            childnetworks = new Dictionary<Guid, INetwork>();
            parentnetwork = null;
            dimension = null;
        }


        #region INetwork Members

        public Guid ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Point3D Position
        {
            get { return position; }
            set
            {
                position = value;
                NotifyPropertyChanged("Position");
            }
        }

        public Dictionary<Guid, INeuron> Neurons
        {
            get { return neurons; }
        }

        public INeuron[, ,] DimensionNeurons
        {
            get { return dimensionneurons; }
        }

        public Dictionary<Guid, INetwork> ChildNetworks
        {
            get { return childnetworks; }
        }

        public INetwork ParentNetwork
        {
            get { return parentnetwork; }
            set
            {
                if (value == null)
                {
                    if (parentnetwork != null)
                    {
                        try
                        {
                            parentnetwork.ChildNetworks.Remove(this.id);
                        }
                        catch (Exception e)
                        {
                        }
                        parentnetwork = value;
                    }
                }
                else
                {
                    if (parentnetwork == null)
                    {
                        parentnetwork = value;
                        try
                        {
                            parentnetwork.ChildNetworks.Add(this.id, this);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    else
                    {
                        if (parentnetwork != value)
                        {
                            try
                            {
                                parentnetwork.ChildNetworks.Remove(this.id);
                                value.ChildNetworks.Add(this.id, this);
                            }
                            catch (Exception e)
                            {
                            }
                            parentnetwork = value;
                        }
                    }
                }
            }
        }

        public void Update(double deltaT, double currentT, ISolver solver)
        {
            if (GlobleSettings.IsRunInParallel)
            {
                Parallel.For(0, neurons.Count, (i) =>
                {
                    neurons.ElementAt(i).Value.Update(deltaT, currentT, solver);
                });
                Parallel.For(0, childnetworks.Count, (i) =>
                {
                    childnetworks.ElementAt(i).Value.Update(deltaT, currentT, solver);
                });
            }
            else
            {
                for (int i = 0; i < neurons.Count; i++)
                {
                    neurons.ElementAt(i).Value.Update(deltaT, currentT, solver);
                }
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.Update(deltaT, currentT, solver);
                }
            }
        }

        public void Tick(double currentT)
        {
            if (GlobleSettings.IsRunInParallel)
            {
                Parallel.For(0, neurons.Count, (i) =>
                {
                    neurons.ElementAt(i).Value.Tick(currentT);
                });
                Parallel.For(0, childnetworks.Count, (i) =>
                {
                    childnetworks.ElementAt(i).Value.Tick(currentT);
                });
            }
            else
            {
                for (int i = 0; i < neurons.Count; i++)
                {
                    neurons.ElementAt(i).Value.Tick(currentT);
                }
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.Tick(currentT);
                }
            }
        }

        public void RegisterUpdated(EventHandler onoutput)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.RegisterUpdated(onoutput);
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.RegisterUpdated(onoutput);
            }
        }

        public void RegisterSpike(EventHandler onspike)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.RegisterSpike(onspike);
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.RegisterSpike(onspike);
            }
        }

        public void UnRegisterUpdated(EventHandler onoutput)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.UnRegisterUpdated(onoutput);
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.UnRegisterUpdated(onoutput);
            }
        }

        public void UnRegisterSpike(EventHandler onspike)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.UnRegisterUpdated(onspike);
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.UnRegisterSpike(onspike);
            }
        }

        public void RaiseUpdated()
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.RaiseUpdated();
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.RaiseUpdated();
            }
        }

        public string Summary
        {
            get
            {
                var s = new StringBuilder();
                s.AppendLine("# Network Summary.");
                s.AppendLine("# ID=" + id.ToString("N"));
                s.AppendLine("# Name=" + name);
                s.AppendLine("# Position=" + position);
                s.AppendLine("# NumberOfNeuron=" + neurons.Count);
                if (neurons.Count > 0)
                {
                    s.AppendLine("# ----------------------------------------");
                    for (int i = 0; i < neurons.Count; i++)
                    {
                        var n = neurons.ElementAt(i).Value;
                        s.AppendLine("# Index=" + i);
                        s.Append(n.Summary);
                        s.AppendLine("# ----------------------------------------");
                    }
                }
                s.AppendLine("# NumberOfChildNetwork=" + childnetworks.Count);
                if (childnetworks.Count > 0)
                {
                    s.AppendLine("# ****************************************");
                    for (int i = 0; i < childnetworks.Count; i++)
                    {
                        var n = childnetworks.ElementAt(i).Value;
                        s.AppendLine("# Index=" + i);
                        s.Append(n.Summary);
                        s.AppendLine("# ****************************************");
                    }
                }
                return s.ToString();
            }
        }

        public void Set(bool isincludechildnetwork = false, Point3D? position = null, double? potential = null, double? output = null, double? lastoutput = null, double? r = null, double? c = null, double? restpotential = null)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.Set(position, potential, output, lastoutput, r, c, restpotential);
            }
            if (isincludechildnetwork)
            {
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.Set(isincludechildnetwork, position, potential, output, lastoutput, r, c, restpotential);
                }
            }
        }

        public Nullable<Point3D> Dimension
        {
            get { return dimension; }
        }

        public void ReShape(Point3D newdimension, Vector3D? neurondistance = null, bool isincludechildnetwork = false)
        {
            var dx = (int)Math.Max(1, newdimension.X);
            var dy = (int)Math.Max(1, newdimension.Y);
            var dz = (int)Math.Max(1, newdimension.Z);
            if (dx * dy * dz == Neurons.Count)
            {
                dimensionneurons = new INeuron[dx, dy, dz];
                Vector3D ND = GlobleSettings.NeuronDistance;
                if (neurondistance.HasValue)
                {
                    ND = neurondistance.Value;
                }
                for (var i = 0; i < dx; i++)
                {
                    for (var j = 0; j < dy; j++)
                    {
                        for (var k = 0; k < dz; k++)
                        {
                            var x = (i - dx / 2.0) * ND.X;
                            var y = (j - dy / 2.0) * ND.Y;
                            var z = (k - dz / 2.0) * ND.Z;
                            var neuron = neurons.ElementAt(i * dy * dz + j * dz + k).Value;
                            neuron.Position = new Point3D(x, y, z);
                            dimensionneurons[i, j, k] = neuron;
                        }
                    }
                }
                dimension = new Point3D(dx, dy, dz);
            }
            if (isincludechildnetwork)
            {
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.ReShape(newdimension, neurondistance, isincludechildnetwork);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public void ReSet(double startT = 0.0, bool isincludechildnetwork = true)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.ReSet(startT);
            }
            if (isincludechildnetwork)
            {
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.ReSet(startT, isincludechildnetwork);
                }
            }
        }

        #endregion

        #region IRandomizable Members

        public void Randomize(bool isincludechildnetwork = false, params Tuple<string, Randomizer>[] targets)
        {
            for (var i = 0; i < targets.Length; i++)
            {
                Randomize(targets[i].Item1, targets[i].Item2, isincludechildnetwork);
            }
        }

        public void Randomize(string property, Randomizer randomizer, bool isincludechildnetwork = false)
        {
            PropertyInfo propertyinfo;
            try
            {
                propertyinfo = typeof(INeuron).GetProperty(property);
            }
            catch (Exception e)
            {
                try
                {
                    propertyinfo = typeof(IHillock).GetProperty(property);
                }
                catch (Exception ee)
                {
                    return;
                }
            }

            if (dimensionneurons != null)
            {
                randomizer.Randomize(dimensionneurons, propertyinfo);
            }

            if (isincludechildnetwork)
            {
                for (int i = 0; i < childnetworks.Count; i++)
                {
                    childnetworks.ElementAt(i).Value.Randomize(property, randomizer, isincludechildnetwork);
                }
            }
        }

        #endregion

    }
}
