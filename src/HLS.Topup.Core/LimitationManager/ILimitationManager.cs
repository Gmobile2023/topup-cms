using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.LimitationManager.Dtos;

namespace HLS.Topup.LimitationManager
{
    public interface ILimitationManager
    {
        Task<bool> CheckLimitConfigProduct(string productcode, string accountCode, int quantity, decimal amount);
        Task<LimitProductDetailDto> GetLimitConfigProduct(string productcode, string accountCode);

        Task<List<LimitProductDetailDto>> GetLimitProductsDetails(int LimitProductId);
    }
}
