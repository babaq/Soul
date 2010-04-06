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
using System.Reflection;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SCore;
using SSolver;

namespace Soul
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            var soulversion = "Soul\t Version: \t" + Assembly.GetExecutingAssembly().GetName().Version;
            var scoreversion = "SCore\t Version: \t" + Assembly.GetAssembly(typeof (INeuron)).GetName().Version;
            var ssolverversion = "SSolver\t Version: \t" + Assembly.GetAssembly(typeof (ISolver)).GetName().Version;

            var version = new Label
                              {
                                  Content = soulversion + "\n" +
                                            scoreversion + "\n" +
                                            ssolverversion,
                                  HorizontalAlignment = HorizontalAlignment.Center,
                                  Margin = new Thickness(12, 55, 0, 12),
                                  FontSize = 15,
                                  Foreground = Brushes.DeepPink
                              };
            Info.Children.Add(version);
        }


        private void AboutOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        static void OpenLink(string url)
        {
            var process = new Process {StartInfo = {FileName = url}};
            process.Start();
        }

        private void Codeplex_Click(object sender, RoutedEventArgs e)
        {
            OpenLink("http://soul.codeplex.com");
        }

    }
}
