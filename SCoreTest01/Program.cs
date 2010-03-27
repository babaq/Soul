using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SCore;
using SSolver;

namespace SCoreTest01
{
    class Program
    {
        private static Simulator simulator;
        private static Network network;
        private static ODESolver solver;
        private static Recorder recorder;


        static void Init()
        {
            Console.WriteLine("Init Neural Network And Simulation Environment . . .\n");
            // neural network
            network = new Network();
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
                    break;
            }
            // solver to be used
            solver = new ODESolver();
            // recorder to be used
            recorder = new Recorder(simulator,RecordType.None,"SCoreTest01");
            // simulator
            simulator = new Simulator(0.01, 5000, network, solver, recorder);
            Console.WriteLine("Init Done. Ready for Simulation.\n");
        }

        static void InitMP()
        {
            var mp0 = new MP(0.3, 1.0) { ParentNetwork = network };
            var mp1 = new MP(0.5, 0.0);
            mp0.ProjectTo(mp1, new WeightSynapse(mp0, 0.6));
            mp0.ProjectedFrom(mp1, new WeightSynapse(mp1, 0.8));
            mp0.ProjectTo(mp0, new WeightSynapse(mp0, 0.2));
            mp1.ProjectTo(mp1, new WeightSynapse(mp1, 0.4));
        }

        static void InitLI()
        {
        }

        static void InitIF()
        {
        }

        static void InitHH()
        {
        }

        static void OnUpdated(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            Console.WriteLine(simulator.CurrentT.ToString("F3") + " " + neuron.Output.ToString("F3") + " " + neuron.ID.ToString("N"));
        }

        static void OnSpike(object sender, EventArgs e)
        {
            var neuron = sender as INeuron;
            Console.WriteLine(simulator.CurrentT.ToString("F3") + " " + neuron.ID.ToString("N"));
        }

        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine("1: Run   2: Step   3: ReInit   4: Save   5: Open   6: Exit\n");
                var select = Console.ReadLine();
                switch (select)
                {
                    case "1":
                        Console.WriteLine(simulator.Summary);
                        simulator.Run();
                        try
                        {
                            Console.CursorVisible = false;
                            Console.ForegroundColor = ConsoleColor.Green;
                            while (true)
                            {
                                if (simulator.IsRunning)
                                {
                                    Console.Write((simulator.Progress * 100.0).ToString("F1") + "%");
                                    Console.CursorLeft = 0;
                                }
                                else
                                {
                                    Console.Beep(440, 150);
                                    Console.WriteLine("Simulation Done !\n");
                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        finally
                        {
                            Console.CursorVisible = true;
                            Console.ResetColor();
                        }
                        break;
                    case "2":
                        simulator.Network.RegisterUpdated(OnUpdated);
                        simulator.Network.RegisterSpike(OnSpike);
                        simulator.Step(simulator.DeltaT);
                        simulator.Network.UnRegisterSpike(OnSpike);
                        simulator.Network.UnRegisterUpdated(OnUpdated);
                        break;
                    case "3":
                        Init();
                        break;
                    case "4":
                        Console.Write("Enter Network Save File Name: ");
                        try
                        {
                            select = Console.ReadLine();
                            simulator.Recorder.Save(simulator.Network, select);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "5":
                        Console.Write("Enter Network Open File Name: ");
                        try
                        {
                            select = Console.ReadLine();
                            simulator.Network = simulator.Recorder.Open(select);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "6":
                        return;
                }
            }
        }
    }
}

