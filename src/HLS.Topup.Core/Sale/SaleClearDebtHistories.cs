using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Sale
{
    [Table("SaleClearDebtHistories")]
    public class SaleClearDebtHistory : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual long UserId { get; set; }
        [ForeignKey("UserId")] public User UserFk { get; set; }
        public virtual DateTime StartDate { get; set; }
    }
}
