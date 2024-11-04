using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Discounts;
using JetBrains.Annotations;

namespace HLS.Topup.DiscountManager
{
    public interface IDiscountManger
    {
        Task<bool> CreateOrUpdateDiscount(List<DiscountDetailDto> detail, User user);

        Task UpdateDiscountsStop();

        Task<List<DiscountDetailDto>> GetDiscountDetails(int? discountId = null,
            [CanBeNull] List<int?> cateIds = null, [CanBeNull] bool? isGetAll = false);

        Task<List<ProductDiscountDto>> GetProductDiscounts(string categoryCode, string accountCode,
            string productCode = null);

        // Task<ProductDiscountDto> GetDiscountAccount(string product, string accountCode,
        //     decimal amount = 0, int total = 1);

        Task<ProductDiscountDto> GeProductDiscountAccount(string product, string accountCode,
            decimal amount = 0, int total = 1);

        Task<List<ProductDiscountDto>> GetProductDiscountsByService(string serviceCode, string accountCode,
            string search = null);
    }
}