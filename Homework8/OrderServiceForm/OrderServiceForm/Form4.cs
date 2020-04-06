using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderServiceForm
{
    public partial class Form4 : Form
    {
        public string stringName { get; set; }
        public int Index { get; set; }
        public bool Flag { get; set; }
        public Form4()
        {
            InitializeComponent();
            //textBox1.Text = "default";
            textBox1.DataBindings.Add("Text", this, "stringName");
            Index = 3;
            radioButton3.Checked = true;
            Flag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Flag = true;
            Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Index = 3;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Index = 2;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Index = 1;
        }
    }
}
