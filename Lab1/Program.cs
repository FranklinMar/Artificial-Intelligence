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
            Network neural = new(And.Instance, list, output);
            neural.ShowResult = true;
            Console.WriteLine($"{Line}\n\tAND");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    neurons[0].Value = i;
                    neurons[1].Value = j;
                    Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n");
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
                    Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n");
                }
            }

            // NOT Network
            List<Neuron> neuron = new() { new Neuron(0) };
            weights = new() { -1.5 };
            output.SetWeights(neuron, weights);
            neural.Function = Not.Instance;
            Console.WriteLine($"{Line}\n\tNOT");
            Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n\n{new string('-', 10)}");
            neuron[0].Value = 1;
            Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n");

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
                    Console.WriteLine($"{Line}\nFinal Result: {neural.Calculate()}\n");
                }
            }

            Line = new string('=', 18);
            Console.WriteLine($"\n{Line}\n\tPAUSE\n{Line}");
            Console.ReadKey();

            // Back Propagation - Network 3 + 3 + 1
            neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            Neuron result = new();
            Network network = new(Sigmoid.Instance, neurons, result, 1, 1);
            network.Debug = true;
            network.Propagate(1, 1);
            Console.ReadKey();
        }
    }
}
