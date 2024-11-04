using System.Collections.Generic;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.BalanceManager.Exporting
{
    public interface IAccountBlockBalancesExcelExporter
    {
        FileDto ExportToFile(List<GetAccountBlockBalanceForViewDto> accountBlockBalances);
    }
}