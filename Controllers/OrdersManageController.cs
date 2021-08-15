using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OnlineShopDemo.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrdersManageController : Controller
    {
        private readonly OnlineShopDemoDBEntities db;

        public OrdersManageController(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        // GET: OrdersManage/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRecord = await db.OrderRecord
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (orderRecord == null)
            {
                return NotFound();
            }

            return View(orderRecord);
        }

        // GET: OrdersManage
        public async Task<IActionResult> Index()
        {
            return View(await db.OrderRecord.ToListAsync());
        }

        public async Task<IActionResult> OrderCancel(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRecord = await db.OrderRecord.SingleOrDefaultAsync(m => m.OrderID == id);
            if (orderRecord == null)
            {
                return NotFound();
            }
            else
            {
                orderRecord.IsCanceled = true;
                await db.SaveChangesAsync();
            }
            return View("Details", orderRecord);
        }

        // GET: OrdersManage/Edit/5
        public async Task<IActionResult> OrderComplete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderRecord = await db.OrderRecord.SingleOrDefaultAsync(m => m.OrderID == id);
            if (orderRecord == null)
            {
                return NotFound();
            }
            else
            {
                orderRecord.IsShipped = true;
                await db.SaveChangesAsync();
            }
            return View("Details", orderRecord);
        }

        public async Task<IActionResult> OrderList(bool? isShipped, bool? isCanceled, string Orderby, int PageSize, int Page)
        {

            var Orders = await db.OrderRecord
                .Where(O => O.IsShipped == isShipped && O.IsCanceled == isCanceled)
                .OrderBy(Orderby)
                .ThenBy(o => o.OrderCreateDate)
                .Skip(PageSize * (Page - 1))
                .Take(PageSize)
                .ToListAsync();

            var Morepage = db.OrderRecord
                .Where(O => O.IsShipped == isShipped && O.IsCanceled == isCanceled)
                .OrderBy(Orderby)
                .ThenBy(o => o.OrderCreateDate)
                .Skip(PageSize * Page)
                .Take(PageSize)
                .Count();

            if (isShipped == null && isCanceled == null)//IF isShipped,isCanceled has value
            {
                Orders = await
                    db.OrderRecord.OrderBy(Orderby)
                    .ThenBy(o => o.OrderCreateDate)
                    .Skip(PageSize * (Page - 1))
                    .Take(PageSize)
                    .ToListAsync();

                Morepage = db.OrderRecord
                     .OrderBy(Orderby)
                     .ThenBy(o => o.OrderCreateDate)
                     .Skip(PageSize * Page)
                     .Take(PageSize)
                     .Count();
            }


            if (Morepage != 0) //for display next page and last page
            {
                ViewBag.MorePage = true;
            }
            else
            {
                ViewBag.MorePage = false;
            }


            ViewBag.PageSize = PageSize;
            ViewBag.Page = Page;
            ViewBag.Orderby = Orderby;
            ViewBag.ShippedFilter = isShipped;
            ViewBag.CanceledFilter = isCanceled;

            return View(Orders);
        }

        private bool OrderRecordExists(string id)
        {
            return db.OrderRecord.Any(e => e.OrderID == id);
        }
    }
}