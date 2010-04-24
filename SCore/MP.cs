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
    /// <summary>
    /// McCulloch-Pitts Model
    /// </summary>
    [Serializable]
    public class MP : INeuron
    {
        private Guid id;
        private string name;
        private Point3D position;
        private Dictionary<Guid, ISynapse> synapses;
        private Dictionary<Guid, ICurrentSource> currentsources;
        private IHillock hillock;
        private double initpotential;
        private double potential;
        private double output;
        private double lastoutput;
        private INetwork parentnetwork;
        private Derivative dynamicrule;
        protected NeuronType type;


        public MP(double threshold, double initpotential)
            : this("MP", threshold, initpotential)
        {
        }

        public MP(string name, double threshold, double initpotential)
            : this(name, new Point3D(), new ThresholdHeaviside(null, threshold), initpotential, 0.0)
        {
        }

        public MP(string name, Point3D position, IHillock hillock, double initpotential, double startT)
        {
            id = Guid.NewGuid();
            this.name = name;
            this.position = position;
            synapses = new Dictionary<Guid, ISynapse>();
            currentsources = new Dictionary<Guid, ICurrentSource>();
            this.hillock = hillock;
            this.hillock.HostNeuron = this;
            this.initpotential = initpotential;
            ReSet(startT);
            parentnetwork = null;
            dynamicrule = null;
            type = NeuronType.MP;
        }


        #region INeuron Members

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

        public Dictionary<Guid, ISynapse> Synapses
        {
            get { return synapses; }
        }

        public IHillock Hillock
        {
            get { return hillock; }
            set { hillock = value; }
        }

        public double InitPotential
        {
            get { return initpotential; }
            set { initpotential = value; }
        }

        public double Potential
        {
            get { return potential; }
            set { potential = value; }
        }

        public double Output
        {
            get { return output; }
            set { output = value; }
        }

        public double LastOutput
        {
            get { return lastoutput; }
            set { lastoutput = value; }
        }

        public virtual void Update(double deltaT, double currentT, ISolver solver)
        {
            output = hillock.Fire(SynapseCurrents(deltaT, currentT), currentT);
            RaiseUpdated();
        }

        public void Tick(double currentT)
        {
            lastoutput = output;
            hillock.Tick(currentT);
        }

        public void ProjectTo(INeuron targetneuron, ISynapse targetsynapse)
        {
            if (!targetneuron.Synapses.ContainsValue(targetsynapse))
            {
                targetneuron.Synapses.Add(targetsynapse.ID, targetsynapse);
                if (targetneuron.ParentNetwork == null && this.parentnetwork != null)
                {
                    targetneuron.ParentNetwork = this.parentnetwork;
                    return;
                }
                if (targetneuron.ParentNetwork != null && this.parentnetwork == null)
                {
                    this.parentnetwork = targetneuron.ParentNetwork;
                    return;
                }
            }
        }

        public void ProjectedFrom(INeuron sourceneuron, ISynapse selfsynapse)
        {
            if (!this.Synapses.ContainsValue(selfsynapse))
            {
                this.Synapses.Add(selfsynapse.ID, selfsynapse);
                if (sourceneuron.ParentNetwork == null && this.parentnetwork != null)
                {
                    sourceneuron.ParentNetwork = this.parentnetwork;
                    return;
                }
                if (sourceneuron.ParentNetwork != null && this.parentnetwork == null)
                {
                    this.parentnetwork = sourceneuron.ParentNetwork;
                    return;
                }
            }
        }

        public void DisConnect(Guid selfsynapseid)
        {
            if (Synapses.ContainsKey(selfsynapseid))
            {
                Synapses.Remove(selfsynapseid);
            }
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
                            parentnetwork.Neurons.Remove(this.id);
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
                            parentnetwork.Neurons.Add(this.id, this);
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
                                parentnetwork.Neurons.Remove(this.id);
                                value.Neurons.Add(this.id, this);
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

        public Derivative DynamicRule
        {
            get { return dynamicrule; }
            set { dynamicrule = value; }
        }

        public event EventHandler Updated;

        public void RaiseUpdated()
        {
            if (Updated != null)
            {
                Updated(this, EventArgs.Empty);
            }
        }

        public virtual double R
        {
            get { return 0.0; }
            set { }
        }

        public virtual double C
        {
            get { return 0.0; }
            set { }
        }

        public double Tao
        {
            get { return R * C; }
        }

        public virtual double RestPotential
        {
            get { return 0.0; }
            set { }
        }

        public virtual void RegisterUpdated(EventHandler onoutput)
        {
            Updated += onoutput;
        }

        public virtual void RegisterSpike(EventHandler onspike)
        {
        }

        public virtual void UnRegisterUpdated(EventHandler onoutput)
        {
            Updated -= onoutput;
        }

        public virtual void UnRegisterSpike(EventHandler onspike)
        {
        }

        public NeuronType Type
        {
            get { return type; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public string Summary
        {
            get
            {
                var s = new StringBuilder();
                s.AppendLine("# Neuron Summary.");
                s.AppendLine("# ID=" + id.ToString("N"));
                s.AppendLine("# Name=" + name);
                s.AppendLine("# Position=" + position);
                return s.ToString();
            }
        }

        public void Set(Point3D? position = null, double? potential = null, double? output = null, double? lastoutput = null, double? r = null, double? c = null, double? restpotential = null)
        {
            if (position.HasValue)
            {
                Position = position.Value;
            }
            if (potential.HasValue)
            {
                Potential = potential.Value;
            }
            if (output.HasValue)
            {
                Output = output.Value;
            }
            if (lastoutput.HasValue)
            {
                LastOutput = lastoutput.Value;
            }
            if (r.HasValue)
            {
                R = r.Value;
            }
            if (c.HasValue)
            {
                C = c.Value;
            }
            if (restpotential.HasValue)
            {
                RestPotential = restpotential.Value;
            }
        }

        public void ReSet(double startT = 0.0)
        {
            potential = initpotential;
            output = lastoutput = hillock.Fire(initpotential, startT);
            RaiseUpdated();
        }

        public void InjectedFrom(ICurrentSource currentsource)
        {
            if (!this.CurrentSources.ContainsValue(currentsource))
            {
                this.CurrentSources.Add(currentsource.ID, currentsource);
            }
        }

        public Dictionary<Guid, ICurrentSource> CurrentSources
        {
            get { return currentsources; }
        }

        public double InjectedCurrents(double currentT)
        {
            var current = 0.0;
            for (int i = 0; i < currentsources.Count; i++)
            {
                current += currentsources.ElementAt(i).Value.Flow(currentT);
            }
            return current;
        }

        public double SynapseCurrents(double deltaT, double currentT)
        {
            var sigma = 0.0;
            for (int i = 0; i < Synapses.Count; i++)
            {
                sigma += Synapses.ElementAt(i).Value.Release(deltaT, currentT);
            }
            return sigma;
        }

        #endregion


        #region ICloneable Members

        public virtual object Clone()
        {
            var clone = new MP(this.hillock.Threshold, this.lastoutput);
            return clone;
        }

        #endregion

    }
}
