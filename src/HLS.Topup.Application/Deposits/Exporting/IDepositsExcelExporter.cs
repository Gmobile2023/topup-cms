using System.Collections.Generic;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.Deposits.Exporting
{
    public interface IDepositsExcelExporter
    {
        FileDto ExportToFile(List<GetDepositForViewDto> deposits);
    }
}