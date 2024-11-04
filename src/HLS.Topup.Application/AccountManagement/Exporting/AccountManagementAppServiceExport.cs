using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.AgentsManage.Dtos;
using HLS.Topup.Authorization.Users;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.Storage;


namespace HLS.Topup.AccountManagement.Exporting
{
    public interface IAccountManagementAppServiceExport
    {
        FileDto ExportToFile(List<UserProfileDto> list);
 
    }
    public class AccountManagementAppServiceExport : NpoiExcelExporterBase, IAccountManagementAppServiceExport
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AccountManagementAppServiceExport(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<UserProfileDto> list)
        {
            try
            {
                return CreateExcelPackage(
                    "Agent.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("Agent");
                        AddHeader(
                            sheet,
                            "Mã",
                            "Số điện thoại",
                            "Tên",
                            "Thời gian tạo",
                            "Trạng thái"
                            );

                        AddObjects(
                            sheet, 2, list,
                            _ => _.AccountCode,
                            _ => _.PhoneNumber,
                            _ => _.FullName, 
                            _ => CellOption.Create(_.CreationTime, "dd/MM/yyyy HH:mm:ss"), 
                             _ => (_.IsActive ? "Hoạt động" : "Khóa")
                            );
                       

                    });
            }
            catch (Exception ex)
            {
                return new FileDto();
            }
        }
        
    }
}
