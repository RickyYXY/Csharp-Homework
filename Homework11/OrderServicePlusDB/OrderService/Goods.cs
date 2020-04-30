using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace OrderApp
{
    public class Goods
    {
        public string GoodsID { get; set; }//主键
        public string Name { get; set; }
        public double Price { get; set; }

        //public int OrderItemID { get; set; }//外键

        [Required]
        public OrderItem OrderItem { get; set; }

        public Goods() { }

        public Goods(string iD, string name, double price)
        {
            GoodsID = iD;
            Name = name;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            var goods = obj as Goods;
            return goods != null &&
                   GoodsID == goods.GoodsID &&
                   Name == goods.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GoodsID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }


}
