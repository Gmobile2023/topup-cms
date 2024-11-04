using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common.Dto;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Dtos.Policy;
using HLS.Topup.Editions.Dto;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Services.Dtos;
using HLS.Topup.StockManagement.Dtos;
using JetBrains.Annotations;

namespace HLS.Topup.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();


        Task<UserInfoDto> GetUserInfoQuery(GetUserInfoRequest input);
        Task<UserInfoDto> GetUserInfo();
        Task<List<UserInfoDto>> GetListUserSearch(GetUserQueryRequest input);

        Task<UserInfoDto> GetUserSaleLeaderBySale(int userId);
        Task<List<UserInfoDto>> GetListUserSaleSearch(GetUserQueryRequest input);
        Task<List<CategoryDto>> GetCategories(CategorySearchInput input);

        Task<List<CategoryDto>> GetCategoriesMuti(CategorySearchInput input);
        Task<List<CategoryDto>> GetCategoriesTwoBy(CategorySearchTwoInput input);

        Task<List<ServiceDto>> GetSetvices(bool isActive = true);

        Task<List<BankDto>> GetBanks(bool isActive = true);

        List<decimal> CardValues();
        Task<List<DiscountDetailDto>> GetDiscountDetails(GetDiscountDetailTableInput input);
        Task<List<ProductDiscountDto>> GetProductDiscounts(GetProductDiscountInput input);
        Task<ProductDiscountDto> GetProductDiscount(GetProductDiscountUserDto input);
        Task<List<ProductInfoDto>> GetProducts(ProductSearchInput input);

        Task<ProductInfoDto> GetProductByCode(string productCode);

        Task<List<ProductDto>> GetProductByCategoryMuti(List<string> categoryCode, bool isActive = true);
        Task<List<CategoryDto>> GetCategoryUseCard(bool isActive = true);

        Task<PagedResultDto<ProductDiscountDto>> GetProductDiscountAccount(
            GetProductDiscountAccountTableInput input);

        Task<PagedResultDto<PolicyAccountDto>> GetPolicyAccount(
            GetProductDiscountAccountTableInput input);

        Task<List<CityDto>> GetProvinces();

        Task<CityDto> GetCityById(int cityId);
        Task<List<DistrictDto>> GetDistricts(int? provinceId, bool showAll = false);

        Task<DistrictDto> GetDistrictsById(int districtid);
        Task<List<WardDto>> GetWards(int? districtId, bool showAll = false);

        Task<WardDto> GetWardsById(int wardId);

        Task<List<ProductDto>> GetProductByCategory(string categoryCode, bool isActive = true, bool isShowOnFrontend = true);

        Task<List<ProductDto>> GetProductTwoByCategory(List<int> categoryIds, bool isActive = true);

        Task<List<ProviderDto>> GetProvider(bool isActive = true);
        Task<List<UserStaffDto>> GetStaffUserQuery(string search);
        Task<List<VendorTransDto>> GetListVendorTrans(string search);

        Task<List<UserInfoDto>> GetAgentSaleLocationQuery(long saleId, string search);

        Task<List<UserInfoDto>> GetUserSaleLeader(GetUserQueryRequest input);

        Task<List<UserInfoDto>> GetUserSaleBySaleLeader(GetUserQueryRequest input);

        Task<List<UserInfoDto>> GetUserAgentLevel(GetUserQueryRequest input);

        Task<List<CommonLookupTableDto>> ServiceCardList();
        Task<List<UserInfoDto>> GetUserAgentStaff(GetUserQueryRequest input);
        Task<ProductDto> GetProductByProductCode(string productCode, bool isActive = true);
    }
}
