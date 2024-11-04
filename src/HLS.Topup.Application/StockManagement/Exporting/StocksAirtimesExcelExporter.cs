using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.StockManagement.Exporting
{
    public interface IStocksAirtimesExcelExporter
    {
        FileDto ExportToFile(List<StocksAirtimeDto> stocksAirtimes);
    }
    public class StocksAirtimesExcelExporter : NpoiExcelExporterBase, IStocksAirtimesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StocksAirtimesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<StocksAirtimeDto> data)
        {
            return CreateExcelPackage(
                "StockAirtime.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Airtime"));
                    AddHeader(
                        sheet,
                        L("ProviderCode"),
                        L("TotalAirtime"),
                        L("MinLimitAirtime"),
                        L("MaxLimitAirtime"),
                        L("Description")
                    );
                    AddObjects(
                        sheet, 2, data,
                        _ => _.ProviderCode + " - " + _.ProviderName,
                        _ => _.TotalAirtime,
                        _ => _.Status,
                        _ => _.MinLimitAirtime,
                        _ => _.MaxLimitAirtime,
                        _ => _.Description
                    );

                });
        }
    }
}
