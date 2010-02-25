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

namespace SSolver

open System

/// <summary>
/// Derivative Function Delegate
/// </summary>
type Derivative = delegate of seq<float> * float * float -> float

/// <summary>
/// Solver Interface
/// </summary>
type ISolver = 
    abstract Settings: int with get,set
    abstract Solve: float * float * float * seq<float> * Derivative -> float