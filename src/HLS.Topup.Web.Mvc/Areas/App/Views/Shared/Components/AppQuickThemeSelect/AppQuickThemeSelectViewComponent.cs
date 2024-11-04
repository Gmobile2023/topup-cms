using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Views;

namespace HLS.Topup.Web.Areas.App.Views.Shared.Components.
    AppQuickThemeSelect
{
    public class AppQuickThemeSelectViewComponent : TopupViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            return Task.FromResult<IViewComponentResult>(View(new QuickThemeSelectionViewModel
            {
                CssClass = cssClass
            }));
        }
    }
}
