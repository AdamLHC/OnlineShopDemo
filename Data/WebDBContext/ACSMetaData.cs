using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopDemo.Data.WebDBContext
{
    /*
 This class exits because Entity Framework Database-First does not support valdation from database  (2016/12/17)
 According to: http://stackoverflow.com/questions/14059455/adding-validation-attributes-with-an-entity-framework-data-model
     */

#pragma warning disable 0649
    /*  Usually it's bad idea to supress warrnings. But in here compiler will warning 
     *  " XXX is not Field 'field' is never assigned to, and will always have its default value null" 
     So we turn it off here since these Database field will invoke at other place. */

    internal sealed class CategoryMetedata //Valadtion Data for Category in Database
    {
        [Required(ErrorMessage = "CategoryID is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
    }

    internal sealed class ProductsMetedata //Valadtion Data for Products in Database
    {
        [Required(ErrorMessage = "請輸入ID (如果看到這段文字請與網管聯絡)")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "請輸入商品名稱(必要)")]
        [StringLength(30)]
        public string ProductName { get; set;}

        [Required(ErrorMessage = "請輸入商品介紹(必要)")]
        public string Introduction { get; set; }

        [DataType(DataType.Date,ErrorMessage = "格式錯誤(請輸入yyyy/mm/dd)")]     
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PublishDate { get; set; }

        [DataType("number", ErrorMessage = "格式錯誤(只接受純數字，單位為公克)")]   
        public double? Weight { get; set; }

        [Required(ErrorMessage = "請輸入商品價格(必要)")]
        public double? Price { get; set; }
    }

    internal sealed class ProductSetRecordMeteData //Valadtion Data for ProductSetRecord in Database
    {
        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "CategoryID is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "ProductSetID is required")]
        public int ProductSetID { get; set; }

        [Required(ErrorMessage = "SetCategoryID is required")]
        public int SetCategory { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }

    internal sealed class OrderRecordMetedata //Valadtion Data for OrderRecord in Database
    {
        //OrderID is handled by database.

        [Required(ErrorMessage = "OrderCreateDate is required(If you see this, please contact service)")]
        [DataType(DataType.Date, ErrorMessage = "格式錯誤(請輸入yyyy/mm/dd)")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime OrderCreateDate { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入收件人姓名")]
        public string OrdererName{ get; set;}

        [Required(ErrorMessage = "請輸入電話號碼")]
        public string PhoneNumber{ get; set;}

        [Required(ErrorMessage = "請輸入收件地址")]
        public string ShippingAddress{ get; set;}

        [Required(ErrorMessage = "TotalPrice is required")]
        public decimal TotalPrice{ get; set;}

        [Required(ErrorMessage = "PaymentMethod is required")]
        public string PaymentMethod { get; set; }

        [DataType(DataType.Date, ErrorMessage = "格式錯誤(請輸入yyyy/mm/dd)")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ShippingDate { get; set; }

        [Required(ErrorMessage = "IsShipped is required")]
        public bool IsShipped{ get; set;}
    }

#pragma warning restore 0649

    [ModelMetadataType(typeof(CategoryMetedata))]
    public partial class Category
    {
    }

    [ModelMetadataType(typeof(ProductsMetedata))]
    public partial class Products
    {
    }

    [ModelMetadataType(typeof(ProductSetRecordMeteData))]
    public partial class ProductSetRecord
    {
    }

    [ModelMetadataType(typeof(OrderRecordMetedata))]
    public partial class OrderRecord
    {
    }
}
