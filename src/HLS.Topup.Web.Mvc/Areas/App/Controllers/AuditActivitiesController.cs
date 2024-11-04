using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Auditing;
using HLS.Topup.Authorization;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AuditActivities)]
    public class AuditActivitiesController : TopupControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}