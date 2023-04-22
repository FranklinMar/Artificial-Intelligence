using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab1;

namespace Lab3
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [STAThread]
        static void Main()
        {
            AllocConsole();
            /*double Num1 = 257;
            Genome<double> Genome1 = new(Num1);
            Console.WriteLine($"{Genome1.Variable} = {Genome1.GenesToString()}");
            Genome1 = new(Genome1.Genes);
            Console.WriteLine($"{Genome1.Variable} = {Genome1.GenesToString()}");
            Genome1.InverseGene(8);
            Console.WriteLine($"{Genome1.Variable} = {Genome1.GenesToString()}");
            double Num2 = 4;
            Genome <double> Genome2 = new(Num2);
            Genome2.InverseGene(2);
            Console.WriteLine($"{Genome2.Variable} = {Genome2.GenesToString()}");
            Genome <double> CrossResult = Genome1.Cross(Genome2, Genome1.BitCount / 2);
            Console.WriteLine($"{CrossResult.Variable} = {CrossResult.GenesToString()}");*/

            double 
                X_min = -2,
                X_max = 5,
                InitialPopulationNumber = 25000,
                GenerationNumber = 1000;
            MathFunctions Function = new(X_min, X_max);
            Func<Genome<double>, double> 
                Min = Function.MinSineFunction,
                Max = Function.MaxSineFunction;
            Population<double> 
                PopulationForMax = new(),
                PopulationForMin = new();
            SecureRandom Generator = new();
            Genome<double> Temp;

            for (int i = 0; i < InitialPopulationNumber; i++)
            {
                Temp = new(Generator.NextDouble(X_min, X_max));
                PopulationForMax.Add(Temp);
                PopulationForMin.Add(Temp);
            }
            for (int i = 0; i < GenerationNumber; i++)
            {
                PopulationForMax = PopulationForMax
                    .Selection(Min, (int) (PopulationForMax.Count * 0.5))
                    .NewGeneration(PopulationForMax.Count);
                PopulationForMin = PopulationForMin
                    .Selection(Max, (int) (PopulationForMin.Count * 0.5))
                    .NewGeneration(PopulationForMin.Count);
            }
            string LINE = new('-', 25);
            Console.WriteLine($"{LINE}\n\t{GenerationNumber} Generation\n{LINE}\n");
            Console.WriteLine($"Maximum of Function: {PopulationForMax.Selection(Max, 1)[0].Variable, 1:0.00}");
            Console.WriteLine($"Minimum of Function: {PopulationForMin.Selection(Min, 1)[0].Variable, 1:0.00}");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
