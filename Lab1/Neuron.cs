using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{

    class Neuron
    {
        //private IEnumerable<double>? InputWeights { get; set; }
        public double Value { get; private set; }

        #nullable enable
        public IDictionary<Neuron, double>? PreviousWeights { get; private set; } = null;
        private Layer PreviousLayer { get; set; }
        /*public double Output { 
            get { return Function.Calculate(Value); } 
            set { Value = value; } 
        }*/

        public Neuron(double value/*, IFunction function, IEnumerable<double>? input = null*/)
        {
            /*Function = function;*/
            //Output = value;
            Value = value;
            //InputWeights = input;
        }
        public void RandomizeWeights(Layer previousLayer, double Const)
        {
            Random generator = new();
            PreviousWeights = new Dictionary<Neuron, double>();
            foreach (Neuron neuron in previousLayer.Neurons)
            {
                /*Double Const = 5;*/
                // random.NextDouble() * (Maximum - Minimum) + Minimum;
                // Maximum = Value + Const
                // Minimum = Value - Const
                // Maximum - Minimum = Value + Const - Value + Const = 2 * Const
                PreviousWeights.Add(neuron, generator.NextDouble() * 2 * Const + Output - Const);
                /*PreviousWeights.Add(new Tuple<Neuron, Neuron>(thisNeuron, previousNeuron), generator.Next(Math.Floor(thisNeuron.Output) - Const, Math.Ceiling(thisNeuron.Output) + Const));*/

            }
            PreviousLayer = previousLayer;
        }
        public void NullifyWeights()
        {
            PreviousWeights = null;
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
                Value += Function.Calculate(pair.Key.Value) * pair.Value;
            }
            //Output = sum;
            return Value;
        }
    }

}
