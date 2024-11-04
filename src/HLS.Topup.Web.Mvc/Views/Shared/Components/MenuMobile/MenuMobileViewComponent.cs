using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Configuration;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Url;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Session;
using HLS.Topup.Web.Startup;
using HLS.Topup.Web.Views.Shared.Components.Header;

namespace HLS.Topup.Web.Views.Shared.Components.MenuMobile
{
    public class MenuMobileViewComponent : TopupViewComponent
        {
            private readonly IUserNavigationManager _userNavigationManager;
            private readonly IMultiTenancyConfig _multiTenancyConfig;
            private readonly IAbpSession _abpSession;
            private readonly ILanguageManager _languageManager;
            private readonly ISettingManager _settingManager;
            private readonly IPerRequestSessionCache _sessionCache;
            private readonly IWebUrlService _webUrlService;
            private readonly TenantManager _tenantManager;
    
            public MenuMobileViewComponent(
                IUserNavigationManager userNavigationManager,
                IMultiTenancyConfig multiTenancyConfig,
                IAbpSession abpSession,
                ILanguageManager languageManager,
                ISettingManager settingManager,
                IPerRequestSessionCache sessionCache,
                IWebUrlService webUrlService,
                TenantManager tenantManager)
            {
                _userNavigationManager = userNavigationManager;
                _multiTenancyConfig = multiTenancyConfig;
                _abpSession = abpSession;
                _languageManager = languageManager;
                _settingManager = settingManager;
                _sessionCache = sessionCache;
                _webUrlService = webUrlService;
                _tenantManager = tenantManager;
            }
    
            public async Task<IViewComponentResult> InvokeAsync(string currentPageName = "")
            {
                var model = new MenuViewModel
                {
                    Menu = await _userNavigationManager.GetMenuAsync(LeftMenuNavigationProvider.MenuName, _abpSession.ToUserIdentifier()),
                    CurrentPageName = currentPageName
                };
                return View("Default", model);
            }
        }
}
