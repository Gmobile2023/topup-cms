using System.Collections.Generic;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.BillFees;

namespace HLS.Topup.FeeManager.Exporting
{
    public interface IFeesExcelExporter
    {
        FileDto ExportToFile(List<GetFeeForViewDto> fees);
        FileDto DetailFeesExportToFile(List<BillFeeDetailDto> fees, string fileName);
    }
}