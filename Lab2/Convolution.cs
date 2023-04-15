using System;

namespace Lab2
{
    public class Convolution
    {
        private readonly static int SIZE = 3;
        public int POOLSIZE = 2;
        public int[,] Filter { get; private set; } = new int[SIZE, SIZE];

        public Convolution()
        {
            //var provider = new RNGCryptoServiceProvider();
            var Gen = new Random();
            for (int i = 0; i < Filter.GetLength(0); i++)
            {
                for (int j = 0; j < Filter.GetLength(1); j++)
                {
                    Filter[i, j] = Gen.Next(0, 1);
                }
            }
        }
        public Convolution(int [,] filter)
        {
            if (filter.GetLength(0) != Filter.GetLength(0) || filter.GetLength(1) != Filter.GetLength(1))
            {
                throw new ArgumentException("Filter sizes must match");
            }
            for(int i = 0; i < Filter.GetLength(0); i++)
            {
                for (int j = 0; j < Filter.GetLength(1); j++)
                {
                    Filter[i, j] = filter[i, j];
                }
            }
        }

        public int [][] Convolute(int [][] Field)
        {
            if (Field.Length - 2 <= Filter.GetLength(0) || Field[0].Length - 2 <= Filter.GetLength(1))
            {
                throw new ArgumentException("Field array too small for filter");
            }
            int[][] NewField = new int[Field.Length - 2][];
            for (int ind = 0; ind < NewField.Length; ind++)
            {
                NewField[ind] = new int[Field[0].Length - 2];
            }
            int i = 0, j = 0, y = 0, x = 0, Sum;
            while (true)
            {
                if (j + SIZE > Field[0].Length)
                {
                    j = 0;
                    x = 0;
                    i++;
                    y++;
                }
                if (i + SIZE > Field.Length)
                {
                    break;
                }
                Sum = 0;
                for (int k = 0; k < Filter.GetLength(0) && k + i < Field.Length; k++)
                {
                    for (int l = 0; l < Filter.GetLength(1) && l + j < Field[0].Length; l++)
                    {
                        Sum += Field[k + i][l + j] * Filter[k, l];
                    }
                }
                NewField[y][x] = Sum;
                j++;
                x++;
            }
            return NewField;
        }
        public int [][] MaxPool(int [][] Field)
        {
            if (Field.Length % POOLSIZE != 0 || Field[0].Length % POOLSIZE != 0)
            {
                throw new ArgumentException("Pixel field is not suitable for pooling");
            }
            int[][] NewField = new int[Field.Length / POOLSIZE][];
            for (int ind = 0; ind < NewField.Length; ind++)
            {
                NewField[ind] = new int[Field[0].Length / POOLSIZE];
            }
            int Max, Value;
            for (int i = 0; i < NewField.Length; i++)
            {
                for (int j = 0; j < NewField[0].Length; j++)
                {
                    Max = Field[i * POOLSIZE][j * POOLSIZE];
                    for (int k = 0; k < POOLSIZE; k++)
                    {
                        for (int l = 0; l < POOLSIZE; l++)
                        {
                            Value = Field[i * POOLSIZE + k][j * POOLSIZE + l];
                            if (Value > Max)
                            {
                                Max = Value;
                            }
                        }
                    }
                    NewField[i][j] = Max;
                }
            }
            return NewField;
        }
    }
}