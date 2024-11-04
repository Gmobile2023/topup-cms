using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HLS.Topup.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Web.Views.Shared.Components.Footer
{
    public class FooterViewComponent : TopupViewComponent
    {
        private readonly IConfigurationRoot _appConfiguration;

        public FooterViewComponent(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var link = _appConfiguration["App:DownloadApp"];
            ViewBag.Android = link.Split("|")[1];
            ViewBag.Ios = link.Split("|")[0];
            return View("_Footer");
        }
    }
}