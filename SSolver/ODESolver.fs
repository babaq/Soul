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
open System.Text
open System.Reflection
open CoreFunc

/// <summary>
/// Basic Ordinary Differential Equation Solver
/// </summary>
type ODESolver =
    val mutable private settings: SolverSettings
    val mutable private solve: Solve
    new() = new ODESolver(new SolverSettings())
    new(s: SolverSettings) as osr =
        {
            settings=s;
            solve = null
        } then
        osr.settings.SolverType <- SolverType.ODESolver
        osr.SetMethod osr.settings.NumericMethod
    member private osr.SetMethod (x: NumericMethod)=
        match x with
        | NumericMethod.Euler -> osr.solve <- new Solve(ddEuler)
        | NumericMethod.RK4 -> osr.solve <- new Solve(ddRK4)
        | _ -> ()
    interface ISolver with
        member osr.Settings with get() = osr.settings and set(v) = osr.settings <- v; osr.SetMethod osr.settings.NumericMethod
        member osr.Solve with get() = osr.solve
        member osr.Summary with get() =
            let s = new StringBuilder()
            s.AppendLine("# Solver Summary. SSolver Version: "+Assembly.GetExecutingAssembly().GetName().Version.ToString())
            s.AppendLine("# SolverType="+osr.settings.SolverType.ToString())
            s.AppendLine("# NumericMethod="+osr.settings.NumericMethod.ToString())
            s.ToString()

