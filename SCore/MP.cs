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
    /// <summary>
    /// McCulloch-Pitts Model
    /// </summary>
    [Serializable]
    public class MP : INeuron
    {
        private Guid id;
        private string name;
        private Point3D position;
        private Dictionary<Guid,ISynapse> weightsynapses;
        private IHillock hillock;
        private double output;
        private double lastoutput;
        private INetwork parentnetwork;
        

        public MP(double threshold,double initoutput)
            : this("MP",new Point3D(), new ThresholdHeaviside(null, threshold),initoutput)
        {
        }

        public MP(string name,Point3D position, IHillock hillock,double initoutput)
        {
            id = Guid.NewGuid();
            this.name = name;
            this.position = position;
            weightsynapses = new Dictionary<Guid,ISynapse>();
            this.hillock = hillock;
            this.hillock.HostNeuron = this;
            output = lastoutput = initoutput;
            parentnetwork =null;
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
            set { position = value; }
        }

        public Dictionary<Guid,ISynapse> Synapses
        {
            get { return weightsynapses; }
        }

        public IHillock Hillock
        {
            get { return hillock; }
            set { hillock = value; }
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

        public virtual void Update(double deltaT,double currentT,ISolver solver)
        {
            var sigma = 0.0;
            for (int i = 0; i < weightsynapses.Count; i++)
            {
                sigma += weightsynapses.ElementAt(i).Value.Release(deltaT, currentT);
            }
            output = hillock.Fire(sigma,currentT);
            RaiseUpdated();
        }

        public void Tick()
        {
            lastoutput = output;
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
            get { return null; }
            set { }
        }

        public event EventHandler Updated;

        public void RaiseUpdated()
        {
            if(Updated!=null)
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
            set {}
        }

        public double Tao
        {
            get { return R*C; }
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
