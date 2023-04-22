using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            double Num = 257;
            Genome<double> Genome = new(Num);
            Console.WriteLine($"{Genome.Variable} = {Genome.GenesToString()}");
            Genome = new(Genome.Genes);
            Console.WriteLine($"{Genome.Variable} = {Genome.GenesToString()}");
            Genome.InverseGene(8);
            Console.WriteLine($"{Genome.Variable} = {Genome.GenesToString()}");
            Num = 4;
            Genome = new(Num);
            Genome.InverseGene(2);
            Console.WriteLine($"{Genome.Variable} = {Genome.GenesToString()}");



            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
