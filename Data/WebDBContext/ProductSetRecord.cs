using System;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class ProductSetRecord
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int ProductSetID { get; set; }
        public int SetCategory { get; set; }
        public int Quantity { get; set; }

        public virtual Products ProductSet { get; set; }
        public virtual Products Product { get; set; }
    }
}
