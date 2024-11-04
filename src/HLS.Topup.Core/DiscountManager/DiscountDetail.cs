using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HLS.Topup.Categories;
using HLS.Topup.Products;
using HLS.Topup.Services;

namespace HLS.Topup.DiscountManager
{
    [Table("DiscountDetails")]
    public class DiscountDetail : AuditedEntity, IMayHaveTenant
    {
        public virtual decimal? DiscountValue { get; set; }
        public virtual decimal? FixAmount { get; set; }

        public virtual int DiscountId { get; set; }

        [ForeignKey("DiscountId")] public Discount DiscountFk { get; set; }

        public virtual int? ServiceId { get; set; }

        [ForeignKey("ServiceId")] public Service ServiceFk { get; set; }

        public virtual int? CategorysId { get; set; }

        [ForeignKey("CategoryId")] public Category CategoryFk { get; set; }

        public virtual int? ProductId { get; set; }

        [ForeignKey("ProductId")] public Product ProductFk { get; set; }

        [StringLength(255)] public virtual string Description { get; set; }
        public int? TenantId { get; set; }
    }
}
