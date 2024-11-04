using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Notifications.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Notifications.Exporting
{
    public class NotificationSchedulesExcelExporter : NpoiExcelExporterBase, INotificationSchedulesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NotificationSchedulesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNotificationScheduleForViewDto> notificationSchedules)
        {
            return CreateExcelPackage(
                "NotificationSchedules.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("NotificationSchedules"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        "Tên thông báo",
                        L("Title"),
                        L("DateSchedule"),
                        L("DateSend"),
                        L("Status"),
                        // L("AccountType"),
                        L("AgentType"),
                        L("DateApproved"),
                        "Đại lý áp dụng"
                        );

                    AddObjects(
                        sheet, 2, notificationSchedules,
                        _ => _.NotificationSchedule.Code,
                        _ => _.NotificationSchedule.Name,
                        _ => _.NotificationSchedule.Title,
                        _ => _timeZoneConverter.Convert(_.NotificationSchedule.DateSchedule, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.NotificationSchedule.DateSend, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => L("Enum_SendNotificationStatus_" + (int)_.NotificationSchedule.Status),
                        // _ => _.NotificationSchedule.AccountType,
                        _ => L("Enum_AgentType_" + (int)_.NotificationSchedule.AgentType),
                        _ => _timeZoneConverter.Convert(_.NotificationSchedule.DateApproved, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName
                        );


					for (var i = 1; i <= notificationSchedules.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(4);
                    for (var i = 1; i <= notificationSchedules.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(5);
                    for (var i = 1; i <= notificationSchedules.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(8);
                });
        }
    }
}
