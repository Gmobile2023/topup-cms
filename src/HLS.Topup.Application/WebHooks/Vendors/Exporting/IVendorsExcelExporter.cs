using System.Collections.Generic;
using HLS.Topup.Vendors.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Vendors.Exporting
{
    public interface IVendorsExcelExporter
    {
        FileDto ExportToFile(List<GetVendorForViewDto> vendors);
    }
}