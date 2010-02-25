using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCore;
using SSolver;

namespace SCoreTest01
{
    class Program
    {
        private static Simulator simulator;
        private static Population network;
        private static ODESolver solver;


        static void Init()
        {
            Console.WriteLine("Init Neural Network And Simulation Environment . . .\n");
            // neural network
            network = new Population();
            Console.WriteLine("1: MP   2: LI   3: IF   4: HH\n");
            var select = Console.ReadLine();
            switch (select)
            {
                case "1":
                    InitMP();
                    break;
                case "2":
                    InitLI();
                    break;
                case "3":
                    InitIF();
                    break;
                case "4":
                    InitHH();
                    return;
            }
            // solver to be used
            solver = new ODESolver();
            // simulator
            simulator = new Simulator(0.5, 20, network, solver);
            Console.WriteLine("Init Done. Ready for Simulation.\n");
        }

        static void InitMP()
        {
            var mp0 = new MP(0.3, 1.0) { Population = network };
            var mp1 = new MP(0.5, 0.0);
            mp0.ProjectTo(mp1, new WeightSynapse(mp0, 0.1));
            mp0.ProjectedFrom(mp1, new WeightSynapse(mp1, 0.6));
            mp0.ProjectTo(mp0, new WeightSynapse(mp0, 0.7));
            mp1.ProjectTo(mp1, new WeightSynapse(mp1, 0.2));
        }

        static void InitLI()
        {
            var li0 = new LI(0.3, 0.5, 3.0){Population = network};
            var li1 = new LI(0.5, 0.1, 5.0);
            li0.ProjectTo(li1,new WeightSynapse(li0,-0.3));
            li0.ProjectedFrom(li1,new WeightSynapse(li1,1.6));
            li0.ProjectTo(li0,new WeightSynapse(li0,0.2));
            li1.ProjectTo(li1,new WeightSynapse(li1,0.8));
        }

        static void InitIF()
        {
        }

        static void InitHH()
        {
        }


        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine("1: Run   2: Step   3: ReInit   4: Exit\n");
                var select = Console.ReadLine();
                switch (select)
                {
                    case "1":
                        simulator.Run();
                        Console.WriteLine("\nSimulation Done !\n");
                        Console.Beep();
                        break;
                    case "2":
                        simulator.Step();
                        Console.WriteLine("Time: " + simulator.CurrentT+" ms");
                        Console.WriteLine("Neuron0 Output: " + network.Neurons.Values.ElementAt(0).LastOutput.ToString("F2"));
                        Console.WriteLine("Neuron1 Output: " + network.Neurons.Values.ElementAt(1).LastOutput.ToString("F2")+"\n");
                        break;
                    case "3":
                        Init();
                        break;
                    case "4":
                        return;
                }
            }
        }
    }
}

