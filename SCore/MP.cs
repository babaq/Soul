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
using SSolver;

namespace SCore
{
    /// <summary>
    /// McCulloch-Pitts Model
    /// </summary>
    public class MP : INeuron
    {
        private Guid id;
        private string name;
        private Point3D position;
        private List<ISynapse> weightsynapses;
        private IHilllock hilllock;
        private double output;
        private double lastoutput;
        private IPopulation population;
        private Derivative dynamicrule;

        public MP(double threshold,double initoutput)
            : this(new Point3D(0.0,0.0,0.0), new ThresholdHeaviside(threshold),initoutput)
        {
        }

        public MP(Point3D position, IHilllock hilllock,double initoutput)
        {
            id = Guid.NewGuid();
            name = "MP";
            this.position = position;
            weightsynapses = new List<ISynapse>();
            this.hilllock = hilllock;
            output = 0.0;
            lastoutput = initoutput;
            population =null;
            dynamicrule = null;
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

        public List<ISynapse> Synapses
        {
            get { return weightsynapses; }
        }

        public IHilllock Hilllock
        {
            get { return hilllock; }
            set { hilllock = value; }
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
            for (int i = 0; i < weightsynapses.Count; i++)
            {
                output += weightsynapses[i].PreSynapticNeuron.LastOutput*weightsynapses[i].Weight;
            }

            output = hilllock.Fire(output);
        }

        public void Tick()
        {
            lastoutput = output;
            output = 0.0;
        }

        public void ProjectTo(INeuron targetneuron, ISynapse targetsynapse)
        {
            targetneuron.Synapses.Add(targetsynapse);
            if (targetneuron.Population==null && this.population!=null)
            {
                targetneuron.Population=this.population;
                return;
            }
            if (targetneuron.Population != null && this.population== null)
            {
                this.population = targetneuron.Population;
                return;
            }
        }

        public void ProjectedFrom(INeuron sourceneuron, ISynapse selfsynapse)
        {
            this.Synapses.Add(selfsynapse);
            if (sourceneuron.Population == null && this.population!= null)
            {
                sourceneuron.Population = this.population;
                return;
            }
            if (sourceneuron.Population != null && this.population == null)
            {
                this.population = sourceneuron.Population;
                return;
            }
        }

        public void DisConnect(ISynapse selfsynapse)
        {
            if (Synapses.Contains(selfsynapse))
                Synapses.Remove(selfsynapse);
        }

        public IPopulation Population
        {
            get { return population; }
            set
            {
                if (value == null)
                {
                    if (population != null)
                    {
                        population.Neurons.Remove(this.id);
                        population = value;
                    }
                }
                else
                {
                    if (population == null)
                    {
                        population = value;
                        population.Neurons.Add(this.id, this);
                    }
                    else
                    {
                        population.Neurons.Remove(this.id);
                        population = value;
                        population.Neurons.Add(this.id, this);
                    }
                }
            }
        }

        public virtual double Tao
        {
            get { return 0.0; }
            set {}
        }

        public Derivative DynamicRule
        {
            get { return dynamicrule; }
            set { dynamicrule = value; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var clone = new MP(this.position, this.hilllock, lastoutput);
            return clone;
        }

        #endregion
    }
}
