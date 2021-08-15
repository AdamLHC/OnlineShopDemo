using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "需要會員名稱")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "需要電子郵件")]
        [EmailAddress]
        [Display(Name ="Emailaddress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "需要會員密碼")]
        [StringLength(100, ErrorMessage = "密碼最短需要{2}個字", MinimumLength = 5)]
        [DataType(DataType.Password,ErrorMessage = "密碼格式錯誤")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "確認密碼和輸入的密碼不同")]
        public string ConfirmPassword { get; set; }
    }
}
