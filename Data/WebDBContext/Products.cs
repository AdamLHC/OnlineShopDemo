using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OnlineShopDemo.Data.WebDBContext
{
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.ProductSet = new HashSet<ProductSetRecord>();
            this.ProductsINProductSet = new HashSet<ProductSetRecord>();
            this.ShoppingCartPool = new HashSet<ShoppingCartPool>();
        }

        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public DateTime? DbaddDate { get; set; }
        public string Introduction { get; set; }
        public string Notes { get; set; }
        public string PackageSize { get; set; }
        public double? Price { get; set; }
        
        public string ProductName { get; set; }

        public DateTime? PublishDate { get; set; }
        public string Status { get; set; }
        public double? Weight { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductSetRecord> ProductSet { get; set; }
        
        public virtual ICollection<ProductSetRecord> ProductsINProductSet { get; set; }
        
        public virtual ICollection<ShoppingCartPool> ShoppingCartPool { get; set; }
    }
}
