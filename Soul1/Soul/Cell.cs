//--------------------------------------------------------------------------------
// This file is part of the Soul - Neural Network Simulation System.
//
// Copyright © 2010 Alex-Joyce. All rights reserved.
//
// For information about this application and licensing, go to http://soul.codeplex.com.
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
using SoulCore;

namespace Soul
{
    public class Cell : DependencyObject, ICell
    {
        private INeuron neuron;
        private ModelVisual3D mophology;
        public bool IsPushing { get; set; }
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point3D), typeof(Cell), new PropertyMetadata(OnPositionChanged));
        public Point3D Position
        {
            get { return (Point3D)GetValue(PositionProperty); }
            set { neuron.Position = value; }
        }
        public RotateTransform3D Rotate { get; set; }
        public TranslateTransform3D Translate { get; set; }
        public ScaleTransform3D Scale { get; set; }
        private Imaging imager;
        private static void OnPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Cell thiscell = obj as Cell;
            if (thiscell.IsPushing)
            {
                var p = (Point3D)args.NewValue;
                thiscell.Translate.OffsetX = p.X;
                thiscell.Translate.OffsetY = p.Y;
                thiscell.Translate.OffsetZ = p.Z;
            }
        }


        public Cell(INeuron neuron, Tuple<ModelVisual3D, Imaging> mophology_imager)
            : this(neuron, mophology_imager.Item1, mophology_imager.Item2)
        {
        }

        public Cell(INeuron neuron, ModelVisual3D mophology, Imaging imager)
        {
            this.neuron = neuron;
            this.mophology = mophology;
            this.imager = imager;
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

            var binding = new Binding()
            {
                Source = neuron,
                Path = new PropertyPath("Position"),
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, Cell.PositionProperty, binding);
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

        public Imaging Imager
        {
            get { return imager; }
            set { imager = value; }
        }

        #endregion

    }
}
