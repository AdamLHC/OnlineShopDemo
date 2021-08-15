using System;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class ShoppingCartPool
    {
        public string ItemID { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOrdered { get; set; }
        public int Quantity { get; set; }
        public DateTime RecordCreateDate { get; set; }
        public string UserID { get; set; }

        public virtual Products Products { get; set; }
    }
}
