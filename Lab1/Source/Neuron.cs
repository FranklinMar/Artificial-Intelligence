using System.Collections.Generic;

namespace Lab1
{
    public class Neuron
    {
        public double Value { get; set; }
        public double Delta { get; set; }

        public List<Synapse> Inputs { get; private set; } = new();
        public List<Synapse> Outputs { get; private set; } = new();

        public Neuron(double value = 0) => Value = value;

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

        public double CalculateValue(IFunction Function)
        {
            Value = 0;
            Inputs.ForEach(Synapse => Value += Synapse.Output(Function));
            return Value;
        }
    }
}