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
                foreach(Neuron neuron in value)
                {
                    neuron.NullifyWeights();
                }
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
                foreach (Neuron neuron in MetaNeurons)
                {
                    if (value != null)
                    {
                        neuron.RandomizeWeights(value, Const);
                    } 
                    else
                    {
                        neuron.NullifyWeights();
                    }
                }
                MetaPreviousLayer = value;
            }
        }

        public Layer(IList<Neuron> neurons, IFunction function, Layer? previous = null)
        {
            Neurons = neurons;
            Function = function;
            PreviousLayer = previous;
        }

    }
}
