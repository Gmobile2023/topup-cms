using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using HLS.Topup.DataExporting.Excel.NPOI;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using HLS.Topup.Dto;
using HLS.Topup.Storage;

namespace HLS.Topup.Configuration.Exporting
{
    public class PartnerServiceConfigurationsExcelExporter : NpoiExcelExporterBase, IPartnerServiceConfigurationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PartnerServiceConfigurationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPartnerServiceConfigurationForViewDto> serviceConfigurations)
        {
            return CreateExcelPackage(
                "ServiceConfigurations.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ServiceConfigurations"));

                    AddHeader(
                        sheet,
                        L("ConfigurationName"),
                        L("AgentType"),
                        L("AgentName"),
                        L("ServiceName"),
                        L("CategoryName"),
                        L("ProviderName"),
                        L("Status"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, serviceConfigurations,
                        _ => _.ServiceConfiguration.Name,
                        _ => _.AgentType,
                        _ => _.UserName,
                        _ => _.ServiceServicesName,
                        _ => _.CategoryCategoryName,
                        _ => _.ProviderName,
                        _ => L("Enum_PartnerServiceConfigurationStatus_" + _.ServiceConfiguration.Status),
                        _ => _.ServiceConfiguration.Description
                        );
                });
        }
    }
}
