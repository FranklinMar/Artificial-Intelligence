using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using Lab1;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Lab3
{
    static class Program
    {
        //static string File = new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath;
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
            //Console.WriteLine(File);

            /*string ExeFilePath = Assembly.GetEntryAssembly().Location;
            string WorkPath = Path.GetDirectoryName(ExeFilePath);
            Console.WriteLine(WorkPath);*/
            double
                X_min = -2,
                X_max = 5,
                InitialPopulationNumber = 25000,
                GenerationNumber = 1;//000;
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
                    .Selection(Max, (int) (PopulationForMax.Count * 0.5))
                    .NewGeneration(PopulationForMax.Count);
                PopulationForMin = PopulationForMin
                    .Selection(Min, (int) (PopulationForMin.Count * 0.5))
                    .NewGeneration(PopulationForMin.Count);
            }
            string LINE = new('-', 25);
            double MaxValue = PopulationForMax.Selection(Max, 1)[0].Variable, MinValue = PopulationForMin.Selection(Min, 1)[0].Variable;
            Console.WriteLine($"{LINE}\n\t{GenerationNumber} Generation\n{LINE}\n");
            Console.WriteLine($"Maximum of Function: {MaxValue, 1:0.00}");
            Console.WriteLine($"Minimum of Function: {MinValue, 1:0.00}");

            var HTMLFile = File.ReadAllText($"..\\..\\..\\ChartFile.html");
            string FunctionString = JsonSerializer.Serialize("x * sin(5 * x)");// MaxSineFunction = x * sin(5 * x)
            string ValueString = JsonSerializer.Serialize(new List<double> { MaxValue, MinValue });
            HTMLFile = HTMLFile.Replace("let Function", $"let Function = {FunctionString}");
            HTMLFile = HTMLFile.Replace("let Values", $"let Values = {ValueString}");
            HTMLFile = HTMLFile.Replace("let Xmin", $"let Xmin = {JsonSerializer.Serialize(X_min)}");
            HTMLFile = HTMLFile.Replace("let Xmax", $"let Xmax = {JsonSerializer.Serialize(X_max)}");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(HTMLFile));
        }
    }
}
