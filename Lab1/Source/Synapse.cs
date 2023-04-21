using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    /*public interface ISynapse
    {
        public double Output(IFunction Function = null);
    }*/

    public class Synapse//: ISynapse
    {
        public Neuron FromNeuron { get; protected set; }
        public Neuron ToNeuron { get; protected set; }
        public double Weight { get; set; }
        public double Delta { get; set; } = 0;

        public Synapse(Neuron From, Neuron To, double weight)
        {
            FromNeuron = From;
            ToNeuron = To;
            Weight = weight;
        }

        public Synapse(Neuron From, Neuron To)
        {
            FromNeuron = From;
            ToNeuron = To;
            var Generator = new SecureRandom();
            Weight = Generator.NextDouble();
        }

        public double Output(IFunction Function)
        {
            Delta = 0;
            return (Function == null ? FromNeuron.Value : Function.Calculate(FromNeuron.Value)) * Weight;
        }
    }

    /*public class InputSynapse: ISynapse
    {

        public double _Output;
        //public Neuron ToNeuron { get; set; }

        public InputSynapse(double output = 0)
        {
            //ToNeuron = To;
            _Output = output;
        }

        public double Output(IFunction Function) => _Output;
    }*/
}
