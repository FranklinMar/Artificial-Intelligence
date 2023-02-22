using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Network
    {
        private IFunction Function { get; set; }
        private LinkedList<Layer> Layers { get; set; }
        public double GlobalError { get; private set; }
        public double Result;
        public Neuron Output;
        public Layer Input;

        public Network(IFunction function, IList<Neuron> input, double result, int LayersNum = 1/*, int neurons = 1*/)
        {
            if (LayersNum < 1 /*|| neurons < 1 || input.Count < 1*/)
            {
                throw new InvalidOperationException("Number of networks elements (layers or neurons) must be bigger than zero");
            }
            if (input.Count < 1)
            {
                throw new InvalidOperationException("Number of input neurons must be bigger than zero");
            }
            Function = function;
            Layer Input = new (input);
            Layer layer = Input;
            Layers = new LinkedList<Layer>();
            IList<Neuron> list;
            for (int i = 0; i < LayersNum; i++)
            {
                list = new List<Neuron>(input.Count);
                for (int j = 0; j < list.Count; j++)
                {
                    list.Add(new Neuron());
                }
                layer = new(list, layer);
            }
            Output = new();
        }

        public void Propagate(double LearningSpeed)
        {
            if (LearningSpeed < 0 || LearningSpeed > 1)
            {
                throw new InvalidOperationException("Learning speed is restricted in 0 to 1 (%)");
            }

            do
            {
                
            } while (Function.Calculate(Output.Value) != Result);
        }
    }
}
