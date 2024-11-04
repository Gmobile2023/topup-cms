using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Products.Dtos;
using System.Collections.Generic;
using HLS.Topup.Authorization.Users.Dto;
using Abp.Extensions;
using HLS.Topup.Dtos.Discounts;

namespace HLS.Topup.Web.Areas.App.Models.Discounts
{
    public class CreateOrEditDiscountModalViewModel
    {
        public CreateOrEditDiscountDto Discount { get; set; }

        public string UserName { get; set; }
        
        public string CreationTime { get; set; }

        public string UserApproved { get; set; }
        
        public string UserCreated { get; set; }

        public List<DiscountUserLookupTableDto> DiscountUserList { get; set; }

        public List<ProductCategoryLookupTableDto> ProductCategoryList { get; set; }
        
        public List<DiscountServiceLookupTableDto> DiscountServiceList { get; set; }

        public bool IsEditMode => Discount.Id.HasValue;
        public bool IsViewMode { get; set; }
        public List<DiscountDetailDto> ProductDiscounts { get; set; }
    }
}