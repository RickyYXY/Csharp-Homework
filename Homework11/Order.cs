using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApp
{
    public class Order : IComparable<Order>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key] //取消自增
        public int OrderID { get; set; }
        
        [Required]
        public Customer Customer { get; set; }

        public string CustomerName { get => (Customer != null) ? Customer.Name : ""; }

        public DateTime CreateTime { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public double TotalPrice
        {
            get => OrderItems.Sum(item => item.TotalPrice);
        }

        public Order() {
            OrderItems = new List<OrderItem>();
            CreateTime = DateTime.Now;
        }

        public Order(int orderId, Customer customer, List<OrderItem> items)
        {
            this.OrderID = orderId;
            this.Customer = customer;
            this.CreateTime = DateTime.Now;
            this.OrderItems = (items == null) ? new List<OrderItem>() : items;
        }

        public void AddItem(OrderItem orderItem)
        {
            if (OrderItems.Contains(orderItem))
                throw new ApplicationException($"添加错误：订单项{orderItem.GoodsName} 已经存在!");
            orderItem.OrderID = OrderID;
            orderItem.Order = this;
            using (OrderContext context = new OrderContext())
                //{
                //   context.OrderItems.Add(orderItem);
                //    context.SaveChanges();
                //}
                OrderItems.Add(orderItem);
        }

        public void RemoveItem(OrderItem orderItem)
        {/*
            using (OrderContext context = new OrderContext())
            {
                OrderItem orderitem = context.OrderItems.Include("Goods")
                    .FirstOrDefault(oi => oi.OrderID == this.OrderId);
                context.OrderItems.Remove(orderitem);
                context.SaveChanges();
            }*/
            OrderItems.Remove(orderItem);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"Id:{OrderID}, customer:{Customer},orderTime:{CreateTime},totalPrice：{TotalPrice}");
            OrderItems.ForEach(od => strBuilder.Append("\n\t" + od));
            return strBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null &&
                   OrderID == order.OrderID;
        }

        public override int GetHashCode()
        {
            var hashCode = -531220479;
            hashCode = hashCode * -1521134295 + OrderID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerName);
            hashCode = hashCode * -1521134295 + CreateTime.GetHashCode();
            return hashCode;
        }

        public int CompareTo(Order other)
        {
            if (other == null) return 1;
            return this.OrderID.CompareTo(other.OrderID);
        }
    }
}
