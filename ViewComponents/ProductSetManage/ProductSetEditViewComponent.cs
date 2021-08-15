using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopDemo.Data.WebDBContext;

namespace OnlineShopDemo.ViewComponents
{
    //From ProductsManage Controller
    //For ProductSetRecord Editor inside modal in ~/ProductSetList/_ProductSetDetail

    public class ProductSetEditViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductSetEditViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pid, int psid)
        {
            ViewBag.ProductID = new SelectList(await db.Products.Where(p => p.CategoryID != 2).ToListAsync(), "ProductID", "ProductName");
            ProductSetRecord product = await db.ProductSetRecord.Where(p => p.ProductSetID == psid && p.ProductID == pid).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new Exception("Specified Product does not found in Database.");
            }

            return View("_ProductSetEdit", product);
        }
    }
}
