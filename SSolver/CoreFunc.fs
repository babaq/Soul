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

module SSolver.CoreFunc

open System

/// <summary>
/// Heaviside Unit Step Function
/// </summary>
let Heaviside x =
    if x<0.0 then
        0.0
    else
        1.0

/// <summary>
/// Sigmoid Function
/// </summary>
let Sigmoid x =
    1.0 / ( 1.0 + exp( -x ) )

/// <summary>
/// Sigma Function
/// </summary>
let Sigma (x : seq<float>) =
    Seq.sum x

/// <summary>
/// Product Function
/// </summary>
let Prod (x : seq<float>) (y : seq<float>) = 
    Seq.map2 (fun a b -> a*b) x y

/// <summary>
/// Leaky Integrator Model
/// </summary>
let LIDeriv tao u w v =
    ( -u + Sigma(Prod w v) ) / tao

/// <summary>
/// Leaky Integrator Model
/// </summary>
let LIDeriv_Tao_Sigma (param : seq<float>) t u =
    ( -u + Seq.nth 1 param ) / (Seq.nth 0 param)


/// <summary>
/// Euler Method
/// </summary>
let Euler (delta_x : float) y deriv_y=
    y + delta_x * deriv_y

/// <summary>
/// Runge-Kutta 4 Method
/// </summary>
let RK4 h y k1 k2 k3 k4 =
    y + h * (k1 + 2.0*k2 + 2.0*k3 + k4) / 6.0

/// <summary>
/// Runge-Kutta 4 Method
/// </summary>
let RK4By h y fparam (f : seq<float> -> float -> float -> float) =
    let k1 = f fparam 0.0 y
    let k2 = f fparam (h/2.0) (y + h*y/2.0)
    let k3 = f fparam (h/2.0) (y + h*k2/2.0)
    let k4 = f fparam h (y + h*k3)
    RK4 h y k1 k2 k3 k4

/// <summary>
/// Runge-Kutta 4 Method
/// </summary>
let RK4By_Del h y fparam (del : Deriv) =
    let f p h y = del.Invoke(p,h,y)
    RK4By h y fparam f
    