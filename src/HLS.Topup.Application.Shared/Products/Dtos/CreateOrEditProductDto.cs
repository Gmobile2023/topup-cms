using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Products.Dtos
{
    public class CreateOrEditProductDto : EntityDto<int?>
    {
        [Required]
        [StringLength(ProductConsts.MaxProductCodeLength, MinimumLength = ProductConsts.MinProductCodeLength)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxProductNameLength, MinimumLength = ProductConsts.MinProductNameLength)]
        public string ProductName { get; set; }
        
        public int Order { get; set; }
        
        public decimal? ProductValue { get; set; }

        public CommonConst.ProductType ProductType { get; set; }
        
        public CommonConst.ProductStatus Status { get; set; }

        [StringLength(ProductConsts.MaxUnitLength, MinimumLength = ProductConsts.MinUnitLength)]
        public string Unit { get; set; }

        public decimal? RefValue { get; set; }
        
        [StringLength(ProductConsts.MaxImageLength, MinimumLength = ProductConsts.MinImageLength)]
        public string Image { get; set; }

        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        
        public string CustomerSupportNote { get; set; }
        
        public string UserManualNote { get; set; }

        public bool IsShowOnFrontend { get; set; }

        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }
    }
}