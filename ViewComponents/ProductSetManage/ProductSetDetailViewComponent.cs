using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Data.WebDBContext;

//From ProductsManage Controller

namespace OnlineShopDemo.ViewComponents
{
    public class ProductSetDetailViewComponent :ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductSetDetailViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }



        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var Productsets = db.ProductSetRecord.Include(p => p.Product).Include(i => i.ProductSet);
            return View("_ProductSetDetail", await Productsets.Where(item => item.ProductSetID == id).ToListAsync());
        }
    }
}
