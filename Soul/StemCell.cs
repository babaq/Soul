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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using SCore;

namespace Soul
{
    public class StemCell: IStemCell
    {
        public static Imaging Imager =new Imaging();


        #region IStemCell Members

        public ICell Develop(NeuronType neurontype)
        {
            INeuron neuron = null;
            switch (neurontype)
            {
                default:
                    neuron = new MP(0.5, 1.0);
                    break;
            }

            return Develop(neuron);
        }

        public ICell Develop(INeuron neuron)
        {
            if(neuron==null)
            {
                return null;
            }

            var cell = new Cell(neuron, DevelopMophology(neuron));
            return cell;
        }

        public ICellNet Develop(INetwork network)
        {
            if(network==null)
            {
                return null;
            }

            var cellnet = new CellNet(network);
            for (int i = 0; i < cellnet.Network.Neurons.Count; i++)
            {
                var cell = Develop(cellnet.Network.Neurons.ElementAt(i).Value);
                cellnet.Cells.Add(cell.Neuron.ID,cell);
                cellnet.Mophology.Children.Add(cell.Mophology);
            }
            for (int i = 0; i <cellnet.Network.ChildNetworks.Count; i++)
            {
                var childcellnet = Develop(cellnet.Network.ChildNetworks.ElementAt(i).Value);
                cellnet.ChildCellNet.Add(childcellnet.Network.ID,childcellnet);
                cellnet.Mophology.Children.Add(childcellnet.Mophology);
            }
            return cellnet;
        }

        public ModelVisual3D DevelopMophology(INeuron neuron)
        {
            var mophology = new Model3DGroup();
            DevelopSomaMophology(neuron,mophology);
            DevelopDendriteMophology(neuron,mophology);
            return new ModelVisual3D {Content = mophology};
        }

        void DevelopSomaMophology(INeuron neuron, Model3DGroup mophology)
        {
            mophology.Children.Add(new GeometryModel3D(DevelopSomaGeometry(neuron), DevelopSomaMaterial(neuron)));
        }

        void DevelopDendriteMophology(INeuron neuron, Model3DGroup mophology)
        {
            
        }

        MeshGeometry3D DevelopSomaGeometry(INeuron neuron)
        {
            MeshGeometry3D mesh=null;
            switch (neuron.Type)
            {
                case NeuronType.MP:
                case NeuronType.LI:
                case NeuronType.IF:
                case NeuronType.HH:
                    mesh = Ellipsoid.Mesh;
                    break;
            }
            return mesh;
        }

        MaterialGroup DevelopSomaMaterial(INeuron neuron)
        {
            MaterialGroup mg = new MaterialGroup();
            switch (neuron.Type)
            {
                case NeuronType.LI:
                case NeuronType.IF:
                case NeuronType.HH:
                case NeuronType.MP:
                    var brush = new SolidColorBrush();

                    var binding = new Binding()
                                      {
                                          Source = neuron,
                                          Path = new PropertyPath("Output"),
                                          Converter = Imager,
                                          ConverterParameter = Imaging.ImagingDye(neuron.Type),
                                          Mode = BindingMode.OneWay
                                      };
                    BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, binding);

                    mg.Children.Add(new DiffuseMaterial(brush));
                    break;
            }
            return mg;
        }

        #endregion

    }
}
