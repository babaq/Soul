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
using SCore;
using System.Windows;
using System.Windows.Data;

namespace Soul
{
    public class CellNet : DependencyObject, ICellNet
    {
        private INetwork network;
        private ModelVisual3D mophology;
        private Dictionary<Guid, ICell> cells;
        private Dictionary<Guid, ICellNet> childcellnet;
        private bool ispushing;
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point3D), typeof(CellNet), new PropertyMetadata(OnPositionChanged));
        public Point3D Position
        {
            get { return network.Position; }
            set { network.Position = value; }
        }
        public RotateTransform3D Rotate { get; set; }
        public TranslateTransform3D Translate { get; set; }
        public ScaleTransform3D Scale { get; set; }
        private static void OnPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CellNet thiscellnet = obj as CellNet;
            if (thiscellnet.IsPushing)
            {
                var p = (Point3D)args.NewValue;
                thiscellnet.Translate.OffsetX = p.X;
                thiscellnet.Translate.OffsetY = p.Y;
                thiscellnet.Translate.OffsetZ = p.Z;
            }
        }


        public CellNet(INetwork network)
            : this(network, new ModelVisual3D(), new Dictionary<Guid, ICell>(), new Dictionary<Guid, ICellNet>())
        {
        }

        public CellNet(INetwork network, ModelVisual3D mophology, Dictionary<Guid, ICell> cells, Dictionary<Guid, ICellNet> childcellnet)
        {
            this.network = network;
            this.mophology = mophology;
            this.cells = cells;
            this.childcellnet = childcellnet;
            IsPushing = true;

            var transforms = new Transform3DGroup();
            Rotate = new RotateTransform3D(new QuaternionRotation3D());
            Translate = new TranslateTransform3D(network.Position.X, network.Position.Y, network.Position.Z);
            Scale = new ScaleTransform3D();
            transforms.Children.Add(Rotate);
            transforms.Children.Add(Translate);
            transforms.Children.Add(Scale);
            Mophology.Transform = transforms;

            var binding = new Binding()
            {
                Source = network,
                Path = new PropertyPath("Position"),
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, CellNet.PositionProperty, binding);
        }


        #region ICellNet Members

        public INetwork Network
        {
            get { return network; }
            set { network = value; }
        }

        public ModelVisual3D Mophology
        {
            get { return mophology; }
            set { mophology = value; }
        }

        public Dictionary<Guid, ICell> Cells
        {
            get { return cells; }
        }

        public Dictionary<Guid, ICellNet> ChildCellNet
        {
            get { return childcellnet; }
        }

        public bool IsPushing
        {
            get { return ispushing; }
            set
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    cells.ElementAt(i).Value.IsPushing = value;
                }
                for (int i = 0; i < childcellnet.Count; i++)
                {
                    childcellnet.ElementAt(i).Value.IsPushing = value;
                }
                ispushing = value;
            }
        }

        #endregion
    }
}
