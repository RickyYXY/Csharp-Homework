using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OrderApp
{
    public class OScontext:DbContext
    {
        public OScontext():base("OSDataBase")
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<OScontext>());
        }

        public DbSet<Goods> Goods { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
