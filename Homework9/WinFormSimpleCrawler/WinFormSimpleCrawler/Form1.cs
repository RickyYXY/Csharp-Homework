using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WinFormSimpleCrawler
{
    public partial class Form1 : Form
    {
        SimpleCrawler crawler = new SimpleCrawler();
        public Form1()
        {
            InitializeComponent();
            crawler.PageDownloaded += Crawler_PageDownloaded;
            textBox1.Text = "";
            //textBox1.DataBindings.Add("Text", crawler, "Starturl");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            crawler.Clear();
            crawler.AddStartUrl(textBox1.Text);
            if (comboBox1.SelectedItem != null)
                crawler.UrlNum = Int32.Parse(comboBox1.SelectedItem.ToString());
            listBox1.Items.Clear();
            new Thread(crawler.Crawl).Start();
        }

        private void Crawler_PageDownloaded(string obj, DownloadEventArgs args)
        {
            if (listBox1.InvokeRequired)
            {
                Action<String> action = AddUrl;
                this.Invoke(action, new object[] { obj });
            }
            else
            {
                this.AddUrl(obj);
            }
        }

        private void AddUrl(string url)
        {
            listBox1.Items.Add(url);
        }
    }
}
