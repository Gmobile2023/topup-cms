using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Runtime.Session;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Banks;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Common.Dto;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Editions;
using HLS.Topup.Editions.Dto;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Services.Dtos;
using Abp.UI;
using HLS.Topup.Address;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Dtos.Policy;
using HLS.Topup.FeeManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using HLS.Topup.Providers;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Sale;
using HLS.Topup.Services;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : TopupAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Service> _serivceRepository;
        private readonly ICommonManger _commonManger;
        private readonly IDiscountManger _discountManger;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<StaffConfiguration> _staffRepository;
        private readonly ISaleManManager _saleManManager;
        private readonly TopupAppSession _topupAppSession;
        private readonly UrlExtentions _extentions;
        private readonly IFeeManager _feeManager;

        public CommonLookupAppService(EditionManager editionManager, IRepository<Product> productRepository,
            IRepository<City> cityRepository, IRepository<District> districtRepository,
            IRepository<Ward> wardRepository,
            IRepository<Category> categoryRepository, IRepository<Provider> providerRepository,
            ICommonManger commonManger, IWebHostEnvironment env,
            IRepository<Bank> bankRepository, IDiscountManger discountManger,
            IRepository<StaffConfiguration> staffRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<SaleLimitDebt> saleLimitDebtRepository,
            IRepository<Service> serivceRepository,
            ISaleManManager saleManManager, TopupAppSession topupAppSession, UrlExtentions extentions,
            IFeeManager feeManager)
        {
            _editionManager = editionManager;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _commonManger = commonManger;
            _bankRepository = bankRepository;
            _discountManger = discountManger;
            _staffRepository = staffRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _saleManManager = saleManManager;
            _topupAppSession = topupAppSession;
            _extentions = extentions;
            _feeManager = feeManager;
            _appConfiguration = env.GetAppConfiguration();
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _providerRepository = providerRepository;
            _serivceRepository = serivceRepository;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(
            bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e =>
                    new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                        )
                    ).ToList()
                );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }

        public async Task<UserInfoDto> GetUserInfoQuery(GetUserInfoRequest input)
        {
            User user = null;
            if (!string.IsNullOrEmpty(input.AccountCode))
                user = await UserManager.GetUserByAccountCodeAsync(input.AccountCode);
            if (!string.IsNullOrEmpty(input.UserName))
                user = await UserManager.GetUserByUserNameAsync(input.UserName);
            if (!string.IsNullOrEmpty(input.Email))
                user = await UserManager.GetUserByEmailAsync(input.Email);
            if (!string.IsNullOrEmpty(input.PhoneNumber))
                user = await UserManager.GetUserByMobileAsync(input.PhoneNumber);
            if (input.UserId != null && input.UserId != 0)
                user = await UserManager.GetUserByIdAsync(input.UserId ?? 0);
            if (!string.IsNullOrEmpty(input.Search))
                user = await UserManager.GetUserAnyFieldAsync(input.Search);
            if (user == null)
                return null;
            if (user.IsAccountSystem())
                return null;
            return user.ConvertTo<UserInfoDto>();
        }

        public async Task<List<UserInfoDto>> GetListUserSearch(GetUserQueryRequest input)
        {
            var agentType = input.AgentType.HasValue
                ? (CommonConst.AgentType)input.AgentType
                : CommonConst.AgentType.Default;

            var request = new Dtos.Sale.UserInfoSearch()
            {
                SaleId = input.SaleId,
                Search = input.Search,
                AgentType = agentType,
            };

            if (request.SaleId == 0)
            {
                var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
                if (user.AccountType == CommonConst.SystemAccountType.Sale)
                    request.SaleId = Convert.ToInt32(user.Id);
                else if (user.AccountType == CommonConst.SystemAccountType.SaleLead)
                    request.SaleLeadId = Convert.ToInt32(user.Id);
                else if (user.AccountType != CommonConst.SystemAccountType.System)
                    request.SaleId = Convert.ToInt32(user.Id);
            }

            var list = await _saleManManager.GetUserAgentBy(request);

            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<List<UserInfoDto>> GetListUserSaleSearch(GetUserQueryRequest input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var list = await _saleManManager.GetUserSaleBySaleLeader(new Dtos.Sale.UserInfoSearch()
            {
                AccountType = user.AccountType,
                Search = input.Search,
                SaleLeadId = Convert.ToInt32(user.AccountType == CommonConst.SystemAccountType.System
                    ? 0
                    : user.AccountType == CommonConst.SystemAccountType.Sale
                        ? user.UserSaleLeadId
                        : user.Id),
                SaleId = Convert.ToInt32(user.AccountType == CommonConst.SystemAccountType.Sale ? user.Id : 0),
            });
            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<UserInfoDto> GetUserSaleLeaderBySale(int userId)
        {
            var user = await _saleManManager.GetUserSaleLeaderBySale(userId);
            return user;
        }

        public async Task<UserInfoDto> GetUserSaleAssign(int userId)
        {
            var user = await _saleManManager.GetSaleAssignDetail(userId);

            return user;
        }

        public async Task<List<UserInfoDto>> GetUserSaleLeader(GetUserQueryRequest input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var list = await _saleManManager.GetUserSaleLeader(new Dtos.Sale.UserInfoSearch()
            {
                AccountType = user.AccountType,
                Search = input.Search,
                LoginCode = user.AccountCode,
            });
            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<List<UserInfoDto>> GetUserSaleBySaleLeader(GetUserQueryRequest input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);

            var request = new Dtos.Sale.UserInfoSearch()
            {
                AccountType = user.AccountType,
                SaleLeadId = input.SaleLeaderId,
                SaleLeadCode = input.SaleLeaderCode,
                Search = input.Search,
            };

            if (user.AccountType == CommonConst.SystemAccountType.SaleLead)
                request.SaleLeadId = Convert.ToInt32(user.Id);
            else if (user.AccountType == CommonConst.SystemAccountType.Sale)
                request.SaleId = Convert.ToInt32(user.Id);

            var list = await _saleManManager.GetUserSaleBySaleLeader(request);
            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<List<UserInfoDto>> GetUserAgentStaff(GetUserQueryRequest input)
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var list = await _saleManManager.GetUserAgentStaff(new Dtos.Sale.UserInfoSearch()
            {
                AccountType = user.AccountType,
                SaleLeadId = Convert.ToInt32(user.AccountType == CommonConst.SystemAccountType.SaleLead
                    ? user.Id
                    : user.AccountType == CommonConst.SystemAccountType.Sale
                        ? user.UserSaleLeadId
                        : 0),
                Search = input.Search,
            });
            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<List<UserInfoDto>> GetUserAgentLevel(GetUserQueryRequest input)
        {
            //var user = await UserManager.GetUserByAccountCodeAsync(input.SaleLeaderCode);
            //if (user == null) return new List<UserInfoDto>();
            var list = await _saleManManager.GetUserAgentLevel(new Dtos.Sale.UserInfoSearch()
            {
                SaleLeadCode = input.SaleLeaderCode,
                Search = input.Search,
            });
            return list.ConvertTo<List<UserInfoDto>>();
        }

        public async Task<UserInfoDto> GetUserInfo()
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            return user.ConvertTo<UserInfoDto>();
        }

        public async Task<List<CategoryDto>> GetCategories(CategorySearchInput input)
        {
            var cates = await _commonManger.GetCategories(input.ServiceCode, input.CategoryCode, input.IsActive);
            if (cates == null || !cates.Any())
                return null;
            var list = cates.ConvertTo<List<CategoryDto>>();
            foreach (var item in list)
            {
                item.Image = _extentions.GetFullPath(item.Image);
            }

            return list;
        }

        public async Task<List<CategoryDto>> GetCategoriesMuti(CategorySearchInput input)
        {

            if (input.ServiceCodes == null || input.ServiceCodes.Count <= 0)
                return new List<CategoryDto>();
            else
            {
                input.ServiceCodes = input.ServiceCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
                if (input.ServiceCodes == null || input.ServiceCodes.Count <= 0)
                    return new List<CategoryDto>();
            }



            var cates = await _commonManger.GetCategoriesMuti(input.ServiceCodes, input.CategoryCode, input.IsActive);
            if (cates == null || !cates.Any())
                return null;
            var list = cates.ConvertTo<List<CategoryDto>>();
            foreach (var item in list)
            {
                item.Image = _extentions.GetFullPath(item.Image);
            }

            return list;
        }

        public async Task<List<CategoryDto>> GetCategoriesTwoBy(CategorySearchTwoInput input)
        {
            var cates = await _commonManger.GetCategories(input.ServiceId, input.ServiceIds, input.CategoryId, input.IsActive);
            if (cates == null || !cates.Any())
                return null;
            var list = cates.ConvertTo<List<CategoryDto>>();
            foreach (var item in list)
            {
                item.Image = _extentions.GetFullPath(item.Image);
            }

            return list;
        }

        public async Task<List<ServiceDto>> GetSetvices(bool isActive = true)
        {
            var cate = await _commonManger.GetServices();
            return cate.ConvertTo<List<ServiceDto>>();
        }

        public async Task<List<BankDto>> GetBanks(bool isActive = true)
        {
            var cate = _bankRepository.GetAll().WhereIf(isActive, x => x.Status == CommonConst.BankStatus.Active)
                .OrderBy(x => x.BankName);
            var items = cate.ConvertTo<List<BankDto>>();
            if (items == null || !items.Any())
                return null;
            foreach (var bank in items)
            {
                bank.Images = _extentions.GetFullPath(bank.Images);
            }

            return items;
        }

        public List<decimal> CardValues()
        {
            var unit = _appConfiguration["CardConfig:CardValues"];
            return unit.Split(',').Select(decimal.Parse).ToList();
        }

        public async Task<List<DiscountDetailDto>> GetDiscountDetails(GetDiscountDetailTableInput input)
        {
            return await _discountManger.GetDiscountDetails(input.DiscountId, input.CateIds);
        }

        public async Task<List<ProductInfoDto>> GetProducts(ProductSearchInput input)
        {
            var items = await _productRepository.GetAllIncluding(x => x.CategoryFk)
                .WhereIf(input.IsActive, x => x.Status == CommonConst.ProductStatus.Active)
                .WhereIf(input.IsActive, x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(!string.IsNullOrEmpty(input.CategoryCode),
                    x => x.CategoryFk.CategoryCode == input.CategoryCode)
                .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
                .OrderBy(x => x.ProductValue).ThenBy(x => x.ProductName).ToListAsync();
            return items.Select(x => new ProductInfoDto
            {
                CategoryCode = x.CategoryFk.CategoryCode,
                Id = x.Id,
                Image = _extentions.GetFullPath(x.Image),
                CategoryId = x.CategoryId,
                Order = x.Order,
                Status = x.Status,
                Unit = x.Unit,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductType = x.ProductType,
                ProductValue = x.ProductValue,
                CategoryName = x.CategoryFk.CategoryName
            }).ToList();
        }

        public async Task<ProductInfoDto> GetProductByCode(string productCode)
        {
            var items = (from x in _productRepository.GetAll().Where(c => c.ProductCode == productCode)
                         .Include(c => c.CategoryFk)
                         select new ProductInfoDto
                         {
                             CategoryCode = x.CategoryFk != null ? x.CategoryFk.CategoryCode : string.Empty,
                             CategoryName = x.CategoryFk != null ? x.CategoryFk.CategoryName : string.Empty,
                             ProductCode = x.ProductCode,
                             ProductName = x.ProductName,
                             ProductType = x.ProductType,
                             ProductValue = x.ProductValue,
                         }).FirstOrDefault();

            return items;
        }

        public async Task<List<CategoryDto>> GetCategoryUseCard(bool isActive = true)
        {
            var items = await _categoryRepository.GetAllIncluding(x => x.ServiceFk)
                .WhereIf(isActive, x => x.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(isActive, x => x.Status == CommonConst.CategoryStatus.Active)
                .Where(x => x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ||
                            x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ||
                            x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_GAME)
                .OrderBy(x => x.CategoryName).ToListAsync();
            var list = items.ConvertTo<List<CategoryDto>>();
            if (list == null || !list.Any())
                return null;
            foreach (var item in list)
            {
                item.Image = _extentions.GetFullPath(item.Image);
            }

            return list;
        }

        public async Task<PagedResultDto<ProductDiscountDto>> GetProductDiscountAccount(
            GetProductDiscountAccountTableInput input)
        {
            var accountInfo = UserManager.GetAccountInfo();
            var item = await _discountManger.GetProductDiscountsByService(input.ServiceCode,
                accountInfo.NetworkInfo.AccountCode, input.Search);
            return new PagedResultDto<ProductDiscountDto>(
                item.Count,
                item
            );
        }

        public async Task<PagedResultDto<PolicyAccountDto>> GetPolicyAccount(
            GetProductDiscountAccountTableInput input)
        {
            var accountInfo = UserManager.GetAccountInfo();
            var items = await _discountManger.GetProductDiscountsByService(input.ServiceCode,
                accountInfo.NetworkInfo.AccountCode, input.Search);
            if (items == null || !items.Any())
                return new PagedResultDto<PolicyAccountDto>(
                    0,
                    new List<PolicyAccountDto>()
                );
            var lst = items.ConvertTo<List<PolicyAccountDto>>();
            if (input.ServiceCode == CommonConst.ServiceCodes.PAY_BILL)
            {
                foreach (var item in lst)
                {
                    var fee = await _feeManager.GetProductFee(item.ProductCode,
                        accountInfo.NetworkInfo.AccountCode, 0);
                    if (fee == null) continue;
                    item.AmountIncrease = fee.AmountIncrease;
                    item.FeeId = fee.FeeId;
                    item.FeeDetailId = fee.FeeDetailId;
                    item.FeeValue = fee.FeeValue;
                    item.ProductId = fee.ProductId;
                    item.SubFee = fee.SubFee;
                    item.AmountMinFee = fee.AmountMinFee;
                    item.MinFee = fee.MinFee;
                }
            }

            return new PagedResultDto<PolicyAccountDto>(
                lst.Count,
                lst //chỗ này pagging tạm
            );
        }

        public async Task<List<ProductDiscountDto>> GetProductDiscounts(GetProductDiscountInput input)
        {
            var accountInfo = UserManager.GetAccountInfo();
            return await _discountManger.GetProductDiscounts(input.CategoryCode, accountInfo.NetworkInfo.AccountCode);
        }
        public async Task<ProductDiscountDto> GetProductDiscount(GetProductDiscountUserDto input)
        {
            var accountInfo = UserManager.GetAccountInfo();
            return await _discountManger.GeProductDiscountAccount(input.ProductCode, accountInfo.NetworkInfo.AccountCode);
        }

        public async Task<List<CityDto>> GetProvinces()
        {
            var items = await _cityRepository.GetAll().OrderBy(x => x.CityName).ToListAsync();
            return items.ConvertTo<List<CityDto>>();
        }

        public async Task<CityDto> GetCityById(int cityId)
        {
            var items = await _cityRepository.FirstOrDefaultAsync(c => c.Id == cityId);
            return items.ConvertTo<CityDto>();
        }

        public async Task<List<DistrictDto>> GetDistricts(int? provinceId, bool showAll = false)
        {
            if (!provinceId.HasValue && showAll == false) return new List<DistrictDto>();
            var items = await _districtRepository.GetAll()
                .WhereIf(provinceId.HasValue, x => x.CityId == provinceId.Value)
                .OrderBy(x => x.DistrictName)
                .ToListAsync();
            return items.ConvertTo<List<DistrictDto>>();
        }

        public async Task<DistrictDto> GetDistrictsById(int districtid)
        {
            var items = await _districtRepository.FirstOrDefaultAsync(c => c.Id == districtid);
            return items.ConvertTo<DistrictDto>();
        }

        public async Task<List<WardDto>> GetWards(int? districtId, bool showAll = false)
        {
            if (!districtId.HasValue && showAll == false) return new List<WardDto>();
            var items = await _wardRepository.GetAll()
                .WhereIf(districtId.HasValue, x => x.DistrictId == districtId.Value)
                .OrderBy(x => x.WardName)
                .ToListAsync();
            return items.ConvertTo<List<WardDto>>();
        }

        public async Task<WardDto> GetWardsById(int wardId)
        {
            var items = await _wardRepository.FirstOrDefaultAsync(c => c.Id == wardId);
            return items.ConvertTo<WardDto>();
        }

        public async Task<List<ProductDto>> GetProductByCategory(string categoryCode, bool isActive = true, bool isShowOnFrontend = true)
        {
            var data = await _productRepository
                .GetAllIncluding(x => x.CategoryFk)
                .Where(x => x.CategoryFk.CategoryCode == categoryCode)
                .WhereIf(isActive, x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(isActive, x => x.Status == CommonConst.ProductStatus.Active)
                .WhereIf(isShowOnFrontend, x => x.IsShowOnFrontend == isShowOnFrontend)
                .ToListAsync();
            if (data == null || !data.Any())
                return null;
            var lst = new List<ProductDto>();
            foreach (var item in data)
            {
                var pro = item.ConvertTo<ProductDto>();
                pro.Image = _extentions.GetFullPath(item.Image);
                pro.CategoryName = item.CategoryFk.CategoryName;
                pro.CategoryCode = item.CategoryFk.CategoryCode;
                pro.CategoryId = item.CategoryFk.Id;
                lst.Add(pro);
            }

            return lst.OrderBy(x => x.ProductValue).ThenBy(x => x.Order).ToList();
        }

        public async Task<List<ProductDto>> GetProductByCategoryMuti(List<string> categoryCode, bool isActive = true)
        {
            var data = await _productRepository
                .GetAllIncluding(x => x.CategoryFk)
                .Where(x => categoryCode.Contains(x.CategoryFk.CategoryCode))
                .WhereIf(isActive, x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(isActive, x => x.Status == CommonConst.ProductStatus.Active)
                .ToListAsync();
            if (data == null || !data.Any())
                return null;
            var lst = new List<ProductDto>();
            foreach (var item in data)
            {
                var pro = item.ConvertTo<ProductDto>();
                pro.Image = _extentions.GetFullPath(item.Image);
                pro.CategoryName = item.CategoryFk.CategoryName;
                lst.Add(pro);
            }

            return lst;
        }

        public async Task<List<ProductDto>> GetProductTwoByCategory(List<int> categoryIds, bool isActive = true)
        {
            var data = await _productRepository
                .GetAllIncluding(x => x.CategoryFk)
                .Where(x => categoryIds.Contains(x.CategoryFk.Id))
                .WhereIf(isActive, x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(isActive, x => x.Status == CommonConst.ProductStatus.Active)
                .ToListAsync();
            if (data == null || !data.Any())
                return null;
            var lst = new List<ProductDto>();
            foreach (var item in data)
            {
                var pro = item.ConvertTo<ProductDto>();
                // item.CategoryName = data.FirstOrDefault(x => x.ProductCode == item.ProductCode)?.CategoryFk
                //     .CategoryName;
                pro.Image = _extentions.GetFullPath(item.Image);
                pro.CategoryName = item.CategoryFk.CategoryName;
                lst.Add(pro);
            }

            return lst;
        }

        public async Task<ProductDto> GetProductByProductCode(string productCode, bool isActive = true)
        {
            var data = await _productRepository
                .GetAllIncluding(x => x.CategoryFk)
                .Where(x => x.ProductCode == productCode)
                .WhereIf(isActive, x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active)
                .WhereIf(isActive, x => x.Status == CommonConst.ProductStatus.Active)
                .FirstOrDefaultAsync();

            var lst = new ProductDto();
            if (data != null)
            {
                lst = data.ConvertTo<ProductDto>();
                lst.Image = _extentions.GetFullPath(data.Image);
                lst.CategoryName = data.CategoryFk.CategoryName;
            }

            return lst;
        }

        public async Task<List<ProviderDto>> GetProvider(bool isActive = true)
        {
            var data = _providerRepository.GetAll()
                .WhereIf(isActive, x => x.ProviderStatus == CommonConst.ProviderStatus.Active);
            return data?.ConvertTo<List<ProviderDto>>();
        }

        public async Task<List<UserStaffDto>> GetStaffUserQuery(string search)
        {
            try
            {
                var networkInfo = UserManager.GetAccountInfo();
                var query = from ouUser in _userOrganizationUnitRepository.GetAll()
                            join ou in _organizationUnitRepository.GetAll() on ouUser.OrganizationUnitId equals ou.Id
                            join user in UserManager.Users on ouUser.UserId equals user.Id
                            join staff in _staffRepository.GetAll() on user.Id equals staff.UserId
                            where ouUser.OrganizationUnitId == networkInfo.NetworkInfo.OrganizationUnitId
                            select new UserStaffDto
                            {
                                Gender = user.Gender,
                                Name = user.Name,
                                Surname = user.Surname,
                                AccountCode = user.AccountCode,
                                AccountType = user.AccountType,
                                CreationTime = user.CreationTime,
                                Id = user.Id,
                                DoB = user.DoB,
                                LimitAmount = staff.LimitAmount,
                                EmailAddress = user.EmailAddress,
                                IsActive = user.IsActive,
                                FullName = user.FullName,
                                PhoneNumber = user.PhoneNumber,
                                UserName = user.UserName,
                                NetworkLevel = user.NetworkLevel,
                                TreePath = user.TreePath
                            };
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x =>
                        x.AccountCode.Contains(search)
                        || x.PhoneNumber.Contains(search)
                        || x.UserName.Contains(search));
                }

                var list = await query.Take(100).ToListAsync();
                if (networkInfo.NetworkInfo.AccountType == CommonConst.SystemAccountType.Agent
                    || networkInfo.NetworkInfo.AccountType == CommonConst.SystemAccountType.MasterAgent
                    || networkInfo.NetworkInfo.AccountType == CommonConst.SystemAccountType.Company)
                {
                    if (networkInfo.NetworkInfo.AccountCode.Contains(search)
                        || networkInfo.NetworkInfo.PhoneNumber.Contains(search)
                        || networkInfo.NetworkInfo.UserName.Contains(search))

                    {
                        var single = new UserStaffDto
                        {
                            Gender = networkInfo.NetworkInfo.Gender,
                            Name = networkInfo.NetworkInfo.Name,
                            Surname = networkInfo.NetworkInfo.Surname,
                            AccountCode = networkInfo.NetworkInfo.AccountCode,
                            AccountType = networkInfo.NetworkInfo.AccountType,
                            CreationTime = networkInfo.NetworkInfo.CreationTime,
                            Id = networkInfo.NetworkInfo.Id,
                            DoB = networkInfo.NetworkInfo.DoB,
                            LimitAmount = 0,
                            EmailAddress = networkInfo.NetworkInfo.EmailAddress,
                            IsActive = networkInfo.NetworkInfo.IsActive,
                            FullName = networkInfo.NetworkInfo.FullName,
                            PhoneNumber = networkInfo.NetworkInfo.PhoneNumber,
                            UserName = networkInfo.NetworkInfo.UserName,
                            NetworkLevel = networkInfo.NetworkInfo.NetworkLevel,
                            TreePath = networkInfo.NetworkInfo.TreePath
                        };
                        list.Add(single);
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Lỗi");
            }
        }

        public async Task<List<UserInfoDto>> GetAgentSaleLocationQuery(long saleId, string search)
        {
            return await _saleManManager.GetAgentLocationSale(saleId, search);
        }

        public async Task<List<CommonLookupTableDto>> ServiceCardList()
        {
            var data = await _serivceRepository.GetAll()
                .Where(x => x.ServiceCode.StartsWith("PIN_"))
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.ServiceCode,
                    DisplayName = p == null || p.ServicesName == null ? "" : p.ServicesName.ToString(),
                    Payload = ""
                }).ToListAsync();
            return data;
        }

        public async Task<List<VendorTransDto>> GetListVendorTrans(string search)
        {
            return await _commonManger.GetListVendorTrans(search);
        }
    }
}
