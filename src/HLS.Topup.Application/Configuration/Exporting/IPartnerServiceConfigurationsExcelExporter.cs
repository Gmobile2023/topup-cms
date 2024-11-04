using System.Collections.Generic;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Configuration.Exporting
{
    public interface IPartnerServiceConfigurationsExcelExporter
    {
        FileDto ExportToFile(List<GetPartnerServiceConfigurationForViewDto> serviceConfigurations);
    }
}