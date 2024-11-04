using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Address.Exporting
{
    public class WardsExcelExporter : NpoiExcelExporterBase, IWardsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WardsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWardForViewDto> wards)
        {
            return CreateExcelPackage(
                "Wards.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Wards"));

                    AddHeader(
                        sheet,
                        L("WardCode"),
                        L("WardName"),
                        L("Status"),
                        (L("District")) + L("DistrictName")
                        );

                    AddObjects(
                        sheet, 2, wards,
                        _ => _.Ward.WardCode,
                        _ => _.Ward.WardName,
                        _ => _.Ward.Status,
                        _ => _.DistrictDistrictName
                        );

					
					
                });
        }
    }
}
