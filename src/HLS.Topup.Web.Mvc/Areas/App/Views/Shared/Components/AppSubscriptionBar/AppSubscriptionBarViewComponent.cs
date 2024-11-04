using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization;
using HLS.Topup.Configuration;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Session;
using HLS.Topup.Web.Views;

namespace HLS.Topup.Web.Areas.App.Views.Shared.Components.AppSubscriptionBar
{
    public class AppSubscriptionBarViewComponent : TopupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppSubscriptionBarViewComponent(
            IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                SubscriptionExpireNotifyDayCount = SettingManager.GetSettingValue<int>(AppSettings.TenantManagement.SubscriptionExpireNotifyDayCount)
            };

            return View(headerModel);
        }

    }
}
