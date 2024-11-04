using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.StockManagement.Exporting
{
    public class CardBatchsExcelExporter : NpoiExcelExporterBase, ICardBatchsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CardBatchsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CardBatchDto> cardBatchs)
        {
            return CreateExcelPackage(
                "CardBatchs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CardBatchs"));

                    AddHeader(
                        sheet,
                        
                        L("CreatedDate"),
                        L("BatchCode"), 
                        L("ProviderCode"),  
                        L("ProviderName"), 
                        L("TotalQuantity"),  
                        L("TotalAmount"),  
                        L("ImportType"),  
                        L("Status")
                        );

                    AddObjects(
                        sheet, 2, cardBatchs,
                        _ => CellOption.Create(_.CreatedDate, "dd/MM/yyyy HH:mm:ss"),
                        _ => _.BatchCode,
                        _ => _.ProviderCode,
                        _ => _.ProviderName,
                        _ => CellOption.Create(_.TotalQuantity, "Number"), 
                        _ => CellOption.Create(_.TotalAmount, "Number"), 
                        _ => _.ImportType, 
                        _ => L("Enum_CardStockStatus_"+(byte)_.Status)
                        ); 
                });
        }
    }
}
