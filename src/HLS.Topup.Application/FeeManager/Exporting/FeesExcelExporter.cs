using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.BillFees;
using HLS.Topup.Storage;
using JetBrains.Annotations;

namespace HLS.Topup.FeeManager.Exporting
{
    public class FeesExcelExporter : NpoiExcelExporterBase, IFeesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FeesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFeeForViewDto> fees)
        {
            return CreateExcelPackage(
                "Fees.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Fees"));

                    AddHeader(
                        sheet,
                        L("Fees_PolicyCode"),
                        L("Fees_AgentType"),
                        L("Fees_ApplyForAgent"),
                        L("Status"),
                        L("Fees_ApprovedDate"),
                        L("Fees_TimeCreated"),
                        L("Fees_FromTimeApplied"),
                        L("Fees_ToTimeApplied"),
                        L("Fees_ApprovedUser")
                    );

                    AddObjects(
                        sheet, 2, fees,
                        _ => _.Fee.Code,
                        _ => L("Enum_AgentType_" + (int) _.Fee.AgentType),
                        _ => _.Fee.AgentName,
                        _ => L("Fees_FeeStatus_" + (int) _.Fee.Status),
                        _ => _timeZoneConverter.Convert(_.Fee.DateApproved, _abpSession.TenantId,
                            _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Fee.CreationTime, _abpSession.TenantId,
                            _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Fee.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Fee.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName
                    );


                    for (var i = 1; i <= fees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }

                    //sheet.AutoSizeColumn(3);
                    for (var i = 1; i <= fees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }

                    //sheet.AutoSizeColumn(4);
                    for (var i = 1; i <= fees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }

                    //sheet.AutoSizeColumn(5);
                });
        }

        public FileDto DetailFeesExportToFile(List<BillFeeDetailDto> feesDetail, string fileName)
        {
            return CreateExcelPackage(
                !string.IsNullOrEmpty(fileName) ? fileName + ".xlsx" : "Fees-Detail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Fees"));

                    AddHeader(
                        sheet,
                        L("Fees_ProductCategory"),
                        L("Fees_Product"),
                        L("Fees_Minimum"),
                        L("Fees_MinimumForApply"),
                        L("Fees_BlockForApply"),
                        L("Fees_Additional")
                    );

                    AddObjects(
                        sheet, 2, feesDetail,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.MinFee, "Number"),
                        _ => CellOption.Create(_.AmountMinFee, "Number"),
                        _ => CellOption.Create(_.AmountIncrease, "Number"),
                         _ => CellOption.Create(_.SubFee, "Number")
                    );

                    //sheet.AutoSizeColumn(5);
                });
        }
    }
}