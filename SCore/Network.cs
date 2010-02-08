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

namespace SCore
{
    public class Network : INetwork
    {
        private Guid id;
        private string name;
        private Point3D position;
        private Dictionary<Guid,INetwork> subnetworks;
        private INetwork parentnetwork;

        public Network()
        {
            this.id = Guid.NewGuid();
            this.name = "Network";
            this.position = new Point3D(0.0,0.0,0.0);
            subnetworks = new  Dictionary<Guid, INetwork>();
            parentnetwork = null;
        }


        #region INetwork Members

        public Guid ID
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public Point3D Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Dictionary<Guid, INetwork> SubNetworks
        {
            get { return subnetworks; }
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
                        parentnetwork.SubNetworks.Remove(this.id);
                    }
                }
                else
                {
                    if (parentnetwork == null)
                    {
                        parentnetwork = value;
                        parentnetwork.SubNetworks.Add(this.id, this);
                    }
                    else
                    {
                        parentnetwork.SubNetworks.Remove(this.id);
                        parentnetwork = value;
                        parentnetwork.SubNetworks.Add(this.id, this);
                    }
                }
            }
        }

        public void Update(double deltaT)
        {
            for(int i=0;i<subnetworks.Count;i++)
            {
                subnetworks.ElementAt(i).Value.Update(deltaT);
            }
        }

        public void Tick()
        {
            for (int i = 0; i < subnetworks.Count; i++)
            {
                subnetworks.ElementAt(i).Value.Tick();
            }
        }

        #endregion
    }
}
