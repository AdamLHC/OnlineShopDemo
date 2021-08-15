using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Controllers
{
    //Any Action without a prefix is for Product, like Delete is for ProductDelete

    [Authorize(Roles = "Administrator")]
    public class ProductsManageController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly OnlineShopDemoDBEntities db;

        public ProductsManageController(OnlineShopDemoDBEntities context, IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;
        }

        public async Task<IActionResult> CategoryList()
        {
            return View(await db.Category.Skip(2).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CategoryCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryID = await db.Category.MaxAsync(c => c.CategoryID) + 1;
                category.DbaddDate = DateTime.Now;
                db.Category.Add(category);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public async Task<IActionResult> CategoryEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("CategoryList");
            }
            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var category = await db.Category.Where(c => c.CategoryID == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return BadRequest();
            }

            //Delete all ProductSet record having products from this category
            var ProductSets = await db.ProductSetRecord.Where(P => P.CategoryID == id).ToListAsync();
            if (ProductSets.Count() != 0)
            {
                db.ProductSetRecord.RemoveRange(ProductSets);
            }
            
            //Delete all Products within this Category
            var products =await db.Products.Where(p => p.CategoryID == id).ToListAsync();
            if (products.Count() != 0)
            {
                db.Products.RemoveRange(products);
            }

            db.Category.Remove(category);
            await db.SaveChangesAsync();

            return RedirectToAction("CategoryList");
        }


        // GET: ProductsManage/Create
        public IActionResult Create()
        {
            ViewBag.ProductStatus = new SelectList(new[]
                        {
                        new SelectListItem {Text= "等待上市",Value="未上市" },
                        new SelectListItem {Text= "上市",Value="上市" },
                        new SelectListItem {Text= "下架",Value="下架" },
                        new SelectListItem {Text= "缺貨",Value="缺貨" }
                    }, "Text", "Value", "未上市");
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName");
            return View("Create");
        }

        // POST: ProductsManage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products Products)
        {
            if (ModelState.IsValid)
            {
                Products.ProductID = await db.Products.MaxAsync(c => c.ProductID) + 1;
                Products.DbaddDate = DateTime.Now;
                db.Products.Add(Products);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.ProductStatus = new SelectList(new[]{
                        new SelectListItem {Text= "等待上市",Value="未上市" },
                        new SelectListItem {Text= "上市",Value="上市" },
                        new SelectListItem {Text= "下架",Value="下架" },
                        new SelectListItem {Text= "缺貨",Value="缺貨" }
                    }, "Text", "Value", "未上市");
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", Products.CategoryID);
            return View(Products);
        }

        // GET: ProductsManage/Delete  The confirm delete action.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Products Products = await db.Products.Where(item => item.ProductID == id).FirstOrDefaultAsync();
            if (Products == null)
            {
                return NotFound();
            }
            return View(Products);
        }

        // POST: ProductsManage/Delete  The actual delete action.
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Following table will be affected too:  ShoopingCartPool,ProductSetRecords.

            Products Products = await db.Products.Where(item => item.ProductID == id).FirstOrDefaultAsync();

            var Carts = await db.ShoppingCartPool.Where(item => item.ProductID == id).ToListAsync();//Check ShoppingCarts
            db.ShoppingCartPool.RemoveRange(Carts);

            if (Products.CategoryID == 2) //Check if this product is a ProductSet
            {
                var records = await db.ProductSetRecord.Where(item => item.ProductSetID == id).ToListAsync(); //Check ProductSetRecords
                db.ProductSetRecord.RemoveRange(records);
            }
            else
            {
                var records = await db.ProductSetRecord.Where(item => item.ProductID == id).ToListAsync(); //Check ProductRecords
                db.ProductSetRecord.RemoveRange(records);
            }

            db.Products.Remove(Products);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: ProductsManage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Products Products = await db.Products.Where(item => item.ProductID == id).FirstOrDefaultAsync();
            if (Products == null)
            {
                return NotFound();
            }
            return View(Products);
        }

        // GET: ProductsManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Products Products = await db.Products.Where(item => item.ProductID == id).FirstOrDefaultAsync();
            if (Products == null)
            {
                return NotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", Products.CategoryID);
            return View(Products);
        }

        // POST: ProductsManage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products Products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Products).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", Products.CategoryID);
            return View(Products);
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(ICollection<IFormFile> FileSelect, int PID)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images/ProductResource/");
            var file = FileSelect.First();
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, PID + Path.GetExtension(file.FileName)), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                ViewBag.path = $"{Path.Combine(uploads, file.FileName)}";
            }
            return RedirectToActionPermanent("Index");
        }

        // GET: ProductsManage
        public async Task<IActionResult> Index()
        {
            var Products = db.Products.Include(p => p.Category);
            return View(await Products.OrderBy(c => c.CategoryID).ToListAsync());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductSetCreate(ProductSetRecord ProductSetRecord)
        {
            if (ModelState.IsValid)
            {
                var SetRecord = await db.ProductSetRecord.Where(s => s.ProductID == ProductSetRecord.ProductID).FirstOrDefaultAsync();
                if (SetRecord == null) //Check if record exits
                {
                    var Cat = await db.Products.Where(c => c.ProductID == ProductSetRecord.ProductID).FirstOrDefaultAsync();
                    ProductSetRecord.CategoryID = Cat.CategoryID; //Just to add CategoryID, cause' it makes no sense asking people doing it.
                    ProductSetRecord.SetCategory = 2;
                    db.ProductSetRecord.Add(ProductSetRecord);
                    await db.SaveChangesAsync();
                }
                else
                {
                    SetRecord.Quantity = ProductSetRecord.Quantity;
                    db.Entry(SetRecord).State = EntityState.Modified; //Update, not Add
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("ProductSetList");
            }
            return RedirectToActionPermanent("ProductSetList");
        }

        public async Task<IActionResult> ProductSetDelete(int pid, int psid)
        {
            ProductSetRecord productSetRecord = db.ProductSetRecord.Where(p => p.ProductSetID == psid && p.ProductID == pid).FirstOrDefault();
            db.ProductSetRecord.Remove(productSetRecord);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductSetList");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult ProductSetEdit(ProductSetRecord ProductSetRecord) //For ProductSetEditor from ProductSetEditViewComponent to sumbit changes
        {
            if (ModelState.IsValid)
            {
                db.Entry(ProductSetRecord).State = EntityState.Modified;  //Database Can't create new entity from this method, if database can't update any data then will throw exception

                //db.ProductSetRecord.Add(ProductSetRecord);
                db.SaveChanges();
                return RedirectToAction("ProductSetList");
            }
            return RedirectToActionPermanent("ProductSetList");
        }

        public async Task<IActionResult> ProductSetList()
        {
            var Products = db.Products.Include(p => p.Category);
            return View(await Products.Where(c => c.Category.CategoryID == 2).ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}