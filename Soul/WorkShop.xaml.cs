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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soul
{
    /// <summary>
    /// Interaction logic for WorkShop.xaml
    /// </summary>
    public partial class WorkShop : UserControl
    {
        public ActionType ActionType { get; set; }
        public Point PreviousMousePoint { get; set; }
        public Vector3D PreviousMouseVector { get; set; }
        public TranslateTransform3D TranslateTransform { get; set; }
        public RotateTransform3D RotateTransform { get; set; }
        public ScaleTransform3D ScaleTransform { get; set; }


        public WorkShop()
        {
            InitializeComponent();


            var transformGroup = new Transform3DGroup();
            ModelVisual.Transform = transformGroup;

            TranslateTransform = new TranslateTransform3D();
            transformGroup.Children.Add(TranslateTransform);

            ScaleTransform = new ScaleTransform3D();
            transformGroup.Children.Add(ScaleTransform);

            RotateTransform = new RotateTransform3D {Rotation = new QuaternionRotation3D()};
            transformGroup.Children.Add(RotateTransform);


            ActionType = ActionType.None;
            MouseLeftButtonDown += WorkShop_MouseLeftButtonDown;
            MouseLeftButtonUp += WorkShop_MouseLeftButtonUp;
            MouseRightButtonDown += WorkShop_MouseRightButtonDown;
            MouseRightButtonUp += WorkShop_MouseRightButtonUp;
            MouseMove += WorkShop_MouseMove;
            MouseWheel += WorkShop_MouseWheel;
            

            var mg = new MaterialGroup();
            mg.Children.Add(new DiffuseMaterial(Brushes.DarkGoldenrod));
            mg.Children.Add(new SpecularMaterial(Brushes.White, 1000.0));
            //mg.Children.Add(new EmissiveMaterial(Brushes.Red));
            //var t = Ellipsoid.GetEllipsoid(2);
            //ModelGroup.Children.Add(new GeometryModel3D(Ellipsoid.GetEllipsoid(1.5, 1.3, 1, 5), mg));
            var t = new GeometryModel3D(Ellipsoid.GetEllipsoid(1,1,1,3), mg);
            t.Transform = new TranslateTransform3D(3, 0, 0);
            var t1 = new GeometryModel3D(SuperQuadrics.GetSuperQuadric(1,2,1,1,1,1,36,18), new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(@"E:\Programs\Soul\Soul\Soul.png")))));
            t1.Transform = new TranslateTransform3D(-3, 0, 0);
            var t2 = new GeometryModel3D(Ellipsoid.Octahedron(1,1,1), mg);
            t2.Transform = new TranslateTransform3D(0, 0, -3);
            var t3 = new GeometryModel3D(Cylinder.Mesh, mg);
            t3.Transform = new TranslateTransform3D(0, 0, 3);
            var t4 = new GeometryModel3D(Cylinder.GetCylinder(0.3,2,1,16), mg);
            t4.Transform = new TranslateTransform3D(0, 3, 0);
            ModelGroup.Children.Add(t);
            ModelGroup.Children.Add(t1);
            ModelGroup.Children.Add(t2);
            ModelGroup.Children.Add(t3);
            ModelGroup.Children.Add(t4);
        }

        void WorkShop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        void WorkShop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        void WorkShop_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ActionType == ActionType.None)
            {
                CaptureMouse();

                PreviousMousePoint = e.GetPosition(this);
                PreviousMouseVector = ProjectToTrackBall(PreviousMousePoint);

                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    ActionType = ActionType.Translate;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    //this.pdbViewer.ActionType = PdbActionType.Deselect;
                }
                else
                {
                    ActionType = ActionType.Rotate;
                }
            }
        }

        void WorkShop_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ActionType == ActionType.Rotate||ActionType==ActionType.Translate )
            {
                ActionType = ActionType.None;
                ReleaseMouseCapture();
            }
        }

        void WorkShop_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePoint = e.GetPosition(this);

            if (ActionType == ActionType.Rotate)
            {
                Vector3D currentMouseVector = ProjectToTrackBall(currentMousePoint);

                Vector3D axis = Vector3D.CrossProduct(
                    PreviousMouseVector, currentMouseVector);
                double angle = 2 * Vector3D.AngleBetween(
                    PreviousMouseVector, currentMouseVector);

                if (axis.LengthSquared > 0)
                {
                    QuaternionRotation3D rotation =
                        RotateTransform.Rotation as QuaternionRotation3D;

                    if (rotation != null)
                    {
                        rotation.Quaternion = new Quaternion(axis, angle) *
                            rotation.Quaternion;
                    }
                }

                PreviousMouseVector = currentMouseVector;
            }
            else if (ActionType==ActionType.Translate)
            {
                double multiplier = 2 * Math.Tan(Math.PI / 8) * 4 /
                    ScaleTransform.ScaleX;

                double deltaX = (currentMousePoint.X - PreviousMousePoint.X) /
                    Space.ActualWidth * multiplier;
                double deltaY = -(currentMousePoint.Y - PreviousMousePoint.Y) /
                    Space.ActualHeight * multiplier;

                Vector3D deltaVector = new Vector3D(deltaX, deltaY, 0);

                QuaternionRotation3D rotation =
                    (QuaternionRotation3D)RotateTransform.Rotation;

                Matrix3D matrix = new Matrix3D();
                matrix.Rotate(rotation.Quaternion);
                matrix.Invert();

                deltaVector = matrix.Transform(deltaVector);

                TranslateTransform.OffsetX += deltaVector.X;
                TranslateTransform.OffsetY += deltaVector.Y;
                TranslateTransform.OffsetZ += deltaVector.Z;

                PreviousMousePoint = currentMousePoint;
            }
        }

        void WorkShop_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ActionType == ActionType.None)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    //this.pdbViewer.ActionType = PdbActionType.Clip;

                    //this.clip = Math.Max(0, Math.Min(1, this.clip + (double)e.Delta / 1200));
                    //this.UpdateClipping();

                    //this.pdbViewer.ActionType = PdbActionType.None;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    //this.pdbViewer.ActionType = PdbActionType.Slab;

                    //this.slab = Math.Max(-1, Math.Min(1, this.slab + (double)e.Delta / 600));
                    //this.UpdateClipping();

                    //this.pdbViewer.ActionType = PdbActionType.None;
                }
                else
                {
                    ActionType = ActionType.Zoom;

                    double multiplier = Math.Exp((double)e.Delta / -1000);
                    double scale = Math.Max(0.01, Math.Min(200,
                        ScaleTransform.ScaleX * multiplier));

                   ScaleTransform.ScaleX = scale;
                    ScaleTransform.ScaleY = scale;
                    ScaleTransform.ScaleZ = scale;

                    //UpdateClipping();

                    ActionType = ActionType.None;
                }
            }
        }

        private Vector3D ProjectToTrackBall(Point point)
        {
            double x = 2 * point.X / Space.ActualWidth - 1;
            double y = 1 - 2 * point.Y / Space.ActualHeight;

            double zSquared = 1 - x * x - y * y;
            double z = zSquared > 0 ? Math.Sqrt(zSquared) : 0;

            return new Vector3D(x, y, z);
        }

        private void UpdateClipping()
        {
            double clipRadius = 1.75 * 1 * ScaleTransform.ScaleX;
            double clipOffset = 1.75 * (1 - 1) * 0 * ScaleTransform.ScaleX;

            Camera.NearPlaneDistance =
                Math.Max(0.125, 4 + clipOffset - clipRadius);
            Camera.FarPlaneDistance = 4 + clipOffset + clipRadius;
        }
    }
}
