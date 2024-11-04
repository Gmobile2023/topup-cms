using System;
using System.Linq;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using HLS.Topup.Common;
using Stripe;

namespace HLS.Topup.Authorization.Users
{
    public class TopupAppSession : ClaimsAbpSession, ITransientDependency
    {
        public TopupAppSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) : base(
            principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
        }

        public string UserEmail
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "email_address");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public string AccountCode
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "account_code");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        // public bool IsUserStaff
        // {
        //     get
        //     {
        //         var userAccounteClaim =
        //             PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "user_staff");
        //         return string.IsNullOrEmpty(userAccounteClaim?.Value) ? false : bool.Parse(userAccounteClaim.Value);
        //     }
        // }

        public string ParentCode
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "parent_code");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public long ParentId
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "parent_id");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? 0 : long.Parse(userAccounteClaim.Value);
            }
        }

        public string UserName
        {
            get
            {
                var userAccounteClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "user_name");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public CommonConst.SystemAccountType AccountType
        {
            get
            {
                try
                {
                    var userAccountTypeClaim =
                        PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "account_type");
                    if (string.IsNullOrEmpty(userAccountTypeClaim?.Value))
                        return CommonConst.SystemAccountType.Default;
                    Enum.TryParse(userAccountTypeClaim.Value, out CommonConst.SystemAccountType type);
                    return type;
                }
                catch
                {
                    return CommonConst.SystemAccountType.Default;
                }
            }
        }

        public CommonConst.AgentType AgentType
        {
            get
            {
                try
                {
                    var userAccountTypeClaim =
                        PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "agent_type");
                    if (string.IsNullOrEmpty(userAccountTypeClaim?.Value))
                        return CommonConst.AgentType.Default;
                    Enum.TryParse(userAccountTypeClaim.Value, out CommonConst.AgentType type);
                    return type;
                }
                catch
                {
                    return CommonConst.AgentType.Default;
                }
            }
        }

        public string PhoneNumber
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "phone_number");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public string PhoneNumberOtp
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "phone_number_otp");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public bool IsActive
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "is_active");
                var value = string.IsNullOrEmpty(userAccounteClaim?.Value) ? "False" : userAccounteClaim.Value;
                return value == "True";
            }
        }

        public string FullName
        {
            get
            {
                var userAccounteClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "full_name");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public string Name
        {
            get
            {
                var userAccounteClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "name");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public string SurName
        {
            get
            {
                var userAccounteClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "sur_name");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }


        public string CurrentNetworkLevel
        {
            get
            {
                var userAccounteClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "network_level");
                return string.IsNullOrEmpty(userAccounteClaim?.Value) ? null : userAccounteClaim.Value;
            }
        }

        public string AgentName
        {
            get
            {
                var userAccountClaim =
                    PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "agent_name");
                return string.IsNullOrEmpty(userAccountClaim?.Value) ? null : userAccountClaim.Value;
            }
        }
    }
}