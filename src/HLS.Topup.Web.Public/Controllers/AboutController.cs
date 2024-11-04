using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Controllers;

namespace HLS.Topup.Web.Public.Controllers
{
    public class AboutController : TopupControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}