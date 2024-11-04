using HLS.Topup.Common;
using HLS.Topup.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using HLS.Topup.Categories;

namespace HLS.Topup.BalanceManager
{
    [Table("PayBatchBills")]
    public class PayBatchBill : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(PayBatchBillConsts.MaxCodeLength, MinimumLength = PayBatchBillConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(PayBatchBillConsts.MaxNameLength, MinimumLength = PayBatchBillConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual CommonConst.PayBatchBillStatus Status { get; set; }

        public virtual int TotalAgent { get; set; }

        public virtual int TotalTrans { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual int TotalBlockBill { get; set; }

        public virtual decimal AmountPayBlock { get; set; }

        public virtual long? ApproverId { get; set; }

        public virtual DateTime? DateApproved { get; set; }

        [StringLength(PayBatchBillConsts.MaxDescriptionLength, MinimumLength = PayBatchBillConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual decimal? MinBillAmount { get; set; }

        public virtual decimal? MaxAmountPay { get; set; }


        public virtual int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual int ? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

    }
}
