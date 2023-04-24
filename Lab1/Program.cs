using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string Line = new('-', 18);

            // AND Network
            NeuralNetwork NeuralNetwork = new(And.Instance, new int[] { 2, 1 });
            NeuralNetwork.Output[0].Inputs.ForEach(Synapse => Synapse.Weight = 1);
            NeuralNetwork.SHOW_RESULT = true;
            Console.WriteLine($"{Line}\n\tAND");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    NeuralNetwork.Calculate(new double[] { i, j });
                    Console.WriteLine($"{Line}\nFinal Result: {And.Instance.Calculate(NeuralNetwork.Output[0].Value)}\n");
                }
            }

            // OR Network
            NeuralNetwork.Function = Or.Instance;
            Console.WriteLine($"{Line}\n\tOR");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    NeuralNetwork.Calculate(new double[] { i, j });
                    Console.WriteLine($"{Line}\nFinal Result: {Or.Instance.Calculate(NeuralNetwork.Output[0].Value)}\n");
                }
            }

            // NOT Network
            NeuralNetwork = new(Not.Instance, new int[] { 1, 1 });
            NeuralNetwork.Output.Neurons.ForEach(Neuron => Neuron.Inputs.Clear());
            NeuralNetwork.Input.Neurons.ForEach(Neuron => Neuron.Outputs.Clear());
            NeuralNetwork.Output.ConnectPrevious(NeuralNetwork.Input, -1.5);
            NeuralNetwork.SHOW_RESULT = true;
            Console.WriteLine($"{Line}\n\tNOT");
            NeuralNetwork.Calculate(new double[] { 0 });
            Console.WriteLine($"{Line}\nFinal Result: {Not.Instance.Calculate(NeuralNetwork.Output[0].Value)}\n\n");
            NeuralNetwork.Calculate(new double[] { 1 });
            Console.WriteLine($"{Line}\nFinal Result: {Not.Instance.Calculate(NeuralNetwork.Output[0].Value)}\n");

            // XOR Network
            NeuralNetwork = new(Xor.Instance, new int[] { 2, 2, 1});
            NeuralNetwork.Input[0].Outputs[0].Weight = 1;
            NeuralNetwork.Input[0].Outputs[1].Weight = -1;
            NeuralNetwork.Input[1].Outputs[0].Weight = -1;
            NeuralNetwork.Input[1].Outputs[1].Weight = 1;
            NeuralNetwork.Output[0].Inputs.ForEach(Synapse => Synapse.Weight = 1);
            NeuralNetwork.SHOW_RESULT = true;
            Console.WriteLine($"{Line}\n\tXOR");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    NeuralNetwork.Calculate(new double[] { i, j });
                    Console.WriteLine($"{Line}\nFinal Result: {Xor.Instance.Calculate(NeuralNetwork.Output[0].Value)}\n");
                }
            }

            Line = new string('=', 18);
            Console.WriteLine($"\n{Line}\n\tPAUSE\n{Line}");
            Console.ReadKey();
            Console.Clear();

            // Back Propagation - Network 3 + 3 + 1
            NeuralNetwork = new(Sigmoid.Instance, new int[] { 3, 3, 1 });
            NeuralNetwork.SHOW = true;
            List<double> Results = new();
            Results.Add(4.29);
            
            double [] LearnData = new double[] {2.56, 4.20, 1.60, 4.29, 1.17, 4.40, 4.14, 0.07, 4.77, 1.95, 4.18, 0.04, 5.05, 1.40};
            /*
            List<double> Results = new ();
            int Epochs = 100000;
            var Watch = new System.Diagnostics.Stopwatch();
            for (int Epoch = 0; Epoch < Epochs; Epoch++)
            {
                Watch.Restart();
                for (int i = 0; i < LearnData.Length - NeuralNetwork.Input.Count - Results.Count; i++)
                {
                    for (int j = 0; j < NeuralNetwork.Input.Count; j++)
                    {
                        NeuralNetwork.Input[j].Value = LearnData[i + j];
                    }
                    for (int j = 0; j < Results.Count; j++)
                    {
                        Results[j] = LearnData[i + NeuralNetwork.Input.Count + j];
                    }
                    NeuralNetwork.BackPropagate(Results, 0.1, 0.1, 1E-12);
                }
                Watch.Stop();
                if ((Epoch / (double) Epochs * 1000.0) % 1 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Epochs Total: {Epochs}\nEpoch: {Epoch}\nProgress: {(Epoch / (double)Epochs * 100.0),2:0.0}%");
                    Console.WriteLine($"Estimated time waiting: {(Epochs - Epoch) * Watch.ElapsedMilliseconds / 1000.0 }s");
                }
            }*/
            NeuralNetwork.Learn(LearnData, 100000, 0.1, 0.1);
            for (int i = 0; i < LearnData.Length - NeuralNetwork.Input.Count - Results.Count; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < NeuralNetwork.Input.Count; j++)
                {
                    Console.Write($"X {(NeuralNetwork.Input.Count != 1 ? j.ToString() : "")} = {LearnData[i + j]} |");
                    NeuralNetwork.Input[j].Value = LearnData[i + j];
                }
                NeuralNetwork.Calculate();
                Console.WriteLine("\nExpected result");
                for (int j = 0; j < Results.Count; j++)
                {
                    Console.Write($"y {(Results.Count != 1 ? j.ToString() : "")} = {LearnData[i + NeuralNetwork.Input.Count + j]} |");
                }
                Console.WriteLine("\nActual result");
                for (int j = 0; j < NeuralNetwork.Output.Count; j++)
                {
                    Console.Write($"Y {(NeuralNetwork.Output.Count != 1 ? j.ToString() : "")} = {NeuralNetwork.Output[j].Value} |");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
