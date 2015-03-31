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
using System.Windows.Media.Media3D;
using SoulCore;

namespace Soul
{
    public interface ICellNet
    {
        INetwork Network { get; set; }
        ModelVisual3D Mophology { get; set; }
        Dictionary<Guid, ICell> Cells { get; }
        Dictionary<Guid, ICellNet> ChildCellNet { get; }
        bool IsPushing { get; set; }
        Point3D Position { get; set; }
        RotateTransform3D Rotate { get; set; }
        TranslateTransform3D Translate { get; set; }
        ScaleTransform3D Scale { get; set; }
    }
}
