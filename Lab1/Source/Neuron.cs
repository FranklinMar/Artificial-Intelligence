using System;
using System.Collections.Generic;

namespace Lab1
{

    class Neuron
    {
        public double Value { get; set; }

        //public double Output { get; private set; }

        // For propagation option 2
        public double Delta { get; set; }

        public Dictionary<Neuron, double> PreviousWeights { get; private set; } = null;
        //private List<Neuron> PreviousLayer { get; set; }

        public Neuron(double value = 0)
        {
            Value = value;
        }

        public void RandomizeWeights(List<Neuron> previousLayer/*, double? Value = null, double? Const = null*/)
        {
            Random generator = new();
            PreviousWeights = new Dictionary<Neuron, double>();
            foreach (Neuron neuron in previousLayer)
            {
                // random.NextDouble() * (Maximum - Minimum) + Minimum;
                // Maximum = Value + Const
                // Minimum = Value - Const
                // Maximum - Minimum = Value + Const - Value + Const = 2 * Const
                //if (value != null && Const != null) {
                double Random = 0;
                //double constant = (Const == 0 || Const == null ? 1 : (double)Const), value = (Value == 0 || Value == null ? 1 : (double)Value);
                while (Random == 0)
                {
                    Random = generator.NextDouble() - 0.5;// * 2 * constant + value - constant;
                    //Random = generator.NextDouble() * ((Value ?? 1) + (Const ?? 1) - ((Value ?? 1) - (Const ?? 1))) + ((Value ?? 1) - (Const ?? 1));
                    //Console.WriteLine(Random);
                }
                PreviousWeights.Add(neuron, Random);
                //} 
                /*else
                {
                    double Value = 0;
                    while (Value == 0) {
                        Value = generator.NextDouble();
                        //Console.WriteLine(Value);
                    }
                    PreviousWeights.Add(neuron, Value);
                }*/

            }
            //PreviousLayer = previousLayer;
        }

        public void SetWeights(List<Neuron> previousLayer, List<double> weights)
        {
            if (previousLayer.Count != weights.Count)
            {
                throw new InvalidOperationException("Number of neurons and weights do not match");
            }
            PreviousWeights = new Dictionary<Neuron, double>();
            for (int i = 0; i < previousLayer.Count; i++)
            {
                PreviousWeights.Add(previousLayer[i], weights[i]);
            }
            //PreviousLayer = previousLayer;
        }
        public void NullifyWeights()
        {
            PreviousWeights = null;
            //PreviousLayer = null;
        }

        public double WeightSum(IFunction Function)
        {
            /*if (PreviousLayer == null)
            {
                throw new InvalidOperationException("No previous layer of neurons found");
            }*/
            if (PreviousWeights == null)
            {
                throw new InvalidOperationException("No weights of neuron connection found");
            }
            Value = 0;
            foreach (KeyValuePair<Neuron, double> pair in PreviousWeights)
            {
                Value += (Function == null ? pair.Key.Value : Function.Calculate(pair.Key.Value)) * pair.Value;
            }
            return Value;
        }
    }

}
