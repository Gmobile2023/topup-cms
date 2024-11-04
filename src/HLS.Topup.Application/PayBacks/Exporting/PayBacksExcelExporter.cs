using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dto;
using HLS.Topup.PayBacks.Dtos;
using HLS.Topup.Storage;
using ServiceStack;

namespace HLS.Topup.PayBacks.Exporting
{
    public class PayBacksExcelExporter : NpoiExcelExporterBase, IPayBacksExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PayBacksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPayBackForViewDto> payBacks)
        {
            try
            {
                return CreateExcelPackage(
                    "Danh sach tra thuong khuyen mai.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet(L("PayBacks"));

                        AddHeader(
                            sheet,
                            L("PayBacks_Code"),
                            L("PayBacks_Name"),
                            L("PayBacks_Payment_Period"),
                            L("PayBacks_Agent_Num"),
                            L("PayBacks_Total_Amount"),
                            L("Status"),
                            L("PayBacks_Date_CreationTime"),
                            L("PayBacks_User_CreationTime"),
                            L("PayBacks_Date_Approved"),
                            L("PayBacks_User_Approved")
                        );

                        AddObjects(
                            sheet, 2, payBacks,
                            _ => _.PayBack.Code,
                            _ => _.PayBack.Name,
                            _ => CellOption.Create(_.PayBack.FromDate, "dd/MM/yyyy HH:mm:ss") + " - " +
                                 CellOption.Create(_.PayBack.ToDate, "dd/MM/yyyy HH:mm:ss"),
                            _ => _.TotalAgent,
                            _ => _.TotalAmount,
                            _ => L("Enum_PayBacksStatus_" + (int) _.PayBack.Status),
                            _ => CellOption.Create(_.PayBack.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                            _ => _.UserName,
                            _ => CellOption.Create(_.PayBack.DateApproved, "dd/MM/yyyy HH:mm:ss"),
                            _ => _.UserApproved
                        );
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public FileDto DetailPayBacksExportToFile(List<PayBacksDetailDto> payBacksDetail, string fileName)
        {
            return CreateExcelPackage(
                fileName != null ? fileName + ".xlsx" : "Chi tiet danh sach tra thuong khuyen mai.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("PayBacks"));

                    AddHeader(
                        sheet,
                        L("AgentCode"),
                        L("PhoneNumber"),
                        L("FullName"),
                        L("Amount"),
                        L("TransCode")
                    );

                    AddObjects(
                        sheet, 2, payBacksDetail,
                        _ => _.AgentCode,
                        _ => _.PhoneNumber,
                        _ => _.FullName,
                        _ => CellOption.Create(_.Amount, "Number"),
                        _ => _.TransCode
                    );
                });
        }
    }
}