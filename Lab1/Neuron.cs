using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{

    class Neuron
    {
        //private IEnumerable<double>? InputWeights { get; set; }
        public double Output { get; set; }
        /*public double Output { 
            get { return Function.Calculate(Value); } 
            set { Value = value; } 
        }*/

        public Neuron(/*IFunction function, */double value/*, IEnumerable<double>? input = null*/)
        {
            /*Function = function;*/    
            Output = value;
            //InputWeights = input;
        }

        public double WeightSum(Layer Layer)
        {
            if (Layer.PreviousWeights == null)
            {
                throw new InvalidOperationException("No weights of neuron connection found");
            }


            foreach (KeyValuePair<Tuple<Neuron, Neuron>, double> i in Layer.PreviousWeights)
            {
                Console.WriteLine(i.Key + ": " + i.Value);
            }
            return 0;
        }
    }

}
