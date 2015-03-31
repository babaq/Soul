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
/// Time Dependent Sine Function
/// </summary>
let TFSine (freq_phase: seq<float>) t = 
    Math.Sin(2.0*Math.PI*(Seq.nth 0 freq_phase) * t + Seq.nth 1 freq_phase)

/// <summary>
/// Gaussian Function
/// </summary>
let Gauss x u d =
    Math.Exp( -( Math.Pow(x - u, 2.0) ) / ( 2.0 * Math.Pow(d, 2.0) ) )

/// <summary>
/// General Random Number(mean, std)
/// </summary>
let RNMeanStd (sample:float) mean std =
    mean+std*sample

/// <summary>
/// Heaviside Unit Step Function
/// </summary>
let Heaviside x =
    if x<0.0 then
        0.0
    else
        1.0

/// <summary>
/// General Sigmoid Function
/// </summary>
let gSigmoid (x: float) slope shift=
    1.0 / ( 1.0 + exp( shift - (x/slope) ) )

/// <summary>
/// Sigmoid Function
/// </summary>
let Sigmoid x =
    gSigmoid x 1.0 0.0

/// <summary>
/// Dirac Delta Function
/// </summary>
let DiracDelta x =
    if x=0.0 then
        1.0
    else
        0.0

/// <summary>
/// Range Dirac Delta Function
/// </summary>
let rDiracDelta (x: float) delta =
    if x>(-delta/2.0) && x<=(delta/2.0) then
        1.0
    else
        0.0

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
/// Dynamic Rule Of Leaky Integrator Model In Derivative Function
/// </summary>
let LI tao u e w v =
    ( e - u + Sigma(Prod w v) ) / tao

/// <summary>
/// Dynamic Rule Of Leaky Integrator Model In Derivative Function Delegate
/// </summary>
let dLI (tao_e_sigma : seq<float>) t u =
    ( Seq.nth 1 tao_e_sigma - u + Seq.nth 2 tao_e_sigma ) / (Seq.nth 0 tao_e_sigma)

/// <summary>
/// Dynamic Rule Of Integrate-and-Fire Model In Derivative Function
/// </summary>
let IF tao r u e w d =
    ( (e-u) / tao ) + ( r * Sigma(Prod w d) )

/// <summary>
/// Dynamic Rule Of Integrate-and-Fire Model In Derivative Function Delegate
/// </summary>
let dIF (tao_r_e_sigma : seq<float>) t u =
    ( (Seq.nth 2 tao_r_e_sigma - u) / Seq.nth 0 tao_r_e_sigma ) + Seq.nth 1 tao_r_e_sigma * Seq.nth 3 tao_r_e_sigma

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
let ddEuler (h : float) t y y'_param (y'_delegate : Derivative) =
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
let ddRK4 h t y y'_param (y'_delegate : Derivative) =
    let y' y'_param t y = y'_delegate.Invoke(y'_param, t, y)
    dRK4 h t y y'_param y'
    