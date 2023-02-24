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
        //private LinkedList<Layer> Layers { get; set; }
        private LinkedList<IList<Neuron>> Layers { get; set; }
        public Neuron Output { get; private set; }
        public IList<Neuron> Input { get; private set; }
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
        public double GlobalError { get; private set; }
        public double Result;
        public bool Debug = false;
        //public Layer Input;

        public Network(IFunction function, IList<Neuron> input, double result, int LayersNum, int constant = 5/*, int neurons = 1*/)
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
            Input = input;
            Const = constant;
            Result = result;
            //Layer Input = new (input);
            //Layer layer = Input;
            //Layers = new LinkedList<Layer>();
            Layers = new LinkedList<IList<Neuron>>();
            Layers.AddFirst(input);
            Neuron Temp;
            IList<Neuron> list;
            for (int i = 1; i < LayersNum; i++)
            {
                list = new List<Neuron> (input.Count);
                for (int j = 0; j < list.Count; j++)
                {
                    Temp = new Neuron();
                    list.Add(Temp);
                    Temp.RandomizeWeights(input, Temp.Value, Const);
                }
                Layers.AddLast(list);
                input = list;
                //layer = new(list, layer);
            }
            Output = new();
            Output.RandomizeWeights(Layers.Last.Value, Result, Const);
            Layers.AddLast(new List<Neuron>() { Output });
        }
        public Network(IFunction function, LinkedList<IList<Neuron>> network, Neuron output, double result)
        {
            if (network.Count < 1)
            {
                throw new InvalidOperationException("Number of networks layers must be bigger than zero");
            }
            if (!Enumerable.All<IList<Neuron>>(network, list => list.Count > 0))
            {
                throw new InvalidOperationException("Number of neurons in a layer must be bigger than zero");
            }
            Function = function;
            Input = network.First.Value;
            Result = result;
            Output = output;
        }

        public void Propagate(double LearningSpeed)
        {
            if (LearningSpeed < 0 || LearningSpeed > 1)
            {
                throw new InvalidOperationException("Learning speed is restricted in 0 to 1 (%)");
            }
            if (Debug)
            {
                Console.WriteLine($"Learning Speed: {LearningSpeed}");
                Console.WriteLine("\nInput values:");
                for (int i = 0; i < Input.Count; i++) {
                    Console.WriteLine($"\tX{i}: {Input[i].Value}");
                }
                Console.WriteLine($"\nExpected Result: {Result}\n\n");
            }

            LinkedListNode<IList<Neuron>> Temp;
            while (Function.Calculate(Output.Value) != Result)
            {
                Temp = Layers.First.Next;
                //Console.WriteLine(Temp);
                int i = 0;
                
                do
                {
                    if (Debug)
                    {
                        Console.WriteLine($"Layer #{i}");
                    }
                    for (int j = 0; j < Temp.Value.Count; j++)// Neuron neuron in Temp.Value)
                    {
                        Neuron neuron = Temp.Value[j];
                        // Y = F(S)
                        // S = Σ (Wi * xi)
                        neuron.WeightSum(Function);
                        if (Debug)
                        {
                            Console.WriteLine($"Y#{i}#{j} = {neuron.Value}");
                        }
                    }
                    i++;
                } while ((Temp = Temp.Next) != null);
                if (/*Function.Calculate(Output.WeightSum(Function))*/Function.Calculate(Output.Value) != Result)
                {
                    // Δ = y - Y
                    GlobalError = Result - Function.Calculate(Output.Value);
                    if (Debug)
                    { 
                        Console.WriteLine($"Actual Result: {Function.Calculate(Output.Value)}");
                        Console.WriteLine($"Global Error: {GlobalError}\n");
                    }
                    Temp = Layers.First.Next;
                    double delta_i;
                    double delta_Wi;
                    //Console.WriteLine(Temp);
                    do
                    {
                        foreach (Neuron neuron in Temp.Value)
                        {
                            foreach (KeyValuePair<Neuron, double> pair in neuron.PreviousWeights)
                            {
                                // Δi = Δ * w * F'(S)
                                delta_i = GlobalError * pair.Value * Function.CalculateDerivative(pair.Key.Value);
                                // ΔWi = Δi * E
                                delta_Wi = delta_i * LearningSpeed;
                                // New Wi = Wi - ΔWi
                                neuron.PreviousWeights[pair.Key] = pair.Value - delta_Wi;
                            }
                        }

                        /*foreach (KeyValuePair<Neuron, double> pair in Output.PreviousWeights)
                        {
                            // Δi = Δ * w * F'(S)
                            delta_i = GlobalError * pair.Value * Function.CalculateDerivative(pair.Key.Value);
                            // ΔWi = Δi * E
                            delta_Wi = delta_i * LearningSpeed;
                            // New Wi = Wi - ΔWi
                            Output.PreviousWeights[pair.Key] = pair.Value - delta_Wi;
                        }*/
                    } while ((Temp = Temp.Next) != null);
                }
            }
            if (Debug)
            {
                Console.WriteLine("Done!");
            }
        }
    }
}
