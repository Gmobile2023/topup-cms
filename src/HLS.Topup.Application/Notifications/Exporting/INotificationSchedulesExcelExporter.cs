using System.Collections.Generic;
using HLS.Topup.Notifications.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Notifications.Exporting
{
    public interface INotificationSchedulesExcelExporter
    {
        FileDto ExportToFile(List<GetNotificationScheduleForViewDto> notificationSchedules);
    }
}