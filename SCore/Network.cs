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
        private bool isrunning;
        private bool isrunover;

        public Network(double deltatime,double durationtime)
        {
            neuronlist = new Dictionary<int, INeuron>();
            deltaT = deltatime;
            durationT = durationtime;
            isrunning = false;
            isrunover = false;
        }


        #region INetwork Members

        public Dictionary<int, INeuron> NeuronList
        {
            get { return neuronlist; }
        }

        public void AddNeuron(INeuron neuron)
        {
            if(!neuronlist.ContainsKey(neuron.ID))
            {
                neuronlist.Add(neuron.ID, neuron);
            }
            neuron.Network = this;
        }

        public void RemoveNeuron(INeuron neuron)
        {
            if(neuronlist.ContainsKey(neuron.ID))
            {
                neuronlist.Remove(neuron.ID);
            }
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
            isrunning = true;
            do
            {
                Update();
                Tick();
                currentT += deltaT;
            } while (currentT<=durationT);
            isrunning = false;
            isrunover = true;
        }

        public void StepRun()
        {
            if(currentT<=durationT)
            {
                isrunning = true;
                Update();
                Tick();
                currentT += deltaT;
            }
            else
            {
                isrunning = false;
                isrunover = true;
            }
        }

        public bool IsRunning
        {
            get { return isrunning; }
        }

        public bool IsRunOver
        {
            get { return isrunover; }
        }

        #endregion
    }
}
