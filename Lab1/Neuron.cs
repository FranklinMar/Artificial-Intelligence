using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{

    class Neuron
    {
        //private IEnumerable<double>? InputWeights { get; set; }
        public double Value { get; private set; }

        public IDictionary<Neuron, double> PreviousWeights { get; private set; } = null;
        //private Layer PreviousLayer { get; set; }
        private IList<Neuron> PreviousLayer { get; set; }
        /*public double Output { 
            get { return Function.Calculate(Value); } 
            set { Value = value; } 
        }*/

        public Neuron(double value = 0/*, IFunction function, IEnumerable<double>? input = null*/)
        {
            /*Function = function;*/
            //Output = value;
            Value = value;
            //InputWeights = input;
        }
        public void RandomizeWeights(IList<Neuron> previousLayer, double? value = null, double? Const = null)
        {
            Random generator = new();
            PreviousWeights = new Dictionary<Neuron, double>();
            foreach (Neuron neuron in previousLayer)
            {
                /*Double Const = 5;*/
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
                /*PreviousWeights.Add(new Tuple<Neuron, Neuron>(thisNeuron, previousNeuron), generator.Next(Math.Floor(thisNeuron.Output) - Const, Math.Ceiling(thisNeuron.Output) + Const));*/

            }
            PreviousLayer = previousLayer;
        }

        public void SetWeights(IList<Neuron> previousLayer, IList<double> weights)
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

        public double WeightSum(IFunction Function/*Layer Layer*/)
        {
            if (PreviousLayer == null)
            {
                throw new InvalidOperationException("No previous layer of neurons found");
            }
            if (PreviousWeights == null)
            {
                throw new InvalidOperationException("No weights of neuron connection found");
            }
            //double sum = 0;
            Value = 0;
            foreach (KeyValuePair<Neuron, double> pair in PreviousWeights)
            {
                Value += (Function == null ? pair.Key.Value : Function.Calculate(pair.Key.Value)) * pair.Value;
            }
            //Output = sum;
            return Value;
        }
    }

}
