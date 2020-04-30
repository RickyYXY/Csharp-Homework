using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApp
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }

        //public int OrderID { set; get; }//外键

        [Required]
        public Order Order { get; set; }
         
        public Customer() { }

        public Customer(string iD, string name)
        {
            CustomerID = iD;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var customer = obj as Customer;
            return customer != null &&
                   CustomerID == customer.CustomerID &&
                   Name == customer.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
