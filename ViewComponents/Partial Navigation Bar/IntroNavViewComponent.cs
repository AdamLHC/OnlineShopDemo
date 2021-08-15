using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopDemo.Models;
using OnlineShopDemo.Data.WebDBContext;

namespace OnlineShopDemo.ViewComponents
{
    //From Reception Controller in Reception/CoffeeIntroduction

    public class IntroNavViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            await Task.Delay(50);
            ViewBag.Number = id;
            return View("_IntroNav");
        }
    }
}
