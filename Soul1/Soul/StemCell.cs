﻿//--------------------------------------------------------------------------------
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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using SoulCore;

namespace Soul
{
    public class StemCell : IStemCell
    {
        public static Imaging Imager = new Imaging();


        #region IStemCell Members

        public ICell Develop(NeuronType neurontype, double threshold = -50, double initpotential = -60, double restpotential = -60, double r = 5, double c = 2, double resetpotential = -60, double refractoryperiod = 1)
        {
            INeuron neuron = null;
            switch (neurontype)
            {
                case NeuronType.MP:
                    neuron = new MP(threshold, initpotential);
                    break;
                case NeuronType.LI:
                    neuron = new LI(threshold, initpotential, r, c, restpotential);
                    break;
                case NeuronType.IF:
                    neuron = new IF(threshold, resetpotential, refractoryperiod, initpotential, r, c, restpotential);
                    break;
            }

            return Develop(neuron);
        }

        public ICell Develop(INeuron neuron)
        {
            if (neuron == null)
            {
                return null;
            }

            var cell = new Cell(neuron, DevelopMophology(neuron));
            return cell;
        }

        public ICellNet Develop(INetwork network)
        {
            if (network == null)
            {
                return null;
            }

            var cellnet = new CellNet(network);
            for (int i = 0; i < cellnet.Network.Neurons.Count; i++)
            {
                var cell = Develop(cellnet.Network.Neurons.ElementAt(i).Value);
                cellnet.Cells.Add(cell.Neuron.ID, cell);
                cellnet.Mophology.Children.Add(cell.Mophology);
            }
            for (int i = 0; i < cellnet.Network.ChildNetworks.Count; i++)
            {
                var childcellnet = Develop(cellnet.Network.ChildNetworks.ElementAt(i).Value);
                cellnet.ChildCellNet.Add(childcellnet.Network.ID, childcellnet);
                cellnet.Mophology.Children.Add(childcellnet.Mophology);
            }
            return cellnet;
        }

        public Tuple<ModelVisual3D, Imaging> DevelopMophology(INeuron neuron)
        {
            var mophology = new Model3DGroup();
            var imager = Imager;
            DevelopSomaMophology(neuron, mophology, imager);
            DevelopDendriteMophology(neuron, mophology, imager);
            DevelopAxonMophology(neuron, mophology, imager);
            return new Tuple<ModelVisual3D, Imaging>(new ModelVisual3D { Content = mophology }, imager);
        }

        void DevelopSomaMophology(INeuron neuron, Model3DGroup mophology, Imaging imager)
        {
            mophology.Children.Add(new GeometryModel3D(DevelopSomaGeometry(neuron), DevelopSomaMaterial(neuron, imager)));
        }

        void DevelopDendriteMophology(INeuron neuron, Model3DGroup mophology, Imaging imager)
        {
        }

        void DevelopAxonMophology(INeuron neuron, Model3DGroup mophology, Imaging imager)
        {
        }

        MeshGeometry3D DevelopSomaGeometry(INeuron neuron)
        {
            MeshGeometry3D mesh = null;
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

        MaterialGroup DevelopSomaMaterial(INeuron neuron, Imaging imager)
        {
            MaterialGroup mg = new MaterialGroup();
            switch (neuron.Type)
            {
                case NeuronType.LI:
                case NeuronType.IF:
                case NeuronType.HH:
                case NeuronType.MP:
                    var brush = new SolidColorBrush();
                    imager.Dye = Imaging.Dyes(neuron.Type);

                    var binding = new Binding()
                                      {
                                          Source = neuron,
                                          Path = new PropertyPath("Output"),
                                          Converter = imager,
                                          ConverterParameter = neuron.Hillock.Type,
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
