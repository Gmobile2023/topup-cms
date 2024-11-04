using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Session;
using HLS.Topup.Web.Views;

namespace HLS.Topup.Web.Areas.App.Views.Shared.Themes.Theme7.Components.AppTheme7Brand
{
    public class AppTheme7BrandViewComponent : TopupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme7BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
