using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using HLS.Topup.Authorization.Users;
using HLS.Topup.MultiTenancy;

namespace HLS.Topup.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}