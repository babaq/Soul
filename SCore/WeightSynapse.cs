using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCore
{
    public class WeightSynapse : ISynapse
    {
        private double weight;
        private int presynapticID;

        public WeightSynapse(int presynapticID,double weight)
        {
            this.presynapticID = presynapticID;
            this.weight = weight;
        }


        #region ISynapse Members

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public int PreSynapticID
        {
            get { return presynapticID; }
            set { presynapticID = value; }
        }

        #endregion
    }
}
