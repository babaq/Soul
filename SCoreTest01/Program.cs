using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCore;

namespace SCoreTest01
{
    class Program
    {
        private static Network testnet;

        static void Init()
        {
            Console.WriteLine("Init Neural Network . . .\n");
            testnet = new Network(0.5, 20);
            var mp0 = new MP(0, new ThresholdSigmoid(0.3),1.0);
            testnet.AddNeuron(mp0);
            var mp1 = new MP(1, new ThresholdSigmoid(0.5), 0.0);
            mp0.ConnectTo(mp1, new WeightSynapse(mp1.ID, 0.1));
            mp0.ConnectFrom(mp1, new WeightSynapse(mp0.ID, 0.6));
            mp0.ConnectTo(mp0,new WeightSynapse(mp0.ID,0.7));
            mp1.ConnectTo(mp1,new WeightSynapse(mp1.ID,0.2));
            Console.WriteLine("Init Done. Ready for Simulation.\n");
        }

        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine("1: Run   2: StepRun   3: ReInit   4: Exit\n");
                var select = Console.ReadLine();
                switch (select)
                {
                    case "1":
                        testnet.Run();
                        Console.WriteLine("\nSimulation Done !\n");
                        Console.Beep();
                        break;
                    case "2":
                        do
                        {
                            testnet.StepRun();
                            Console.WriteLine("Run Step Time: "+testnet.CurrentT);
                            Console.WriteLine("mp0 state: "+testnet.NeuronList[0].LastAxonPotential);
                            Console.WriteLine("mp1 state: " + testnet.NeuronList[1].LastAxonPotential);
                            Console.ReadLine();
                        } while (!testnet.IsRunOver);
                        Console.WriteLine("Simulation Done !\n");
                        Console.Beep();
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

