using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public interface INeuron
    {
        int ID { set; get; }
        List<ISynapse> SynapseList { get; }
        IHilllock AxonHilllock { set; get; }
        double AxonPotential { get; }
        double LastAxonPotential { get; }
        void Update();
        void Tick();
        INetwork Network { get; set; }
    }
}
