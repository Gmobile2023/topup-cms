using HLS.Topup.BalanceManager;
using HLS.Topup.Dtos.Stock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HLS.Topup.Paybacks
{
    public interface IPayBatchManageReponse
    {
        Task<ApiResponseDto<List<PayBatchBillItem>>> PayBatchBillGetRequest(
          PayBatchBillRequest request);
    }
}
