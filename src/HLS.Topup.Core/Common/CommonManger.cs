using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using HLS.Topup.Authorization;
using HLS.Topup.Categories;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Products;
using HLS.Topup.Services;
using HLS.Topup.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Providers;
using ServiceStack;
using ServiceStack.Redis;

namespace HLS.Topup.Common
{
    public class CommonManger : TopupDomainServiceBase, ICommonManger
    {
        private readonly IRepository<Category> _categoryrepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Service> _serivceRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<AbpIdentittyServerStorage> _idenRepository;
        private readonly IRepository<Provider> _provider;
        private readonly IStaffConfigurationManager _staffConfigurationManager;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<CommonManger> _logger;
        private readonly IRedisClientsManagerAsync _redisClientsManager;

        public CommonManger(IRepository<Service> serivceRepository,
            IRepository<Category> categoryrepository, IStaffConfigurationManager staffConfigurationManager,
            ILogger<CommonManger> logger, IRepository<Vendor> vendorRepository, IRepository<Product> productRepository,
            IRepository<Provider> provider, ICacheManager cacheManager,
            IRepository<AbpIdentittyServerStorage> idenRepository, IRedisClientsManagerAsync redisClientsManager)
        {
            _serivceRepository = serivceRepository;
            _categoryrepository = categoryrepository;
            _staffConfigurationManager = staffConfigurationManager;
            _logger = logger;
            _vendorRepository = vendorRepository;
            _productRepository = productRepository;
            _provider = provider;
            _cacheManager = cacheManager;
            _idenRepository = idenRepository;
            _redisClientsManager = redisClientsManager;
        }

        public async Task<string> GetIncrementCodeAsync(string pref)
        {
            try
            {
                var dateParam = DateTime.Now.ToString("yyMMdd");
                await using var client = await _redisClientsManager.GetClientAsync();
                var key = $"TopupWeb_TransCode:Items:{pref + dateParam}";
                var id = await client.IncrementValueAsync(key);
                if (id == 1) //Bắt đầu ngày mới
                {
                    var oldkey = $"TopupWeb_TransCode:Items:{pref + DateTime.Now.AddDays(-1).ToString("yyMMdd")}";
                    await client.RemoveAsync(oldkey);
                }

                return await Task.FromResult(pref + dateParam + id.ToString().PadLeft(8, '0'));
            }
            catch (Exception ex)
            {
                var rand = new Random();
                var date = DateTime.Now.ToString("yy");
                return await Task.FromResult(pref + date + rand.Next(000000000, 999999999));
            }
        }


        public Task<List<Service>> GetServices(bool isActive = true)
        {
            return _serivceRepository.GetAll().WhereIf(isActive, x => x.Status == ServiceStatus.Active)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.ServicesName)
                .ToListAsync();
        }

        public Task<List<Category>> GetCategories(string serviceCode, string cateCode, bool isActive = true)
        {
            var query = _categoryrepository.GetAllIncluding(x => x.ServiceFk)
                .WhereIf(isActive, x => x.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(!string.IsNullOrEmpty(serviceCode), x => x.ServiceFk.ServiceCode == serviceCode)
                .WhereIf(!string.IsNullOrEmpty(cateCode), x => x.CategoryCode.Contains(cateCode))
                .OrderBy(x => x.Order).ThenBy(x => x.CategoryCode)
                .ThenBy(x => x.CategoryName);
            return query.ToListAsync();
        }

        public Task<List<Category>> GetCategoriesMuti(List<string> serviceCode, string cateCode, bool isActive = true)
        {
            var query = _categoryrepository.GetAllIncluding(x => x.ServiceFk)
                .WhereIf(isActive, x => x.Status == CommonConst.CategoryStatus.Active)
                .Where(x => serviceCode.Contains(x.ServiceFk.ServiceCode))
                .WhereIf(!string.IsNullOrEmpty(cateCode), x => x.CategoryCode.Contains(cateCode))
                .OrderBy(x => x.Order).ThenBy(x => x.CategoryCode)
                .ThenBy(x => x.CategoryName);
            return query.ToListAsync();
        }

        public Task<List<Category>> GetCategories(int serviceId, List<int> serviceIds, int cateId, bool isActive = true)
        {
            var query = _categoryrepository.GetAllIncluding(x => x.ServiceFk)
                .WhereIf(isActive, x => x.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(serviceIds != null && serviceIds.Count > 0, x => serviceIds.Contains(x.ServiceFk.Id))
                .WhereIf(serviceId > 0, x => x.ServiceFk.Id == serviceId)
                .WhereIf(cateId > 0, x => x.Id == cateId)
                .OrderBy(x => x.Order).ThenBy(x => x.CategoryCode)
                .ThenBy(x => x.CategoryName);
            return query.ToListAsync();
        }

        public async Task<Category> GetCategory(string cateCode)
        {
            return await _categoryrepository.FirstOrDefaultAsync(x => x.CategoryCode == cateCode);
        }

        public Task<List<Product>> GetProducts(string productCode, string cateCode)
        {
            var query = _productRepository.GetAllIncluding(x => x.CategoryFk)
                .Include(x => x.CategoryFk.ServiceFk)
                .Where(c => c.Status == CommonConst.ProductStatus.Active)
                .WhereIf(!string.IsNullOrEmpty(productCode), x => x.ProductCode == productCode)
                .WhereIf(!string.IsNullOrEmpty(cateCode), x => x.CategoryFk.CategoryCode.Contains(cateCode))
                .OrderBy(x => x.Order).ThenBy(x => x.ProductCode)
                .ThenBy(x => x.ProductName);
            return query.ToListAsync();
        }

        public async Task<bool> CheckServiceActive(string serviceCode)
        {
            var service = await _serivceRepository.FirstOrDefaultAsync(x =>
                x.ServiceCode == serviceCode && x.Status == ServiceStatus.Active);
            return service != null;
        }

        public async Task<bool> CheckCategoryActive(string categoryCode)
        {
            var service = await _categoryrepository.FirstOrDefaultAsync(x =>
                x.CategoryCode == categoryCode && x.Status == CommonConst.CategoryStatus.Active);
            return service != null;
        }

        public async Task<bool> CheckStaffTime(long userId)
        {
            return await _staffConfigurationManager.CheckStaffAction(userId);
        }

        public Task<bool> CheckProductInCate(string productCode, string categoryCode)
        {
            throw new NotImplementedException();
        }

        public string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" +
                   HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public async Task<List<VendorTransDto>> GetListVendorTrans(string search)
        {
            var vendor = _vendorRepository.GetAll().Where(x => x.Status == 1)
                .WhereIf(!string.IsNullOrEmpty(search), x => x.Code.Contains(search) || x.Name.Contains(search));
            var lst = vendor.Select(x => new VendorTransDto
            {
                Code = x.Code,
                Name = x.Name
            }).ToList();
            var query = from p in _productRepository.GetAllIncluding(x => x.CategoryFk)
                join s in _serivceRepository.GetAll() on p.CategoryFk.ServiceId equals s.Id
                where s.ServiceCode == CommonConst.ServiceCodes.PAY_BILL
                select new VendorTransDto
                {
                    Code = p.ProductCode,
                    Name = p.ProductName
                };
            if (string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));
            lst.AddRange(query);
            return lst;
        }

        public async Task<List<VendorTransDto>> GetListVendorTrans()
        {
            var vendor = _vendorRepository.GetAll().Where(x => x.Status == 1);
            var lst = vendor.Select(x => new VendorTransDto
            {
                Code = x.Code,
                Name = x.Name
            }).ToList();
            var query = from p in _productRepository.GetAllIncluding(x => x.CategoryFk)
                join s in _serivceRepository.GetAll() on p.CategoryFk.ServiceId equals s.Id
                where s.ServiceCode == CommonConst.ServiceCodes.PAY_BILL
                select new VendorTransDto
                {
                    Code = p.ProductCode,
                    Name = p.ProductName
                };
            lst.AddRange(query);
            return lst;
        }

        public async Task<List<Provider>> GetProviderCache()
        {
            try
            {
                return await _cacheManager.GetCache("ProviderAirtime").AsTyped<string, List<Provider>>().GetAsync($"ProviderAirtime_Items",
                    async () =>
                    {
                        return await _provider.GetAll()
                            .Where(x => x.ProviderStatus == CommonConst.ProviderStatus.Active).ToListAsync();
                    });
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<List<IdentityServerStorageDto>> GetClientIds()
        {
            var item = await _idenRepository.GetAll().Where(x => x.IsActive).ToListAsync();
            return item?.ConvertTo<List<IdentityServerStorageDto>>();
        }

        public async Task<IdentityServerStorageDto> GetClientId(string accountCode)
        {
            var item = await _idenRepository.FirstOrDefaultAsync(x => x.AccountCode == accountCode);
            if (item == null)
                return null;
            return item.ConvertTo<IdentityServerStorageDto>();
        }

        public async Task<bool> CreateOrUpdateClientId(IdentityServerStorageInputDto input)
        {
            try
            {
                var check = await _idenRepository.FirstOrDefaultAsync(x => x.AccountCode == input.AccountCode);
                if (check == null)
                {
                    var item = new AbpIdentittyServerStorage
                    {
                        AccountCode = input.AccountCode,
                        ClientId = input.ClientId,
                        ClientName = input.ClientName,
                        IsActive = input.IsActive,
                        RequireConsent = input.RequireConsent,
                        AllowOfflineAccess = input.AllowOfflineAccess,
                        ClientSecrets = input.ClientSecrets.ToJson(),
                        AllowedScopes = input.AllowedScopes.ToJson(),
                        RedirectUris = !string.IsNullOrEmpty(input.RedirectUris)
                            ? input.RedirectUris.Split(";").ToList().ToJson()
                            : null,
                        PostLogoutRedirectUris = !string.IsNullOrEmpty(input.PostLogoutRedirectUris)
                            ? input.PostLogoutRedirectUris.Split(";").ToList().ToJson()
                            : null,
                        AllowedGrantTypes = input.AllowedGrantTypes.ToJson()
                    };
                    await _idenRepository.InsertAsync(item);
                }
                else
                {
                    check.ClientName = input.ClientName;
                    check.IsActive = input.IsActive;
                    check.RequireConsent = input.RequireConsent;
                    check.AllowOfflineAccess = input.AllowOfflineAccess;
                    check.ClientSecrets = input.ClientSecrets.ToJson();
                    check.AllowedScopes = input.AllowedScopes.ToJson();
                    check.RedirectUris = !string.IsNullOrEmpty(input.RedirectUris)
                        ? input.RedirectUris.Split(";").ToList().ToJson()
                        : null;
                    check.PostLogoutRedirectUris = !string.IsNullOrEmpty(input.PostLogoutRedirectUris)
                        ? input.PostLogoutRedirectUris.Split(";").ToList().ToJson()
                        : null;
                    check.AllowedGrantTypes = input.AllowedGrantTypes.ToJson();
                    await _idenRepository.UpdateAsync(check);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateOrUpdateClientId error:{e}");
                return false;
            }
        }
    }
}