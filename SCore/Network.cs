using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class Network : INetwork
    {
        private Dictionary<int, INeuron> neuronlist;
        private double deltaT;
        private double durationT;
        private double currentT;

        public Network(double deltatime,double durationtime)
        {
            neuronlist = new Dictionary<int, INeuron>();
            deltaT = deltatime;
            durationT = durationtime;
        }


        #region INetwork Members

        public Dictionary<int, INeuron> NeuronList
        {
            get { return neuronlist; }
        }

        public void AddNeuron(INeuron neuron)
        {
            neuronlist.Add(neuron.ID,neuron);
            neuron.Network = this;
        }

        public void RemoveNeuron(INeuron neuron)
        {
            neuronlist.Remove(neuron.ID);
            neuron.Network = null;
        }

        public double GetLastAxonPotential(int neuronID)
        {
            return neuronlist[neuronID].LastAxonPotential;
        }

        public void Update()
        {
            for (int i = 0; i < neuronlist.Count; i++)
            {
                neuronlist.ElementAt(i).Value.Update();
            }
        }

        public void Tick()
        {
            for (int i = 0; i < neuronlist.Count; i++)
            {
                neuronlist.ElementAt(i).Value.Tick();
            }
        }

        public double DeltaT
        {
            get { return deltaT; }
            set { deltaT = value; }
        }

        public double DurationT
        {
            get { return durationT; }
            set { durationT = value; }
        }

        public double CurrentT
        {
            get { return currentT; }
        }

        public void Run()
        {
            do
            {
                Update();
                Tick();
                currentT += deltaT;
            } while (currentT>durationT);
        }

        #endregion
    }
}
