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
    public partial class Form2 : Form
    {
        Order order = new Order(0, "default", null);
        OrderItem item = new OrderItem(0, "default", 0, 0);
        public bool Flag { get; set; }

        public Order Order
        {
            get => order;
        }

        public Form2()
        {
            InitializeComponent();
            itembindingSource1.DataSource = order.Items;
            singleOrderbindingSource1.DataSource = order;
            singleItembindingSource.DataSource = item;
            Flag = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                order.AddItem(item);
                //item = new OrderItem();
                //singleItembindingSource.DataSource = item;
                singleOrderbindingSource1.ResetBindings(false);
                itembindingSource1.ResetBindings(false);
            }
            catch (ApplicationException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(order.Items.Count==0)
            {
                MessageBox.Show("订单条目数不足，请添加至少1个条目！");
                return;
            }
            Flag = true;
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
