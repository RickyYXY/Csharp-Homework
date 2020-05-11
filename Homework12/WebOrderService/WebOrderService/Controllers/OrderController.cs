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
        public ActionResult<List<Order>>GetOrderByGoodsName(string goodsName)
        {
            var orders = AllOrders()
          .Where(o => o.Items.Count(i => i.GoodsItem.Name == goodsName) > 0);
            return orders.ToList();
        }

        //GET: api/order/customer?name=
        [HttpGet("customer")]
        public ActionResult<List<Order>>GetOrderByCustomerName(string customerName)
        {
            var orders = AllOrders()
          .Where(o => o.Customer.Name == customerName);
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
    }
}