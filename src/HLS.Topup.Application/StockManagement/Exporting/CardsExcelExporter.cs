using System;
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Storage;

namespace HLS.Topup.StockManagement.Exporting
{
    public class CardsExcelExporter : NpoiExcelExporterBase, ICardsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CardsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CardResponseDto> cards)
        {
            return CreateExcelPackage(
                "Cards.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Cards"));

                    AddHeader(
                        sheet,
                        L("Serial"),
                        L("CardValue"),
                        L("Service"),
                        L("Categories"),
                        L("Provider"),
                        L("BatchCode"),
                        L("ExpiredDate"),
                        L("Status"),
                        L("StockCode"),
                        L("ImportedDate"),
                        L("ExportedDate")
                    );

                    AddObjects(
                        sheet, 2, cards,
                        _ => _.Serial,
                        _ => _.CardValue,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        _ => _.ProviderName,
                        _ => _.BatchCode,
                        _ => _timeZoneConverter.Convert(_.ExpiredDate.ToLocalTime(), _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => L("Enum_CardStatus_" + (byte)_.Status),
                        _ => _.StockCode,
                        _ => _.ImportedDate != DateTime.MinValue ? _timeZoneConverter.Convert(_.ImportedDate.ToLocalTime(), _abpSession.TenantId, _abpSession.GetUserId())?.ToString("yyyy-MM-dd HH:mm:ss") : "",
                        _ => _.ExportedDate.HasValue && _.ExportedDate.Value != DateTime.MinValue ? _timeZoneConverter.Convert(_.ExportedDate.Value.ToLocalTime(), _abpSession.TenantId, _abpSession.GetUserId())?.ToString("yyyy-MM-dd HH:mm:ss") : ""
                    );
                    for (var i = 1; i <= cards.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    for (var i = 1; i <= cards.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd hh:mm:ss");
                    }
                    // for (var i = 1; i <= 11; i++)
                    // {
                    //     sheet.AutoSizeColumn(i);
                    // }
                });
        }

        public FileDto ExportListToFile(List<StockTransRequestDto> cards)
        {
            return CreateExcelPackage(
                "TransCardList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Cards"));

                    AddHeader(
                        sheet,
                       "Mã giao dịch",
                       "Mã lô",
                        "Trạng thái",
                       "Mệnh giá",
                        "Số lượng",
                       "Thành tiền",
                       "Dịch vụ",
                       "Ngày nhập",
                       "Nhà CC",
                       "Đồng bộ kho",
                       "Ngày hết hạn"
                    );

                    AddObjects(
                        sheet, 2, cards,
                        _ => _.TransCodeProvider,
                        _ => _.BatchCode,
                        _ => L("Enum_CardTransStatus_" + (byte)_.Status),
                        _ => _.ItemValue,
                        _ => _.Quantity,
                        _ => _.TotalPrice,
                        _ => _.ServiceCode,
                        _ => _timeZoneConverter.Convert(_.CreatedDate.ToLocalTime(),
                        _abpSession.TenantId, _abpSession.GetUserId())?.ToString("dd/MM/yyyy HH:mm:ss"),
                        _ => _.Provider,
                        _ => _.IsSyncCard ? "Đã đồng bộ" : "Chưa đồng bộ",
                         _ => _.ExpiredDate != null ? _timeZoneConverter.Convert(_.ExpiredDate.Value.ToLocalTime(),
                        _abpSession.TenantId, _abpSession.GetUserId())?.ToString("dd/MM/yyyy") : string.Empty
                        );
                });
        }
    }
}