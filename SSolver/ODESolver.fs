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
open CoreFunc

/// <summary>
/// Basic Ordinary Differential Equation Solver
/// </summary>
type ODESolver =
    val mutable settings: int
    new() = { settings = 0 }
    interface ISolver with
        member s.Settings with get() = s.settings and set(v) = s.settings <- v
        member s.Solve(h, t, y, y'_param, y'_delegate) = ddRK4 h t y y'_param y'_delegate

