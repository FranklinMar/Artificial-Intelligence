using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Lab1;

namespace Lab2
{
    public partial class Form1 : Form
    {
        private bool isDraw;
        int[] Data;
        Graphics Graphics;
        Network Network;
        Convolution Layer;
        public Form1(Network network, Convolution layer)
        {
            InitializeComponent();
            isDraw = false;
            Graphics = panel1.CreateGraphics();
            Data = new int[DatasetManager.SIZE * DatasetManager.SIZE];
            panel1.Height = panel1.Width = panel1.Width - panel1.Height % DatasetManager.SIZE;
            Network = network;
            Layer = layer;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "None";
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDraw = true;
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDraw = false;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int size = panel1.Width / DatasetManager.SIZE;

            if (isDraw && e.X > 0 && e.Y > 0 && e.X / size < DatasetManager.SIZE && e.Y / size < DatasetManager.SIZE)
            {
                Data[e.Y / size * DatasetManager.SIZE + e.X / size] = 1;
                Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(e.X - e.X % size, e.Y - e.Y % size, size, size));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics.Clear(Color.Gray);
            for (int i = 0; i < DatasetManager.SIZE * DatasetManager.SIZE; i++)
                Data[i] = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Index = 0;
            int[][] Matrix = new int[DatasetManager.SIZE][];
            for (int i = 0; i < DatasetManager.SIZE; i++)
            {
                Matrix[i] = new int[DatasetManager.SIZE];
                for(int j = 0; j < DatasetManager.SIZE; j++)
                {
                    Matrix[i][j] = Data[Index++];
                }
            }
            label2.Text = Network.Calculate(Array.ConvertAll<int, double>(DatasetManager.FlattenArray(Layer.MaxPool(Layer.Convolute(Matrix))), x => x))[0].ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
