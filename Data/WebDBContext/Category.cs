using System;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryID { get; set; }
        public string CategoryIntroduction { get; set; }
        public string CategoryName { get; set; }
        public DateTime? DbaddDate { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
