using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Products;
using HLS.Topup.Providers;
using HLS.Topup.Services;

namespace HLS.Topup.Common
{
    public interface ICommonManger
    {
        Task<string> GetIncrementCodeAsync(string pref);

        //Task<string> GetAccessTokenViaCredentialsAsync();
        Task<List<Service>> GetServices(bool isActive=true);
        Task<List<Category>> GetCategories(string serviceCode, string cateCode,bool isActive=true);
        Task<List<Category>> GetCategoriesMuti(List<string> serviceCode, string cateCode, bool isActive = true);
        Task<List<Category>> GetCategories(int serviceId, List<int> serviceIds, int cateId, bool isActive = true);
        Task<Category> GetCategory(string cateCode);

        Task<List<Product>> GetProducts(string productCode, string cateCode);
        Task<bool> CheckServiceActive(string serviceCode);
        Task<bool> CheckCategoryActive(string categoryCode);
        Task<bool> CheckStaffTime(long userId);
        Task<bool> CheckProductInCate(string productCode,string categoryCode);
        string EncryptQueryParameters(string link, string encrptedParameterName = "c");

        Task<List<VendorTransDto>> GetListVendorTrans(string search);
        Task<List<VendorTransDto>> GetListVendorTrans();
        Task<List<Provider>> GetProviderCache();
        Task<List<IdentityServerStorageDto>> GetClientIds();
        Task<IdentityServerStorageDto> GetClientId(string accountCode);
        Task<bool> CreateOrUpdateClientId(IdentityServerStorageInputDto input);

    }
}
