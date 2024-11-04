using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Controllers;

namespace HLS.Topup.Web.Public.Controllers
{
    public class HomeController : TopupControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}