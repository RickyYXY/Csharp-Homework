using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Data.Entity;

namespace OrderApp
{

    /**
     * The service class to manage orders
     * */
    public class OrderService
    {

        //the order list
        //private List<Order> orders;


        public OrderService()
        {
            //using (var db = new OScontext())
            //{
            //    orders = db.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.OrderItems)
            //    .Include(o=>o.OrderItems.Select(item=>item.GoodsItem))
            //    .ToList();
            //}
            //orders = new List<Order>();
        }

        public List<Order> Orders
        {

            get
            {
                using (var db = new OScontext())
                {
                    var orders = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(item => item.GoodsItem))
                        .ToList();
                    return orders;
                }
            }
        }

        public Order GetOrder(int id)
        {
            using (var db = new OScontext())
            {
                var order = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(item => item.GoodsItem))
                    .Where(o => o.OrderID == id)
                    .FirstOrDefault();
                return order;
            }
            //return orders.Where(o => o.OrderID == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            //if (orders.Contains(order))
            //    throw new ApplicationException($"添加错误: 订单{order.OrderID} 已经存在了!");
            using (var db = new OScontext())
            {
                if(db.Orders.Select(o=>o.OrderID).Contains(order.OrderID))
                    throw new ApplicationException($"添加错误: 订单{order.OrderID} 已经存在了!");
                db.Orders.Add(order);
                db.SaveChanges();
                //orders = db.Orders
                //    .Include(o => o.Customer)
                //    .Include(o => o.OrderItems)
                //    .Include(o => o.OrderItems.Select(item => item.GoodsItem))
                //    .ToList();
            }
            //orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            //orders.Remove(order);
            using (var db = new OScontext())
            {
                var order = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(item => item.GoodsItem))
                    .FirstOrDefault(o => o.OrderID == orderId);
                if (order != null)
                {
                    for(int i=0;i<order.OrderItems.Count;i++)
                    {
                        db.Goods.Remove(order.OrderItems[i].GoodsItem);
                        db.OrderItems.Remove(order.OrderItems[i]);
                    }
                    db.Customers.Remove(order.Customer);
                    db.Orders.Remove(order);
                    db.SaveChanges();
                }
            }

        }

        public List<Order> QueryOrdersByGoodsName(string goodsName)
        {
            using (var db = new OScontext())
            {
                var query = db.Orders
                       .Where(order => order.OrderItems.Exists(item => item.GoodsName == goodsName))
                       .OrderBy(o => o.TotalPrice);
                return query.ToList();
            }

        }

        public List<Order> QueryOrdersByCustomerName(string customerName)
        {
            using (var db = new OScontext())
            {
                var query = db.Orders
                    .Where(order => order.CustomerName == customerName)
                    .OrderBy(o => o.TotalPrice);
                return query.ToList();
                //return orders
                //    .Where(order => order.CustomerName == customerName)
                //    .OrderBy(o => o.TotalPrice)
                //    .ToList();
            }

        }

        public void UpdateOrder(Order newOrder)
        {
            //Order oldOrder = GetOrder(newOrder.OrderID);
            
            using (var db = new OScontext())
            {
                var oldOrder = db.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .Include(o => o.OrderItems.Select(item => item.GoodsItem))
                    .FirstOrDefault(o => o.OrderID == newOrder.OrderID);
                if (oldOrder == null)
                    throw new ApplicationException($"修改错误：订单 {newOrder.OrderID} 不存在!");
                for (int i = 0; i < oldOrder.OrderItems.Count; i++)
                {
                    db.Goods.Remove(oldOrder.OrderItems[i].GoodsItem);
                    db.OrderItems.Remove(oldOrder.OrderItems[i]);
                }
                db.Customers.Remove(oldOrder.Customer);
                db.Orders.Remove(oldOrder);
                db.Orders.Add(newOrder);
                db.SaveChanges();
            }

            //orders.Remove(oldOrder);
            //orders.Add(newOrder);
        }

        public void Sort()
        {
            using (var db = new OScontext())
            {
                db.Orders.OrderBy(o => o.OrderID);
                db.SaveChanges();
            }
            //   orders.Sort();
        }

        public void Sort(Func<Order, Order, int> func)
        {
            using (var db = new OScontext())
            {
                db.Orders.ToList().Sort((o1, o2) => func(o1, o2));
                db.SaveChanges();
            }
            //Orders.Sort((o1,o2)=>func(o1,o2));
        }

        public void Export(String fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (var db = new OScontext())
                {
                    xs.Serialize(fs, db.Orders.ToList());
                }
                //xs.Serialize(fs, Orders);
            }
        }

        public void Import(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Order> temp = (List<Order>)xs.Deserialize(fs);
                using (var db = new OScontext())
                {
                    temp.ForEach(order =>
                    {
                        if (!db.Orders.Select(o => o.OrderID).Contains(order.OrderID))
                        {
                            db.Orders.Add(order);
                        }
                    });
                    db.SaveChanges();
                }

            }
        }

        public object QueryByTotalAmount(float amout)
        {
            using (var db = new OScontext())
            {
                return db.Orders
                   .Where(order => order.TotalPrice >= amout)
                   .OrderByDescending(o => o.TotalPrice)
                   .ToList();
            }

        }
    }
}
