using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopDemo.Data.WebDBContext;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopDemo.ViewComponents
{
    public class ProductsListViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductsListViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View("_ProductList", await db.Products.Where(p => p.CategoryID == id).ToListAsync());
        }


    }
}
