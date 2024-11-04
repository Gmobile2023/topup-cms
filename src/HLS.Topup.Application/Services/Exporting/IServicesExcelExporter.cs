using System.Collections.Generic;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Services.Exporting
{
    public interface IServicesExcelExporter
    {
        FileDto ExportToFile(List<GetServiceForViewDto> services);
    }
}