using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Address.Exporting
{
    public class CitiesExcelExporter : NpoiExcelExporterBase, ICitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCityForViewDto> cities)
        {
            return CreateExcelPackage(
                "Cities.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Cities"));

                    AddHeader(
                        sheet,
                        L("CityCode"),
                        L("CityName"),
                        L("Status"),
                        (L("Country")) + L("CountryName")
                        );

                    AddObjects(
                        sheet, 2, cities,
                        _ => _.City.CityCode,
                        _ => _.City.CityName,
                        _ => _.City.Status,
                        _ => _.CountryCountryName
                        );

					
					
                });
        }
    }
}
