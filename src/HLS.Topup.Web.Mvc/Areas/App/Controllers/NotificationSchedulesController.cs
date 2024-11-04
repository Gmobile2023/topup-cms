using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.NotificationSchedules;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.Notifications;
using HLS.Topup.Notifications.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_NotificationSchedules)]
    public class NotificationSchedulesController : TopupControllerBase
    {
        private readonly INotificationSchedulesAppService _notificationSchedulesAppService;

        public NotificationSchedulesController(INotificationSchedulesAppService notificationSchedulesAppService)
        {
            _notificationSchedulesAppService = notificationSchedulesAppService;
        }

        public ActionResult Index()
        {
            var model = new NotificationSchedulesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_NotificationSchedules_Create,
            AppPermissions.Pages_NotificationSchedules_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetNotificationScheduleForEditOutput getNotificationScheduleForEditOutput;

            if (id.HasValue)
            {
                getNotificationScheduleForEditOutput =
                    await _notificationSchedulesAppService.GetNotificationScheduleForEdit(new EntityDto
                        {Id = (int) id});
            }
            else
            {
                getNotificationScheduleForEditOutput = new GetNotificationScheduleForEditOutput
                {
                    NotificationSchedule = new CreateOrEditNotificationScheduleDto()
                };
                getNotificationScheduleForEditOutput.NotificationSchedule.DateSchedule = DateTime.Now;
            }

            var viewModel = new CreateOrEditNotificationScheduleModalViewModel()
            {
                NotificationSchedule = getNotificationScheduleForEditOutput.NotificationSchedule,
                UserName = getNotificationScheduleForEditOutput.UserName,
                NotificationScheduleUserList = await _notificationSchedulesAppService.GetAllUserForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewNotificationScheduleModal(int id)
        {
            var getNotificationScheduleForViewDto =
                await _notificationSchedulesAppService.GetNotificationScheduleForView(id);

            var model = new NotificationScheduleViewModel()
            {
                NotificationSchedule = getNotificationScheduleForViewDto.NotificationSchedule,
                UserName = getNotificationScheduleForViewDto.UserName
            };

            return PartialView("_ViewNotificationScheduleModal", model);
        }
    }
}
