using HLS.Topup.Services;
using HLS.Topup.Providers;
using HLS.Topup.Categories;
using HLS.Topup.Products;
using HLS.Topup.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace HLS.Topup.Configuration
{
    [Table("ServiceConfigurations")]
    public class ServiceConfiguration : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        [Required]
        [StringLength(ServiceConfigurationConsts.MaxNameLength,
            MinimumLength = ServiceConfigurationConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual bool IsOpened { get; set; }

        public virtual int Priority { get; set; }

        [StringLength(ServiceConfigurationConsts.MaxDescriptionLength,
            MinimumLength = ServiceConfigurationConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }


        public virtual int? ServiceId { get; set; }

        public virtual int? ProviderSetTransactionTimeout { get; set; }
        public virtual int? ProviderMaxWaitingTimeout { get; set; }
        public virtual bool? IsEnableResponseWhenJustReceived { get; set; }
        public virtual int? WaitingTimeResponseWhenJustReceived { get; set; }

        [StringLength(ServiceConfigurationConsts.MaxStatusResponseWhenJustReceivedLength,
            MinimumLength = ServiceConfigurationConsts.MinStatusResponseWhenJustReceivedLength)]
        public virtual string StatusResponseWhenJustReceived { get; set; }

        public virtual bool IsLastConfiguration { get; set; }

        [ForeignKey("ServiceId")] public Service ServiceFk { get; set; }

        public virtual int? ProviderId { get; set; }

        [ForeignKey("ProviderId")] public Provider ProviderFk { get; set; }

        public virtual int? CategoryId { get; set; }

        [ForeignKey("CategoryId")] public Category CategoryFk { get; set; }

        public virtual int? ProductId { get; set; }

        [ForeignKey("ProductId")] public Product ProductFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
        [StringLength(20)] public string AllowTopupReceiverType { get; set; }

        public  decimal? RateRunning { get; set; }
    }
}