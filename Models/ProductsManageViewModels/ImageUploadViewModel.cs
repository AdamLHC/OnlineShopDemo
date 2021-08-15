using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineShopDemo.Models.ProductsManageViewModels
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Require File")]
        public IFormFile firle { get; set; }
    }
}
