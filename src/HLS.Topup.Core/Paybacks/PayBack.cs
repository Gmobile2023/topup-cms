using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Common;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.Paybacks
{
    [Table("PayBacks")]
    public class PayBack : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required] [StringLength(50)] public virtual string Code { get; set; }

        [Required] [StringLength(255)] public virtual string Name { get; set; }
        
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }

        public virtual int? Total { get; set; }
        public virtual decimal? TotalAmount { get; set; }

        public virtual CommonConst.PayBackStatus Status { get; set; }
        public virtual DateTime? DateApproved { get; set; }
        public virtual DateTime? DatePay { get; set; } //Kỳ thanh toán

        public virtual long? ApproverId { get; set; }
        [StringLength(255)] public virtual string Description { get; set; }
    }
}
