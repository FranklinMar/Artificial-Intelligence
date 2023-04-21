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
            int Num = 257;
            Genome<int> genome = new(Num);
            Console.WriteLine($"{Num} = {Genome<int>.BytesToString(genome.BytesOfVariable)}");
            genome = new(genome.BytesOfVariable);
            Console.WriteLine($"{Num} = {Genome<int>.BytesToString(genome.BytesOfVariable)}");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
