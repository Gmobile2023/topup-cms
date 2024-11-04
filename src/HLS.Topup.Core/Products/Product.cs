using System.ComponentModel;
using HLS.Topup.Common;
using HLS.Topup.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Products
{
    [Table("Products")]
    public class Product : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(ProductConsts.MaxProductCodeLength, MinimumLength = ProductConsts.MinProductCodeLength)]
        public virtual string ProductCode { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxProductNameLength, MinimumLength = ProductConsts.MinProductNameLength)]
        public virtual string ProductName { get; set; }

        public virtual int Order { get; set; }

        public virtual decimal? ProductValue { get; set; }

        public virtual CommonConst.ProductType ProductType { get; set; }

        public virtual CommonConst.ProductStatus Status { get; set; }

        [StringLength(ProductConsts.MaxUnitLength, MinimumLength = ProductConsts.MinUnitLength)]
        public virtual string Unit { get; set; }

        public virtual decimal? RefValue { get; set; }

        [StringLength(ProductConsts.MaxImageLength, MinimumLength = ProductConsts.MinImageLength)]
        public virtual string Image { get; set; }

        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }
        
        [StringLength(500)]
        public virtual string CustomerSupportNote { get; set; }
        
        [StringLength(500)]
        public virtual string UserManualNote { get; set; }

        public virtual decimal? MinAmount { get; set; }

        public virtual decimal? MaxAmount { get; set; }

        public virtual int CategoryId { get; set; }

        [DefaultValue("true")]
        public virtual bool IsShowOnFrontend { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

    }
}
