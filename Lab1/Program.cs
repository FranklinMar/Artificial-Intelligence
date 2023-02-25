using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Sigmoid function = Sigmoid.Instance;
            List<Neuron> neurons = new () { new Neuron(0), new Neuron(0), new Neuron(0) };
            Layer first = new (neurons, function);
            Layer last = new (new List<Neuron>() { new Neuron(1) }, function, first);

            foreach (Neuron i in last.Neurons)
            {
                foreach (KeyValuePair<Neuron, double> pair in i.PreviousWeights)
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }
            //Console.WriteLine("Hello World!");*/
            /*List<Neuron> neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            Neuron result = new ();
            Network network = new (Sigmoid.Instance, neurons, result, 1, 1);
            network.Debug = true;
            network.Propagate(0.1, 1);*/
            LinkedList<IList<Neuron>> list = new();

            // And Network
            List<Neuron> neurons = new() { new Neuron(1), new Neuron(1) };
            List<double> weights = new() { 1, 1 };
            Neuron output = new();  
            output.SetWeights(neurons, weights);
            list.AddLast(neurons);
            list.AddLast(new List<Neuron>() { output });
            Network neural = new(And.Instance, list, output);
            neural.Debug = true;
            Console.WriteLine("AND");
            //Console.WriteLine($"\nX1: {neurons[0].Value}\nX2: {neurons[1].Value}");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            // Or Network
            neural.Function = Or.Instance;
            Console.WriteLine("OR");
            //Console.WriteLine($"\nX1: {neurons[0].Value}\nX2: {neurons[1].Value}");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            // Not Network
            List<Neuron> neuron = new() { new Neuron(0) };
            weights = new() { -1.5 };
            output.SetWeights(neuron, weights);
            neural.Function = Not.Instance;
            Console.WriteLine("NOT");
            //Console.WriteLine($"\nX: {neurons[0].Value}");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            // Xor Network
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
            //Console.WriteLine($"\nX1: {neurons[0].Value}\nX2: {neurons[1].Value}");
            Console.WriteLine($"\nFinal Result {neural.Calculate()}\n");

            neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            Neuron result = new();
            Network network = new(Sigmoid.Instance, neurons, result, 1, 1);
            network.Debug = true;
            network.Propagate(0.8, 1);
        }
    }
}
