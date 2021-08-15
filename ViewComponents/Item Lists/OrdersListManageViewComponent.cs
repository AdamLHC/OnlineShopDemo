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
    public class OrdersListManageViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public OrdersListManageViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        //For Listing Orders in OrdersManage/Index
        public async Task<IViewComponentResult> InvokeAsync(string Orderby, bool Shipped, bool Deleted, int Select)
        {
            var orderlist = await db.OrderRecord
                .Where(o => o.IsShipped == Shipped && o.IsCanceled == Deleted)
                .Take(Select)
                .OrderBy(o => o.OrderCreateDate)
                .ToListAsync();


            return View("_OrdersListManage", orderlist); // Display CategoryID for all range
        }
    }
}