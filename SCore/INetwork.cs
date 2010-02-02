using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SCore
{
    public interface INetwork
    {
        Dictionary<int, INeuron> NeuronList { get; }
        void AddNeuron(INeuron neuron);
        void RemoveNeuron(INeuron neuron);
        double GetLastAxonPotential(int neuronID);
        void Update();
        void Tick();
        double DeltaT { set; get; }
        double DurationT { set; get; }
        double CurrentT { get; }
        void Run();
        void StepRun();
        bool IsRunning { get; }
        bool IsRunOver { get; }
    }
}
