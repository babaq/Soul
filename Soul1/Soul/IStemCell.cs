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
    public interface IStemCell
    {
        ICell Develop(NeuronType neurontype, double threshold = -50, double initpotential = -60, double restpotential = -60, double r = 5, double c = 2, double resetpotential = -60, double refractoryperiod = 1);
        ICell Develop(INeuron neuron);
        ICellNet Develop(INetwork network);
        Tuple<ModelVisual3D,Imaging> DevelopMophology(INeuron neuron);
    }
}
