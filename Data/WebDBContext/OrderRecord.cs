using System;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class OrderRecord
    {
        public OrderRecord()
        {
            OrderItemPool = new HashSet<OrderItemPool>();
        }

        public string OrderID { get; set; }
        public string EmailAddress { get; set; }
        public bool IsShipped { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime OrderCreateDate { get; set; }
        public string OrdererName { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserName { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public string ShopNote { get; set; }

        public virtual ICollection<OrderItemPool> OrderItemPool { get; set; }
    }
}
