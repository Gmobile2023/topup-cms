using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.LimitationManager.Exporting
{
    public class LimitProductsExcelExporter : NpoiExcelExporterBase, ILimitProductsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LimitProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLimitProductForViewDto> limitProducts, string fileName)
        {
            return CreateExcelPackage(
                fileName != null ? fileName + ".xlsx" : "LimitProducts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("LimitProducts"));

                    AddHeader(
                        sheet,
                        L("LimitProducts_Code"),
                        L("LimitProducts_Name"),
                        L("LimitProducts_AgentType"),
                        L("LimitProducts_Agent"),
                        L("Status"),
                        L("Created"),
                        L("LimitProducts_CreationTime"),
                        L("Updated"),
                        L("LimitProducts_ApprovedTime"),
                        L("LimitProducts_AppliedFromTime"),
                        L("LimitProducts_AppliedToTime")
                    );

                    AddObjects(
                        sheet, 2, limitProducts,
                        _ => _.LimitProduct.Code,
                        _ => _.LimitProduct.Name,
                        _ => L("Enum_AgentType_" + (int) _.LimitProduct.AgentType),
                        _ => _.AgentName,
                        _ => L("Enum_LimitProductConfigStatus_" + (int) _.LimitProduct.Status),
                        _ => _.UserName,
                        _ => CellOption.Create(_.LimitProduct.CreationTime, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.UserApproved,
                        _ => _.UserApproved != null ? CellOption.Create(_.LimitProduct.DateApproved, "dd/MM/yyyy HH:mm:ss") : null,
                        _ => CellOption.Create(_.LimitProduct.FromDate, "dd/MM/yyyy"),
                        _ => CellOption.Create(_.LimitProduct.ToDate, "dd/MM/yyyy"),
                        _ => _.UserName
                    );

                    //sheet.AutoSizeColumn(5);
                });
        }
        
        public FileDto DetailLimitExportToFile(List<LimitProductDetailDto> limitProductsDetail, string fileName)
        {
            return CreateExcelPackage(
                fileName != null ? fileName + ".xlsx" : "Chi tiet han muc ban hang.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("PayBacks"));

                    AddHeader(
                        sheet,
                        L("LimitProducts_Service"),
                        L("LimitProducts_ProductType"),
                        L("LimitProducts_Product"),
                        L("LimitProducts_LimitQty"),
                        L("LimitProducts_LimitPayment")
                    );

                    AddObjects(
                        sheet, 2, limitProductsDetail,
                        _ => _.ServiceName,
                        _ => _.ProductType,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.LimitQuantity, "Number"),
                        _ => CellOption.Create(_.LimitAmount, "Number")
                    );
                });
        }
    }
}