using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Categories;
using HLS.Topup.Products;
using HLS.Topup.Services;

namespace HLS.Topup.FeeManager
{
    [Table("FeeDetails")]
    public class FeeDetail : AuditedEntity, IMayHaveTenant
    {
        public virtual decimal? AmountMinFee { get; set; }//Số tiền áp dụng phí tối thiểu
        public virtual decimal? MinFee { get; set; }//Phí tối thiểu
        public virtual decimal? AmountIncrease { get; set; }//Số tiền tăng thêm
        public virtual decimal? SubFee { get; set; }//Phụ phí

        public virtual int FeeId { get; set; }

        [ForeignKey("FeeId")] public Fee FeeFk { get; set; }

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
