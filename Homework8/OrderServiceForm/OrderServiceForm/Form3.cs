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
    public partial class Form3 : Form
    {

        List<Order> orders;
        Order order = new Order(0, "default", null);
        OrderItem item = new OrderItem(0, "default", 0, 0);

        public Form3(List<Order>orders)
        {
            InitializeComponent();
            this.orders = orders;
            //order.AddItem(item);

            itembindingSource1.DataSource = order.Items;
            singleItembindingSource.DataSource = item;
            singleOrderbindingSource1.DataSource = order;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = orders.FindIndex(o => o.OrderId == order.OrderId);
            if(index<0)
            {
                MessageBox.Show("Didn't find such order.");
                return;
            }
            orders[index].Customer = order.Customer;
            int index2 = orders[index].Items.FindIndex(i => i.Index == item.Index);
            if(index2>=0)
            {
                orders[index].Items[index2] = item;
            }
            itembindingSource1.DataSource = orders[index].Items;
            itembindingSource1.ResetBindings(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
