using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.StockManagement.Exporting
{
    public interface IBatchAirtimesExcelExporter
    {
        FileDto ExportToFile(List<BatchAirtimeDto> batchAirtimes);
    }
    
    public class BatchAirtimesExcelExporter : NpoiExcelExporterBase, IBatchAirtimesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BatchAirtimesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<BatchAirtimeDto> batchAirtimes)
        {
            return CreateExcelPackage(
                "BatchAirtimes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("BatchAirtimes"));
                    AddHeader(
                        sheet,
                        L("BatchCode"),
                        L("ProviderCode"), 
                        L("Amount")+ "(đ)",
                        L("Discounted") + "(%)",
                        L("Airtime"),
                        L("Status"),
                        L("CreatedDate")
                    );

                    AddObjects(
                        sheet, 2, batchAirtimes,
                        _ => _.BatchCode,
                        _ => (_.ProviderCode + " - " +   _.ProviderName), 
                        _ => CellOption.Create(_.Amount, "Number"),
                        _ => CellOption.Create(_.Discount, "Number"),
                        _ => CellOption.Create(_.Airtime, "Number"),
                        _ => L("Enum_BatchAirtimeStatus_"+(byte)_.Status),
                        _ => CellOption.Create(_.CreatedDate, "dd/MM/yyyy HH:mm:ss") 
                    );
                });
        }
    }
}