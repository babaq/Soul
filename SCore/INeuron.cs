using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public interface INeuron : ICloneable
    {
        int ID { set; get; }
        string Name { set; get; }
        List<ISynapse> SynapseList { get; }
        IHilllock AxonHilllock { set; get; }
        double AxonPotential { get; }
        double LastAxonPotential { get; }
        void Update();
        void Tick();
        void ConnectTo(INeuron targetneuron,ISynapse targetsynapse);
        void ConnectFrom(INeuron sourceneuron,ISynapse selfsynapse);
        void DisConnect(ISynapse selfsynapse);
        INetwork Network { get; set; }
    }
}
