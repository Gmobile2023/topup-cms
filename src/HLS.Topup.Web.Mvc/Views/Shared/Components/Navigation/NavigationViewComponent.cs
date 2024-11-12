using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Views.Shared.Components.Navigation
{
    public class NavigationViewComponent : TopupViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Add any additional data you need for the navigation
            var navigationModel = new NavigationViewModel
            {
                CurrentController = RouteData.Values["controller"]?.ToString(),
                CurrentAction = RouteData.Values["action"]?.ToString()
            };

            return View(navigationModel);
        }
    }
    
    public class NavigationViewModel
    {
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
    }
}