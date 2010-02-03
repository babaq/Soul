using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class ThresholdHeaviside : IHilllock
    {
        private double threshold;

        public ThresholdHeaviside(double threshold)
        {
            this.threshold = threshold;
        }


        #region IHilllock Members

        public double Threshold
        {
            get
            {
                return threshold;
            }
            set
            {
                threshold = value;
            }
        }

        public virtual double Fire(double hilllockpotential)
        {
            if (hilllockpotential-threshold<0)
            {
                return 0.0;
            }
            return 1.0;
        }

        #endregion
    }
}
