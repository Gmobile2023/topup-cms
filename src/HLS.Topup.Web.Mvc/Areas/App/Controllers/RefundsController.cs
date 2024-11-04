using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Controllers;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RefundsReconcile)]
    public class RefundsController : TopupControllerBase
    {
        // public ActionResult Index()
        // {
        //     return View();
        // }
    }
}
