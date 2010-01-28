using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class MP : INeuron
    {
        int id;
        List<ISynapse> weightsynapselist;
        IHilllock axonhilllock;
        double axonpotential;
        double lastaxonpotential;
        INetwork network;

        public MP(int neuronID, double threshold)
        {
            id = neuronID;
            weightsynapselist = new List<ISynapse>();
            axonhilllock = new ThresholdHeaviside(threshold);
            axonpotential = 0.0;
            lastaxonpotential = 0.0;
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

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
        }

        #endregion
    }
}
