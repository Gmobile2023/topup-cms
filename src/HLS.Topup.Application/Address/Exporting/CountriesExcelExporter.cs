using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Address.Exporting
{
    public class CountriesExcelExporter : NpoiExcelExporterBase, ICountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountryForViewDto> countries)
        {
            return CreateExcelPackage(
                "Countries.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Countries"));

                    AddHeader(
                        sheet,
                        L("CountryCode"),
                        L("CountryName"),
                        L("Status")
                        );

                    AddObjects(
                        sheet, 2, countries,
                        _ => _.Country.CountryCode,
                        _ => _.Country.CountryName,
                        _ => _.Country.Status
                        );

					
					
                });
        }
    }
}
