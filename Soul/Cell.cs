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
    public class Cell:ICell
    {
        private INeuron neuron;
        private ModelVisual3D mophology;
        public bool IsPushing { get; set; }


        public Cell(INeuron neuron, ModelVisual3D mophology)
        {
            this.neuron = neuron;
            this.mophology = mophology;

            neuron.Updated += OnUpdated;
            neuron.Hillock.Spike += OnSpike;
            IsPushing = true;



            //var ty = Mophology;
            //var t = ty.Content as Model3DGroup;
            //var m = t.Children[0] as GeometryModel3D;
            //var mm = m.Material as MaterialGroup;
            //var mmm = mm.Children[0] as DiffuseMaterial;
            ////mmm.Brush = new SolidColorBrush((Color)Imager.Convert(neuron.Output, null, Imaging.DefaultImaging(neuron.Type), null));
            //var vv = mmm.Brush as SolidColorBrush;
            //var b = new Binding();
            //b.Source = neuron;
            //b.Path = new PropertyPath("Output");
            //b.Converter = Imager;
            //b.ConverterParameter = Imaging.ImagingDye(neuron.Type);
            //BindingOperations.SetBinding(vv, SolidColorBrush.ColorProperty, b);
            
            //vv.Color = (Color)Imager.Convert(neuron.Output, null, Imaging.DefaultImaging(neuron.Type), null);
        }

        ~Cell()
        {
            neuron.Updated -= OnUpdated;
            neuron.Hillock.Spike -= OnSpike;
        }


        #region ICell Members

        public INeuron Neuron
        {
            get{return neuron;}
            set{neuron = value;}
        }

        public ModelVisual3D Mophology
        {
            get{return mophology;}
            set{mophology = value;}
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
            //if (!this.mophology.Dispatcher.CheckAccess())
            //{
            //    mophology.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
            //    {
            //        n.NotifyPropertyChanged("Output");
            //        //var ty = Mophology;
            //        //var t = ty.Content as Model3DGroup;
            //        //var m = t.Children[0] as GeometryModel3D;
            //        //var mm = m.Material as MaterialGroup;
            //        //var mmm = mm.Children[0] as DiffuseMaterial;
            //        //mmm.Brush = new SolidColorBrush((Color)Imager.Convert(n.Output, null, Imaging.DefaultImaging(n.Type), null));
            //        //var vv = mmm.Brush as SolidColorBrush;
            //        //color = new SolidColorBrush((Color)Imager.Convert(n.Output, null, Imaging.DefaultImaging(n.Type), null));


            //    });
            //}
            //else
            //{

            //}
        }

        void OnSpike(object sender, EventArgs e)
        {
        }

        #endregion

    }
}
