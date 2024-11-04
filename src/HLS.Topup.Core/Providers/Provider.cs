using HLS.Topup.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Providers
{
    [Table("Providers")]
    public class Provider : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(ProviderConsts.MaxCodeLength, MinimumLength = ProviderConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(ProviderConsts.MaxNameLength, MinimumLength = ProviderConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ProviderConsts.MaxImagesLength, MinimumLength = ProviderConsts.MinImagesLength)]
        public virtual string Images { get; set; }

        [StringLength(ProviderConsts.MaxPhoneNumberLength, MinimumLength = ProviderConsts.MinPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        [StringLength(ProviderConsts.MaxEmailAddressLength, MinimumLength = ProviderConsts.MinEmailAddressLength)]
        public virtual string EmailAddress { get; set; }

        [StringLength(ProviderConsts.MaxAddressLength, MinimumLength = ProviderConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        public virtual CommonConst.ProviderType ProviderType { get; set; }

        public virtual CommonConst.ProviderStatus ProviderStatus { get; set; }

        [StringLength(ProviderConsts.MaxDescriptionLength, MinimumLength = ProviderConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(50)] public virtual string TransCodeConfig { get; set; }
        public virtual bool? IsSlowTrans { get; set; }
        [StringLength(50)] public string ParentProvider { get; set; }
        public bool IsAutoDeposit { get; set; }
        public bool IsRoundRobinAccount { get; set; }
        public decimal MinBalance { get; set; }
        public decimal MinBalanceToDeposit { get; set; }
        public decimal DepositAmount { get; set; }

        [StringLength(30)] public  string WorkShortCode { get; set; }
    }
}