using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Common;
using HLS.Topup.Providers;
using HLS.Topup.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HLS.Topup.Configuration
{
    [Table("PartnerServiceConfigurations")]
    public class PartnerServiceConfiguration : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ServiceConfigurationConsts.MaxNameLength,
            MinimumLength = ServiceConfigurationConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual CommonConst.PartnerServiceConfigurationStatus Status { get; set; }

        [StringLength(ServiceConfigurationConsts.MaxDescriptionLength,
            MinimumLength = ServiceConfigurationConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual int? ServiceId { get; set; }

        [ForeignKey("ServiceId")] public Service ServiceFk { get; set; }

        public virtual int? ProviderId { get; set; }

        [ForeignKey("ProviderId")] public Provider ProviderFk { get; set; }

        public virtual int? CategoryId { get; set; }

        [ForeignKey("CategoryId")] public Category CategoryFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")] public User UserFk { get; set; }
    }
}