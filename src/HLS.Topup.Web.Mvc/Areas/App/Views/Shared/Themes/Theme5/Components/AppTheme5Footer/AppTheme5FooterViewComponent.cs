using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Session;
using HLS.Topup.Web.Views;

namespace HLS.Topup.Web.Areas.App.Views.Shared.Themes.Theme5.Components.AppTheme5Footer
{
    public class AppTheme5FooterViewComponent : TopupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme5FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
