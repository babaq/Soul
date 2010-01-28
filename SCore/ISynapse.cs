using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public interface ISynapse
    {
        double Weight { set; get; }
        int PreSynapticID { set; get; }
    }
}
