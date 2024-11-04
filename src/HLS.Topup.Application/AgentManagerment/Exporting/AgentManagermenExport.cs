using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.AgentsManage.Dtos;
using HLS.Topup.Common;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.Storage;


namespace HLS.Topup.AgentManagerment.Exporting
{
    public interface IAgentManagermenExport
    {
        FileDto ExportToFile(List<AgentsDto> list);

        FileDto ExportAgentSupplerToFile(List<AgentsSupperDto> list);
    }
    public class AgentManagermenExport : NpoiExcelExporterBase, IAgentManagermenExport
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AgentManagermenExport(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<AgentsDto> list)
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
                            "Mã đại lý",
                            "Số điện thoại",
                            "Họ và tên",
                            "Loại đại lý",
                            "Đại lý tổng",
                            "Nhân viên quản lý",
                            "Trưởng nhóm kinh doanh",
                            "Thời gian tạo",
                            "Trạng thái nhân viên kinh doanh",
                            "Trạng thái",
                            "Địa chỉ",
                            "Số giấy tờ"
                            );

                        AddObjects(
                            sheet, 2, list,
                            _ => _.AccountCode,
                            _ => _.PhoneNumber,
                            _ => _.FullName,
                            _ => L("Enum_AgentType_" + (int)_.AgentType),
                            _ => _.AgentType == CommonConst.AgentType.SubAgent ? _.AgentGeneral : "",
                             _ => _.ManagerName,
                             _ => _.SaleLeadName,
                            _ => CellOption.Create(_.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                            _ => (_.IsMapSale ? "Đã gán" : "Chưa gán"),
                             _ => (_.Status == 0 ? "Chưa xác thực" : _.Status == 1 ? "Hoạt động" : "Khóa"),
                             _ => _.Address,
                             _ => _.Exhibit
                            );

                    });
            }
            catch (Exception ex)
            {
                return new FileDto();
            }
        }

        public FileDto ExportAgentSupplerToFile(List<AgentsSupperDto> list)
        {
            try
            {
                return CreateExcelPackage(
                    "Tổng đại lý.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("Sheet1");
                        AddHeader(
                            sheet,
                            "Mã đại lý",
                            "Tên đại lý",
                            "Kỳ đối soát",
                            "Trạng thái",
                            "Thời gian tạo"
                            );

                        AddObjects(
                            sheet, 2, list,
                            _ => _.AccountCode,
                            _ => _.FullName,
                            _ => _.CrossCheckPeriod,
                            _ => _.StatusName,
                            _ => CellOption.Create(_.CreatedDate, "dd/MM/yyyy HH:mm:ss")
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
