using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Configuration.Exporting
{
    public class ServiceConfigurationsExcelExporter : NpoiExcelExporterBase, IServiceConfigurationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ServiceConfigurationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetServiceConfigurationForViewDto> serviceConfigurations)
        {
            return CreateExcelPackage(
                "ServiceConfigurations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ServiceConfigurations"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsOpened"),
                        L("Priority"),
                        (L("Service")) + L("ServicesName"),
                        (L("Provider")) + L("Name"),
                        (L("Category")) + L("CategoryName"),
                        (L("Product")) + L("ProductName"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, serviceConfigurations,
                        _ => _.ServiceConfiguration.Name,
                        _ => _.ServiceConfiguration.IsOpened,
                        _ => _.ServiceConfiguration.Priority,
                        _ => _.ServiceServicesName,
                        _ => _.ProviderName,
                        _ => _.CategoryCategoryName,
                        _ => _.ProductProductName,
                        _ => _.UserName
                        );

					
					
                });
        }
    }
}
