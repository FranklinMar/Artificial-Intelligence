namespace Lab1
{

    public class Synapse
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
}
