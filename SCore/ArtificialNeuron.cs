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
using SSolver;

namespace SCore
{
    public class AMP:MP
    {
        public AMP(string name, Point3D position,double initpotential)
            : base(name, position, new ThresholdHeaviside(null, 0.5), initpotential, 0.0)
        {
        }


        public override void Update(double deltaT, double currentT, ISolver solver)
        {
            base.Update(deltaT, currentT, solver);
        }

        public override object Clone()
        {
            return base.Clone();
        }

    }
}
