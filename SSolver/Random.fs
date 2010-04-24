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
/// Random Number Generator based on System.Random
/// </summary>
type RNG = 
    inherit Random
    new() = {inherit Random()}
    new(seed:int) = {inherit Random(seed)}
    override this.Sample() = 
        base.Sample()
    override this.NextDouble() = 
        this.Sample()
    override this.Next() = 
        this.Next(Int32.MaxValue)
    override this.Next(maxValue:int) = 
        int (this.Sample() * float maxValue)
    override this.Next(minValue:int,maxValue:int) = 
        if minValue > maxValue then
            raise(new ArgumentOutOfRangeException("minValue","ArgumentMinValueGreaterThanMaxValue"))
        int (this.Sample() * float(maxValue - minValue)) + minValue
    abstract Nexts: int*int->int[]
    default this.Nexts(n:int,maxValue:int) = 
        if n<1 then
            raise(new ArgumentException("ArgumentMustBePositive"))
        Array.init n (fun x ->this.Next(maxValue))
    override this.NextBytes(buffer:byte[]) =
        if buffer = null then
            raise(new ArgumentNullException("buffer"))
        for i=0 to buffer.Length-1 do
            buffer.[i] <- byte( this.Next() % 256 )
        ()
    abstract NextDoubles: int->float[]
    default this.NextDoubles(n:int) = 
        if n<1 then
            raise(new ArgumentException("ArgumentMustBePositive"))
        Array.init n (fun x ->this.NextDouble())
    abstract NextMeanStdDouble: float*float->float
    default this.NextMeanStdDouble(mean:float,std:float) = 
        let minValue = mean - std
        let maxValue  = mean + std
        this.Sample() * (maxValue - minValue) + minValue

/// <summary>
/// Generate Gaussian Random Number Using Polar Form of Box-Muller Transformation
/// </summary>
type GaussRNG = 
    inherit RNG
    val mutable private gn:float
    val mutable private ign:int
    val mutable Random:RNG
    new() = new GaussRNG(new RNG())
    new(seed:int)=new GaussRNG(new RNG(seed))
    new(random:RNG)={gn=0.0;ign=0;Random = random}
    /// <summary>
    /// Gets Standard Gaussian Distribution Number (mean: 0, std: 1)
    /// </summary>
    /// <returns></returns>
    override this.Sample() = 
        let mutable un1=2.0 * this.Random.NextDouble() - 1.0
        let mutable un2 = 2.0 * this.Random.NextDouble() - 1.0
        let mutable w = un1*un1 + un2*un2
        if this.ign=0 then
            while (w >= 1.0 || w = 0.0) do
                un1<-2.0 * this.Random.NextDouble() - 1.0
                un2 <- 2.0 * this.Random.NextDouble() - 1.0
                w <- un1*un1 + un2*un2
            let sqw = Math.Sqrt((-2.0 * Math.Log(w)) / w)
            this.gn <- un2 * sqw
            this.ign <- 1
            un1 * sqw
        else
            this.ign <- 0
            this.gn
    /// <summary>
    /// Gets Custom Gaussian Distribution Number (mean, std)
    /// </summary>
    /// <param name="mean"></param>
    /// <param name="std"></param>
    /// <returns></returns>
    override this.NextMeanStdDouble(mean:float,std:float) = 
        CoreFunc.RNMeanStd (this.Sample()) mean std 