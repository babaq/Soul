//--------------------------------------------------------------------------------
// This file is part of The Soul, A Neural Network Simulation System.
//
// Copyright © 2010 LBE Group. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.ComponentModel;
using SSolver;

namespace SCore
{
    public class DCSource : ICurrentSource
    {
        private Guid id;
        private string name;
        private Point3D position;
        private bool on;
        private double amplitude;


        public DCSource(string name, Point3D position, double amplitude, bool on = true)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.position = position;
            this.amplitude = amplitude;
            this.on = on;
        }


        #region ICurrentSource Members

        public Guid ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Point3D Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ON
        {
            get { return on; }
            set { on = value; }
        }

        public double Amplitude
        {
            get { return amplitude; }
            set { amplitude = value; }
        }

        public void InjectTo(INeuron neuron)
        {
            neuron.InjectedFrom(this);
        }

        public virtual double Flow(double currentT)
        {
            if (ON)
            {
                return amplitude;
            }
            else
            {
                return 0.0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public virtual TimeFunc TimeFunc
        {
            get { return null; }
            set { }
        }

        public virtual double[] TimeFuncParams
        {
            get { return null; }
            set { }
        }

        public virtual RNG Random
        {
            get { return null; }
            set { }
        }

        public virtual double[] RandomParams
        {
            get { return null; }
            set { }
        }

        #endregion

    }

    public class ACSource : DCSource
    {
        private TimeFunc tf;
        private double[] tfparams;


        public ACSource(string name, Point3D position, double amplitude, TimeFunc timefunc, bool on = true, params double[] timefuncparams)
            : base(name, position, amplitude, on)
        {
            this.tf = timefunc;
            this.tfparams = timefuncparams;
        }


        public override TimeFunc TimeFunc
        {
            get { return tf; }
            set { tf = value; }
        }

        public override double[] TimeFuncParams
        {
            get { return tfparams; }
            set { tfparams = value; }
        }

        public override double Flow(double currentT)
        {
            if (ON)
            {
                return Amplitude * tf(currentT, tfparams);
            }
            else
            {
                return 0.0;
            }
        }

    }

    public class RCSource : DCSource
    {
        private RNG randomsource;
        private double[] randomparams;


        public RCSource(string name, Point3D position, double amplitude, RNG random, bool on = true, double mean = 0.0, double std = 1.0)
            : base(name, position, amplitude, on)
        {
            randomsource = random;
            randomparams = new[] { mean, std };
        }


        public override RNG Random
        {
            get { return randomsource; }
            set { randomsource = value; }
        }

        public override double[] RandomParams
        {
            get { return randomparams; }
            set { randomparams = value; }
        }

        public override double Flow(double currentT)
        {
            if (ON)
            {
                return Amplitude * randomsource.NextMeanStdDouble(randomparams[0], randomparams[1]);
            }
            else
            {
                return 0.0;
            }
        }

    }

    public class CCSource : DCSource
    {
        private List<double> amplitudes;
        private List<double> times;


        public CCSource(string name, Point3D position, List<double> times, List<double> amplitudes, bool on = true)
            : base(name, position, 0.0, on)
        {
            Times = times;
            Amplitudes = amplitudes;
        }


        public List<double> Amplitudes
        {
            get { return amplitudes; }
            set
            {
                if (value != null)
                {
                    amplitudes = value;
                }
            }
        }

        public List<double> Times
        {
            get { return times; }
            set
            {
                if (value != null)
                {
                    times = value;
                }
            }
        }

        public override double Flow(double currentT)
        {
            double amp = 0.0;
            if (ON)
            {
                var index = Times.FindLastIndex(
                    delegate(double t)
                    {
                        return t < currentT;
                    });
                try
                {
                    amp = Amplitudes[index];
                }
                catch (Exception e)
                {
                }
                return amp;
            }
            else
            {
                return amp;
            }
        }

    }

    public class PCSource : DCSource
    {
        private ICurrentSource basesource;
        private double startT, pulsebeginT, pulseT, pulserestT;
        private int pulses, pcount;
        private bool ispulsing, ispulserested;


        public PCSource(string name, Point3D position, ICurrentSource basesource, double startT = 0.0, double pulseT = 1.0, double pulserestT = 1.0, int pulses = 10, double amplitude = 1.0, bool on = true)
            : base(name, position, amplitude, on)
        {
            this.basesource = basesource;
            this.startT = startT;
            this.pulseT = pulseT;
            this.pulserestT = pulserestT;
            this.pulses = pulses;
            ispulserested = true;
        }


        public ICurrentSource BaseSource
        {
            get { return basesource; }
            set
            {
                basesource = value;
                if (basesource != null)
                {
                    basesource.ON = false;
                }
            }
        }

        public double StartT
        {
            get { return startT; }
            set { startT = value; }
        }

        public double PulseT
        {
            get { return pulseT; }
            set { pulseT = value; }
        }

        public double PulseRestT
        {
            get { return pulserestT; }
            set { pulserestT = value; }
        }

        public int Pulses
        {
            get { return pulses; }
            set { pulses = value; }
        }

        public override double Flow(double currentT)
        {
            double amp = 0.0;
            if (ON)
            {
                if (basesource != null)
                {
                    if (currentT >= startT)
                    {
                        if (pcount < pulses && ispulserested)
                        {
                            basesource.ON = true;
                            pulsebeginT = currentT;
                            ispulsing = true;
                            pcount++;
                        }
                        if (currentT - pulsebeginT >= pulseT)
                        {
                            if (ispulsing)
                            {
                                basesource.ON = false;
                                ispulsing = false;
                            }
                        }
                        if (currentT - pulsebeginT >= pulseT + pulserestT)
                        {
                            ispulserested = true;
                        }
                    }
                    amp = basesource.Flow(currentT);
                }
                else
                {
                    amp = base.Flow(currentT);
                }
            }
            return amp;
        }
    }

}