using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Layer
    {
        private IFunction Function { get; set; }
        private int MetaConst = 5;
        public int Const
        {
            get { return MetaConst; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Positive or zero values only");
                }
                MetaConst = value;
            }
        }
        private IList<Neuron> MetaNeurons;
        public IList<Neuron> Neurons 
        { 
            get { return MetaNeurons; }
            set 
            { 
                PreviousWeights = null;
                MetaNeurons = value;
            }
        }
        #nullable enable
        private Layer? MetaPreviousLayer;
        public Layer? PreviousLayer
        {
            get { return MetaPreviousLayer; }
            set
            {
                PreviousWeights = null;
                MetaPreviousLayer = value;
            }
        }

        public IDictionary<Tuple<Neuron, Neuron>, double>? PreviousWeights { get; private set; }

        public Layer(IList<Neuron> neurons, IFunction function, Layer? previous = null)
        {
            Neurons = neurons;
            Function = function;
            PreviousLayer = previous;
        }

        public void RandomizeWeights()
        {
            if (PreviousLayer == null)
            {
                throw new InvalidOperationException("No previous layer found");
            }
            
            Random generator = new ();
            PreviousWeights = new Dictionary<Tuple<Neuron, Neuron>, double>();
            foreach (Neuron thisNeuron in Neurons)
            {
                foreach (Neuron previousNeuron in PreviousLayer.Neurons)
                {
                    /*Double Const = 5;*/
                    // random.NextDouble() * (Maximum - Minimum) + Minimum;
                    // Maximum = Value + Const
                    // Minimum = Value - Const
                    // Maximum - Minimum = Value + Const - Value + Const = 2 * Const
                    PreviousWeights.Add(new Tuple<Neuron, Neuron>(thisNeuron, previousNeuron), generator.NextDouble() * 2 * Const + thisNeuron.Output - Const);
                    /*PreviousWeights.Add(new Tuple<Neuron, Neuron>(thisNeuron, previousNeuron), generator.Next(Math.Floor(thisNeuron.Output) - Const, Math.Ceiling(thisNeuron.Output) + Const));*/

                }
            }
        } 
    }
}
