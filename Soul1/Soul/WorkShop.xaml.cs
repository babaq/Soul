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
using SoulCore;
using SoulSolver;
using System.Windows.Media.Animation;

namespace Soul
{
    /// <summary>
    /// Interaction logic for WorkShop.xaml
    /// </summary>
    public partial class WorkShop : UserControl
    {
        public bool IsReportProgress { get; set; }
        public bool IsImaging
        {
            get { return CellNet.IsPushing; }
            set { CellNet.IsPushing = value; }
        }
        public bool IsRunning { get { return simulator.IsRunning; } }
        public bool IsPaused { get { return simulator.IsPaused; } }
        private ISimulation simulator;
        public ISimulation Simulator
        {
            get { return simulator; }
            set
            {
                if (value != null)
                {
                    if (simulator != null)
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
        public ICellNet CellNet { get; set; }
        public static readonly IStemCell StemCell = new StemCell();

        public ActionType ActionType { get; set; }
        public Point PreviousMousePoint { get; set; }
        public Vector3D PreviousMouseVector { get; set; }
        public TranslateTransform3D TranslateTransform { get; set; }
        public RotateTransform3D RotateTransform { get; set; }
        public ScaleTransform3D ScaleTransform { get; set; }


        public WorkShop()
        {
            InitializeComponent();

            var network = new Network();
            var solver = new ODESolver();
            var recorder = new Recorder(Simulator, RecordType.None, "Soul");
            Simulator = new Simulator(0.01, 50, network, solver, recorder);
            IsReportProgress = true;
            CellNet = new CellNet(network);
            IsImaging = true;

            var transformGroup = new Transform3DGroup();
            TranslateTransform = new TranslateTransform3D();
            RotateTransform = new RotateTransform3D(new QuaternionRotation3D());
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



            var n = new LI(-50, -48, 5, 2, -55);
            var net0 = Proliferation.Division(n, new Point3D(1, 10, 10), "InitPotential", new Randomizer(new RNG(), dimyend: 9, dimzend: 9, mean: -50.0, std: 5));
            net0.ReSet();
            //net0.ReShape(new Point3D(2, 10, 5));
            var net1 = Proliferation.Division(n, new Point3D(1, 10, 10), "InitPotential", new Randomizer(new RNG(), dimyend: 9, dimzend: 9, mean: -50.0, std: 10));
            net1.ReSet();
            Projection.From_To(net0, net1, new WeightSynapse(null, 1), ProjectionType.OneToOne, 1.0);
            Projection.From_To(net0, net0, new WeightSynapse(null, -0.4), ProjectionType.AllToAll, 0.3);
            Projection.From_To(net1, net0, new WeightSynapse(null, -0.1), ProjectionType.AllToAll, 0.5);
            Projection.From_To(net1, net1, new WeightSynapse(null, 0.1), ProjectionType.OneToOne, 0.8);
            var net = new Network();
            net.ChildNetworks.Add(net0.ID, net0);
            net.ChildNetworks.Add(net1.ID, net1);
            LoadNetwork(net);
            CellNet.ChildCellNet[net0.ID].Position = new Point3D(-15, 0, 15);
        }


        public void LoadNetwork(INetwork network)
        {
            Simulator.Network = network;
            CellNet = StemCell.Develop(network);
            ModelVisual.Children.Clear();
            ModelVisual.Children.Add(CellNet.Mophology);
        }

        #region Viewport3D Interactive

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

        #endregion

        #region Simulation Control

        void OnRunOver(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
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
            var isp = IsReportProgress;
            var isi = IsImaging;
            simulator.Stop();
            IsReportProgress = isp;
            IsImaging = isi;
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
            var isp = IsReportProgress;
            IsReportProgress = false;
            simulator.Step(Convert.ToDouble(DeltaTBox.Text));
            CurrentTBox.Text = simulator.CurrentT.ToString("F2");
            IsReportProgress = isp;
        }

        public void ReSet()
        {
            var startT = Convert.ToDouble(CurrentTBox.Text);
            simulator.Network.ReSet(startT);
        }

        #endregion

    }
}
