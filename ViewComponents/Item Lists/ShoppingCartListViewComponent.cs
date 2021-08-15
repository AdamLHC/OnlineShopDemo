using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

//Originally From Products controller
namespace OnlineShopDemo.ViewComponents
{
    public class ShoppingCartList : ViewComponent
    {
        private readonly OnlineShopDemoDBEntities db;

        public ShoppingCartList(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        //for Shopping Cart List in Orders/OrderConfirm
        //Mode: for backstage mode
        public async Task<IViewComponentResult> InvokeAsync(string mode)
        {
            ViewBag.mode = mode;
            var ShoppingCart = db.ShoppingCartPool.Include(p => p.Products);
            return View("_ShoppingCartList", await ShoppingCart.Where(c => c.UserID == User.Identity.Name && c.IsDeleted == false && c.IsOrdered == false).ToListAsync()); // Display CategoryID for all range
        }
    }
}