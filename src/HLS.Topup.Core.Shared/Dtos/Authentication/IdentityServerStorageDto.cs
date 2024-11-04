using System.Collections.Generic;

namespace HLS.Topup.Dtos.Authentication
{
    public class IdentityServerStorageDto
    {
        public int? TenantId { get; set; }
        public virtual string ClientId { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string AllowedGrantTypes { get; set; }
        public virtual string AllowedScopes { get; set; }
        public virtual string ClientSecrets { get; set; }
        public virtual string RedirectUris { get; set; }
        public virtual string PostLogoutRedirectUris { get; set; }
        public virtual bool AllowOfflineAccess { get; set; }
        public virtual bool RequireConsent { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string AccountCode { get; set; }
    }

    public class IdentityServerStorageInputDto
    {
        public virtual string ClientId { get; set; }
        public virtual string ClientName { get; set; }
        public virtual List<string> AllowedGrantTypes { get; set; }
        public virtual List<string> AllowedScopes { get; set; }
        public virtual List<string> ClientSecrets { get; set; }
        public virtual string RedirectUris { get; set; }
        public virtual string PostLogoutRedirectUris { get; set; }
        public virtual bool AllowOfflineAccess { get; set; }
        public virtual bool RequireConsent { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string AccountCode { get; set; }
        public virtual string Email { get; set; }
    }
}
