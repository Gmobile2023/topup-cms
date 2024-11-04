using System.Collections.Generic;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.BalanceManager.Exporting
{
    public interface IPayBatchBillsExcelExporter
    {
        FileDto ExportToFile(List<GetPayBatchBillForViewDto> payBatchBills);

        FileDto ExportDetailToFile(List<PayBatchBillItem> payBatchDetails);
    }
}