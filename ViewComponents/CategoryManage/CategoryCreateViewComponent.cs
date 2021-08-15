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

    public class CategoryCreateViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public CategoryCreateViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var category = new Category();
            return View("_CategoryCreate",category);
        }

    }
}
