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
            return orderContext.Orders.Include(o => o.Items.Select(i => i.GoodsItem))
                .Include("Customer");
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
    }
}