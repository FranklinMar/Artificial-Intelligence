using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Lab1;

namespace Lab2
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<string, List<int[][]>> Templates = ReadJSON(@"..\..\..\numbers.json");
            //LoadJSON("dataset.json", ExpandDataset(Templates));
            Dictionary<string, List<int[][]>> Datasets = DatasetManager.ReadJSON(@"..\..\..\dataset.json");
            Convolution ConvolutionLayer = new(new int[,]
            {
                {0, 2, 0 },
                {2, 1, 2 },
                {0, 2, 0 }
            });
            Dictionary<string, List<int[]>> ConvolutedDatasets = DatasetManager.ConvoluteDataset(Datasets, ConvolutionLayer);

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
            for (int epoch = 0; epoch < 10; epoch++)
            {
                Console.WriteLine((epoch / 10.0) + "%");
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
