using System.Collections.Generic;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Address.Exporting
{
    public interface IDistrictsExcelExporter
    {
        FileDto ExportToFile(List<GetDistrictForViewDto> districts);
    }
}