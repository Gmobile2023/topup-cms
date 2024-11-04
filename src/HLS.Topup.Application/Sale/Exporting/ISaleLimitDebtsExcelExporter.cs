using System.Collections.Generic;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Sale.Exporting
{
    public interface ISaleLimitDebtsExcelExporter
    {
        FileDto ExportToFile(List<GetSaleLimitDebtForViewDto> saleLimitDebts);
    }
}