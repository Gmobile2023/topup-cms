using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Storage;

namespace HLS.Topup.DiscountManager.Exporting
{
    public class DiscountsExcelExporter : NpoiExcelExporterBase, IDiscountsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountForViewDto> discounts)
        {
            return CreateExcelPackage(
                "Discounts.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Discounts"));

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("FromDate"),
                        L("ToDate"),
                        L("DateApproved"),
                        L("Status"),
                        L("AgentType"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, discounts,
                        _ => _.Discount.Code,
                        _ => _.Discount.Name,
                        _ => _timeZoneConverter.Convert(_.Discount.FromDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Discount.ToDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Discount.DateApproved, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Discount.Status,
                        _ => _.Discount.AgentType,
                        _ => _.UserName
                        );

					
					for (var i = 1; i <= discounts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(3);
                    for (var i = 1; i <= discounts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(4);
                    for (var i = 1; i <= discounts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    //sheet.AutoSizeColumn(5);
                });
        }

        public FileDto DiscountDetailsExportToFile(List<DiscountDetailDto> discounts, string fileName)
        {
            return CreateExcelPackage(
                fileName!= null ? fileName + ".xlsx" : "Chi tiet chinh sach chiet khau.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Discounts"));

                    AddHeader(
                        sheet,
                        "Danh mục sản phẩm",
                        "Sản phẩm",
                        "Mệnh giá",
                        "Chiết khấu (%)",
                        "Số tiền"
                    );

                    AddObjects(
                        sheet, 2, discounts,
                        _ => _.CategoryName,
                        _ => _.ProductName,
                        _ => CellOption.Create(_.ProductValue, "Number"),
                        _ => _.DiscountValue,
                        _ => CellOption.Create(_.FixAmount, "Number")
                    );
                });
        }
    }
}
