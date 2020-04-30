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

    /**
     **/
    public class OrderItem
    {
        public int OrderItemID { get; set; } //主键

        public int OrderID { get; set; }//外键
        public Order Order { get; set; }

        public Goods GoodsItem { get; set; }

        public string GoodsName { get => GoodsItem != null ? this.GoodsItem.Name : ""; }

        public double UnitPrice { get => GoodsItem != null ? this.GoodsItem.Price : 0.0; }


        public uint Quantity { get; set; }

        public OrderItem() { }

        public OrderItem(int index, Goods goods, uint quantity)
        {
            this.OrderItemID = index;
            this.GoodsItem = goods;
            this.Quantity = quantity;
        }

        public double TotalPrice
        {
            get => GoodsItem == null ? 0.0 : GoodsItem.Price * Quantity;
        }

        public override string ToString()
        {
            return $"[No.:{OrderItemID},goods:{GoodsName},quantity:{Quantity},totalPrice:{TotalPrice}]";
        }

        public override bool Equals(object obj)
        {
            var item = obj as OrderItem;
            return item != null &&
                   GoodsName == item.GoodsName;
        }

        public override int GetHashCode()
        {
            var hashCode = -2127770830;
            hashCode = hashCode * -1521134295 + OrderItemID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GoodsName);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
        }
    }
}
