using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Data.WebDBContext;

//Originally From Products controller
namespace OnlineShopDemo.ViewComponents
{
    public class CategoryNavViewComponent : ViewComponent
    {

        private readonly OnlineShopDemoDBEntities db;

        public CategoryNavViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        //for function bar (Categorys nav bar) in Product List
        //SectionMode: For coffeeshop and farm shop. only display related categories
        public async Task<IViewComponentResult> InvokeAsync(string mode, int? Start, int? End)
        {
            ViewBag.mode = mode;
            if (Start.HasValue)
            {
                if (End.HasValue)
                {
                    return View("_CategoryBar", await db.Category.Where(item => item.CategoryID > Start && item.CategoryID <= End).ToListAsync()); // Display CategoryID from "start" to "end"
                }
                else
                {
                    return View("_CategoryBar", await db.Category.Where(item => item.CategoryID > Start).ToListAsync()); // Display CategoryID from "start"
                }
            }
            return View("_CategoryBar", await db.Category.ToListAsync()); // Display CategoryID for all range
        }
    }
}
