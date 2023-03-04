using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    class Network
    {
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
        public bool Debug = false;
        public bool ShowResult = false;
        public IFunction Function { get; set; }

        public Network(IFunction function, List<Neuron> input, Neuron output, double Result, int LayersNum, int constant = 1)
        {
            if (LayersNum < 1)
            {
                throw new InvalidOperationException("Number of networks elements (layers or neurons) must be bigger than zero");
            }
            if (input.Count < 1)
            {
                throw new InvalidOperationException("Number of input neurons must be bigger than zero");
            }

            Function = function;
            Input = input;
            Layers = new LinkedList<List<Neuron>>();
            Layers.AddFirst(input);
            Neuron Temp;
            List<Neuron> list;
            for (int i = 0; i < LayersNum; i++)
            {
                list = new List<Neuron> ();
                for (int j = 0; j < input.Count; j++)
                {
                    Temp = new Neuron();
                    list.Add(Temp);
                    Temp.RandomizeWeights(Layers.Last.Value, Temp.Value, Const);
                }
                Layers.AddLast(list);
            }
            Output = output;
            Output.RandomizeWeights(Layers.Last.Value, Result, Const);
            Layers.AddLast(new List<Neuron>() { Output });
            Const = constant;
        }
        public Network(IFunction function, LinkedList<List<Neuron>> network, Neuron output, int constant = 1)
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
            Output = output;
            Const = constant;
        }

        public double Calculate()
        {
            string Line = new ('-', 14);
            LinkedListNode<List<Neuron>> Temp = Layers.First.Next;
            if (Debug || ShowResult)
            {
                Console.WriteLine($"{Line}\nInput values:\n{Line}");
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
                    // S = Σ (Wi * xi)
                    // Y = F(S)
                    neuron.WeightSum(Temp == Layers.First.Next ? null : Function);
                    if (Debug)
                    {
                        Console.WriteLine($"\tS#{i}#{j} = {neuron.Value}");
                        Console.WriteLine($"\tY#{i}#{j} = {Function.Calculate(neuron.Value)}");
                    }
                }
                i++;
            } while ((Temp = Temp.Next) != null);
            return Output.Value;//Function.Calculate(Output.Value);
        }

        public void Propagate(double LearningSpeed, double Result, double Epsilon = 0)
        {
            if (LearningSpeed < 0 || LearningSpeed > 1)
            {
                throw new InvalidOperationException("Learning speed is restricted in 0 to 1 (%)");
            }
            if (Debug || ShowResult)
            {
                Console.WriteLine($"Learning Speed: {LearningSpeed}");
                Console.WriteLine("\nInput values:");
                for (int i = 0; i < Input.Count; i++) {
                    Console.WriteLine($"\tX{i}: {Input[i].Value}");
                }
                Console.WriteLine($"\nExpected Result: {Result}\n\n");
            }

            LinkedListNode<List<Neuron>> Temp;
            long counter = 0;
            do
            {
                if (Debug || ShowResult)
                {
                    Console.WriteLine($"Epoch #{counter}");
                }
                
                Calculate();

                double ActualResult = Output.Value;//Function.Calculate(Output.Value);
                GlobalError = Result - ActualResult;//ActualResult - Result;//Result - ActualResult;
                if (Math.Abs(GlobalError) > Epsilon)
                {
                    // Option 1
                    Temp = Layers.First.Next;
                    int i = 0;
                    double delta_j, delta_Wj, average_W = 0;
                    do
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"\nLayer #{i}");
                        }
                        for (int j = 0; j < Temp.Value.Count; j++)
                        {
                            Neuron neuron = Temp.Value[j];

                            if (Debug)
                            {
                                Console.WriteLine($"\tS#{j} = {neuron.Value}");
                                Console.WriteLine($"\tY#{j} = {(Temp == Layers.Last ? neuron.Value : Function.Calculate(neuron.Value)) /*Function.Calculate(neuron.Value)*/}");
                            }
                            for (int k = 0; k < neuron.PreviousWeights.Count; k++)
                            {
                                KeyValuePair<Neuron, double> connection = neuron.PreviousWeights.ToList()[k];
                                // If previous layer is the first - use X and not S
                                double value = (Temp == Layers.First.Next ? connection.Key.Value : Function.Calculate(connection.Key.Value));

                                // Δ#i#j = Error * F'(S#j) * Y#i

                                // #i - previous node
                                // #j - next node
                                delta_j = GlobalError * Function.CalculateDerivative(neuron.Value) * value;
                                // ΔW#i#j = T * Δ#i#j
                                delta_Wj = LearningSpeed * delta_j;
                                if (Debug)
                                {
                                    Console.WriteLine($"\tW#{j}#{k} = {connection.Value}");
                                    Console.WriteLine($"\tDELTA W#{j}#{k} = {delta_Wj}");
                                }
                                // AVERAGE ΔW = (Σ#i (ΔW#i#j)) / N => j = (0; N)
                                average_W += delta_Wj / neuron.PreviousWeights.Count;
                            }
                            if (Debug)
                            {
                                Console.WriteLine($"AVERAGE DELTA W = {average_W}");
                            }
                            for (int k = 0; k < neuron.PreviousWeights.Count; k++)
                            {
                                KeyValuePair<Neuron, double> connection = neuron.PreviousWeights.ToList()[k];
                                // W = W + AVERAGE ΔW
                                neuron.PreviousWeights[connection.Key] += average_W;
                                if (Debug)
                                {
                                    Console.WriteLine($"\tNEW W#{j}#{k} = {neuron.PreviousWeights[connection.Key]}\n");
                                }
                            }
                        }
                        i++;
                    } while ((Temp = Temp.Next) != null);
                    
                    // Option 2
                    /*Temp = Layers.Last;
                    double delta_j;
                    int i = Layers.Count - 1;
                    do
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"\n  Layer #{i}");
                        }
                        for(int j = 0; j < Temp.Value.Count; j++)
                        {
                            Neuron neuron = Temp.Value[j];

                            if (Debug)
                            {
                                Console.WriteLine($"\tS#{j} = {neuron.Value}");
                                Console.WriteLine($"\tY#{j} = {Function.Calculate(neuron.Value)}");
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
                                Console.WriteLine($"\tDELTA #{j} = {neuron.Delta}\n");
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

                                neuron.PreviousWeights[connection.Key] += delta_Wj;

                                if (Debug)
                                {
                                    Console.WriteLine($"\tNEW W#{j}#{k} = {neuron.PreviousWeights[connection.Key]}\n");
                                }
                            }
                        }
                        i++;
                    } while ((Temp = Temp.Next) != null);*/
                }
                
                if (Debug || ShowResult)
                {
                        Console.WriteLine($"\nGlobal Error: {GlobalError}\n");
                    Console.WriteLine($"Expected Result: {Result}");
                    Console.WriteLine($"Actual Result: {Output.Value/*Function.Calculate(Output.Value)*/}\n");
                }
                counter++;
                if (counter > 1000000)
                {
                    throw new Exception("No Solution Found!");
                }
            }while (Math.Abs(GlobalError)/*Result - Function.Calculate(Output.Value)*/ > Epsilon);
            if (Debug || ShowResult)
            {
                Console.WriteLine("Completed!");
            }
        }
    }
}
