using HLS.Topup.Products.Dtos;
using System.Collections.Generic;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
        public CreateOrEditProductDto Product { get; set; }

        public string CategoryCategoryName { get; set; }
        
        public List<ProductCategoryLookupTableDto> ProductCategoryList { get; set; }

        public bool IsEditMode => Product.Id.HasValue;
    }
}