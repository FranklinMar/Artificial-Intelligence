using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class NeuronLayer
    {
        public List<Neuron> Neurons { get; protected set; }
        /*public NeuronLayer First;
        public NeuronLayer Last;*/
        public NeuronLayer Previous;
        public NeuronLayer Next;

        public int Index { get; set; }

        public Neuron this[int i]
        {
            get { return Neurons[i]; }
            set { Neurons[i] = value; }
        }

        public int Count
        {
            get { return Neurons.Count; }
        }

        public NeuronLayer(List<Neuron> neurons = null, int index = 0)
        {
            Neurons = neurons ?? new();
            Index = index;
        }

        public void ConnectPrevious(NeuronLayer PreviousLayer, double? Weight = null/*, bool InputLayer = false*/)
        {
            Previous = PreviousLayer;
            PreviousLayer.Next = this;
            PreviousLayer.Neurons.ForEach(PreviousNeuron => Neurons.ForEach(Neuron => PreviousNeuron.AddOutput(Neuron, Weight/*, InputLayer ? 1 : null*/)));
            /*PreviousLayer.Last = Last;
            PreviousLayer.First = First;*/
        }

        public void ConnectNext(NeuronLayer NextLayer, double? Weight = null/*, bool InputLayer = false*/)
        {
            Next = NextLayer;
            NextLayer.Previous = this;
            NextLayer.Neurons.ForEach(NextNeuron => Neurons.ForEach(Neuron => NextNeuron.AddInput(Neuron, Weight/*, InputLayer ? 1 : null*/)));
            /*PreviousLayer.Last = Last;
            PreviousLayer.First = First;*/
        }

        public void ForEach(Action<Neuron> action) => Neurons.ForEach(action);

        public bool IsFirst() => Previous == null;

        public bool IsLast() => Next == null;
    }
}
