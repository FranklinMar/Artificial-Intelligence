using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class Network
    {
        private static string LINE = new('-', 14);
        //private LinkedList<List<Neuron>> Layers { get; set; }
        public NeuronLayer Output { get; private set; } = null;
        //public NeuronLayer First { get; private set; }
        public NeuronLayer Input { get; private set; }

        /*private int MetaConst;
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
        }*/
        public double GlobalError { get; private set; }
        public bool Debug = false;
        public bool ShowResult = false;
        public IFunction Function { get; set; }

        public Network(IFunction function, int [] NeuronLayers)
        {
            if (NeuronLayers.Length < 2)
            {
                throw new ArgumentException("Network with no neurons cannot be created");
            }

            if (Enumerable.Any(NeuronLayers, Integer => Integer <= 0))
            {
                throw new ArgumentException("Layer number of neurons must be natural number");
            }
            Function = function;
            List<Neuron> Neurons;
            NeuronLayer Previous;

            Neurons = new();
            for (int j = 0; j < NeuronLayers[0]; j++)
            {
                Neurons.Add(new Neuron());
            }
            Input = new(Neurons, 0);
            Neurons = new();
            for (int j = 0; j < NeuronLayers[1]; j++)
            {
                Neurons.Add(new Neuron());
            }
            Output = new NeuronLayer(Neurons, 1);
            Output.ConnectPrevious(Input);
            for (int i = 2; i < NeuronLayers.Length; i++)
            {
                Neurons = new();
                for (int j = 0; j < NeuronLayers[i]; j++)
                {
                    Neurons.Add(new Neuron());
                }
                Previous = Output;
                Output = new(Neurons, i);
                Output.ConnectPrevious(Previous);
            }
        }

        /*public Network(IFunction function, List<Neuron> input, List<Neuron> output, int LayersNum)
        {
            if (LayersNum < 1)
            {
                throw new InvalidOperationException("Number of networks elements (layers or neurons) must be bigger than zero");
            }
            if (input.Count < 1)
            {
                throw new InvalidOperationException("Number of input neurons must be bigger than zero");
            }

            if (output.Count < 1)
            {
                throw new InvalidOperationException("Number of input neurons must be bigger than zero");
            }
            Function = function;
            Input = input;
            Output = output;
            Layers = new LinkedList<List<Neuron>>();
            //Const = constant;
            Layers.AddFirst(input);
            Neuron Temp;
            List<Neuron> list;
            for (int i = 0; i < LayersNum; i++)
            {
                list = new List<Neuron>();
                for (int j = 0; j < Math.Max(input.Count, output.Count); j++)
                {
                    Temp = new Neuron(Layers.Last.Value[i].Value);
                    list.Add(Temp);
                    Temp.RandomizeWeights(Layers.Last.Value*//*, Temp.Value, Const*//*);
                }
                Layers.AddLast(list);
            }
            //Output.RandomizeWeights(Layers.Last.Value, Result, Const);
            foreach (Neuron neuron in Output)
            {
                neuron.RandomizeWeights(Layers.Last.Value*//*, neuron.Value, Const*//*);
            }
            Layers.AddLast(output);
        }
        public Network(IFunction function, LinkedList<List<Neuron>> network)
        {
            if (network.Count < 1)
            {
                throw new InvalidOperationException("Number of networks layers must be bigger than zero");
            }
            if (!Enumerable.All(network, list => list.Count > 0))
            {
                throw new InvalidOperationException("Number of neurons in a layer must be bigger than zero");
            }
            Layers = network;
            Function = function;
            Input = network.First.Value;
            Output = network.Last.Value;
            //Const = constant;
        }

        public void Print()
        {
            Layers.ToList().ForEach(i => Console.WriteLine(string.Concat(Enumerable.Repeat("0 ", i.Count))));
        }*/

        public double[] Calculate(double [] Array, bool ActivateInput = false)
        {
            if (Array.Length != Input.Neurons.Count)
            {
                throw new ArgumentException("Input array size doesn't correspond to Input neurons number");
            }
            for(int i = 0; i < Array.Length; i++)
            {
                Input[i].Value = Array[i];
            }
            return Calculate(ActivateInput);
        }

        public double[] Calculate(bool ActivateInput = false)
        {
            NeuronLayer Temp = Input.Next;
            if (Debug || ShowResult)
            {
                Console.WriteLine($"{LINE}\nInput values:\n{LINE}");
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
                for (int j = 0; j < Temp.Count; j++)
                {
                    Neuron neuron = Temp[j];
                    // S = Σ (Wi * xi)
                    // Y = F(S)
                    neuron.CalculateValue(Temp == Input.Next && !ActivateInput ? null : Function);
                    neuron.Delta = 0;
                    //neuron.WeightSum(Temp == Layers.First.Next ? null : Function);
                    if (Debug)
                    {
                        Console.WriteLine($"\tS#{i}#{j} = {neuron.Value}");
                        Console.WriteLine($"\tY#{i}#{j} = {Function.Calculate(neuron.Value)}");
                    }
                }
                i++;
            } while ((Temp = Temp.Next) != null);

            double[] Result = new double[Output.Count];
            for (int l = 0; l < Result.Length; l++)
            {
                Result[l] = Output[l].Value;
            }
            return Result;
            //return Output.Value;//Function.Calculate(Output.Value);
        }

        public void Propagate(List<double> Result, double LearningSpeed, double Momentum, double Epsilon = 0)
        {
            if (LearningSpeed < 0 || LearningSpeed > 1)
            {
                throw new ArgumentException("Learning speed is restricted in 0 to 1 (%)");
            }
            if (Momentum < 0 || Momentum > 1)
            {
                throw new ArgumentException("Momentum is restricted in 0 to 1 (%)");
            }
            if (Result.Count != Output.Count)
            {
                throw new ArgumentException("Expected results number doesn't match output layer");
            }
            if (Debug || ShowResult)
            {
                Console.WriteLine($"Learning Speed: {LearningSpeed}");
                /*Console.WriteLine("\nInput values:");
                for (int i = 0; i < Input.Count; i++) {
                    Console.WriteLine($"\tX{i}: {Input[i].Value}");
                }*/
                //Console.WriteLine($"\nExpected Result: {Result}\n\n");
                Console.WriteLine("\n\nExpected Results:");
                for (int i = 0; i < Result.Count; i++)
                {
                    Console.WriteLine($"\tY{i}: {Result[i]}");
                }
                Console.WriteLine();
            }

            Calculate();
            GlobalError = 0;

            long Counter = 0;
            NeuronLayer Temp;
            Neuron Neuron;
            double Error;
            double Y;
            double Delta_Wjk;

            for (int i = 0; i < Output.Count; i++)
            {
                Error = (Result[i] - Output[i].Value);
                GlobalError += Error * Error / 2;
            }
            while (GlobalError > Epsilon)
            {
                if (Debug || ShowResult)
                {
                    Console.WriteLine($"Iteration #{Counter}");
                }

                //double ActualResult = Output.Value;//Function.Calculate(Output.Value);
                //GlobalError = Result - ActualResult;//ActualResult - Result;//Result - ActualResult;
                /*GlobalError = 0;
                
                for (int i = 0; i < Output.Count; i++)
                {
                    Error = (Result[i] - Output[i].Value);
                    GlobalError += Error * Error / 2;
                }
                if (GlobalError > Epsilon)
                {*/
                    // Option 1
                    /*Temp = Layers.First.Next;
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
                                Console.WriteLine($"\tY#{j} = {(Temp == Layers.Last ? neuron.Value : Function.Calculate(neuron.Value))}"); //Function.Calculate(neuron.Value)}");
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
                    } while ((Temp = Temp.Next) != null);*/

                // Option 2
                Temp = Output;
                //double delta_j;
                // int i = Layers.Count - 1;
                do
                {
                    if (Debug)
                    {
                        Console.WriteLine($"\n  Layer #{Temp.Index}");
                    }
                    for (int j = 0; j < Temp.Count; j++)
                    {
                        Neuron = Temp[j];
                        Y = (Temp == Input) ? Neuron.Value : Function.Calculate(Neuron.Value);

                        if (Debug)
                        {
                            Console.WriteLine($"\tS#{j} = {Neuron.Value}");
                            Console.WriteLine($"\tY#{j} = {Function.Calculate(Neuron.Value)}");
                        }
                        if (Temp == Output)
                        {
                            //delta_j = ActualResult * (1 - ActualResult) * GlobalError;
                            /*delta_j*/Neuron.Delta = (Neuron.Value - Result[j]) * Function.CalculateDerivative(Neuron.Value);//neuron.Value * (1 - neuron.Value) * (Result[j] - neuron.Value); //Function.Calculate(neuron.Value) * (1 - Function.Calculate(neuron.Value)) * (Result[j] - Function.Calculate(neuron.Value));
                        }
                        else
                        {
                            //List<Neuron> NextLayer = Temp.Next.Value;
                            double Sum = 0, N = 0; //(Temp == Layers.First) ? neuron.Value : Function.Calculate(neuron.Value);
                            //NextLayer.
                            /*Temp.Next.ForEach(NextNeuron => Sum += NextNeuron.PreviousWeights.Keys.Contains(Neuron) ? NextNeuron.Delta * NextNeuron.PreviousWeights[Neuron] * (++N / N) : 0);*/
                            foreach(Synapse synapse in Neuron.Outputs)
                            {
                                Sum += synapse.Weight * synapse.ToNeuron.Delta;
                                ++N;
                            }
                                
                            //Neuron connectedNeuron = NextLayer.Find(neuro => neuro.PreviousWeights.Keys.Contains(neuron));
                            /*if (Temp != Layers.First)
                            {
                                //delta_j = Function.Calculate(neuron.Value) * (1 - Function.Calculate(neuron.Value)) * connectedNeuron.PreviousWeights[neuron] * connectedNeuron.Delta;
                                delta_j = Function.Calculate(neuron.Value) * (1 - Function.Calculate(neuron.Value)) * Sum;
                            }
                            else
                            {
                                delta_j = neuron.Value * (1 - neuron.Value) * connectedNeuron.PreviousWeights[neuron] * connectedNeuron.Delta;
                            }*/
                            Neuron.Delta = Function.CalculateDerivative(Neuron.Value) * Sum;//Value * (1 - Value) * Sum;
                        }
                        //neuron.Delta = delta_j;
                        if (Debug)
                        {
                            Console.WriteLine($"\tDELTA #{j} = {Neuron.Delta}\n");
                        }
                        for (int k = 0; k < Neuron.Outputs.Count; k++) {
                            Synapse synapse = Neuron.Outputs[k];
                            Delta_Wjk = -LearningSpeed * Neuron.Value * synapse.ToNeuron.Delta;
                            if (Debug)
                            {
                                Console.WriteLine($"\tW#{j}#{k} = {synapse.Weight}");
                                Console.WriteLine($"\tDELTA W#{j}#{k} = {Delta_Wjk}");
                            }
                            synapse.Weight += Delta_Wjk + Momentum * synapse.Delta;
                            synapse.Delta = Delta_Wjk;
                            if (Debug) {
                                Console.WriteLine($"\tNEW W#{j}#{k} = {synapse.Weight}\n");
                            }
                        }
                    }
                    //i--;
                    Temp = Temp.Previous;
                } while (Temp != null);

                    /*Temp = Input.Next;
                    //i = 0;
                    do
                    {
                        if (Debug)
                        {
                            Console.WriteLine($"Layer #{Temp.Index}");
                        }
                        for (int j = 0; j < Temp.Count; j++)
                        {
                            Neuron = Temp[j];
                            for (int k = 0; k < Neuron.PreviousWeights.Count; k++)
                            {
                                KeyValuePair<Neuron, double> connection = Neuron.PreviousWeights.ToList()[k];
                                //delta_Wj = LearningSpeed * neuron.Delta * Function.Calculate(neuron.Value);//(Temp == Layers.First.Next ? connection.Value : Function.Calculate(connection.Value));
                                delta_Wj = -LearningSpeed * Neuron.Value * connection.Key.Delta;//Function.Calculate(neuron.Value)//(Temp == Layers.First.Next ? connection.Key.Value : Function.Calculate(connection.Key.Value));
                                Neuron.PreviousWeights[connection.Key] += delta_Wj + Momentum * Neuron.DeltaWeights[connection.Key];
                                Neuron.DeltaWeights[connection.Key] = delta_Wj;
                                if (Debug)
                                {
                                    Console.WriteLine($"\tW#{j}#{k} = {connection.Value}");
                                    Console.WriteLine($"\tDELTA W#{j}#{k} = {delta_Wj}");
                                    Console.WriteLine($"\tNEW W#{j}#{k} = {Neuron.PreviousWeights[connection.Key]}\n");
                                }
                            }
                        }
                    } while ((Temp = Temp.Next) != null);*/
                //}
                
                if (Debug || ShowResult)
                {
                    Console.WriteLine($"\nGlobal Error: {GlobalError}\nResults:");
                    for (int i = 0; i < Result.Count; i++)
                    {
                        Console.WriteLine($"Expected Result #{i}: {Result[i]}");
                        Console.WriteLine($"Actual Result #{i}: {Output[i].Value/*Output.Value*//*Function.Calculate(Output.Value)*/}\n");
                    }
                    //Console.WriteLine($"Expected Result: {Result}");
                    //Console.WriteLine($"Actual Result: {/*Output.Value*//*Function.Calculate(Output.Value)*/}\n");
                }
                Counter++;
                Calculate();
                GlobalError = 0;

                for (int i = 0; i < Output.Count; i++)
                {
                    Error = (Result[i] - Output[i].Value);
                    GlobalError += Error * Error / 2;
                }
                /*if (counter > 1000000)
                {
                    throw new Exception("No Solution Found!");
                }*/
            } /*while (Math.Abs(GlobalError)*//*Result - Function.Calculate(Output.Value)*//* > Epsilon);*/
            if (Debug || ShowResult)
            {
                Console.WriteLine("Completed!");
            }
        }
    }
}