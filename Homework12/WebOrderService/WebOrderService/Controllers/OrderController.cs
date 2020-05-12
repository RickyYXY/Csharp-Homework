using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebOrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderContext orderContext;

        public OrderController(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        private IQueryable<Order> AllOrders()
        {
            return orderContext.Orders.Include(o => o.Items)
                .ThenInclude(i => i.GoodsItem)
                .Include(o => o.Customer);
        }

        //GET: api/order/ini
        //初始化
        [HttpGet("ini")]
        public ActionResult<List<Order>>InitializeOrders()
        {
            Order order = new Order(new Customer("li"), new List<OrderItem>());
            order.AddItem(new OrderItem(1, new Goods("apple", 100.0), 10));
            Order order2 = new Order(new Customer("zhang"), new List<OrderItem>());
            order2.AddItem(new OrderItem(1, new Goods("egg", 200.0), 10));
            try
            {
                orderContext.Orders.Add(order);
                orderContext.Orders.Add(order2);
                orderContext.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return AllOrders().ToList();
        }

        //GET: api/order
        [HttpGet]
        public ActionResult<List<Order>>GetAllOrders()
        {
            var orders = AllOrders();
            return orders.ToList();
        }

        //GET: api/order/{id}
        [HttpGet("{id}")]
        public ActionResult<Order>GetOrderByID(string id)
        {
            var order = AllOrders().FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();
            return order;
        }

        //GET: api/order/goods?name=
        [HttpGet("goods")]
        public ActionResult<List<Order>>GetOrderByGoodsName(string name)
        {
            var orders = AllOrders()
          .Where(o => o.Items.Count(i => i.GoodsItem.Name == name) > 0);
            return orders.ToList();
        }

        //GET: api/order/customer?name=
        [HttpGet("customer")]
        public ActionResult<List<Order>>GetOrderByCustomerName(string name)
        {
            var orders = AllOrders()
          .Where(o => o.Customer.Name == name);
            return orders.ToList();
        }

        //Get: api/order/total?total=
        [HttpGet("total")]
        public ActionResult<List<Order>>GetOrderByTotalAmount(double total)
        {
            var orders = AllOrders()
          .Where(o => o.Items.Sum(item => item.GoodsItem.Price * item.Quantity) > total);
            return orders.ToList();
        }

        //POST: api/order
        [HttpPost]
        public ActionResult<Order>AddOrder(Order order)
        {
            try
            {
                orderContext.Orders.Add(order);
                orderContext.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return order;
        }

        //PUT: api/order/{id}
        [HttpPut("{id}")]
        public ActionResult<Order>UpdateOrder(string id,Order order)
        {
            if(id!=order.Id)
                return BadRequest("Id cannot be modified!");
            try
            {
                orderContext.Entry(order).State = EntityState.Modified;
                orderContext.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        //DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public ActionResult<Order>DeleteOrder(string id)
        {
            try
            {
                var order = AllOrders().FirstOrDefault(o => o.Id == id);
                if(order!=null)
                {
                    for(int i=0;i<order.Items.Count;i++)
                    {
                        orderContext.GoodItems.Remove(order.Items[i].GoodsItem);
                        orderContext.OrderItems.Remove(order.Items[i]);
                    }
                    orderContext.Customers.Remove(order.Customer);
                    orderContext.Orders.Remove(order);
                    orderContext.SaveChanges();
                }                
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return NoContent();
        }
    }
}