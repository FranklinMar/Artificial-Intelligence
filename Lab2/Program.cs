using System;
using System.Collections.Generic;
using Lab1;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            
             Dictionary<string, List<int[][]>> Datasets = DatasetManager.ReadJSON(@"..\..\..\dataset.json");
            Convolution ConvolutionLayer = new(new int[,] {
                {0, 2, 0 },
                {2, 1, 2 },
                {0, 2, 0 }
            });
            Dictionary<string, List<int[]>> ConvolutedDatasets = DatasetManager.ProcessDataset(Datasets, Matrix => ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Matrix)));

            int[][] Dataset = Datasets["1"][0];
            DisplayArray(Dataset);
            Console.WriteLine();
            Dataset = ConvolutionLayer.Convolute(Dataset);
            //Dataset = Layer.Convolute(Dataset);

            DisplayArray(Dataset);
            Console.WriteLine();
            Dataset = ConvolutionLayer.MaxPool(Dataset);
            DisplayArray(Dataset);
            
            List <Neuron> Inputs = new();
            List <Neuron> Outputs = new();
            for (int i = 0; i < Dataset.Length; i++)
            {
                for (int j = 0; j < Dataset[i].Length; j++)
                {
                    Inputs.Add(new Neuron(Dataset[i][j]));
                }
            }
            Neuron Output = new(1);
            Outputs.Add(Output);
            Network NeuralNetwork = new (Sigmoid.Instance, Inputs, Outputs, 3);
            //NeuralNetwork.ShowResult = true;
            NeuralNetwork.Propagate(new List<double>() { Output.Value }, 0.1, 0.1, 1E-18);
            double Epochs = 100;
            for (int epoch = 0; epoch < Epochs; epoch++)
            {
                Console.WriteLine((epoch / Epochs * 100) + "%");
                foreach (KeyValuePair<string, List<int[]>> DataPair in ConvolutedDatasets)
                {
                    List<double> ResultList = new() { Double.Parse(DataPair.Key) };
                    foreach (int[] Data in DataPair.Value)
                    {
                        for (int i = 0; i < Data.Length; i++)
                        {
                            Inputs[0].Value = Data[i];
                        }
                        //Console.WriteLine("Result: \nArray: ");
                        //DisplayArray(new int[][] { Data });
                        DatasetManager.Shuffle(ResultList);
                        NeuralNetwork.Propagate(ResultList, 0.1, 0.1, 1E-18);
                    }
                }
            }
            Console.WriteLine("LEARNING RESULT: ");
            DatasetManager.Shuffle(Datasets["1"]);
            Console.WriteLine("'1': ");
            DisplayArray(Datasets["1"][0]);
            Console.WriteLine($"Result: {NeuralNetwork.Calculate(Array.ConvertAll<int, double>(DatasetManager.FlattenArray(ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Datasets["1"][0]))), x => x))[0]}");
            DatasetManager.Shuffle(Datasets["2"]);
            Console.WriteLine("'2': ");
            DisplayArray(Datasets["2"][0]);
            Console.WriteLine($"Result: {NeuralNetwork.Calculate(Array.ConvertAll<int, double>(DatasetManager.FlattenArray(ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Datasets["2"][0]))), x => x))[0]}");
            DatasetManager.Shuffle(Datasets["3"]);
            Console.WriteLine("'3': ");
            DisplayArray(Datasets["3"][0]);
            Console.WriteLine($"Result: {NeuralNetwork.Calculate(Array.ConvertAll<int, double>(DatasetManager.FlattenArray(ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Datasets["3"][0]))), x => x))[0]}");

        }

        public static void DisplayArray(int [][] Dataset)
        {
            for (int i = 0; i < Dataset.Length; i++)
            {
                for (int j = 0; j < Dataset[i].Length; j++)
                {
                    Console.Write(Dataset[i][j] + (j < Dataset[i].Length - 1 ? ", " : ""));
                }
                Console.WriteLine();
            }
        }
    }
}