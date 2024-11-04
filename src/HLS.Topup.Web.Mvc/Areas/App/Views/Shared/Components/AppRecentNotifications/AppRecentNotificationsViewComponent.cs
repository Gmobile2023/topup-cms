using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Layout;
using HLS.Topup.Web.Views;

namespace HLS.Topup.Web.Areas.App.Views.Shared.Components.AppRecentNotifications
{
    public class AppRecentNotificationsViewComponent : TopupViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            var model = new RecentNotificationsViewModel
            {
                CssClass = cssClass
            };
            
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
