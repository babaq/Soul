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
    [Serializable]
    public class Network : INetwork
    {
        private Guid id;
        private string name;
        private Point3D position;
        private Dictionary<Guid, INeuron> neurons;
        private Dictionary<Guid,INetwork> childnetworks;
        private INetwork parentnetwork;


        public Network():this("Network",new Point3D())
        {
        }

        public Network(string name, Point3D position)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.position = position;
            neurons = new Dictionary<Guid, INeuron>();
            childnetworks = new  Dictionary<Guid, INetwork>();
            parentnetwork = null;
        }


        #region INetwork Members

        public Guid ID
        {
            get{return id;}
        }

        public string Name
        {
            get{return name;}
            set{name = value;}
        }

        public Point3D Position
        {
            get{return position;}
            set{position = value;}
        }

        public Dictionary<Guid,INeuron> Neurons
        {
            get { return neurons; }
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

        public void Update(double deltaT,double currentT,ISolver solver)
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.Update(deltaT, currentT, solver);
            }
            for(int i=0;i<childnetworks.Count;i++)
            {
                childnetworks.ElementAt(i).Value.Update(deltaT,currentT, solver);
            }
        }

        public void Tick()
        {
            for (int i = 0; i < neurons.Count; i++)
            {
                neurons.ElementAt(i).Value.Tick();
            }
            for (int i = 0; i < childnetworks.Count; i++)
            {
                childnetworks.ElementAt(i).Value.Tick();
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
                s.AppendLine("# NumberOfChildNetwork=" + childnetworks.Count);
                return s.ToString();
            }
        }

        #endregion

    }
}
