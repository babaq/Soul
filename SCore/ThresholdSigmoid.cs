using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class ThresholdSigmoid : ThresholdHeaviside
    {
        public ThresholdSigmoid(double threshold) : base(threshold)
        {
        }

        public double Fire(double hilllockpotential)
        {
            var output = 1.0/(1.0 + Math.Exp(-(hilllockpotential - Threshold)));
            return output;
        }

    }
}
