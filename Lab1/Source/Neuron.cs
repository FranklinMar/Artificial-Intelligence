using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{

    class Neuron
    {
        public double Value { get; set; }

        // For propagation option 2
        public double Delta { get; set; }

        public IDictionary<Neuron, double> PreviousWeights { get; private set; } = null;
        private List<Neuron> PreviousLayer { get; set; }

        public Neuron(double value = 0)
        {
            Value = value;
        }
        public void RandomizeWeights(List<Neuron> previousLayer, double? value = null, double? Const = null)
        {
            Random generator = new();
            PreviousWeights = new Dictionary<Neuron, double>();
            foreach (Neuron neuron in previousLayer)
            {
                // random.NextDouble() * (Maximum - Minimum) + Minimum;
                // Maximum = Value + Const
                // Minimum = Value - Const
                // Maximum - Minimum = Value + Const - Value + Const = 2 * Const
                if (value != null && Const != null) { 
                    PreviousWeights.Add(neuron, Math.Round(generator.NextDouble() * 2 * (double) Const + (double) value - (double) Const, 1));
                } 
                else
                {
                    PreviousWeights.Add(neuron, Math.Round(generator.NextDouble(), 1));
                }

            }
            PreviousLayer = previousLayer;
        }

        public void SetWeights(List<Neuron> previousLayer, IList<double> weights)
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
            PreviousLayer = previousLayer;
        }
        public void NullifyWeights()
        {
            PreviousWeights = null;
            PreviousLayer = null;
        }

        public double WeightSum(IFunction Function)
        {
            if (PreviousLayer == null)
            {
                throw new InvalidOperationException("No previous layer of neurons found");
            }
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
