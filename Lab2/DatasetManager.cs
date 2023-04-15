using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Lab2
{

   public static class DatasetManager
    {
        public static int SIZE = 6;
        public static void Shuffle<T>(this IList<T> List)
        {
            var Generator = new RNGCryptoServiceProvider();
            int n = List.Count;
            while (n > 1)
            {
                byte[] Box = new byte[1];
                do Generator.GetBytes(Box);
                while (!(Box[0] < n * (Byte.MaxValue / n)));
                int k = (Box[0] % n);
                n--;
                T Value = List[k];
                List[k] = List[n];
                List[n] = Value;
            }
        }
        /*public static void ShuffleData(List<int[][]> List)
        {
            var Generator = new RNGCryptoServiceProvider();
            int n = List.Count;
            while (n > 1)
            {
                byte[] Box = new byte[1];
                do Generator.GetBytes(Box);
                while (!(Box[0] < n * (Byte.MaxValue / n)));
                int k = (Box[0] % n);
                n--;
                int [][] Value = List[k];
                List[k] = List[n];
                List[n] = Value;
            }
        }*/

        public static Dictionary<string, List<int[]>> ConvoluteDataset(Dictionary<string, List<int[][]>> Datasets, Convolution Layer)
        {
            Dictionary<string, List<int[]>> ConvolutedDatasets = new();
            List<int[]> List;
            int[][] Dataset;
            foreach (KeyValuePair<string, List<int[][]>> Var in Datasets)
            {
                List = new();
                foreach (int[][] Matrix in Var.Value)
                {
                    Dataset = Layer.MaxPool(Layer.Convolute(Matrix));
                    
                    List.Add(FlattenArray(Dataset));
                }
                ConvolutedDatasets.Add(Var.Key, List);
            }
            return ConvolutedDatasets;
        }

        public static int[] FlattenArray(int[][] Array)
        {
            int[] Temp = new int[Array.Length * Array[0].Length];
            int Index = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                for (int j = 0; j < Array[i].Length; j++)
                {
                    Temp[Index++] = Array[i][j];
                }
            }
            return Temp;
        }

        public static Dictionary<string, List<int[][]>> ExpandDataset(Dictionary<string, List<int[][]>> dataset)
        {
            List<int[][]> List;
            int[][] Field;
            int i, j;
            Dictionary<string, List<int[][]>> DataSets = new();
            foreach (KeyValuePair<string, List<int[][]>> Pair in dataset)
            {
                List = new();
                DataSets.Add(Pair.Key, List);
                foreach (int[][] array in Pair.Value)
                {
                    i = 0; j = 0;
                    while (true)
                    {
                        if (j + array[0].Length > SIZE)
                        {
                            j = 0;
                            i++;
                        }
                        if (i + array.Length > SIZE)
                        {
                            break;
                        }
                        Field = new int[SIZE][];
                        for (int k = 0; k < Field.Length; k++)
                        {
                            Field[k] = new int[SIZE];
                            Array.Clear(Field[k], 0, Field[k].Length);
                        }
                        for (int k = 0; k < array.Length && k + i < Field.Length; k++)
                        {
                            for (int l = 0; l < array[0].Length && l + j < Field[0].Length; l++)
                            {
                                Field[k + i][l + j] = array[k][l];
                            }
                        }
                        List.Add(Field);
                        j++;
                    }
                }
            }
            return DataSets;
        }

        public static Dictionary<string, List<int[][]>> ReadJSON(string filename)
        {
            using (StreamReader Reader = new(filename))
            {
                var JSON = new StringBuilder("");
                while (!Reader.EndOfStream)
                {
                    JSON.Append(Reader.ReadLine());
                }
                //Console.WriteLine(JSON);
                return JsonSerializer.Deserialize<Dictionary<string, List<int[][]>>>(JSON.ToString());
                /*Dictionary<string, List<int[][]>> Temporary = 
                Dictionary<string, List<int[,]>> Result = new();
                List<int[,]> DatasetList;
                foreach (KeyValuePair<string, List<int[][]>> Pair in Temporary)
                {
                    DatasetList = new();
                    Result.Add(Pair.Key, DatasetList);
                    Pair.Value.ForEach(array =>
                    {
                        int FirstDim = array.Length,
                            SecondDim = array[0].Length;
                        foreach (int[] Jagged in array)
                        {
                            SecondDim = SecondDim < Jagged.Length ? Jagged.Length : SecondDim;
                        }
                        int[,] NewArray = new int[FirstDim, SecondDim];
                        for (int i = 0; i < FirstDim; i++)
                        {
                            for (int j = 0; j < SecondDim; j++)
                            {
                                NewArray[i, j] = array[i][j];
                            }
                        }
                        DatasetList.Add(NewArray);
                    });
                }*/
            }
        }

        public static void LoadJSON(string filename, Dictionary<string, List<int[][]>> data)
        {
            string JSON = JsonSerializer.Serialize(data);
            //await using FileStream createStream = File.Create(@$"D:\{filename}.json");
            //await JsonSerializer.SerializeAsync(createStream, J);
            File.WriteAllText(@$"D:\{filename}", JSON);
        }
    }
}