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
    public class Population : IPopulation, INetwork
    {
        private Guid id;
        private string name;
        private Point3D position;
        private Dictionary<Guid, INeuron> neurons;
        private INetwork parentnetwork;
        
        public Population()
        {
            this.id = Guid.NewGuid();
            this.name = "Population";
            this.position = new Point3D(0.0,0.0,0.0);
            neurons = new Dictionary<Guid, INeuron>();
            parentnetwork = null;
        }


        #region IPopulation Members

        public Dictionary<Guid, INeuron> Neurons
        {
            get { return neurons; }
        }

        #endregion

        #region INetwork Members

        public Guid ID
        {
            get { return id;}
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

        public INetwork ParentNetwork
        {
            get { return parentnetwork; }
            set
            {
                if (value==null)
                {
                    if(parentnetwork!=null)
                    {
                        parentnetwork.SubNetworks.Remove(this.id);
                    }
                }
                else
                {
                    if(parentnetwork==null)
                    {
                        parentnetwork = value;
                        parentnetwork.SubNetworks.Add(this.id,this);
                    }
                    else
                    {
                        parentnetwork.SubNetworks.Remove(this.id);
                        parentnetwork = value;
                        parentnetwork.SubNetworks.Add(this.id,this);
                    }
                }
            }
        }

        public Dictionary<Guid, INetwork> SubNetworks
        {
            get { return new Dictionary<Guid, INetwork>();}
        }

        public void Update(double deltaT,double currentT,ISolver solver)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.Update(deltaT,currentT, solver);
            }
        }

        public void Tick()
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.Tick();
            }
        }

        #endregion


    }
}
