using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Network
    {
        //prizate LinkedList<Layer> Layers { get; set; }
        private LinkedList<List<Neuron>> Layers { get; set; }
        public Neuron Output { get; private set; }
        public List<Neuron> Input { get; private set; }
        private int MetaConst;
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
        // public double Result;
        public bool Debug = false;
        public IFunction Function { get; set; }
        // public Layer Input;

        public Network(IFunction function, List<Neuron> input, Neuron output, double Result, int LayersNum, int constant = 1/*, int neurons = 1*/)
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
            //Result = result;
            //Layer Input = new (input);
            //Layer layer = Input;
            //Layers = new LinkedList<Layer>();
            Layers = new LinkedList<List<Neuron>>();
            Layers.AddFirst(input);
            Neuron Temp;
            List<Neuron> list;
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
            Output = output;//new();
            Output.RandomizeWeights(Layers.Last.Value, Result, Const);
            Layers.AddLast(new List<Neuron>() { Output });
        }
        public Network(IFunction function, LinkedList<List<Neuron>> network, Neuron output, int constant = 1/*, double result*/)
        {
            if (network.Count < 1)
            {
                throw new InvalidOperationException("Number of networks layers must be bigger than zero");
            }
            if (!Enumerable.All<List<Neuron>>(network, list => list.Count > 0))
            {
                throw new InvalidOperationException("Number of neurons in a layer must be bigger than zero");
            }
            Layers = network;
            Function = function;
            Input = network.First.Value;
            //Result = result;
            Output = output;
            Const = constant;
        }

        public double Calculate()
        {
            LinkedListNode<List<Neuron>> Temp = Layers.First.Next;
            //Console.WriteLine(Temp);
            if (Debug)
            {
                Console.WriteLine("\nInput values:");
                for (int j = 0; j < Input.Count; j++)
                {
                    Console.WriteLine($"\tX{j}: {Input[j].Value}");
                }
            }
            int i = 0;
            do
            {
                if (Debug)
                {
                    Console.WriteLine($"Layer #{i}");
                }
                for (int j = 0; j < Temp.Value.Count; j++)
                {
                    Neuron neuron = Temp.Value[j];
                    // Y = F(S)
                    // S = Σ (Wi * xi)
                    neuron.WeightSum(Temp == Layers.First.Next ? null : Function);
                    if (Debug)
                    {
                        Console.WriteLine($"\tS#{i}#{j} = {neuron.Value}");
                        Console.WriteLine($"\tY#{i}#{j} = {Function.Calculate(neuron.Value)}");
                    }
                }
                i++;
            } while ((Temp = Temp.Next) != null);
            return Function.Calculate(Output.Value);
        }

        public void Propagate(double LearningSpeed, double Result)
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

            LinkedListNode<List<Neuron>> Temp;
            while (Function.Calculate(Output.Value) != Result)
            {
                Calculate();
                /*Temp = Layers.First.Next;
                int i = 0;                
                do
                {
                    if (Debug)
                    {
                        Console.WriteLine($"Layer #{i}");
                    }
                    for (int j = 0; j < Temp.Value.Count; j++)
                    {
                        Neuron neuron = Temp.Value[j];
                        // Y = F(S)
                        // S = Σ (Wi * xi)
                        neuron.WeightSum(Temp == Layers.First.Next ? null : Function);
                        if (Debug)
                        {
                            Console.WriteLine($"\tS#{i}#{j} = {neuron.Value}");
                            Console.WriteLine($"\tY#{i}#{j} = {Function.Calculate(neuron.Value)}");
                        }
                    }
                    i++;
                } while ((Temp = Temp.Next) != null);*/
                double ActualResult = Function.Calculate(Output.Value);
                GlobalError = Result - ActualResult;
                //double error1 = -x1 * 2 * Error;
                //Temp = Layers.Last.Previous;
                if (GlobalError != 0)
                {
                    Temp = Layers.Last;
                    double delta_j;
                    int i = Layers.Count - 1;
                    do
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"Layer #{i}");
                        }
                        for(int j = 0; j < Temp.Value.Count; j++)
                        {
                            Neuron neuron = Temp.Value[j];

                            if (Debug)
                            {
                                Console.WriteLine($"\tS#{j} = {neuron.Value}"/*$"\tS#{i}#{j} = {neuron.Value}"*/);
                                Console.WriteLine($"\tY#{j} = {Function.Calculate(neuron.Value)}"/*$"\tY#{i}#{j} = {Function.Calculate(neuron.Value)}"*/);
                            }
                            if (Temp == Layers.Last)
                            {
                                delta_j = ActualResult * (1 - ActualResult) * GlobalError;
                            }
                            else
                            {
                                List<Neuron> NextLayer = Temp.Next.Value;
                                Neuron connectedNeuron = NextLayer.Find(neuro => neuro.PreviousWeights.Keys.Contains(neuron));
                                if (Temp != Layers.First)
                                {
                                    delta_j = Function.Calculate(neuron.Value) * (1 - Function.Calculate(neuron.Value)) * connectedNeuron.PreviousWeights[neuron] * connectedNeuron.Delta;
                                }
                                else
                                {
                                    delta_j = neuron.Value * (1 - neuron.Value) * connectedNeuron.PreviousWeights[neuron] * connectedNeuron.Delta;
                                }
                            }
                            neuron.Delta = delta_j;
                            if (Debug)
                            {
                                Console.WriteLine($"\tDELTA #{j} = {neuron.Delta}\n"/*$"\tΔ#{i}#{j} = {neuron.Delta}\n"*/);
                            }
                        }
                        i--;
                        Temp = Temp.Previous;
                    } while (Temp != null);

                    double delta_Wj;
                    Temp = Layers.First.Next;
                    i = 0;
                    do
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"Layer #{i}");
                        }
                        for (int j = 0; j < Temp.Value.Count; j++)
                        {
                            Neuron neuron = Temp.Value[j];
                            for (int k = 0; k < neuron.PreviousWeights.Count; k++)
                            {
                                KeyValuePair<Neuron, double> connection = neuron.PreviousWeights.ToList()[k];
                                delta_Wj = LearningSpeed * neuron.Delta * (Temp == Layers.First.Next ? connection.Value : Function.Calculate(connection.Value));
                                if (Debug)
                                {
                                    Console.WriteLine($"\tW#{j}#{k} = {connection.Value}");
                                    Console.WriteLine($"\tDELTA W#{j}#{k} = {delta_Wj}");
                                }

                                neuron.PreviousWeights[connection.Key] -= delta_Wj;

                                if (Debug)
                                {
                                    Console.WriteLine($"\tNEW W#{j}#{k} = {neuron.PreviousWeights[connection.Key]}\n");
                                }
                            }
                        }
                        i++;
                    } while ((Temp = Temp.Next) != null);
                }
                
                if (Debug)
                {
                    Console.WriteLine($"\nGlobal Error: {GlobalError}\n");
                    Console.WriteLine($"Expected Result: {Result}");
                    Console.WriteLine($"Actual Result: {Function.Calculate(Output.Value)}\n");
                }

            }
            if (Debug)
            {
                Console.WriteLine("Done!");
            }
        }
    }
}
