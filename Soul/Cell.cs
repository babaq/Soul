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
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using SCore;

namespace Soul
{
    public class Cell : ICell
    {
        private INeuron neuron;
        private ModelVisual3D mophology;
        public bool IsPushing { get; set; }
        public Point3D Position
        {
            get { return neuron.Position; }
            set
            {
                neuron.Position = value;
                Translate.OffsetX = value.X;
                Translate.OffsetY = value.Y;
                Translate.OffsetZ = value.Z;
            }
        }
        public RotateTransform3D Rotate { get; set; }
        public TranslateTransform3D Translate { get; set; }
        public ScaleTransform3D Scale { get; set; }


        public Cell(INeuron neuron, ModelVisual3D mophology)
        {
            this.neuron = neuron;
            this.mophology = mophology;
            neuron.Updated += OnUpdated;
            neuron.Hillock.Spike += OnSpike;
            IsPushing = true;

            var transforms = new Transform3DGroup();
            Rotate = new RotateTransform3D(new QuaternionRotation3D());
            Translate = new TranslateTransform3D(neuron.Position.X, neuron.Position.Y, neuron.Position.Z);
            Scale = new ScaleTransform3D();
            transforms.Children.Add(Rotate);
            transforms.Children.Add(Translate);
            transforms.Children.Add(Scale);
            Mophology.Transform = transforms;
        }


        #region ICell Members

        public INeuron Neuron
        {
            get { return neuron; }
            set { neuron = value; }
        }

        public ModelVisual3D Mophology
        {
            get { return mophology; }
            set { mophology = value; }
        }

        void OnUpdated(object sender, EventArgs e)
        {
            if (IsPushing)
            {
                var neuron = sender as INeuron;
                if (neuron != null)
                {
                    neuron.NotifyPropertyChanged("Output");
                }
            }
        }

        void OnSpike(object sender, EventArgs e)
        {
        }

        #endregion

    }
}
