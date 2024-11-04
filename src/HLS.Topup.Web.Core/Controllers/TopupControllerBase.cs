using System;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Configuration.Startup;
using Abp.IdentityFramework;
using HLS.Topup.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HLS.Topup.Web.Controllers
{
    [ServiceFilter(typeof(CheckUserAccessAreaFilter))]
    public abstract class TopupControllerBase : AbpController
    {
        protected TopupControllerBase()
        {
            LocalizationSourceName = TopupConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected void SetTenantIdCookie(int? tenantId)
        {
            var multiTenancyConfig = HttpContext.RequestServices.GetRequiredService<IMultiTenancyConfig>();
            Response.Cookies.Append(
                multiTenancyConfig.TenantIdResolveKey,
                tenantId?.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(5),
                    Path = "/"
                }
            );
        }
    }
}
