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
            Network neural = new Network(And.Instance, list, output);
            neural.Debug = true;
            Console.WriteLine($"X1: {neurons[0].Value}\nX@: {neurons[1].Value}");
            Console.WriteLine($"\n\nFinal Result {neural.Calculate()}");
        }
    }
}
