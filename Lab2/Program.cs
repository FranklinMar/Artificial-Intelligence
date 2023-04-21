using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Lab1;

namespace Lab2
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [STAThread]
        static void Main(string[] args)
        {
            AllocConsole();
            string LINE = new('-', 25);
            Dictionary<string, List<int[][]>> Datasets = DatasetManager.ReadJSON(@"..\..\..\dataset.json");
            Convolution ConvolutionLayer = new(new int[,] {
                {1, 0, 1 },
                {0, 1, 0 },
                {1, 0, 1 }
            });
            Dictionary<string, List<int[]>> ConvolutedDatasets = DatasetManager.ProcessDataset(Datasets, Matrix => ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Matrix)));
            int[][] Dataset;
            for (int i = 1; i < 4; i++) {
                DatasetManager.Shuffle(Datasets[$"{i}"]);
                Dataset = Datasets[$"{i}"][0];
                Console.WriteLine($"\n{LINE}\n\t{i}:\n{LINE}\n");
                DisplayArray(Dataset);
                Console.WriteLine();
                Dataset = ConvolutionLayer.Convolute(Dataset);
                DisplayArray(Dataset);
                Console.WriteLine();
                Dataset = ConvolutionLayer.MaxPool(Dataset);
                DisplayArray(Dataset);
            }
            Console.Read();
            Dataset = Datasets[$"1"][0];
            Dataset = ConvolutionLayer.MaxPool(ConvolutionLayer.Convolute(Dataset));
            int Counter = 0;
            for (int i = 0; i < Dataset.Length; i++)
            {
                Counter += Dataset[i].Length;
            }
            Network NeuralNetwork = new (Sigmoid.Instance, new int[] { Counter, 3, 3, 1 });
            //NeuralNetwork.ShowResult = true;
            //NeuralNetwork.Propagate(new List<double>() { Output.Value }, 0.1, 0.1, 1E-18);
            List <KeyValuePair<string, int[]>> CompleteDataset = DatasetManager.DictionaryToList(ConvolutedDatasets);
            double Epochs = 100000;
            var Watch = new System.Diagnostics.Stopwatch();
            for (int Epoch = 0; Epoch < Epochs; Epoch++)
            {
                Console.Clear();
                Console.WriteLine($"Epochs Total: {Epochs}\nEpoch: {Epoch}\nProgress: {(Epoch / (double)Epochs * 100.0), 1:0.00}%");
                Console.WriteLine($"Estimated time waiting: {(Epochs - Epoch) * Watch.ElapsedMilliseconds / 1000.0 }s");
                Watch.Restart();
                DatasetManager.Shuffle(CompleteDataset);
                foreach(KeyValuePair<string, int[]> DataPair in CompleteDataset)
                {
                    List<double> ResultList = new() { Double.Parse(DataPair.Key) };
                    for (int i = 0; i < DataPair.Value.Length; i++)
                    {
                        NeuralNetwork.Input[0].Value = DataPair.Value[i];
                    }
                    //Console.WriteLine("Result: \nArray: ");
                    //DisplayArray(new int[][] { DataPair.Value });
                    NeuralNetwork.Propagate(ResultList, 0.1, 0.1, 1E-16);
                }
                Watch.Stop();
                /*foreach (KeyValuePair<string, List<int[]>> DataPair in ConvolutedDatasets)
                {
                    List<double> ResultList = new() { Double.Parse(DataPair.Key) };

                    DatasetManager.Shuffle(DataPair.Value);
                    foreach (int[] Data in DataPair.Value)
                    {
                        for (int i = 0; i < Data.Length; i++)
                        {
                            Inputs[0].Value = Data[i];
                        }
                        //Console.WriteLine("Result: \nArray: ");
                        //DisplayArray(new int[][] { Data });
                        NeuralNetwork.Propagate(ResultList, 0.1, 0.1, 1E-18);
                    }
                }*/
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
            Console.Read();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(NeuralNetwork, ConvolutionLayer));
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