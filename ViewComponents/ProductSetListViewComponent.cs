using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Data.WebDBContext;

namespace OnlineShopDemo.ViewComponents
{
    //From Products Controller In Products/Detail while Product's Category is ProductSet

    public class ProductSetListViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductSetListViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var Productsets = db.ProductSetRecord.Include(p => p.Product);
            return View("_ProductSetList",await Productsets.Where(item => item.ProductSetID == id).ToListAsync());
        }
    }
}
