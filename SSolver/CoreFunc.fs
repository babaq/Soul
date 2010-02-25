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
/// Dynamic Rule Of Leaky Integrator Model
/// </summary>
let LI tao u w v =
    ( -u + Sigma(Prod w v) ) / tao

/// <summary>
/// Dynamic Rule Of Leaky Integrator Model In Derivative Function
/// </summary>
let dLI (tao_sigma : seq<float>) t u =
    ( -u + Seq.nth 1 tao_sigma ) / (Seq.nth 0 tao_sigma)

/// <summary>
/// Euler Method
/// </summary>
let Euler (h : float) y y' =
    y + h * y'

/// <summary>
/// Euler Method With Derivative Function
/// </summary>
let dEuler (h : float) t y y'_param (y' : seq<float> -> float -> float -> float) =
    y + h * (y' y'_param t y)

/// <summary>
/// Euler Method With Derivative Function Delegate
/// </summary>
let dDEuler (h : float) t y y'_param (y'_delegate : Derivative) =
    let y' y'_param t y = y'_delegate.Invoke(y'_param, t, y)
    dEuler h t y y'_param y'

/// <summary>
/// Fourth Order Runge-Kutta Method
/// </summary>
let RK4 h y k1 k2 k3 k4 =
    y + h * (k1 + 2.0*k2 + 2.0*k3 + k4) / 6.0

/// <summary>
/// Fourth Order Runge-Kutta Method With Derivative Function
/// </summary>
let dRK4 h t y y'_param (y' : seq<float> -> float -> float -> float) =
    let k1 = y' y'_param t y
    let k2 = y' y'_param (t + h/2.0) (Euler (h/2.0) y k1)
    let k3 = y' y'_param (t + h/2.0) (Euler (h/2.0) y k2)
    let k4 = y' y'_param (t + h) (Euler h y k3)
    RK4 h y k1 k2 k3 k4

/// <summary>
/// Fourth Order Runge-Kutta Method With Derivative Function Delegate
/// </summary>
let dDRK4 h t y y'_param (y'_delegate : Derivative) =
    let y' y'_param t y = y'_delegate.Invoke(y'_param, t, y)
    dRK4 h t y y'_param y'
    