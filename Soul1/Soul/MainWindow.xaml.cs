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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SoulCore;

namespace Soul
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        var ofd = new OpenFileDialog();
                        ofd.AddExtension = true;
                        ofd.DefaultExt = "network";
                        ofd.Title = "Opening Neural Network ...";
                        ofd.InitialDirectory = Environment.CurrentDirectory;
                        ofd.CheckFileExists = true;
                        ofd.Filter = "Soul Network (.network)|*.network|" +
                                     "NeuroML(.xml)|*.xml";

                        if (ofd.ShowDialog() == true)
                        {
                            currentworkshop.LoadNetwork(currentworkshop.Simulator.Recorder.Open(ofd.FileName));
                        }
                    }
                }
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        var sfd = new SaveFileDialog();
                        sfd.AddExtension = true;
                        sfd.DefaultExt = "network";
                        sfd.Title = "Saving Neural Network ...";
                        sfd.InitialDirectory = Environment.CurrentDirectory;
                        sfd.FileName = "Soul";
                        sfd.OverwritePrompt = true;
                        sfd.CheckPathExists = true;
                        sfd.Filter = "Soul Network (.network)|*.network|" +
                                     "NeuroML(.xml)|*.xml";

                        if (sfd.ShowDialog() == true)
                        {
                            currentworkshop.Simulator.Recorder.Save(currentworkshop.Simulator.Network, sfd.FileName);
                        }
                    }
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutwindow = new AboutWindow();
            aboutwindow.ShowDialog();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Run_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        if (!currentworkshop.IsRunning)
                        {
                            currentworkshop.Run();
                            RunPause.Content = Resources["pauseico"];
                            RunPause.ToolTip = "Pause Simulation";
                        }
                        else if (currentworkshop.IsPaused)
                        {
                            currentworkshop.Resume();
                            RunPause.Content = Resources["pauseico"];
                            RunPause.ToolTip = "Pause Simulation";
                        }
                        else
                        {
                            currentworkshop.Pause();
                            RunPause.Content = Resources["playico"];
                            RunPause.ToolTip = "Resume Simulation";
                        }
                    }
                }
            }
        }

        private void Run_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof(WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        e.CanExecute = true;
                        if (!currentworkshop.IsRunning)
                        {
                            RunPause.Content = Resources["playico"];
                            RunPause.ToolTip = "Run Simulation";
                        }
                    }
                }
            }
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        currentworkshop.Stop();
                    }
                }
            }
        }

        private void Step_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        currentworkshop.Step();
                    }
                }
            }
        }

        private void Step_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof (WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        e.CanExecute = !currentworkshop.IsRunning;
                    }
                }
            }
        }

        private void ReSet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof(WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        currentworkshop.ReSet();
                    }
                }
            }
        }

        private void ReSet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof(WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        e.CanExecute = !currentworkshop.IsRunning;
                    }
                }
            }
        }

        private void IsReportProgress_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof(WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        currentworkshop.IsReportProgress = !currentworkshop.IsReportProgress;
                        if(currentworkshop.IsReportProgress&&currentworkshop.IsRunning)
                        {
                            currentworkshop.ProgressBar.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void IsImaging_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTab != null)
            {
                var currenttabcontent = MainTab.SelectedContent;
                if (currenttabcontent != null)
                {
                    if (currenttabcontent.GetType() == typeof(WorkShop))
                    {
                        var currentworkshop = currenttabcontent as WorkShop;
                        currentworkshop.IsImaging = !currentworkshop.IsImaging;
                    }
                }
            }
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

    }

}
