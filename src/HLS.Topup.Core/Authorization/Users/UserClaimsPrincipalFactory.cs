using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, IUnitOfWorkManager unitOfWorkManager)
            : base(
                userManager,
                roleManager,
                optionsAccessor, unitOfWorkManager)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);
            claim.Identities.First().AddClaim(new Claim("account_code", user.AccountCode ?? ""));
            claim.Identities.First().AddClaim(new Claim("email_address", user.EmailAddress));
            claim.Identities.First().AddClaim(new Claim("phone_number", user.PhoneNumber ?? ""));
            claim.Identities.First().AddClaim(new Claim("phone_number_otp", user.MobileOtp ?? ""));
            claim.Identities.First().AddClaim(new Claim("name", user.Name));
            claim.Identities.First().AddClaim(new Claim("sur_name", user.Surname));
            claim.Identities.First().AddClaim(new Claim("full_name", user.FullName));
            claim.Identities.First().AddClaim(new Claim("user_name", user.UserName));
            claim.Identities.First().AddClaim(new Claim("parent_Id", user.ParentId?.ToString() ?? ""));
            claim.Identities.First().AddClaim(new Claim("parent_code", user.ParentCode ?? ""));
            //claim.Identities.First().AddClaim(new Claim("user_staff", user.IsUserStaff.ToString()));
            claim.Identities.First().AddClaim(new Claim("account_type", user.AccountType.ToString("G") ?? ""));
            claim.Identities.First().AddClaim(new Claim("agent_type", user.AgentType.ToString("G") ?? ""));
            claim.Identities.First().AddClaim(new Claim("network_level", user.NetworkLevel.ToString() ?? ""));
            claim.Identities.First().AddClaim(new Claim("agent_name", user.AgentName ?? ""));
            claim.Identities.First().AddClaim(new Claim("is_active", user.IsActive.ToString() ?? ""));
            return claim;
        }
    }
}