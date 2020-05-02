using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Xml.Serialization;

namespace OrderApp
{
    public class OrderService
    {
        
        //the order list
        private List<Order> orders;
        public List<Order> Orders
        {
            get => orders;
        }

        public OrderService()
        {
            //orders = new List<Order>();
            //var _temp = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            using (var context = new OrderContext())
            {
                orders = context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(oi => oi.GoodsItem)).ToList();
            }
        }

        public Order GetOrder(int id)
        {
            return orders.Where(o => o.OrderID == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
                throw new ApplicationException($"添加错误: 订单{order.OrderID} 已经存在了!");
            using (var context = new OrderContext())
            {
                //orders.Add(order);
                context.Orders.Add(order);
                context.SaveChanges();
                orders = context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(oi => oi.GoodsItem)).ToList();
            }
        }

        public void RemoveOrder(int orderId)
        {
            //Order order = GetOrder(orderId);
            //if (order == null) return;
            //orders.Remove(order);
            using (var context = new OrderContext())
            {
                Order order = context.Orders.Include(o => o.OrderItems).Include(o => o.Customer)
                    .Include(o => o.OrderItems.Select(oi => oi.GoodsItem))
                    .FirstOrDefault(o => o.OrderID == orderId);
                if (order != null)
                {
                    Customer customer = context.Customers.FirstOrDefault(c => c.OrderID == orderId);
                    context.Orders.Remove(order);
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                    orders = context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.GoodsItem)).ToList();
                }
            }
        }

        public List<Order> QueryOrdersByGoodsName(string goodsName)
        {
            var query = orders
                .Where(order => order.OrderItems.Exists(item => item.GoodsName == goodsName))
                .OrderBy(o => o.TotalPrice);
            return query.ToList();
        }

        public List<Order> QueryOrdersByCustomerName(string customerName)
        {
            return orders
                .Where(order => order.CustomerName == customerName)
                .OrderBy(o => o.TotalPrice)
                .ToList();
        }

        public void UpdateOrder(Order newOrder)
        {
            //Order oldOrder = GetOrder(newOrder.OrderID);
            //if (oldOrder == null)
            //    throw new ApplicationException($"修改错误：订单 {newOrder.OrderID} 不存在!");
            using (var context = new OrderContext())
            {
                Order oldOrder = context.Orders.Include(o => o.OrderItems).Include(o => o.Customer)
                    .Include(o => o.OrderItems.Select(oi => oi.GoodsItem))
                    .FirstOrDefault(o => o.OrderID == newOrder.OrderID);
                if (oldOrder == null)
                    throw new ApplicationException($"修改错误：订单 {newOrder.OrderID} 不存在!");
                Customer customer = context.Customers.FirstOrDefault(c => c.OrderID == newOrder.OrderID);
                //List<OrderItem> orderItems = context.OrderItems.Where(oi => oi.OrderID == newOrder.OrderID).ToList();
                //List<Goods> goods = context.Goods.Where(g => g.OrderID == newOrder.OrderID).ToList();
                context.Orders.Remove(oldOrder);
                context.Customers.Remove(customer);
                context.Orders.Add(newOrder);
                /*
                foreach(OrderItem oi in orderItems)
                {
                    context.OrderItems.Remove(oi);
                }
                foreach(Goods g in goods)
                {
                    context.Goods.Remove(g);
                }*/
                context.SaveChanges();
                orders = context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(oi => oi.GoodsItem)).ToList();
            }
            //orders.Remove(oldOrder);
            //orders.Add(newOrder);
        }

        public void Sort()
        {
            orders.Sort();
        }

        public void Sort(Func<Order, Order, int> func)
        {
            Orders.Sort((o1, o2) => func(o1, o2));
        }

        public void Export(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, Orders);
            }
        }

        public void Import(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Order> temp = (List<Order>)xs.Deserialize(fs);
                using (OrderContext context = new OrderContext())
                {
                    temp.ForEach(order => {
                        order.OrderItems.ForEach(oi =>
                        {
                            oi.Order = order;
                            oi.OrderID = order.OrderID;
                            oi.GoodsItem.Order = order;
                            oi.GoodsItem.OrderID = order.OrderID;
                            oi.GoodsItem.OrderItem = oi;
                            oi.GoodsItem.OrderItemID = oi.OrderItemID;
                        });
                        order.Customer.Order = order;
                        order.Customer.OrderID = order.OrderID;
                        if (!orders.Contains(order))
                        {
                            context.Orders.Add(order);
                            //orders.Add(order);
                        }
                    });
                    context.SaveChanges();
                    orders = context.Orders.Include(o => o.Customer).Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.GoodsItem)).ToList();
                }
            }
        }

        public object QueryByTotalAmount(float amout)
        {
            return orders
               .Where(order => order.TotalPrice >= amout)
               .OrderByDescending(o => o.TotalPrice)
               .ToList();
        }
    }
}
