using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopDemo.Data.WebDBContext;

namespace OnlineShopDemo.ViewComponents
{
    //For uploading image, Original from dot net core project.

    public class ProductImageUploadViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ProductImageUploadViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View("_ProductImageUpload", await db.Products.Where(p => p.ProductID == id).FirstAsync());
        }

    }
}
