using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string Line = new ('-', 18);
            LinkedList<List<Neuron>> list = new();

            // AND Network
            List<Neuron> neurons = new() { new Neuron(), new Neuron() };
            List<double> weights = new() { 1, 1 };
            Neuron output = new();  
            output.SetWeights(neurons, weights);
            list.AddLast(neurons);
            list.AddLast(new List<Neuron>() { output });
            Network neural = new(And.Instance, list);
            neural.ShowResult = true;
            Console.WriteLine($"{Line}\n\tAND");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    neurons[0].Value = i;
                    neurons[1].Value = j;
                    neural.Calculate();
                    Console.WriteLine($"{Line}\nFinal Result: {And.Instance.Calculate(output.Value)}\n");
                }
            }

            // OR Network
            neural.Function = Or.Instance;
            Console.WriteLine($"{Line}\n\tOR");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    neurons[0].Value = i;
                    neurons[1].Value = j;
                    neural.Calculate();
                    Console.WriteLine($"{Line}\nFinal Result: {Or.Instance.Calculate(output.Value)}\n");
                }
            }

            // XOR Network
            List<Neuron> afterFirst = new() { new Neuron(), new Neuron() };
            list.AddAfter(list.First, afterFirst);
            weights = new() { 1, 1 };
            output.SetWeights(afterFirst, weights);
            weights = new() { 1, -1 };
            afterFirst[0].SetWeights(neurons, weights);
            weights = new() { -1, 1 };
            afterFirst[1].SetWeights(neurons, weights);
            neural.Function = Xor.Instance;
            Console.WriteLine($"{Line}\n\tXOR");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    neurons[0].Value = i;
                    neurons[1].Value = j;
                    neural.Calculate();
                    Console.WriteLine($"{Line}\nFinal Result: {Xor.Instance.Calculate(output.Value)}\n");
                }
            }

            // NOT Network
            //List<Neuron> neuron = new() { new Neuron(0) };
            //list.RemoveFirst();
            //list.AddFirst(neuron);
            list.Remove(list.First.Next);
            List<Neuron> neuron = list.First.Value;
            neuron.Clear();
            neuron.Add(new Neuron(0));
            weights = new() { -1.5 };
            output.SetWeights(neuron, weights);
            neural.Function = Not.Instance;
            Console.WriteLine($"{Line}\n\tNOT");
            neural.Calculate();
            Console.WriteLine($"{Line}\nFinal Result: {Not.Instance.Calculate(output.Value)}\n\n");
            neuron[0].Value = 1;
            neural.Calculate();
            Console.WriteLine($"{Line}\nFinal Result: {Not.Instance.Calculate(output.Value)}\n");

            Line = new string('=', 18);
            Console.WriteLine($"\n{Line}\n\tPAUSE\n{Line}");
            Console.ReadKey();
            Console.Clear();

            // Back Propagation - Network 3 + 3 + 1
            //neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            //neurons = new() { new Neuron(10), new Neuron(5), new Neuron(2) };
            neurons = new() { new Neuron(1.59), new Neuron(5.73), new Neuron(0.48) };
            //Neuron result = new(1);
            Neuron result = new(5.28);
            List<double> Results = new ();
            Results.Add(5.28);
            List<Neuron> ResultLayer = new() { result };
            Network network = new(Sigmoid.Instance, neurons, ResultLayer, 3);
            //network.Print();
            network.ShowResult = true;
            //network.Debug = true;
            //network.Propagate(new List<double> { 1 }, 1, 1E-12);
            //network.Propagate(new List<double> { 4 }, 1, 1E-12);
            /*neurons[0].Value = 0;
            neurons[1].Value = 1;
            neurons[2].Value = 0;
            network.Propagate(1, 1, 1E-12);
            neurons[0].Value = 1;
            neurons[1].Value = 0;
            neurons[2].Value = 0;
            network.Propagate(1, 0, 1E-12);
            neurons[0].Value = 1;
            neurons[1].Value = 1;
            neurons[2].Value = 1;
            network.Propagate(1, 1, 1E-12);
            neurons[0].Value = 1;
            neurons[1].Value = 0;
            neurons[2].Value = 0;
            network.Propagate(1, 1, 1E-12);
            neurons[0].Value = 0;
            neurons[1].Value = 1;
            neurons[2].Value = 0;
            Console.WriteLine($"{Line}\n\tSigmoid Logic operator");
            Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n");
            //network.Propagate(1, 1, 1E-15);*/
            /*neurons[0].Value = 1.59;
            neurons[1].Value = 5.73;
            neurons[2].Value = 0.48;*/
            //network.Propagate(1, 5.28, 1E-12);
            /*network.Propagate(1, 1.35, 1E-12);*/
            /*network.Propagate(1, 1, 1E-12);
            neurons[0].Value = 1;
            neurons[1].Value = 0;
            neurons[2].Value = 0;
            network.Propagate(1, 1, 1E-12);
            neurons[0].Value = 0;
            neurons[1].Value = 1;
            neurons[2].Value = 0;*/
            /*neurons[0].Value = 10;
            neurons[1].Value = 2;
            neurons[2].Value = 2.5;*/
            /*neurons[0].Value = 5.73;
            neurons[1].Value = 0.48;
            neurons[2].Value = 5.28;
            network.Propagate(new List<double> { 5.28 }, 1, 1E-12);
            neurons[0].Value = 5.73;
            neurons[1].Value = 0.48;
            neurons[2].Value = 5.28;
            network.Propagate(new List<double> { 1.35 }, 1, 1E-12);
            neurons[0].Value = 0.48;
            neurons[1].Value = 5.28;
            neurons[2].Value = 1.35;
            network.Propagate(new List<double> { 5.91 }, 1, 1E-12);
            neurons[0].Value = 5.28;
            neurons[1].Value = 1.35;
            neurons[2].Value = 5.91;
            Console.WriteLine($"{Line}\n\tSigmoid Logic operator");
            network.Calculate();
            Console.WriteLine($"{Line}\nFinal Result: {result.Value}\n");*/
            double [] learnData = new double[] {2.56, 4.20, 1.60, 4.29, 1.17, 4.40, 0.88, 4.14, 0.07, 4.77, 1.95, 4.18, 0.04, 5.05, 1.40};
            int Num = 5000;
            for (int k = 0; k < Num; k++) 
            {
                for (int i = 0; i < learnData.Length - neurons.Count - Results.Count; i++)
                {
                    for (int j = 0; j < neurons.Count; j++)
                    {
                        neurons[j].Value = learnData[i + j];
                    }
                    for (int j = 0; j < Results.Count; j++)
                    {
                        Results[j] = learnData[i + neurons.Count + j];
                    }
                    network.Propagate(Results, 0.1, 0.1, 1E-12);
                }
                Console.WriteLine((k / (double)Num * 100.0) + "%");
            }
            Console.Clear();
            for (int i = 0; i < learnData.Length - neurons.Count - Results.Count; i++)
            {
                for (int j = 0; j < neurons.Count; j++)
                {
                    Console.Write($"X {(neurons.Count != 1 ? j.ToString() : "")} = {learnData[i + j]} |");
                    neurons[j].Value = learnData[i + j];
                }
                network.Calculate();
                Console.WriteLine("\nExpected result");
                for (int j = 0; j < Results.Count; j++)
                {
                    Console.Write($"y {(Results.Count != 1 ? j.ToString() : "")} = {learnData[i + neurons.Count + j]} |");
                }
                Console.WriteLine("\nActual result");
                for (int j = 0; j < ResultLayer.Count; j++)
                {
                    Console.Write($"Y {(ResultLayer.Count != 1 ? j.ToString() : "")} = {ResultLayer[j].Value} |");
                }
            }
            Console.ReadKey();
        }
    }
}
