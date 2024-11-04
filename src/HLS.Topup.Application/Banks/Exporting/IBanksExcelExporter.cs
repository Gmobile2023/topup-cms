using System.Collections.Generic;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Banks.Exporting
{
    public interface IBanksExcelExporter
    {
        FileDto ExportToFile(List<GetBankForViewDto> banks);
    }
}