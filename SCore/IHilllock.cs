using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public interface IHilllock
    {
        double Threshold { set; get; }
        double Fire(double hilllockpotential);
    }
}
