using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineShopDemoDBEntities db;

        //Dependency Injection, Invoking DB entities
        public ProductsController(OnlineShopDemoDBEntities context)
        {
            db = context;
        }

        // For Coffee Products
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Headtitle = "線上商店 | 產品類型A";
            ViewBag.background = Url.Content("~/images/ShopBanner.jpg");
            ViewBag.background4k = Url.Content("~/images/ShopBanner4k.jpg");
            ViewBag.mode = "List";
            ViewBag.CaeNavStart = 0; //For CategoryNav ViewComponent
            ViewBag.CaeNavEnd = 2; //For CategoryNav ViewComponent
            if (id == null)
            {
                return BadRequest();
            }
            var result = await db.Products.Where(item => item.ProductID == id && item.Status == "上市").FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.HeadtitleSmall = $"商品: {result.ProductName}";
            return View("Details", result);
        }

        //For Farm Products
        public async Task<IActionResult> FarmDetails(int? id)
        {
            ViewBag.Headtitle = "線上商店 | 產品類型B";
            ViewBag.background = Url.Content("~/images/ShopBanner.jpg");
            ViewBag.background4k = Url.Content("~/images/ShopBanner4k.jpg");
            ViewBag.mode = "FarmList";
            ViewBag.CaeNavStart = 2; //For CategoryNav ViewComponent
            if (id == null)
            {
                return BadRequest();
            }
            var result = await db.Products.Where(item => item.ProductID == id && item.Status == "上市").FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.HeadtitleSmall = $"商品: {result.ProductName}";
            return View("Details", result);
        }

        //For Farm Products
        public async Task<IActionResult> FarmList(int? Cid)
        {
            ViewBag.Title = "購買";
            ViewBag.Headtitle = "線上商店 | 產品類型B";
            ViewBag.HeadtitleSmall = "歡迎，這個頁面展示了屬於產品類別B的商品";
            ViewBag.background = Url.Content("~/images/FarmShopBanner.jpg");
            ViewBag.background4K = Url.Content("~/images/FarmShopBanner4K.jpg");
            ViewBag.mode = "FarmList";
            ViewBag.CaeNavStart = 2; //For CategoryNav ViewComponent
            if (Cid.HasValue)
            {
                return View("List", await db.Products.Where(item => item.CategoryID == Cid && item.Status == "上市").ToListAsync());
            }
            return View("List", await db.Products.Where(item => item.CategoryID > 2 && item.Status == "上市").ToListAsync());
        }

        //For Coffee Products
        public async Task<IActionResult> List(int? Cid, string OrderBy)
        {
            ViewBag.Title = "購買";
            ViewBag.Headtitle = "線上商店 | 產品類型A";
            ViewBag.HeadtitleSmall = "歡迎! 這裡有屬於產品類型A的所有商品";
            ViewBag.background = Url.Content("~/images/ShopBanner.jpg");
            ViewBag.background4k = Url.Content("~/images/ShopBanner4k.jpg");
            ViewBag.mode = "List";
            ViewBag.CaeNavStart = 0;
            ViewBag.CaeNavEnd = 2;
            //var Products = db.Products.Include(p => p.Category);
            if (Cid.HasValue)
            {
                return View("List", await db.Products.Where(item => item.CategoryID == Cid && item.Status == "上市").ToListAsync());
            }
            else
            {
                return View("List", await db.Products.Where(item => item.CategoryID <= 2 && item.Status == "上市").OrderBy(item => item.ProductName).ToListAsync());
            }
        }
    }
}