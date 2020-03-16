using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderService os = new OrderService();
            Random rd = new Random();
            while (true)
            {
                Console.WriteLine("A ---> Add an order\n"
                    + "D ---> Delete an order\n"
                    + "S ---> Sort your order\n"
                    + "Q ---> Do a query\n"
                    + "P ---> Print all order\n"
                    + "E ---> End the service");
                string cmd = Console.ReadLine();
                try
                {
                    switch (cmd)
                    {
                        case "A":
                            List<OrderItem> items = new List<OrderItem>();
                            //string customer;
                            Console.WriteLine("Your name: ");
                            string customer = Console.ReadLine();
                            while (cmd != "N")
                            {
                                string itname, quantity, price;
                                Console.WriteLine("Product's name: ");
                                itname = Console.ReadLine();
                                Console.WriteLine("Product's quantity: ");
                                quantity = Console.ReadLine();
                                Console.WriteLine("Product's unit price:");
                                price = Console.ReadLine();
                                items.Add(new OrderItem(itname, Int32.Parse(quantity), Double.Parse(price)));
                                Console.WriteLine("Finsh. Continue? (Y/N): ");
                                cmd = Console.ReadLine();
                            }
                            int id = rd.Next(1, 100);
                            Order order = new Order(customer, id, items);
                            while (!os.AddOrder(order))
                            {
                                id = rd.Next(1, 100);
                                order.ID = id;
                            }
                            Console.WriteLine("Order created.\n" + order.ToString());
                            break;
                        case "D":
                            Console.WriteLine("Order's ID: ");
                            string idnum = Console.ReadLine();
                            if (os.DeleteOrder(Int32.Parse(idnum)))
                                Console.WriteLine("Delete successfully.\n");
                            else
                                Console.WriteLine("Can't find the order.\n");
                            break;
                        case "S":
                            os.OrderSort();
                            Console.WriteLine("Finish.\n");
                            break;
                        case "Q":
                            Console.WriteLine("ID: ");
                            idnum = Console.ReadLine();
                            List<Order> result = os.FindOrder(Int32.Parse(idnum));
                            foreach (Order o in result)
                                Console.WriteLine(o.ToString());
                            break;
                        case "P":
                            os.PrintAllOrder();
                            break;
                        default:
                            break;

                    }
                }
                catch(FormatException)
                {
                    Console.WriteLine("Wrong input format, try again please.");
                }
                if (cmd == "E")
                    break;
            }
        }
    }
    class OrderItem
    {

        public string Name { set; get; }
        public int Quantity { set; get; }
        public double Price { set; get; }
        public OrderItem(string name, int quantity, double price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public override string ToString()
        {
            return "Item name: " + Name + "\nOrder num: " + Quantity.ToString()
                    + "\nItem price: " + Price.ToString();
        }

        public override bool Equals(object obj)
        {
            var item = obj as OrderItem;
            return item != null &&
                   Name == item.Name &&
                   Quantity == item.Quantity &&
                   Price == item.Price;
        }

        public override int GetHashCode()
        {
            var hashCode = 1303464505;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            return hashCode;
        }
    }
    class Order
    {
        public string Customer { set; get; }
        public int ID { set; get; }
        public List<OrderItem> items;
        public Order(string customer, int id, List<OrderItem> item)
        {
            Customer = customer;
            ID = id;
            this.items = item;
        }
        public double SumCost()
        {
            double sum=0;
            foreach (OrderItem item in items)
                sum += item.Price * item.Quantity;
            return sum;
        }

        public override string ToString()
        {
            StringBuilder itemlist = new StringBuilder(20);
            for (int i = 0; i < items.ToArray().Length; i++)
                itemlist.Append(items[i].ToString() + "\n");
            return "Client's name: " + Customer + 
                   "\nOrder's ID: " + ID +
                   "\n" + itemlist;
        }

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null &&
                   ID == order.ID;
        }

        public override int GetHashCode()
        {
            var hashCode = 1162405519;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Customer);
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderItem>>.Default.GetHashCode(items);
            return hashCode;
        }
    }
    class OrderService
    {
        private List<Order> orders;
        public OrderService() { orders = new List<Order>(); }
        public bool AddOrder(Order order)
        {
            if (orders.Contains(order))
                return false; 
            orders.Add(order);
            return true;
        }
        public bool UpdateOrder(int id, Order order)
        {
            int index = orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            orders[index] = order;
            return true;
        }
        public bool DeleteOrder(int id)
        {
            int index = orders.FindIndex(o => o.ID == id);
            if (index == -1)
                return false;
            orders.RemoveAt(index);
            return true;
        }
        public List<Order> FindOrder(Func<Order,bool> action)
        {
            var query = orders.Where(action).OrderBy(order=>order.SumCost());
            return query.ToList();
        }
        public List<Order> FindOrder(int id)
        {
            var query = orders.Where(o => o.ID == id).OrderBy(order => order.SumCost());
            return query.ToList();
        }
        public List<Order> FindOrder(string customer)
        {
            var query = orders.Where(o => o.Customer == customer).OrderBy(order => order.SumCost());
            return query.ToList();

        }
        public void OrderSort()  //default sort
        {
            orders.Sort((o1, o2) => o1.ID - o2.ID);
        }
        public void OrderSort(Func<Order,Order,int>func)
        {
            orders.Sort((o1, o2) => func(o1, o2));
        }
        public void PrintAllOrder()
        {
            for (int i = 0; i < orders.ToArray().Length; i++)
                Console.WriteLine(orders[i].ToString() + "\n");
        }
    }
}
