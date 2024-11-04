using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Categories;
using HLS.Topup.Products;
using HLS.Topup.Services;

namespace HLS.Topup.LimitationManager
{
    [Table("LimitProductDetails")]
    public class LimitProductDetail : AuditedEntity, IMayHaveTenant
    {
        public virtual decimal? LimitAmount { get; set; }//Hạn mức thanh toán
        public virtual int? LimitQuantity { get; set; }//Hạn mức số lượng

        public virtual int LimitProductId { get; set; }

        [ForeignKey("LimitProductId")] public LimitProduct LimitProductFk { get; set; }

        public virtual int? ServiceId { get; set; }

        [ForeignKey("ServiceId")] public Service ServiceFk { get; set; }

        public virtual int? CategoryId { get; set; }

        [ForeignKey("CategoryId")] public Category CategoryFk { get; set; }

        public virtual int? ProductId { get; set; }

        [ForeignKey("ProductId")] public Product ProductFk { get; set; }

        [StringLength(255)] public virtual string Description { get; set; }
        public int? TenantId { get; set; }
    }
}
