using System;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class OrderItemPool
    {
        public int ItemRecordID { get; set; }
        public string OrderID { get; set; }
        public decimal OrderedPrice { get; set; }
        public string ProductName { get; set; }
        public int Quanitiy { get; set; }
        public string UserName { get; set; }

        public virtual OrderRecord Order { get; set; }
    }
}
