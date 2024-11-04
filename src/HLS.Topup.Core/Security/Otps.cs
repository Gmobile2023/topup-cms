using System;
using HLS.Topup.Common;

namespace HLS.Topup.Security
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities.Auditing;
    using Abp.Domain.Entities;

    namespace HLS.Topup.Security
    {
        [Table("AbpOtps")]
        public class Otp : AuditedEntity, IMayHaveTenant
        {
            public int? TenantId { get; set; }
            [Required] public virtual string Code { get; set; }
            [StringLength(50)] public virtual string PhoneNumber { get; set; }
            [StringLength(50)] public virtual string RequestIp { get; set; }
            public long? UserId { get; set; }
            public virtual CommonConst.OtpStatus Status { get; set; }
            public virtual CommonConst.OtpType Type { get; set; }
            public virtual DateTime ConfirmDate { get; set; }
        }
    }
}