using HLS.Topup.Common;
using HLS.Topup.Categories;
using HLS.Topup.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Categories
{
    [Table("Categories")]
    public class Category : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(CategoryConsts.MaxCategoryCodeLength, MinimumLength = CategoryConsts.MinCategoryCodeLength)]
        public virtual string CategoryCode { get; set; }

        [Required]
        [StringLength(CategoryConsts.MaxCategoryNameLength, MinimumLength = CategoryConsts.MinCategoryNameLength)]
        public virtual string CategoryName { get; set; }

        public virtual int Order { get; set; }

        public virtual CommonConst.CategoryStatus Status { get; set; }

        [StringLength(CategoryConsts.MaxImageLength, MinimumLength = CategoryConsts.MinImageLength)]
        public virtual string Image { get; set; }

        [StringLength(CategoryConsts.MaxDescriptionLength, MinimumLength = CategoryConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual CommonConst.CategoryType Type { get; set; }

        public virtual int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")] public Category ParentCategoryFk { get; set; }

        public virtual int? ServiceId { get; set; }

        [ForeignKey("ServiceId")] public Service ServiceFk { get; set; }
    }
}