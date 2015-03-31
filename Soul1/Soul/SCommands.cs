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
using System.Windows.Input;

namespace Soul
{
    public static class SCommands
    {
        private static RoutedUICommand run=new RoutedUICommand("Run","Run",typeof(SCommands));
        public static RoutedUICommand Run
        {
            get{ return run;}
        }

        static RoutedUICommand stop = new RoutedUICommand("Stop","Stop",typeof(SCommands));
        public static RoutedUICommand Stop
        {
            get { return stop; }
        }

        static RoutedUICommand pause = new RoutedUICommand("Pause", "Pause", typeof(SCommands));
        public static RoutedUICommand Pause
        {
            get { return pause; }
        }

        static RoutedUICommand resume = new RoutedUICommand("Resume", "Resume", typeof(SCommands));
        public static RoutedUICommand Resume
        {
            get { return resume; }
        }

        static RoutedUICommand step = new RoutedUICommand("Step", "Step", typeof(SCommands));
        public static RoutedUICommand Step
        {
            get { return step; }
        }

        static RoutedUICommand isreportprogress = new RoutedUICommand("IsReportProgress", "IsReportProgress", typeof(SCommands));
        public static RoutedUICommand IsReportProgress
        {
            get { return isreportprogress; }
        }

        static RoutedUICommand isimaging = new RoutedUICommand("IsImaging", "IsImaging", typeof(SCommands));
        public static RoutedUICommand IsImaging
        {
            get { return isimaging; }
        }

        static RoutedUICommand reset = new RoutedUICommand("ReSet", "ReSet", typeof(SCommands));
        public static RoutedUICommand ReSet
        {
            get { return reset; }
        }
    }

}
