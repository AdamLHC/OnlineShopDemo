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
    //For ProductSetRecord Creator inside modal in ~/ProductSetList/_ProductSetDetail

    public class ProductSetCreateViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductSetCreateViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ProductSetID = new SelectList(await db.Products.Where(p => p.CategoryID ==2).ToListAsync(), "ProductID", "ProductName");
            ViewBag.ProductID = new SelectList(await db.Products.Where(p => p.CategoryID != 2).ToListAsync(), "ProductID", "ProductName");
            ProductSetRecord setRecord = new ProductSetRecord();
            setRecord.Quantity = 1;

            return View("_ProductSetCreate",setRecord);
        }

    }
}