using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Data.WebDBContext;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShopDemo.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly OnlineShopDemoDBEntities db;

        public ShoppingCartController(OnlineShopDemoDBEntities context)
        {
            db = context;    
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Cart()
        {
            var OnlineShopDemoDBEntities = db.ShoppingCartPool.Include(s => s.Products);
            return View(await OnlineShopDemoDBEntities.Where(c => c.UserID == User.Identity.Name.ToString() && c.IsOrdered == false && c.IsDeleted == false ).ToListAsync());
        }

        public async Task<IActionResult> AddtoCart(int id)
        {
            // Get a cart item match product ID, match Username (or anon cookie key), not ordered and not deleted.
            ShoppingCartPool CartItem = await db.ShoppingCartPool.FirstOrDefaultAsync(c => c.ProductID == id && c.UserID == User.Identity.Name.ToString() && c.IsOrdered.Equals(false) && c.IsDeleted.Equals(false));
            if (CartItem == null)
            {
                //Add new record to Cart 
                var Products = await db.Products.FirstOrDefaultAsync(p => p.ProductID == id && p.Status == "�W��"); //Check requested product exits and is published
                if (Products == null)
                {
                    return BadRequest();
                }
                else
                {
                    CartItem = new ShoppingCartPool
                    {

                        ItemID = Guid.NewGuid().ToString(),
                        UserID = User.Identity.Name.ToString(),
                        ProductID = id,
                        CategoryID = Products.CategoryID,
                        Products = Products,
                        Quantity = 1,
                        IsDeleted = false,
                        IsOrdered = false,
                        RecordCreateDate = DateTime.Now
                    };
                    db.ShoppingCartPool.Add(CartItem);
                }
            }
            else
            {
                //Record quantity +1
                CartItem.Quantity++;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> MarkDelete(int id)
        {
            ShoppingCartPool CartItem = await db.ShoppingCartPool.FirstOrDefaultAsync(c => c.ProductID == id && c.UserID == User.Identity.Name.ToString() && c.IsOrdered.Equals(false) && c.IsDeleted.Equals(false));
            if (CartItem == null) //Check theres actually an item exits
            {
                return NotFound();
            }
            else
            {
                CartItem.IsDeleted = true;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> UpdateQuantity(int id, int Qua)
        {
            ShoppingCartPool CartItem = await db.ShoppingCartPool.FirstOrDefaultAsync(c => c.ProductID == id && c.UserID == User.Identity.Name.ToString() && c.IsOrdered.Equals(false) && c.IsDeleted.Equals(false));
            if (CartItem == null) //Check theres actually an item exits
            {
                return NotFound();
            }
            else
            {
                CartItem.Quantity = Qua;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Cart");
        }

        private bool ShoppingCartPoolExists(string id)
        {
            return db.ShoppingCartPool.Any(e => e.ItemID == id);
        }
    }
}
