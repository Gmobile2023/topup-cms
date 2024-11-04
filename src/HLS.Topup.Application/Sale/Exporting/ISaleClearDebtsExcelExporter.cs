using System.Collections.Generic;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Sale.Exporting
{
    public interface ISaleClearDebtsExcelExporter
    {
        FileDto ExportToFile(List<GetSaleClearDebtForViewDto> saleClearDebts);
    }
}