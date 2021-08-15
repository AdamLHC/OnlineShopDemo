using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OnlineShopDemo.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OnlineShopDemoDBEntities db;

        public OrdersController(OnlineShopDemoDBEntities context)
        {
            db = context;
        }



        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.PaymentMethod = new SelectList(new[]
            {
                        new SelectListItem {Text= "貨到付款",Value="貨到付款" },
                        new SelectListItem {Text= "匯款",Value="匯款" }
                    }, "Text", "Value", "貨到付款");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailAddress,OrdererName,PhoneNumber,ShippingAddress,UserName,PaymentMethod,Note")] OrderRecord orderRecord)
        {
            if (ModelState.IsValid)
            {
                var CartItem = await db.ShoppingCartPool.Include(p => p.Products).Where(c => c.UserID == User.Identity.Name && c.IsDeleted == false && c.IsOrdered == false).ToListAsync();
                double Price = 0;
                foreach (var item in CartItem)
                {
                    Price = Price + item.Products.Price.Value * item.Quantity; /*because Products.Price is Nullable*/
                }

                //Phase 1: Create Order
                orderRecord.OrderID = Guid.NewGuid().ToString();
                orderRecord.IsShipped = false;
                orderRecord.TotalPrice = decimal.Parse(Price.ToString()); //Do not let client side handle this value. Html modify attack can modify value.
                orderRecord.OrderCreateDate = DateTime.Now;
                db.Add(orderRecord);
                await db.SaveChangesAsync();
                //Caculate shipping fee
                if (Price > 3000)
                {
                    //No shipping fee
                }
                else if (orderRecord.PaymentMethod == "貨到付款")
                {
                    orderRecord.TotalPrice = orderRecord.TotalPrice + 150;
                }
                else if (orderRecord.PaymentMethod == "匯款")
                {
                    orderRecord.TotalPrice = orderRecord.TotalPrice + 100;
                }
                else
                {
                    RedirectToAction("Create");  //PaymentMethod is empty or different, something wrong.
                }


                //Phase 2: Create Order Items
                foreach (var item in CartItem)
                {
                    db.OrderItemPool.Add(new OrderItemPool
                    {
                        /* Create a new record in OrderItemPool */
                        ItemRecordID = await db.OrderItemPool.DefaultIfEmpty().MaxAsync(c => c == null ? 0 :c.ItemRecordID) + 1,
                        Order = orderRecord,
                        Quanitiy = item.Quantity,
                        UserName = orderRecord.UserName,
                        OrderedPrice = Decimal.Parse((item.Products.Price.Value * item.Quantity).ToString()),
                        ProductName = item.Products.ProductName,
                    });
                    item.IsOrdered = true;
                    await db.SaveChangesAsync(); //call save change immeaditly.
                }

                return View("Finish");
            }
            return View(orderRecord);
        }

        public async Task<IActionResult> OrderList(/*bool? isShipped, bool? isCanceled,*/ string Orderby, int PageSize, int Page)
        {
            if (Orderby=="OrderCreateDate")
            {
                Orderby = "OrderCreateDate Desc";
            }

            var Orders = await db.OrderRecord.Where(o => o.UserName == User.Identity.Name)
                      .OrderBy(Orderby)
                      .ThenByDescending(o => o.OrderCreateDate)
                      .Skip(PageSize * (Page - 1))
                      .Take(PageSize+1)
                      .ToListAsync();

            ViewBag.PageSize = PageSize;
            ViewBag.Page = Page;
            ViewBag.Orderby = Orderby;
            if (Orders.Count() > PageSize) //for display next page and last page
            {
                ViewBag.MorePage = true;
            }
            else
            {
                ViewBag.MorePage = false;
            }
            Orders.Remove(Orders.Last());

            return View(Orders);

            /*if (isShipped == null && isCanceled == null)
            {
                var Orders = await db.OrderRecord.Where(o => o.UserName == User.Identity.Name)
                      .OrderBy(Orderby).ThenByDescending(o => o.OrderCreateDate)
                      .Skip(PageSize * (Page - 1)).Take(PageSize).ToListAsync();
                ViewBag.PageSize = PageSize;
                ViewBag.Page = Page;
                ViewBag.Orderby = Orderby;
                ViewBag.ShippedFilter = isShipped;
                ViewBag.CanceledFilter = isCanceled;

                return View(Orders);
            }
            else
            {
                var Orders = await db.OrderRecord
                  .Where(O => O.UserName == User.Identity.Name && O.IsShipped == isShipped && O.IsCanceled == isCanceled)
                  .OrderBy(Orderby)
                  .ThenBy(o => o.OrderCreateDate)
                  .Skip(PageSize * (Page - 1))
                  .Take(PageSize)
                  .ToListAsync();
                ViewBag.PageSize = PageSize;
                ViewBag.Page = Page;
                ViewBag.Orderby = Orderby;
                ViewBag.ShippedFilter = isShipped;
                ViewBag.CanceledFilter = isCanceled;

                return View(Orders);
            }*/
        }

        // GET: Orders/Details
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

        public IActionResult Finish()
        {
            return View();
        }

        //Confirm Before Creating a new order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderConfirm([Bind("OrderID,OrderCreateDate,UserName,EmailAddress,OrdererName,PhoneNumber,ShippingDate,ShippingAddress,PaymentMethod,Note")] OrderRecord orderRecord)
        {
            if (ModelState.IsValid)
            {
                var CartItem = await db.ShoppingCartPool.Include(p => p.Products).Where(c => c.UserID == User.Identity.Name && c.IsDeleted == false && c.IsOrdered == false).ToListAsync();
                double Price = 0;
                foreach (var item in CartItem)
                {
                    Price = Price + item.Products.Price.Value * item.Quantity; /*because Products.Price is Nullable*/
                }

                /* Shipping fee Only for information, actualy price is caculated in "[Post]Create" */
                if (Price > 3000)
                {
                    ViewBag.PayMethod = "免運費";
                    ViewBag.ShippingFee = 0;
                }
                else if (orderRecord.PaymentMethod == "貨到付款")
                {
                    ViewBag.PayMethod = "貨到付款";
                    ViewBag.ShippingFee = 150;
                }
                else if (orderRecord.PaymentMethod == "匯款")
                {
                    ViewBag.PayMethod = "匯款"; /* Only for information, actualy price is caculated in "[Post]Create" */
                    ViewBag.ShippingFee = 100;
                }
                else
                {
                    RedirectToAction("Create");  //PaymentMethod is empty or different, something wrong.
                }

                orderRecord.UserName = User.Identity.Name.ToString();
                ViewBag.Price = Price;
                int Count = 0;
                foreach (var item in CartItem) { Count = Count + item.Quantity; }
                ViewBag.ProductCount = Count;
                return View("OrderConfirm", orderRecord);
            }
            return View("Create");
        }
    }
}