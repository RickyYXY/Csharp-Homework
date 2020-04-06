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
    public partial class Form1 : Form
    {
        OrderService os;
        Order order;
        public Form1()
        {
            InitializeComponent();
            os = new OrderService();
            orderbindingSource1.DataSource = os.Orders;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Load_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 导入订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                os.Import(openFileDialog1.FileName);
                orderbindingSource1.DataSource = os.Orders;
                orderbindingSource1.ResetBindings(false);
            }
        }

        private void 导出订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                os.Export(saveFileDialog1.FileName);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void 新建订单_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            order = form2.Order;
            if (order.Items.Count == 0)
                return;
            try
            {
                if(form2.Flag)
                {
                    os.AddOrder(order);
                    orderbindingSource1.DataSource = os.Orders;
                    orderbindingSource1.ResetBindings(false);
                }
            }
            catch(ApplicationException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 修改订单_Click(object sender, EventArgs e)
        {
            if(os.Orders.Count==0)
            {
                MessageBox.Show("不存在任何订单！");
                return;
            }
            Form3 form3 = new Form3(os.Orders);
            form3.ShowDialog();
            orderbindingSource1.DataSource = os.Orders;
            orderbindingSource1.ResetBindings(false);
        }

        private void 查询订单_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
            int index = form4.Index;
            string name = form4.stringName;
            if (!form4.Flag)
                return;
            if (index == 1)
            {
                orderbindingSource1.DataSource = os.QueryOrdersByGoodsName(name);
                orderbindingSource1.ResetBindings(false);
            }
            else if(index==2)
            {
                orderbindingSource1.DataSource = os.QueryOrdersByCustomerName(name);
                orderbindingSource1.ResetBindings(false);
            }
            else
            {
                os.Sort();
                orderbindingSource1.DataSource = os.Orders;
                orderbindingSource1.ResetBindings(false);
            }    
        }

        private void 删除订单_Click(object sender, EventArgs e)
        {
            int index = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]
                .Cells[0].Value.ToString());
            if(MessageBox
                .Show("确定删除选中的订单?", "提示", MessageBoxButtons.OKCancel)
                == DialogResult.OK)
            {

                os.RemoveOrder((uint)index);
                orderbindingSource1.DataSource = os.Orders;
                orderbindingSource1.ResetBindings(false);
            }
        }
    }
}
