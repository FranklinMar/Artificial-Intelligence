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
            List<Neuron> neurons = new() { new Neuron(0), new Neuron(0), new Neuron(0) };
            Network network = new (Sigmoid.Instance, neurons, 1, 1);
            network.Debug = true;
            network.Propagate(0.1);


        }
    }
}
