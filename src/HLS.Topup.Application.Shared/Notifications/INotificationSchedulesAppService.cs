using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Notifications.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.Notifications
{
    public interface INotificationSchedulesAppService : IApplicationService
    {
        Task<PagedResultDto<GetNotificationScheduleForViewDto>> GetAll(GetAllNotificationSchedulesInput input);

        Task<GetNotificationScheduleForViewDto> GetNotificationScheduleForView(int id);

		Task<GetNotificationScheduleForEditOutput> GetNotificationScheduleForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditNotificationScheduleDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetNotificationSchedulesToExcel(GetAllNotificationSchedulesForExcelInput input);


		Task<List<NotificationScheduleUserLookupTableDto>> GetAllUserForTableDropdown();
		Task Approval(EntityDto input);
		Task Cancel(EntityDto input);
		Task Send(EntityDto input);

    }
}
