using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "Number1:";
            label3.Text = "Number2:";
            label1.Text = " ";
            textBox1.Text = "0";
            textBox2.Text = "0";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string op = " ";
            double a = 0, b = 0, ans;
            int j = 0;
            a = Double.Parse(textBox1.Text);
            b = Double.Parse(textBox2.Text);
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    op = checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    j++;
                }
            }
            if (j > 1)
            {
                label1.Text = "Don't choose more than one operator!";
                return;
            }
            else if (j == 0)
            {
                label1.Text = "Don't choose less than one operator!";
            }
            switch (op)
            {
                case " ":
                    label1.Text = "Please choose an operator!";
                    return;
                case "+":
                    ans = a + b;
                    label1.Text = ans.ToString();
                    return;
                case "-":
                    ans = a - b;
                    label1.Text = ans.ToString();
                    return;
                case "*":
                    ans = a * b;
                    label1.Text = ans.ToString();
                    return;
                case "/":
                    if (b == 0)
                    {

                        label1.Text = "The second number cna't be 0!";
                        return;
                    }
                    ans = a / b;
                    label1.Text = ans.ToString();
                    return;
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
