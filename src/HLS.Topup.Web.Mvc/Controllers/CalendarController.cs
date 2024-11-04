using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Controllers
{
    public class CalendarController : TopupControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}