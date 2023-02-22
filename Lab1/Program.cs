using System;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Sigmoid function = Sigmoid.Instance;
            List<Neuron> neurons = new () { new Neuron(0), new Neuron(0), new Neuron(0) };
            Layer first = new (neurons, function);
            Layer last = new (new List<Neuron>() { new Neuron(1) }, function, first);
            last.RandomizeWeights();

            foreach (KeyValuePair <Tuple<Neuron, Neuron>, double> i in last.PreviousWeights)
            {
                Console.WriteLine(i.Key + ": " + i.Value);
            }
            //Console.WriteLine("Hello World!");
        }
    }
}
