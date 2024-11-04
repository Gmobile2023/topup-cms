using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Services.Exporting
{
    public class ServicesExcelExporter : NpoiExcelExporterBase, IServicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServiceForViewDto> services)
        {
            return CreateExcelPackage(
                "Services.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Services"));

                    AddHeader(
                        sheet,
                        L("ServiceCode"),
                        L("ServicesName"),
                        L("Status"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, 2, services,
                        _ => _.Service.ServiceCode,
                        _ => _.Service.ServicesName,
                        _ => _.Service.Status,
                        _ => _.Service.Order
                        );

					
					
                });
        }
    }
}
