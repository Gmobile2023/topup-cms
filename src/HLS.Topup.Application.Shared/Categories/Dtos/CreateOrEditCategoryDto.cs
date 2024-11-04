using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Categories.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {
        [Required]
        [StringLength(CategoryConsts.MaxCategoryCodeLength, MinimumLength = CategoryConsts.MinCategoryCodeLength)]
        public string CategoryCode { get; set; }

        [Required]
        [StringLength(CategoryConsts.MaxCategoryNameLength, MinimumLength = CategoryConsts.MinCategoryNameLength)]
        public string CategoryName { get; set; }

        public int Order { get; set; }
        
        public CommonConst.CategoryStatus Status { get; set; }

        [StringLength(CategoryConsts.MaxImageLength, MinimumLength = CategoryConsts.MinImageLength)]
        public string Image { get; set; }

        [StringLength(CategoryConsts.MaxDescriptionLength, MinimumLength = CategoryConsts.MinDescriptionLength)]
        public string Description { get; set; }
        
        public CommonConst.CategoryType Type { get; set; }
        
        public int? ParentCategoryId { get; set; }

        public int? ServiceId { get; set; }
    }
}