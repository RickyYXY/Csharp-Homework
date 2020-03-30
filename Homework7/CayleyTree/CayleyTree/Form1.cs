using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CayleyTree
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // parameters
        private Graphics graphics;
        double th1 = 30 * Math.PI / 180;
        double th2 = 20 * Math.PI / 180;
        double per1 = 0.6;
        double per2 = 0.7;
        Color red = Color.Red;
        Color pink = Color.FromArgb(255, 0, 255);
        Color yellow = Color.FromArgb(255, 215, 0);
        int draw_depth = 10;
        double leng = 100;
        Pen pen = new Pen(Color.Red);

        private void drawLine(double x0,double y0,double x1,double y1)
        {
            graphics.DrawLine(
                pen, (int)x0, (int)y0, (int)x1, (int)y1);
        }

        private void drawCayleyTree(int n, double x0, double y0,
            double leng, double th)
        {
            if (n == 0) return;

            double x1 = x0 + leng * Math.Cos(th);
            double y1 = y0 + leng * Math.Sin(th);

            drawLine(x0, y0, x1, y1);

            drawCayleyTree(n - 1, x1, y1, per1 * leng, th + th1);
            drawCayleyTree(n - 1, x1, y1, per2 * leng, th - th2);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (graphics == null)
                graphics = this.panel2.CreateGraphics();
            graphics.Clear(panel2.BackColor);
            drawCayleyTree(draw_depth, panel2.Width/2, panel2.Height,
                leng, -Math.PI / 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            leng = Double.Parse(textBox1.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pen.Color = pink;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            pen.Color = red;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            pen.Color = yellow;
        }

        private void trackBar_Depth_ValueChanged(object sender, EventArgs e)
        {
            draw_depth = trackBar_Depth.Value*15 / trackBar_Depth.Maximum;
        }

        private void trackBar_Left_ValueChanged(object sender, EventArgs e)
        {
            per1 = trackBar_Left.Value / (double)trackBar_Left.Maximum;
        }

        private void trackBar_Right_ValueChanged(object sender, EventArgs e)
        {
            per2 = trackBar_Right.Value / (double)trackBar_Right.Maximum;
        }

        private void textBox_LeftTH_TextChanged(object sender, EventArgs e)
        {
            th1 = Double.Parse(textBox_LeftTH.Text)* Math.PI / 180;
        }

        private void textBox_RightTH_TextChanged(object sender, EventArgs e)
        {
            th2 = Double.Parse(textBox_RightTH.Text)* Math.PI / 180;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
