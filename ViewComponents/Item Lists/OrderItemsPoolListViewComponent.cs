using System;
using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OnlineShopDemo.ViewComponents
{
    public class OrderItemsPoolListViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public OrderItemsPoolListViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        //For Showing OrderItemPool in OrdersManage/Detail
        public async Task<IViewComponentResult> InvokeAsync(string id,bool? Manage)
        {
            ViewBag.IsManage = Manage;
            return View("_OrderItemsList", await db.OrderItemPool.Where(i => i.OrderID == id).ToListAsync()); 
        }

    }
}
