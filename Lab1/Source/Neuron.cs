using System;
using System.Collections.Generic;

namespace Lab1
{
    public class Neuron
    {
        public double Value { get; set; }

        //public double Output { get; private set; }

        // For propagation option 2
        public double Delta { get; set; }

        public List<Synapse> Inputs { get; private set; } = new();
        public List<Synapse> Outputs { get; private set; } = new();

        [Obsolete("Deprecated")]
        public Dictionary<Neuron, double> PreviousWeights { get; private set; } = null;

        [Obsolete("Deprecated")]
        public Dictionary<Neuron, double> DeltaWeights { get; set; } = null;
        //private List<Neuron> PreviousLayer { get; set; }

        public Neuron(/*double value = 0*/) { }// => Value = value;

        //public void AddInput(Synapse Synapse) => Inputs.Add(Synapse);

        //public void AddOutput(ISynapse Synapse) => Outputs.Add(Synapse);

        public void AddInput(Neuron Neuron, double? Weight = null)
        {
            Synapse Synapse;
            if (Weight == null) {
                Synapse = new(Neuron, this);
            }
            else
            {
                Synapse = new(Neuron, this, (double) Weight);
            }
            Inputs.Add(Synapse);
            Neuron.Outputs.Add(Synapse);
        }

        public void AddOutput(Neuron Neuron, double? Weight = null)
        {
            Synapse Synapse;
            if (Weight == null)
            {
                Synapse = new(this, Neuron);
            }
            else
            {
                Synapse = new(this, Neuron, (double)Weight);
            }
            Outputs.Add(Synapse);
            Neuron.Inputs.Add(Synapse);
        }

        [Obsolete("Deprecated")]
        public void RandomizeWeights(List<Neuron> previousLayer/*, double? Value = null, double? Const = null*/)
        {
            SecureRandom generator = new();
            PreviousWeights = new Dictionary<Neuron, double>();
            DeltaWeights = new Dictionary<Neuron, double>();
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
                    Random = generator.NextDouble();// * 2 - 1;// - 0.5;// * 2 * constant + value - constant;
                    //Random = generator.NextDouble() * ((Value ?? 1) + (Const ?? 1) - ((Value ?? 1) - (Const ?? 1))) + ((Value ?? 1) - (Const ?? 1));
                    //Console.WriteLine(Random);
                }
                PreviousWeights.Add(neuron, Random);
                DeltaWeights.Add(neuron, 0);

                
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

        public double CalculateValue(IFunction Function)
        {
            Value = 0;
            Inputs.ForEach(Synapse => Value += Synapse.Output(Function));
            return Value;
        }

        [Obsolete("Deprecated")]
        public void SetWeights(List<Neuron> previousLayer, List<double> weights)
        {
            if (previousLayer.Count != weights.Count)
            {
                throw new InvalidOperationException("Number of neurons and weights do not match");
            }
            PreviousWeights = new Dictionary<Neuron, double>();
            DeltaWeights = new Dictionary<Neuron, double>();
            for (int i = 0; i < previousLayer.Count; i++)
            {
                PreviousWeights.Add(previousLayer[i], weights[i]);
                DeltaWeights.Add(previousLayer[i], 0);
            }
            //PreviousLayer = previousLayer;
        }

        /*public void ClearInput() => Inputs.Clear();
        public void ClearOutput() => Outputs.Clear();*/

        [Obsolete("Deprecated")]
        public void NullifyWeights()
        {
            PreviousWeights = null;
            DeltaWeights = null;
            //PreviousLayer = null;
        }

        [Obsolete("Deprecated")]
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
            /*foreach (Synapse Synapse in Inputs)
            {
                Value += Synapse.Output;
            }*/
            foreach (KeyValuePair<Neuron, double> pair in PreviousWeights)
            {
                Value += (Function == null ? pair.Key.Value : Function.Calculate(pair.Key.Value)) * pair.Value;
                DeltaWeights[pair.Key] = 0;
            }
            return Value;
        }
    }
}