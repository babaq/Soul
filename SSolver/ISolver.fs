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
/// Differential Equation Solver Type
/// </summary>
type SolverType =
| ODESolver = 0
| PDESolver = 1

/// <summary>
/// Differential Equation Solver Method
/// </summary>
type NumericMethod =
| Euler = 0
| RK4 = 1

/// <summary>
/// Differential Equation Solver Settings
/// </summary>
type SolverSettings =
    val mutable private solvertype: SolverType
    val mutable private numericmethod: NumericMethod
    new() = new SolverSettings(SolverType.ODESolver,NumericMethod.RK4)
    new(stype: SolverType,smethod: NumericMethod) =
        {
            solvertype = stype;
            numericmethod = smethod
        }
    member ss.SolverType with get() = ss.solvertype and set(v) = ss.solvertype <- v
    member ss.NumericMethod with get() = ss.numericmethod and set(v) = ss.numericmethod <- v

/// <summary>
/// Ordinary Differential Equation Derivative Function Delegate
/// </summary>
type Derivative = delegate of seq<float> * float * float -> float

/// <summary>
/// Ordinary Differential Equation Solve Function Delegate
/// </summary>
type Solve = delegate of float * float * float * seq<float> * Derivative -> float

/// <summary>
/// Differential Equation Solver Interface
/// </summary>
type ISolver = 
    abstract Settings: SolverSettings with get,set
    abstract Solve: Solve with get
    abstract Summary: string with get