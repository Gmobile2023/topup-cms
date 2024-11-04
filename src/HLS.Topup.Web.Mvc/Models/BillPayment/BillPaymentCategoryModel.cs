using System.Collections.Generic;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Products;

namespace HLS.Topup.Web.Models.BillPayment
{
    public class BillPaymentCategoryModel
    {
        public List<CategoryDto> Categorys { get; set; }
        
        public List<Product> Products { get; set; }
    }
}