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

    public class CategoryEditViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public CategoryEditViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            Category category = await db.Category.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Specified Category does not found in Database.");
            }

            return View("_CategoryEdit", category);
        }
    }
}
