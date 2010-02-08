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
