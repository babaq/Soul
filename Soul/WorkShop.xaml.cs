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
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCore;
using SSolver;

namespace Soul
{
    /// <summary>
    /// Interaction logic for WorkShop.xaml
    /// </summary>
    public partial class WorkShop : UserControl
    {
        public bool IsReportProgress { get; set; }
        public bool IsImaging { get; set; }
        public bool IsRunning { get { return simulator.IsRunning; } }
        public bool IsPaused { get { return simulator.IsPaused; } }
        private ISimulation simulator;
        public ISimulation Simulator
        {
            get { return simulator; }
            set
            {
                if(value!=null)
                {
                    if(simulator!=null)
                    {
                        simulator.RunOver -= OnRunOver;
                        simulator.Steped -= OnSteped;
                    }
                    simulator = value;
                    simulator.RunOver += OnRunOver;
                    simulator.Steped += OnSteped;
                }
            }
        }
        public INetwork Network { get; set; }
        public ISolver Solver { get; set; }
        public IRecord Recorder { get; set; }

        public ActionType ActionType { get; set; }
        public Point PreviousMousePoint { get; set; }
        public Vector3D PreviousMouseVector { get; set; }
        public TranslateTransform3D TranslateTransform { get; set; }
        public RotateTransform3D RotateTransform { get; set; }
        public ScaleTransform3D ScaleTransform { get; set; }


        public WorkShop()
        {
            InitializeComponent();


            Network = new Network();
            Solver = new ODESolver();
            Recorder = new Recorder(Simulator, RecordType.None, "Soul");
            Simulator = new Simulator(0.05, 2000, Network, Solver, Recorder);
            IsReportProgress = true;
            IsImaging = true;


            var transformGroup = new Transform3DGroup();
            TranslateTransform = new TranslateTransform3D();
            RotateTransform = new RotateTransform3D {Rotation = new QuaternionRotation3D()};
            ScaleTransform = new ScaleTransform3D();
            transformGroup.Children.Add(TranslateTransform);
            transformGroup.Children.Add(RotateTransform);
            transformGroup.Children.Add(ScaleTransform);
            ModelVisual.Transform = transformGroup;

            ActionType = ActionType.None;
            MouseLeftButtonDown += WorkShop_MouseLeftButtonDown;
            MouseLeftButtonUp += WorkShop_MouseLeftButtonUp;
            MouseRightButtonDown += WorkShop_MouseRightButtonDown;
            MouseRightButtonUp += WorkShop_MouseRightButtonUp;
            MouseMove += WorkShop_MouseMove;
            MouseWheel += WorkShop_MouseWheel;

            init();
        }


        void init()
        {

            


        var mp0 = new MP(0.3, 1.0) { ParentNetwork = Network };
            var mp1 = new MP(0.5, 0.0);
            mp0.ProjectTo(mp1, new WeightSynapse(mp0, 0.6));
            mp0.ProjectedFrom(mp1, new WeightSynapse(mp1, 0.8));
            mp0.ProjectTo(mp0, new WeightSynapse(mp0, 0.2));
            mp1.ProjectTo(mp1, new WeightSynapse(mp1, 0.4));

            var stemcell = new StemCell();
            var cell0 = stemcell.Develop(mp0);
            var cell1 = stemcell.Develop(mp1);
            cell1.Mophology.Transform = new TranslateTransform3D(2, 0, 0);

            ModelVisual.Children.Add(cell0.Mophology);
            ModelVisual.Children.Add(cell1.Mophology);



            var b = new Binding { Source = simulator, Path = new PropertyPath("Progress"), Mode = BindingMode.OneWay };
            ProgressBar.SetBinding(RangeBase.ValueProperty, b);

            //ModelGroup.Children.Concat(cell1.Mophology);
            //ModelGroup.Children.Add(cell1.Mophology[0]);
            //ModelGroup.Children.Concat(cell1.Mophology);

            //var mg = new MaterialGroup();
            //mg.Children.Add(new DiffuseMaterial(Brushes.DarkGoldenrod));
            //mg.Children.Add(new SpecularMaterial(Brushes.White, 1000.0));
            //mg.Children.Add(new EmissiveMaterial(Brushes.Red));
            ////ModelGroup.Children.Add(new GeometryModel3D(Ellipsoid.GetEllipsoid(1.5, 1.3, 1, 5), mg));
            //var t = new GeometryModel3D(Ellipsoid.Mesh, mg);
            //t.Transform = new TranslateTransform3D(3, 0, 0);
            //var tt = t.Clone();
            //tt.Transform = new TranslateTransform3D(-3, 0, 0);
            //var tmg = mg.Clone();
            //tmg.Children.Add(new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(@"E:\Programs\Soul\Soul\Soul.png")))));
            //var t1 = new GeometryModel3D(SuperQuadrics.GetSuperQuadric(1, 1, 1, 2.5, 0.001, 1, 36, 18), tmg);
            //t1.Transform = new TranslateTransform3D(-3, 0, 0);
            //var t2 = new GeometryModel3D(Ellipsoid.Octahedron(1, 1, 1), mg);
            //t2.Transform = new TranslateTransform3D(0, 0, -3);
            //var t3 = new GeometryModel3D(Cylinder.Mesh, mg);
            //t3.Transform = new TranslateTransform3D(0, 0, 3);
            //var t4 = new GeometryModel3D(Cylinder.GetCylinder(0.3, 2, 1, 16), mg);
            //t4.Transform = new TranslateTransform3D(0, 3, 0);
            //ModelGroup.Children.Add(t);
            //ModelGroup.Children.Add(tt);
            //ModelGroup.Children.Add(t1);
            //ModelGroup.Children.Add(t2);
            //ModelGroup.Children.Add(t3);
            //ModelGroup.Children.Add(t4);
        }




        void OnRunOver(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) delegate
                                                                               {
                                                                                   ProgressBar.Visibility = Visibility.Hidden;
                                                                                   CurrentTBox.Text = simulator.CurrentT.ToString("F2");
                                                                               });
            }
            else
            {
                ProgressBar.Visibility = Visibility.Hidden;
                CurrentTBox.Text = simulator.CurrentT.ToString("F2");
            }
        }

        void OnSteped(object sender, EventArgs e)
        {
            if (IsReportProgress)
            {
                var stor = sender as ISimulation;
                if (stor != null)
                {
                    if (!Dispatcher.CheckAccess())
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                                                                                  {
                                                                                      ProgressBar.Value = stor.Progress;
                                                                                  });
                    }
                    else
                    {
                        ProgressBar.Value = stor.Progress;
                    }
                }
            }

            //var stor = sender as ISimulation;
            //if (stor != null)
            //{
            //    Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)delegate
            //                                                             {
            //                                                                 ProgressBar.Value = stor.Progress;
            //                                                             });
            //}

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
                }
                else
                {
                    ActionType = ActionType.Rotate;
                }
            }
        }

        void WorkShop_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ActionType == ActionType.Rotate || ActionType == ActionType.Translate)
            {
                ActionType = ActionType.None;
                ReleaseMouseCapture();
            }
        }

        void WorkShop_MouseMove(object sender, MouseEventArgs e)
        {
            if (ActionType == ActionType.Rotate)
            {
                var currentMousePoint = e.GetPosition(this);
                var currentMouseVector = ProjectToTrackBall(currentMousePoint);

                Vector3D axis = Vector3D.CrossProduct(PreviousMouseVector, currentMouseVector);
                double angle = Vector3D.AngleBetween(PreviousMouseVector, currentMouseVector);

                if (axis.LengthSquared > 0)
                {
                    var rotation = RotateTransform.Rotation as QuaternionRotation3D;
                    if (rotation != null)
                    {
                        rotation.Quaternion = new Quaternion(axis, angle) * rotation.Quaternion;
                        PreviousMouseVector = currentMouseVector;
                    }
                }
            }
            else if (ActionType == ActionType.Translate)
            {
                var currentMousePoint = e.GetPosition(this);

                double multiplier = 2 * (Math.Tan(Math.PI / 8) / ScaleTransform.ScaleX) * 4;

                double deltaX = ((currentMousePoint.X - PreviousMousePoint.X) / Space.ActualWidth) * multiplier;
                double deltaY = -((currentMousePoint.Y - PreviousMousePoint.Y) / Space.ActualHeight) * multiplier;

                var deltaVector = new Vector3D(deltaX, deltaY, 0);
                var rotation = (QuaternionRotation3D)RotateTransform.Rotation;
                var matrix = new Matrix3D();
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
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                }
                else
                {
                    ActionType = ActionType.Zoom;

                    double multiplier = Math.Exp(e.Delta / 1000.0);
                    double scale = Math.Max(0.01, Math.Min(200, ScaleTransform.ScaleX * multiplier));

                    ScaleTransform.ScaleX = scale;
                    ScaleTransform.ScaleY = scale;
                    ScaleTransform.ScaleZ = scale;

                    ActionType = ActionType.None;
                }
            }
        }

        private Vector3D ProjectToTrackBall(Point point)
        {
            double x = 2 * (point.X / Space.ActualWidth) - 1;
            double y = 1 - 2 * (point.Y / Space.ActualHeight);

            double zSquared = 1 - x * x - y * y;
            double z = zSquared > 0 ? Math.Sqrt(zSquared) : 0;

            return new Vector3D(x, y, z);
        }

        public void Run()
        {
            simulator.DeltaT = Convert.ToDouble(DeltaTBox.Text);
            simulator.DurationT = Convert.ToDouble(DurationTBox.Text);
            simulator.CurrentT = Convert.ToDouble(CurrentTBox.Text);
            simulator.Recorder.RecordType = (RecordType)RecordTypeBox.SelectedIndex;
            simulator.Run();
            if (IsReportProgress)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }
        }

        public void Stop()
        {
            StopPushing();
            simulator.Stop();
        }

        public void Pause()
        {
            simulator.Pause();
        }

        public void Resume()
        {
            simulator.Resume();
        }

        public void Step()
        {
            simulator.Step(Convert.ToDouble(DeltaTBox.Text));
            CurrentTBox.Text = simulator.CurrentT.ToString("F2");
        }

        public void StopPushing()
        {
            IsReportProgress = false;
            Thread.Sleep(200);
        }

    }
}
