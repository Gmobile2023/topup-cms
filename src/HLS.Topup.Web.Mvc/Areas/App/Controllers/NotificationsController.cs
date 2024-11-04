using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Notifications;
using HLS.Topup.Web.Controllers;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class NotificationsController : TopupControllerBase
    {
        private readonly INotificationAppService _notificationApp;
        private readonly TopupAppSession _topupAppSession;

        public NotificationsController(INotificationAppService notificationApp, TopupAppSession topupAppSession)
        {
            _notificationApp = notificationApp;
            _topupAppSession = topupAppSession;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> SettingsModal()
        {
            var notificationSettings = await _notificationApp.GetNotificationSettings();
            var isFrontend = _topupAppSession.AccountType != CommonConst.SystemAccountType.System;
            ViewBag.IsFrontEnd = isFrontend;
            return PartialView("_SettingsModal", notificationSettings);
        }
    }
}
