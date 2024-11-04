using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Address.Exporting
{
    public class DistrictsExcelExporter : NpoiExcelExporterBase, IDistrictsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DistrictsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDistrictForViewDto> districts)
        {
            return CreateExcelPackage(
                "Districts.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Districts"));

                    AddHeader(
                        sheet,
                        L("DistrictCode"),
                        L("DistrictName"),
                        L("Status"),
                        (L("City")) + L("CityName")
                        );

                    AddObjects(
                        sheet, 2, districts,
                        _ => _.District.DistrictCode,
                        _ => _.District.DistrictName,
                        _ => _.District.Status,
                        _ => _.CityCityName
                        );

					
					
                });
        }
    }
}
