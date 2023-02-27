using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<List<Neuron>> list = new();

            // AND Network
            List<Neuron> neurons = new() { new Neuron(1), new Neuron(1) };
            List<double> weights = new() { 1, 1 };
            Neuron output = new();  
            output.SetWeights(neurons, weights);
            list.AddLast(neurons);
            list.AddLast(new List<Neuron>() { output });
            Network neural = new(And.Instance, list, output);
            neural.Debug = true;
            Console.WriteLine("AND");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            // OR Network
            neural.Function = Or.Instance;
            Console.WriteLine("OR");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            // NOT Network
            List<Neuron> neuron = new() { new Neuron(0) };
            weights = new() { -1.5 };
            output.SetWeights(neuron, weights);
            neural.Function = Not.Instance;
            Console.WriteLine("NOT");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

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
            Console.WriteLine("XOR");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            Neuron result = new();
            Network network = new(Sigmoid.Instance, neurons, result, 1, 1);
            network.Debug = true;
            network.Propagate(1, 1);
            Console.ReadKey();
        }
    }
}
