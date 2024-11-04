using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Controllers
{
    public class IncludesController : TopupControllerBase
    {
        // GET
        public IActionResult MenuMobile()
        {
            return View();
        }
    }
}