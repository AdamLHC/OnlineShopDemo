using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShopDemo.Controllers
{
    public class ReceptionController : Controller
    {
        //@ViewBag.Headtitle is handled in View since Version 1.2.7

        public IActionResult Home()
        {
            return View();
        }

        /*Note: This page is a deleted legacy from build 1.2.5, I keep it for memory.
        public IActionResult CoffeeIntroduction()
        {
            return View();
        }*/

        public IActionResult Store()
        {
            return View();
        }

        public IActionResult Credit()
        {
            return View();
        }
    }
}