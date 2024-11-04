using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.Authorization
{
    [Table("AbpIdentittyServerStorage")]
    public class AbpIdentittyServerStorage : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [StringLength(50)]
        public virtual string ClientId { get; set; }

        [StringLength(50)]
        public virtual string ClientName { get; set; }

        [StringLength(500)]
        public virtual string AllowedGrantTypes { get; set; }

        [StringLength(500)]
        public virtual string AllowedScopes { get; set; }

        [StringLength(50)]
        public virtual string ClientSecrets { get; set; }

        [StringLength(500)]
        public virtual string RedirectUris { get; set; }
        [StringLength(500)]
        public virtual string PostLogoutRedirectUris { get; set; }
        public virtual bool AllowOfflineAccess { get; set; }
        public virtual bool RequireConsent { get; set; }
        public virtual bool IsActive { get; set; }

        [StringLength(50)]
        public virtual string AccountCode { get; set; }
    }
}
