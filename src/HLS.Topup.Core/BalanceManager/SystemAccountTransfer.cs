using HLS.Topup.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.BalanceManager
{
    [Table("SystemAccountTransfers")]
    public class SystemAccountTransfer : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string TransCode { get; set; }

        [Required]
        [StringLength(SystemAccountTransferConsts.MaxSrcAccountLength,
            MinimumLength = SystemAccountTransferConsts.MinSrcAccountLength)]
        public virtual string SrcAccount { get; set; }

        [Required]
        [StringLength(SystemAccountTransferConsts.MaxDesAccountLength,
            MinimumLength = SystemAccountTransferConsts.MinDesAccountLength)]
        public virtual string DesAccount { get; set; }

        public virtual decimal Amount { get; set; }

        [StringLength(SystemAccountTransferConsts.MaxAttachmentsLength,
            MinimumLength = SystemAccountTransferConsts.MinAttachmentsLength)]
        public virtual string Attachments { get; set; }

        [StringLength(SystemAccountTransferConsts.MaxDescriptionLength,
            MinimumLength = SystemAccountTransferConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual CommonConst.SystemTransferStatus Status { get; set; }
        public virtual long? ApproverId { get; set; }
        public virtual DateTime? DateApproved { get; set; }
    }
}
