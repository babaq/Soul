using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Media3D;
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
            recorder = new Recorder(simulator,RecordType.All,"SCoreTest01");
            // simulator
            simulator = new Simulator(0.1, 20, network, solver, recorder);
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
            var Es = new List<LI>();

            for (int i = 0; i < 100; i++)
            {
                Es.Add(new LI(-50, -60, 2, 5, -60) { ParentNetwork = network });
            }
            var I = new LI(-50, -60, 2, 5, -60) { ParentNetwork = network };
            var Es_Input = new LI(-50, 50, 2, 5, 50) { ParentNetwork = network };

            for (int i = 0; i < 100; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    var ind = i + j;
                    if (ind >= 0 && ind < 100 && i != ind)
                    {
                        Es[i].ProjectedFrom(Es[ind], new WeightSynapse(Es[ind], 10 * CoreFunc.Gauss((j + 5), 10, 3)));
                    }
                }

                Es[i].ProjectedFrom(I, new WeightSynapse(I, -5));
                Es[i].ProjectTo(I, new WeightSynapse(Es[i], 2));
                //Es_Input.ProjectTo(I, new WeightSynapse(I, -0.2));
            }


            for (int i = 45; i < 55; i++)
            {

                Es[i].ProjectedFrom(Es_Input, new WeightSynapse(Es_Input, 10 * CoreFunc.Gauss((i - 45), 10, 3)));
            }

            for (int i = 10; i < 20; i++)
            {

                Es[i].ProjectedFrom(Es_Input, new WeightSynapse(Es_Input, 10 * CoreFunc.Gauss((i - 10), 10, 3)));

            }
            for (int i = 85; i < 95; i++)
            {
                Es[i].ProjectedFrom(Es_Input, new WeightSynapse(Es_Input, 10 * CoreFunc.Gauss((i - 85), 10, 3)));
            }
        }

        static void InitIF()
        {
            var Es = new List<IF>();
            //var Es_spiketime = new List<double>();
            //var EsInput_spiketime = new List<double>();
            //for (int i = 0; i < 100; i++)
            //{
            //    Es_spiketime.Add(0);
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    EsInput_spiketime.Add(i * 0.5);
            //}  
            int N = 10;
            int W = 4;
            for (int i = 0; i < N; i++)
            {
                Es.Add(new IF("IF:" + (i + 1), -55, -60.1, 1, -56, 10, 1, -60) { ParentNetwork = network,Position = new Point3D(-10+(i*2),0,0)});
            }

            var I = new IF("IF:" + (N + 1), -50, -60.1, 1, -56, 10, 1, -60) { ParentNetwork = network,Position = new Point3D(0,-3,0)};

            var Es_Input = new IF("IF:" + (N + 2), -50, 50, 0.05, 50, 2, 5, 50) { ParentNetwork = network,Position = new Point3D(0,3,0)};

            for (int i = 0; i < N; i++)
            {
                for (int j = -W / 2; j < W / 2 + 1; j++)
                {
                    var ind = i + j;
                    if (ind >= 0 && ind < N && i != ind)
                    {
                        Es[i].ProjectedFrom(Es[ind], new SpikeWeightSynapse(Es[ind], 1000 * CoreFunc.Gauss((j + 1), W, 3), 1));
                    }

                }

                Es[i].ProjectedFrom(I, new SpikeWeightSynapse(I, -5, 1));
                Es[i].ProjectTo(I, new SpikeWeightSynapse(Es[i], 2000, 1));
                //Es_Input.ProjectTo(I, new WeightSynapse(I, -0.2,0.1));
            }
            for (int i = 3; i < 7; i++)
            {

                Es[i].ProjectedFrom(Es_Input, new SpikeWeightSynapse(Es_Input, 1000 * CoreFunc.Gauss((i - 3), W, 3),0.5));
            }

            //for (int i = 5; i < 15; i++)
            //{

            //    Es[i].ProjectedFrom(Es_Input, new SpikeWeightSynapse(Es_Input, 100 * CoreFunc.Gauss((i - 10), W, 3)));

            //}
            //for (int i = 85; i < 95; i++)
            //{

            //    Es[i].ProjectedFrom(Es_Input, new SpikeWeightSynapse(Es_Input, 100 * CoreFunc.Gauss((i - 85), W, 3)));
            //}
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

