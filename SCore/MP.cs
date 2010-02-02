using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class MP : INeuron
    {
        int id;
        private string name;
        List<ISynapse> weightsynapselist;
        IHilllock axonhilllock;
        double axonpotential;
        double lastaxonpotential;
        INetwork network;

        public MP(int neuronID, double threshold,double initpotentail)
            : this(neuronID, new ThresholdHeaviside(threshold),initpotentail)
        {
        }

        public MP(int neuronID, IHilllock hilllock,double initpotentail)
        {
            id = neuronID;
            name = "NoName";
            weightsynapselist = new List<ISynapse>();
            axonhilllock = hilllock;
            axonpotential = 0.0;
            lastaxonpotential = initpotentail;
            network = null;
        }


        #region INeuron Members

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<ISynapse> SynapseList
        {
            get { return weightsynapselist; }
        }

        public IHilllock AxonHilllock
        {
            get { return axonhilllock; }
            set { axonhilllock = value; }
        }

        public double AxonPotential
        {
            get { return axonpotential; }
        }

        public double LastAxonPotential
        {
            get { return lastaxonpotential; }
        }

        public void Update()
        {
            for (int i = 0; i < weightsynapselist.Count; i++)
            {
                axonpotential += network.GetLastAxonPotential(weightsynapselist[i].PreSynapticID)*weightsynapselist[i].Weight;
            }

            axonpotential = axonhilllock.Fire(axonpotential);

        }

        public void Tick()
        {
            lastaxonpotential = axonpotential;
            axonpotential = 0.0;
        }

        public void ConnectTo(INeuron targetneuron, ISynapse targetsynapse)
        {
            targetneuron.SynapseList.Add(targetsynapse);
            this.network.AddNeuron(targetneuron);
        }

        public void ConnectFrom(INeuron sourceneuron, ISynapse selfsynapse)
        {
            this.SynapseList.Add(selfsynapse);
            sourceneuron.Network.AddNeuron(this);
        }

        public void DisConnect(ISynapse selfsynapse)
        {
            this.SynapseList.Remove(selfsynapse);
        }

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var clone = new MP(this.id,this.axonhilllock.Threshold,lastaxonpotential);
            return clone;
        }

        #endregion
    }
}
