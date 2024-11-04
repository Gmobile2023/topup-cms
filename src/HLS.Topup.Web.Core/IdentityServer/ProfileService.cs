using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Utils;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using ServiceStack;

namespace HLS.Topup.Web.IdentityServer;

public class ProfileService : IProfileService
{
    public UserManager UserManager { get; set; }


    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        var user = await UserManager.FindByIdAsync(subjectId);
        if (user == null)
            throw new ArgumentException("Invalid subject identifier");

        var claims = await GetClaimsFromUser(user);
        context.IssuedClaims = claims.ToList();
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        var subjectId = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var user = await UserManager.FindByIdAsync(subjectId);

        context.IsActive = false;

        if (user != null)
        {
            if (!user.IsActive)
                context.IsActive = false;
            else
            {
                if (UserManager.SupportsUserSecurityStamp)
                {
                    var securityStamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value)
                        .SingleOrDefault();
                    if (securityStamp != null)
                    {
                        var dbSecurityStamp = await UserManager.GetSecurityStampAsync(user);
                        if (dbSecurityStamp != securityStamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.IsLockoutEnabled || user.LockoutEndDateUtc == null ||
                    user.LockoutEndDateUtc <= DateTime.UtcNow;
            }
        }
    }

    private async Task<IEnumerable<Claim>> GetClaimsFromUser(User user)
    {
        var role = await UserManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id.ToString()),
            new(JwtClaimTypes.PreferredUserName, user.UserName),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtClaimTypes.Role, role.ToJson()),
            new Claim("is_active", user.IsActive.ToString()),
            new Claim("is_verify_account", user.IsVerifyAccount.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.Surname))
            claims.Add(new Claim("sur_name", user.Surname));

        if (!string.IsNullOrWhiteSpace(user.Name))
            claims.Add(new Claim("name", user.Name));

        if (!string.IsNullOrWhiteSpace(user.UserName))
            claims.Add(new Claim("user_name", user.UserName));

        if (!string.IsNullOrWhiteSpace(user.AccountCode))
            claims.Add(new Claim("account_code", user.AccountCode));

        if (!string.IsNullOrWhiteSpace(user.EmailAddress))
            claims.Add(new Claim("email_address", user.EmailAddress));

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new Claim("phone_number", user.PhoneNumber));

        if (!string.IsNullOrWhiteSpace(user.MobileOtp))
            claims.Add(new Claim("phone_number_otp", user.MobileOtp));

        if (!string.IsNullOrWhiteSpace(user.Surname))
            claims.Add(new Claim("sur_name", user.Surname));

        if (!string.IsNullOrWhiteSpace(user.FullName))
            claims.Add(new Claim("full_name", user.FullName));

        if (!string.IsNullOrWhiteSpace(user.ParentId?.ToString()))
            claims.Add(new Claim("parent_Id", user.ParentId?.ToString() ?? ""));

        if (!string.IsNullOrWhiteSpace(user.ParentCode))
            claims.Add(new Claim("parent_code", user.ParentCode));

        // if (!string.IsNullOrWhiteSpace(user.Presenter))
        //     claims.Add(new Claim("presenter", user.Presenter));

        if (!string.IsNullOrWhiteSpace(user.AccountType.ToString("G")))
            claims.Add(new Claim("account_type", user.AccountType.ToString("G")));

        if (!string.IsNullOrWhiteSpace(user.NetworkLevel.ToString()))
            claims.Add(new Claim("network_level", user.NetworkLevel.ToString()));

        claims.Add(new Claim("is_active", user.IsActive.ToString()));
        claims.Add(new Claim("created_date",
            DateTimeHelper.ConvertToTimestamp(user.CreationTime.ToUniversalTime()).ToString()));

        if (UserManager.SupportsUserEmail)
            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.Email, user.EmailAddress),
                new Claim(JwtClaimTypes.EmailVerified, user.IsEmailConfirmed ? "true" : "false",
                    ClaimValueTypes.Boolean)
            });

        if (UserManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.AddRange(new[]
            {
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                new Claim(JwtClaimTypes.PhoneNumberVerified, user.IsPhoneNumberConfirmed ? "true" : "false",
                    ClaimValueTypes.Boolean)
            });

        return claims;
    }
}