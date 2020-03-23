using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderService2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OrderService2.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        private OrderService os = new OrderService();
        private List<Order> orders = new List<Order>();
        //string[] customers = { "Ricky", "Melissa" };
        //int[] ids = { 518, 1125 };
        //string[][] itemNames = new string[2][]{ new string[]{ "iPad", "Textbook" },
        //                                new string[]{"Pen","Flower" } };
        //int[][] quantities = new int[2][] { new int[] { 1, 5 }, new int[] { 3, 10 } };
        //double[][] prices = new double[2][] { new double[] { 1999, 25 }, new double[] { 199, 15 } };

        [TestInitialize]
        public void Initialize()
        {
            string[] customers = { "Ricky", "Melissa" };
            int[] ids = { 518, 1125 };
            string[][] itemNames = new string[2][]{ new string[]{ "iPad", "Textbook" },
                                        new string[]{"Pen","Flower" } };
            int[][] quantities = new int[2][] { new int[] { 1, 5 }, new int[] { 3, 10 } };
            double[][] prices = new double[2][] { new double[] { 1999, 25 }, new double[] { 199, 15 } };
            for (int i = 0; i < ids.Length; i++)
            {
                List<OrderItem> items = new List<OrderItem>();
                for (int j = 0; j < itemNames[i].Length; j++)
                {
                    OrderItem item = new OrderItem(itemNames[i][j], quantities[i][j], prices[i][j]);
                    items.Add(item);
                }
                Order order = new Order(customers[i], ids[i], items);
                orders.Add(order);
            }
            for (int i = 0; i < orders.ToArray().Length; i++)
                os.AddOrder(orders[i]);
        }
        [TestCleanup]
        public void CleanUp()
        {
            os.Clean();
            orders.Clear();
        }

        [TestMethod()]
        public void ExportOrdersTest()
        {
            os.ExportOrders("orderlist.xml");
            FileInfo fileInfo = new FileInfo("orderlist.xml");
            Assert.IsTrue(fileInfo.Exists);
        }

        [TestMethod()]
        public void ImportOrdersTest()
        {
            OrderService os2 = new OrderService();
            os2.ImportOrders("orderlist.xml");
            List<Order> orders2 = os2.GetAllOrders();
            //Assert.AreEqual(orders, orders2);
            for (int i = 0; i < orders.ToArray().Length; i++)
            {
                Assert.AreEqual(orders[i], orders2[i]);
                for (int j = 0; j < orders[i].items.ToArray().Length; j++)
                    Assert.AreEqual(orders[i].items[j], orders2[i].items[j]);
            }
        }

        [TestMethod()]
        public void AddOrderTest()
        {
            List<OrderItem> oi2 = new List<OrderItem>();
            oi2.Add(new OrderItem("toy", 5, 10));
            Order order2 = new Order("A", 1, oi2);
            os.AddOrder(order2);
            List<Order> ol2 = os.FindOrder(1);
            Assert.AreEqual(ol2[0], order2);
            for (int j = 0; j < ol2[0].items.ToArray().Length; j++)
                Assert.AreEqual(ol2[0].items[j], order2.items[j]);
        }

        [TestMethod()]
        public void FindOrderTest()
        {
            List<Order> ol2 = os.FindOrder(1125);
            Assert.AreEqual(ol2[0], orders[1]);

            ol2 = os.FindOrder("Ricky", "c");
            Assert.AreEqual(ol2[0], orders[0]);

            ol2 = os.FindOrder("Flower", "i");
            Assert.AreEqual(ol2[0], orders[1]);
        }

        [TestMethod()]
        public void DeleteOrderTest()
        {
            os.DeleteOrder(518);
            Assert.IsNull(os.FindOrder(518));
        }

        [TestMethod()]
        public void OrderSortTest()
        {
            orders.Sort((o1, o2) => o1.ID - o2.ID);
            os.OrderSort();
            List<Order> ol2 = os.GetAllOrders();
            for (int i = 0; i < orders.ToArray().Length; i++)
                Assert.AreEqual(orders[i], ol2[i]);
        }
    }
}