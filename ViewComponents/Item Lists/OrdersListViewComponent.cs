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
    public class OrdersListViewComponent : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public OrdersListViewComponent(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        //For Listing Orders in Account/Index
        public async Task<IViewComponentResult> InvokeAsync(string UserName, int Select)
        {
            var orderlist = await db.OrderRecord
                .OrderByDescending(o => o.OrderCreateDate)
                .ThenBy(o => o.IsShipped)
                .Where(o => o.UserName == UserName)
                .Take(Select)
                .ToListAsync();

            return View("_OrdersList", orderlist);
        }
    }
}
