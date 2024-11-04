using HLS.Topup.Common;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Sale
{
    [Table("SaleAssignAgents")]
    public class SaleAssignAgent : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual byte Status { get; set; }
        [StringLength(SaleClearDebtConsts.MaxDescriptionsLength,
            MinimumLength = SaleClearDebtConsts.MinDescriptionsLength)]
        public virtual string Descriptions { get; set; }

        public virtual long SaleUserId { get; set; }//UserID của Sale

        [ForeignKey("SaleUserId")] 
        public User SaleUserFk { get; set; }
        
        public virtual long? UserAgentId { get; set; }//UserID của agent

        [ForeignKey("UserAgentId")] public User UserAgentFk { get; set; }
    }
}
