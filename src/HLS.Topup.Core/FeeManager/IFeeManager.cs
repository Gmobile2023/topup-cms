using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.BillFees;
using HLS.Topup.Dtos.Fees;

namespace HLS.Topup.FeeManager
{
    public interface IFeeManager
    {
        Task<List<BillFeeDetailDto>> GetFeeDetails(int? feeId = null, List<int?> cateIds = null, List<int?> prdIds = null);
        Task<ProductFeeDto> GetProductFee(string productcode, string accountCode, decimal amount);
        // Task<ProductFeeDto> GetFeeConfigAccount(string productcode, string accountCode);
    }
}
