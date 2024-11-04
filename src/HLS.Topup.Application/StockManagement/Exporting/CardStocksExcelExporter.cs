using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.StockManagement.Exporting
{
    public class CardStocksExcelExporter : NpoiExcelExporterBase, ICardStocksExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CardStocksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CardStockDto> cardStocks)
        {
            string fileName = string.Format("Kho ma the_{0}.xlsx", System.DateTime.Now.ToString("ddMMyyyy"));
            return CreateExcelPackage(
                fileName,
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("CardStocks"));

                    AddHeader(
                        sheet,
                        L("StockCode"),
                        L("Service"),
                        L("Categories"),
                        //L("Product"),
                        L("CardValue"),
                        L("Inventory"),
                        L("Status"),
                        L("InventoryLimit"),
                        L("MinimumInventoryLimit")
                    );

                    AddObjects(
                        sheet, 2, cardStocks,
                        _ => _.StockCode,
                        _ => _.ServiceName,
                        _ => _.CategoryName,
                        //_ => _.ProductName,
                        _ => CellOption.Create(_.CardValue, "Number"),
                        _ => CellOption.Create(_.Inventory, "Number"),
                        _ => L("Enum_CardStockStatus_" + (byte)_.Status),
                        _ => CellOption.Create(_.InventoryLimit, "Number"),
                        _ => CellOption.Create(_.MinimumInventoryLimit, "Number")
                    );
                });
        }
    }
}