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

using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SoulCore;
using SoulSolver;

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

            var SoulVersion = "Soul\t\t Version: \t" + Assembly.GetExecutingAssembly().GetName().Version;
            var SoulCoreVersion = "SoulCore\t Version: \t" + Assembly.GetAssembly(typeof(INeuron)).GetName().Version;
            var SoulSolverVersion = "SoulSolver\t Version: \t" + Assembly.GetAssembly(typeof(ISolver)).GetName().Version;

            var version = new Label
                              {
                                  Content = SoulVersion + "\n" +
                                            SoulCoreVersion + "\n" +
                                            SoulSolverVersion,
                                  HorizontalAlignment = HorizontalAlignment.Center,
                                  Margin = new Thickness(12, 55, 12, 55),
                                  FontSize = 15,
                                  Foreground = Brushes.DeepPink
                              };
            Info.Children.Add(version);
        }


        private void AboutOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public static void OpenLink(string url)
        {
            var process = new Process { StartInfo = { FileName = url } };
            process.Start();
        }

        private void Codeplex_Click(object sender, RoutedEventArgs e)
        {
            OpenLink("http://soul.codeplex.com");
        }

    }
}
