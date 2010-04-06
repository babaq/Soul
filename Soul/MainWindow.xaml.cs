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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCore;

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
                        }
                        else if (currentworkshop.IsPaused)
                        {
                            currentworkshop.Resume();
                        }
                        else
                        {
                            currentworkshop.Pause();
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

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
