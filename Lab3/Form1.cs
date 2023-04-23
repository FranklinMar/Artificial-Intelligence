using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        string StringHTML;
        public Form1(string HTML)
        {
            StringHTML = HTML;
            InitializeComponent();
            webView21.EnsureCoreWebView2Async();
        }

        private void webView21_Click(object sender, EventArgs e)
        {
        }

        private void webView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            //webView21.CoreWebView2.Navigate("https://www.google.com");
            webView21.NavigateToString(StringHTML);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
