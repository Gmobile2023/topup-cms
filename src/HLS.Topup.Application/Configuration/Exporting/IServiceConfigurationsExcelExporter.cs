using System.Collections.Generic;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Configuration.Exporting
{
    public interface IServiceConfigurationsExcelExporter
    {
        FileDto ExportToFile(List<GetServiceConfigurationForViewDto> serviceConfigurations);
    }
}