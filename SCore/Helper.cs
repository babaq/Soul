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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSolver;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace SCore
{
    public static class Proliferation
    {
        public static INetwork Division(Point3D dimension, NeuronType neurontype, params Tuple<string, Randomizer>[] targets)
        {
            var network = Division(dimension, null, neurontype);
            network.Randomize(targets: targets);
            return network;
        }

        public static INetwork Division(Point3D dimension, Vector3D? neurondistance, NeuronType neurontype, double threshold = -50, double initpotential = -60, double restpotential = -60, double r = 5, double c = 2, double resetpotential = -60, double refractoryperiod = 1)
        {
            INeuron neuron = null;
            switch (neurontype)
            {
                case NeuronType.MP:
                    neuron = new MP(threshold, initpotential);
                    break;
                case NeuronType.LI:
                    neuron = new LI(threshold, initpotential, r, c, restpotential);
                    break;
                case NeuronType.IF:
                    neuron = new IF(threshold, resetpotential, refractoryperiod, initpotential, r, c, restpotential);
                    break;
            }
            return Division(neuron, dimension, neurondistance);
        }

        public static INetwork Division(INeuron neuron, Point3D dimension, Vector3D? neurondistance)
        {
            var network = new Network();
            for (var i = 0; i < dimension.X; i++)
            {
                for (var j = 0; j < dimension.Y; j++)
                {
                    for (var k = 0; k < dimension.Z; k++)
                    {
                        var child = neuron.Clone() as INeuron;
                        child.ParentNetwork = network;
                    }
                }
            }
            network.ReShape(dimension, neurondistance);
            return network;
        }

        public static INetwork Division(INeuron neuron, Point3D dimension, string property, Randomizer randomizer)
        {
            var network = Division(neuron, dimension, null);
            network.Randomize(property, randomizer);
            return network;
        }
    }

    public static class Projection
    {
        public static void From_To(INeuron neuron, INetwork network, Projector projector)
        {
            if (neuron != null && network.Dimension != null)
            {

            }
        }

        public static void From_To(INetwork network, INeuron neuron, double probability = 1.0)
        {

        }

        public static void From_To(INetwork sourcenetwork, INetwork targetnetwork, SynapseType synapsetype, double weight, double axondelay, ProjectionType projectiontype = ProjectionType.AllToAll, double probability = 1.0)
        {
            ISynapse synapse = null;
            switch (synapsetype)
            {
                case SynapseType.WeightSynapse:
                    synapse = new WeightSynapse(null, weight);
                    break;
                case SynapseType.SpikeWeightSynapse:
                    synapse = new SpikeWeightSynapse(null, weight, axondelay);
                    break;
            }
            From_To(sourcenetwork, targetnetwork, synapse, projectiontype, probability);

        }

        public static void From_To(INetwork sourcenetwork, INetwork targetnetwork, ISynapse synapse, ProjectionType projectiontype, double probability)
        {
            if (sourcenetwork.Neurons.Count > 0 && targetnetwork.Neurons.Count > 0)
            {
                switch (projectiontype)
                {
                    case ProjectionType.AllToAll:
                        for (var i = 0; i < sourcenetwork.Neurons.Count; i++)
                        {
                            for (var j = 0; j < targetnetwork.Neurons.Count; j++)
                            {
                                if (Projector.IsProjectionSucceed(probability))
                                {
                                    var source = sourcenetwork.Neurons.ElementAt(i).Value;
                                    var target = targetnetwork.Neurons.ElementAt(j).Value;
                                    var newsynapse = synapse.Clone() as ISynapse;
                                    newsynapse.PreSynapticNeuron = source;
                                    source.ProjectTo(target, newsynapse);
                                }
                            }
                        }
                        break;
                    case ProjectionType.OneToOne:
                        var both = Math.Min(sourcenetwork.Neurons.Count, targetnetwork.Neurons.Count);
                        for (var i = 0; i < both; i++)
                        {
                            if (Projector.IsProjectionSucceed(probability))
                            {
                                var source = sourcenetwork.Neurons.ElementAt(i).Value;
                                var target = targetnetwork.Neurons.ElementAt(i).Value;
                                var newsynapse = synapse.Clone() as ISynapse;
                                newsynapse.PreSynapticNeuron = source;
                                source.ProjectTo(target, newsynapse);
                            }
                        }
                        break;
                }
            }
        }

    }

    public static class Injection
    {
    }
}

