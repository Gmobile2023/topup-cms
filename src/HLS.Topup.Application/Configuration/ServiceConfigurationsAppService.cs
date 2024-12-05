using System;
using HLS.Topup.Services;
using System.Collections.Generic;
using HLS.Topup.Providers;
using HLS.Topup.Categories;
using HLS.Topup.Products;
using HLS.Topup.Authorization.Users;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Configuration.Exporting;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.Runtime.Caching;
using HLS.Topup.Dtos.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;
using HLS.Topup.Common;

namespace HLS.Topup.Configuration
{
    //[AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
    public class ServiceConfigurationsAppService : TopupAppServiceBase, IServiceConfigurationsAppService
    {
        private readonly IRepository<ServiceConfiguration> _serviceConfigurationRepository;
        private readonly IRepository<PartnerServiceConfiguration> _partnerServiceConfiguation;
        private readonly IServiceConfigurationsExcelExporter _serviceConfigurationsExcelExporter;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly IRepository<Provider, int> _lookup_providerRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IRepository<Product, int> _lookup_productRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IConfigurationRoot _appConfiguration;

        private readonly IServiceConfigurationManager _serviceConfigurationManager;

        //private readonly Logger _logger = LogManager.GetLogger("ServiceConfigurationsAppService");
        private readonly ILogger<ServiceConfigurationsAppService> _logger;
        private readonly ICacheManager _cacheManager;


        public ServiceConfigurationsAppService(IRepository<ServiceConfiguration> serviceConfigurationRepository,
            IServiceConfigurationsExcelExporter serviceConfigurationsExcelExporter,
            IRepository<Service, int> lookup_serviceRepository, IRepository<Provider, int> lookup_providerRepository,
            IRepository<Category, int> lookup_categoryRepository, IRepository<Product, int> lookup_productRepository,
            IRepository<User, long> lookup_userRepository, IServiceConfigurationManager serviceConfigurationManager,
            ILogger<ServiceConfigurationsAppService> logger, ICacheManager cacheManager,
            IWebHostEnvironment hostingEnvironment, IRepository<UserProfile> userProfileRepository,
            IRepository<PartnerServiceConfiguration> partnerServiceConfiguation)
        {
            _serviceConfigurationRepository = serviceConfigurationRepository;
            _serviceConfigurationsExcelExporter = serviceConfigurationsExcelExporter;
            _lookup_serviceRepository = lookup_serviceRepository;
            _lookup_providerRepository = lookup_providerRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_userRepository = lookup_userRepository;
            _serviceConfigurationManager = serviceConfigurationManager;
            _logger = logger;
            _cacheManager = cacheManager;
            _userProfileRepository = userProfileRepository;
            _partnerServiceConfiguation = partnerServiceConfiguation;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<PagedResultDto<GetServiceConfigurationForViewDto>> GetAll(
            GetAllServiceConfigurationsInput input)
        {
            try
            {
                input.ServiceIds = input.ServiceIds.Where(c => c > 0).Select(c => c).ToList();
                input.CategoryIds = input.CategoryIds.Where(c => c > 0).Select(c => c).ToList();
                input.ProductIds = input.ProductIds.Where(c => c > 0).Select(c => c).ToList();

                var filteredServiceConfigurations = _serviceConfigurationRepository.GetAll()
                    .Include(e => e.ServiceFk)
                    .Include(e => e.ProviderFk)
                    .Include(e => e.CategoryFk)
                    .Include(e => e.ProductFk)
                    .Include(e => e.UserFk)
                    .WhereIf(input.UserId != null, x => x.UserId == input.UserId)
                    .WhereIf(input.ProviderId != null, x => x.ProviderId == input.ProviderId)
                    .WhereIf(input.ServiceId != null, x => x.ServiceId == input.ServiceId)
                    .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
                    .WhereIf(input.ProductId != null, x => x.ProductId == input.ProductId)
                    .WhereIf(input.ServiceIds != null && input.ServiceIds.Count > 0,
                        x => input.ServiceIds.Contains(x.ServiceId ?? 0))
                    .WhereIf(input.CategoryIds != null && input.CategoryIds.Count > 0,
                        x => input.CategoryIds.Contains(x.CategoryId ?? 0))
                    .WhereIf(input.ProductIds != null && input.ProductIds.Count > 0,
                        x => input.ProductIds.Contains(x.ProductId ?? 0))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                        e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),
                        e => e.Name.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                             e.UserFk.Name.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                             e.UserFk.Surname.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                             e.ProviderFk.Code.ToUpper().Contains(input.NameFilter.ToUpper()))
                    .WhereIf(input.IsOpenedFilter.HasValue && input.IsOpenedFilter > -1,
                        e => (input.IsOpenedFilter == 1 && e.IsOpened) || (input.IsOpenedFilter == 0 && !e.IsOpened))
                    .WhereIf(input.MinPriorityFilter != null, e => e.Priority >= input.MinPriorityFilter)
                    .WhereIf(input.MaxPriorityFilter != null, e => e.Priority <= input.MaxPriorityFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                        e => e.ServiceFk != null && e.ServiceFk.ServicesName == input.ServiceServicesNameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderNameFilter),
                        e => e.ProviderFk != null && e.ProviderFk.Name == input.ProviderNameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                        e => e.CategoryFk != null && e.CategoryFk.CategoryName == input.CategoryCategoryNameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.ProductProductNameFilter),
                        e => e.ProductFk != null && e.ProductFk.ProductName == input.ProductProductNameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                        e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

                var pagedAndFilteredServiceConfigurations = filteredServiceConfigurations
                    .OrderByDescending(x => x.Id).ThenBy(x => x.ProviderFk.Code)
                    .PageBy(input);

                var serviceConfigurations = from o in pagedAndFilteredServiceConfigurations
                    join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()
                    join o2 in _lookup_providerRepository.GetAll() on o.ProviderId equals o2.Id into j2
                    from s2 in j2.DefaultIfEmpty()
                    join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                    from s3 in j3.DefaultIfEmpty()
                    join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                    from s4 in j4.DefaultIfEmpty()
                    join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                    from s5 in j5.DefaultIfEmpty()
                    select new GetServiceConfigurationForViewDto()
                    {
                        ServiceConfiguration = new ServiceConfigurationDto
                        {
                            Name = (o.Name ?? string.Empty).ToUpper(),
                            IsOpened = o.IsOpened,
                            Priority = o.Priority,
                            Id = o.Id,
                            //IsEnableResponseWhenJustReceived = o.IsEnableResponseWhenJustReceived,
                            //ProviderMaxWaitingTimeout = o.ProviderMaxWaitingTimeout,
                            //ProviderSetTransactionTimeout = o.ProviderSetTransactionTimeout,
                            //StatusResponseWhenJustReceived = o.StatusResponseWhenJustReceived
                        },
                        ServiceServicesName = s1 == null || s1.ServicesName == null ? "" : s1.ServicesName.ToString(),
                        ProviderName = s2 == null || s2.Name == null ? "" : s2.Code + "-" + s2.Name.ToString(),
                        CategoryCategoryName = s3 == null || s3.CategoryName == null ? "" : s3.CategoryName.ToString(),
                        ProductProductName = s4 == null || s4.ProductName == null ? "" : s4.ProductName.ToString(),
                        UserName = s5 == null || s5.Name == null ? "" : s5.AccountCode + "-" + s5.Name.ToString()
                    };

                var totalCount = await filteredServiceConfigurations.CountAsync();

                return new PagedResultDto<GetServiceConfigurationForViewDto>(
                    totalCount,
                    await serviceConfigurations.ToListAsync()
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<GetServiceConfigurationForViewDto> GetServiceConfigurationForView(int id)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.GetAsync(id);

            var output = new GetServiceConfigurationForViewDto
                { ServiceConfiguration = ObjectMapper.Map<ServiceConfigurationDto>(serviceConfiguration) };

            if (output.ServiceConfiguration.ServiceId != null)
            {
                var _lookupService =
                    await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ServiceId);
                output.ServiceServicesName = _lookupService?.ServicesName?.ToString();
            }

            if (output.ServiceConfiguration.ProviderId != null)
            {
                var _lookupProvider =
                    await _lookup_providerRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ProviderId);
                output.ProviderName = _lookupProvider?.Name?.ToString();
            }

            if (output.ServiceConfiguration.CategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.CategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            if (output.ServiceConfiguration.ProductId != null)
            {
                var _lookupProduct =
                    await _lookup_productRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ProductId);
                output.ProductProductName = _lookupProduct?.ProductName?.ToString();
            }

            if (output.ServiceConfiguration.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long)output.ServiceConfiguration.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations_Edit)]
        public async Task<GetServiceConfigurationForEditOutput> GetServiceConfigurationForEdit(EntityDto input)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetServiceConfigurationForEditOutput
                { ServiceConfiguration = ObjectMapper.Map<CreateOrEditServiceConfigurationDto>(serviceConfiguration) };

            if (output.ServiceConfiguration.ServiceId != null)
            {
                var _lookupService =
                    await _lookup_serviceRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ServiceId);
                output.ServiceServicesName = _lookupService?.ServicesName?.ToString();
            }

            if (output.ServiceConfiguration.ProviderId != null)
            {
                var _lookupProvider =
                    await _lookup_providerRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ProviderId);
                output.ProviderName = _lookupProvider?.Name?.ToString();
            }

            if (output.ServiceConfiguration.CategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.CategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            if (output.ServiceConfiguration.ProductId != null)
            {
                var _lookupProduct =
                    await _lookup_productRepository.FirstOrDefaultAsync((int)output.ServiceConfiguration.ProductId);
                output.ProductProductName = _lookupProduct?.ProductName?.ToString();
            }

            if (output.ServiceConfiguration.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long)output.ServiceConfiguration.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditServiceConfigurationDto input)
        {
            if (string.IsNullOrEmpty(input.AllowTopupReceiverType))
                input.AllowTopupReceiverType = null;

            input.Name = (input.Name ?? string.Empty).ToUpper();
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations_Create)]
        protected virtual async Task Create(CreateOrEditServiceConfigurationDto input)
        {
            var serviceConfiguration = ObjectMapper.Map<ServiceConfiguration>(input);
            if (AbpSession.TenantId != null)
            {
                serviceConfiguration.TenantId = (int?)AbpSession.TenantId;
            }

            await _serviceConfigurationRepository.InsertAsync(serviceConfiguration);
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations_Edit)]
        protected virtual async Task Update(CreateOrEditServiceConfigurationDto input)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, serviceConfiguration);
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _serviceConfigurationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetServiceConfigurationsToExcel(GetAllServiceConfigurationsForExcelInput input)
        {
            input.ServiceIds = input.ServiceIds.Where(c => c > 0).Select(c => c).ToList();
            input.CategoryIds = input.CategoryIds.Where(c => c > 0).Select(c => c).ToList();
            input.ProductIds = input.ProductIds.Where(c => c > 0).Select(c => c).ToList();

            var filteredServiceConfigurations = _serviceConfigurationRepository.GetAll()
                .Include(e => e.ServiceFk)
                .Include(e => e.ProviderFk)
                .Include(e => e.CategoryFk)
                .Include(e => e.ProductFk)
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.IsOpenedFilter.HasValue && input.IsOpenedFilter > -1,
                    e => (input.IsOpenedFilter == 1 && e.IsOpened) || (input.IsOpenedFilter == 0 && !e.IsOpened))
                .WhereIf(input.MinPriorityFilter != null, e => e.Priority >= input.MinPriorityFilter)
                .WhereIf(input.MaxPriorityFilter != null, e => e.Priority <= input.MaxPriorityFilter)
                .WhereIf(input.ServiceIds != null && input.ServiceIds.Count > 0,
                    x => input.ServiceIds.Contains(x.ServiceId ?? 0))
                .WhereIf(input.CategoryIds != null && input.CategoryIds.Count > 0,
                    x => input.CategoryIds.Contains(x.CategoryId ?? 0))
                .WhereIf(input.ProductIds != null && input.ProductIds.Count > 0,
                    x => input.ProductIds.Contains(x.ProductId ?? 0))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                    e => e.ServiceFk != null && e.ServiceFk.ServicesName == input.ServiceServicesNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderNameFilter),
                    e => e.ProviderFk != null && e.ProviderFk.Name == input.ProviderNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryName == input.CategoryCategoryNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductProductNameFilter),
                    e => e.ProductFk != null && e.ProductFk.ProductName == input.ProductProductNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredServiceConfigurations
                join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_providerRepository.GetAll() on o.ProviderId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                from s4 in j4.DefaultIfEmpty()
                join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                from s5 in j5.DefaultIfEmpty()
                select new GetServiceConfigurationForViewDto()
                {
                    ServiceConfiguration = new ServiceConfigurationDto
                    {
                        Name = (o.Name ?? string.Empty).ToUpper(),
                        IsOpened = o.IsOpened,
                        Priority = o.Priority,
                        Id = o.Id
                    },
                    ServiceServicesName = s1 == null || s1.ServicesName == null ? "" : s1.ServicesName.ToString(),
                    ProviderName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                    CategoryCategoryName = s3 == null || s3.CategoryName == null ? "" : s3.CategoryName.ToString(),
                    ProductProductName = s4 == null || s4.ProductName == null ? "" : s4.ProductName.ToString(),
                    UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                });


            var serviceConfigurationListDtos = await query.ToListAsync();

            return _serviceConfigurationsExcelExporter.ExportToFile(serviceConfigurationListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<List<ServiceConfigurationServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookup_serviceRepository.GetAll()
                .Select(service => new ServiceConfigurationServiceLookupTableDto
                {
                    Id = service.Id,
                    DisplayName = service == null || service.ServicesName == null ? "" : service.ServicesName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<List<ServiceConfigurationProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookup_providerRepository.GetAll()
                .Select(provider => new ServiceConfigurationProviderLookupTableDto
                {
                    Id = provider.Id,
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<List<ServiceConfigurationCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAll()
                .Select(category => new ServiceConfigurationCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryName.ToString()
                }).ToListAsync();
        }


        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<PagedResultDto<ServiceConfigurationProductLookupTableDto>> GetAllProductForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll()
                .WhereIf(input.CategoryId > 0, e => e.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.ProductName != null && e.ProductName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ServiceConfigurationProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ServiceConfigurationProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.ProductName?.ToString()
                });
            }

            return new PagedResultDto<ServiceConfigurationProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<PagedResultDto<ServiceConfigurationUserLookupTableDto>> GetAllUserForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e =>
                    (e.Name != null && e.Name.Contains(input.Filter))
                    || (e.AccountCode != null && e.AccountCode.Contains(input.Filter))
                    || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
            );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ServiceConfigurationUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ServiceConfigurationUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName?.ToString(),
                    AccountCode = user.AccountCode?.ToString(),
                    Phone = user.PhoneNumber?.ToString(),
                });
            }

            return new PagedResultDto<ServiceConfigurationUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<ServiceConfiguationDto>> GetServiceConfiguations(GetServiceConfigurationInput input)
        {
            try
            {
                _logger.LogInformation($"GetServiceConfiguations request: {input.ToJson()}");
                var result = await GetServiceConfiguationsFromDb(input);
                if (result == null || !result.Any())
                    return null;
                var data = new List<ServiceConfiguationDto>();
                foreach (var item in result)
                {
                    if (item.IsRoundRobinAccount)
                    {
                        var listChild = await _lookup_providerRepository.GetAll()
                            .Where(x => x.ParentProvider == item.ProviderCode &&
                                        x.ProviderStatus == CommonConst.ProviderStatus.Active)
                            .ToListAsync();
                        if (listChild.Any())
                        {
                            //Lấy nguyên cấu hình của thằng cha.
                            item.SubConfiguration = new List<ServiceConfiguationDto>();
                            foreach (var child in listChild)
                            {
                                var newItem = new ServiceConfiguationDto().PopulateWith(item);
                                newItem.ProviderCode = child.Code;
                                newItem.ProviderName = child.Name;
                                newItem.ParentProvider = child.ParentProvider;
                                newItem.SubConfiguration = null;
                                newItem.IsAutoDeposit = child.IsAutoDeposit;
                                newItem.IsRoundRobinAccount = child.IsRoundRobinAccount;
                                newItem.MinBalance = child.MinBalance;
                                newItem.MinBalanceToDeposit = child.MinBalanceToDeposit;
                                newItem.DepositAmount = child.DepositAmount;
                                item.SubConfiguration.Add(newItem);
                            }
                        }
                    }

                    data.Add(item);
                    if (item.IsLastConfiguration)
                        break;
                }

                return data.Count <= 0 ? null : data;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetServiceConfiguations error:{e}");
                return null;
            }
        }

        // private async Task<ServiceConfiguationDto> GetRoundRobinProvider(ServiceConfiguationDto parentAccount)
        // {
        //     var list = await GetProviderConfigForTrans(parentAccount);
        //     if (list == null || !list.Any())
        //         return null;
        //     var roundRobinList = new RoundRobinList<ServiceConfiguationDto>(list);
        //     var item = roundRobinList.Next();
        //     return item;
        // }

        private async Task<List<ServiceConfiguationDto>> GetProviderConfigForTrans(ServiceConfiguationDto parentAccount)
        {
            try
            {
                return await _cacheManager.GetCache("ServiceConfiguations_RoundRobin")
                    .AsTyped<string, List<ServiceConfiguationDto>>().GetAsync(
                        $"ServiceConfiguations_RoundRobin:{parentAccount.ProviderCode}",
                        async () =>
                        {
                            var listChild = await _lookup_providerRepository.GetAll()
                                .Where(x => x.ParentProvider == parentAccount.ProviderCode &&
                                            x.ProviderStatus == CommonConst.ProviderStatus.Active)
                                .ToListAsync();
                            if (!listChild.Any())
                                return null;
                            var lst = listChild.ConvertTo<List<ServiceConfiguationDto>>();
                            lst.Insert(0, parentAccount);
                            return lst;
                        });
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<List<ServiceConfiguationDto>> GetServiceConfiguationsFromDb(
            GetServiceConfigurationInput input)
        {
            var limit = 3;
            var configValue = _appConfiguration["App:SwichProviderConfigValue"];
            if (configValue != null) limit = Convert.ToInt32(configValue);
            bool isApplySlowTrans = false;
            if (!string.IsNullOrEmpty(input.AccountCode) && input.IsCheckChannel)
            {
                // var partner = await _cacheManager.GetCache("PartnerInfo").GetAsync(
                //     $"PartnerInfo_{input.AccountCode}",
                //     async () =>
                //     {
                //         return await _userProfileRepository.GetAllIncluding(x => x.UserFk)
                //             .FirstOrDefaultAsync(x => x.UserFk.AccountCode == input.AccountCode);
                //     });
                var partner = await _userProfileRepository.GetAllIncluding(x => x.UserFk)
                    .FirstOrDefaultAsync(x => x.UserFk.AccountCode == input.AccountCode);
                if (partner != null)
                {
                    isApplySlowTrans = partner.IsApplySlowTrans ?? false;
                    if (partner.LimitChannel > 0)
                        limit = partner.LimitChannel;
                }
            }

            var user = await _lookup_userRepository.FirstOrDefaultAsync(p => p.AccountCode == input.AccountCode);

            //Check đóng mở theo dịch vụ
            var product = await _lookup_productRepository.FirstOrDefaultAsync(p => p.ProductCode == input.ProductCode);

            var category = await _lookup_categoryRepository.FirstOrDefaultAsync(p => p.Id == product.CategoryId);

            var listOpen = GetPartnerServiceConfigurations(user.Id, category.ServiceId ?? 0, category.Id);
            _logger.LogInformation(
                $"GetPartnerServiceConfigurations : {input.AccountCode}|{input.CategoryCode}|{input.ProductCode} => {listOpen.ToJson()}");
            if (listOpen.Count == 0)
                return new List<ServiceConfiguationDto>();

            var listOpenProviderCodes = _lookup_providerRepository.GetAll()
                .Where(p => listOpen.Select(x => x.ProviderId).Contains(p.Id)).Select(p => p.Code).ToList();
            _logger.LogInformation(
                $"GetPartnerServiceConfigurations : {input.AccountCode}|{input.CategoryCode}|{input.ProductCode} provider open => {listOpenProviderCodes.ToJson()}");

            var listView = new List<ServiceConfiguationDto>();

            #region 1.Có tài khoản, có sản phẩm

            var list = await _serviceConfigurationManager.GetServiceConfiguations(input.AccountCode,
                input.ServiceCode, input.CategoryCode, input.ProductCode, isApplySlowTrans, true);

            if (list != null && list.Count > 0)
            {
                list = list.Where(p => listOpenProviderCodes.Contains(p.ProviderCode)).ToList();
                listView.AddRange(list);
            }

            #endregion

            if (listView.Count >= limit)
                return listView.Count > limit
                    ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                    : listView;
            else
            {
                #region 2.Có tài khoản, không sản phẩm

                list = await _serviceConfigurationManager.GetServiceConfiguations(input.AccountCode,
                    input.ServiceCode, input.CategoryCode, isApplySlowTrans, true);


                if (list != null && list.Count > 0)
                {
                    list = list.Where(p => listOpenProviderCodes.Contains(p.ProviderCode)).ToList();

                    var providers = listView.Select(c => c.ProviderCode).ToList();
                    listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                }

                if (listView.Count >= limit)
                    return listView.Count > limit
                        ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                        : listView;

                #endregion

                #region 3.Không tài khoản , có sản phẩm

                list = await _serviceConfigurationManager.GetServiceConfiguations(string.Empty,
                    input.ServiceCode,
                    input.CategoryCode, input.ProductCode, isApplySlowTrans, true);

                if (list != null && list.Count > 0)
                {
                    list = list.Where(p => listOpenProviderCodes.Contains(p.ProviderCode)).ToList();
                    var providers = listView.Select(c => c.ProviderCode).ToList();
                    listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                }

                if (listView.Count >= limit)
                    return listView.Count > limit
                        ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                        : listView;

                #endregion

                #region 4.Không tài khoản , không sản phẩm

                list = await _serviceConfigurationManager.GetServiceConfiguations(string.Empty,
                    input.ServiceCode,
                    input.CategoryCode, isApplySlowTrans, true);

                if (list != null && list.Count > 0)
                {
                    list = list.Where(p => listOpenProviderCodes.Contains(p.ProviderCode)).ToList();
                    var providers = listView.Select(c => c.ProviderCode).ToList();
                    listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                }

                #endregion
            }

            return ConvertServiceConfiguationDto(listView, limit);
        }

        private List<PartnerServiceConfiguration> GetPartnerServiceConfigurations(long userId, int serviceId,
            int categoryId)
        {
            var list = _partnerServiceConfiguation.GetAll().Where(p =>
                p.ServiceId == serviceId && (p.UserId == userId || p.UserId == null) &&
                (p.CategoryId == categoryId || p.CategoryId == null)).ToList();

            #region //Close by User + category

            var providerClose =
                list.Where(p =>
                        p.UserId == userId && p.CategoryId == categoryId &&
                        p.Status != CommonConst.PartnerServiceConfigurationStatus.Active).Select(p => p.ProviderId)
                    .ToList();
            //remove all provider close by Close by User + category
            list.RemoveAll(p => providerClose.Contains(p.ProviderId));

            #endregion

            #region //Close by User 

            providerClose =
                list.Where(p =>
                        p.UserId == userId && p.CategoryId == null &&
                        p.Status != CommonConst.PartnerServiceConfigurationStatus.Active).Select(p => p.ProviderId)
                    .ToList();
            //remove 
            list.RemoveAll(p => providerClose.Contains(p.ProviderId) && p.CategoryId == null);

            #endregion

            #region close category

            providerClose =
                list.Where(p =>
                        p.UserId == null && p.CategoryId == categoryId &&
                        p.Status != CommonConst.PartnerServiceConfigurationStatus.Active).Select(p => p.ProviderId)
                    .ToList();
            //remove 
            list.RemoveAll(p => providerClose.Contains(p.ProviderId) && p.CategoryId == categoryId && p.UserId == null);

            #endregion

            #region close all

            providerClose =
                list.Where(p =>
                        p.UserId == null && p.CategoryId == null &&
                        p.Status != CommonConst.PartnerServiceConfigurationStatus.Active).Select(p => p.ProviderId)
                    .ToList();
            //remove 
            list.RemoveAll(p => providerClose.Contains(p.ProviderId) && p.CategoryId == null && p.UserId == null);

            #endregion

            return list;
        }

        public async Task<List<ServiceConfigurationProductLookupTableDto>> GetAllProductForTableDropdown()
        {
            return await _lookup_productRepository.GetAll()
                .Select(provider => new ServiceConfigurationProductLookupTableDto
                {
                    Id = provider.Id,
                    DisplayName = provider.ProductCode + "-" + provider.ProductName
                }).ToListAsync();
        }


        private List<ServiceConfiguationDto> ConvertServiceConfiguationDto(List<ServiceConfiguationDto> list, int limit)
        {
            var li = new List<ServiceConfiguationDto>();
            int index = 1;

            #region Nhom 1

            var li1 = list.Where(c => c.AccountCode != null && c.ProductCode != null).ToList();
            foreach (var x in li1.OrderBy(c => c.Priority))
            {
                li.Add(new ServiceConfiguationDto()
                {
                    Description = x.Description,
                    Priority = index,
                    Name = x.Name,
                    AccountCode = x.AccountCode,
                    CategoryCode = x.CategoryCode,
                    IsOpened = x.IsOpened,
                    ProviderCode = x.ProviderCode,
                    ProviderName = x.ProviderName,
                    ServiceCode = x.ServiceCode,
                    TransCodeConfig = x.TransCodeConfig,
                    IsSlowTrans = x.IsSlowTrans,
                    IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                    ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                    ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                    StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                    WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                    IsLastConfiguration = x.IsLastConfiguration,
                    IsRoundRobinAccount = x.IsRoundRobinAccount,
                    ParentProvider = x.ParentProvider,
                    MinBalance = x.MinBalance,
                    MinBalanceToDeposit = x.MinBalanceToDeposit,
                    DepositAmount = x.DepositAmount,
                    IsAutoDeposit = x.IsAutoDeposit,
                    AllowTopupReceiverType = x.AllowTopupReceiverType,
                    RateRunning = x.RateRunning,
                    WorkShortCode = x.WorkShortCode,
                });

                index = index + 1;
            }

            #endregion

            #region Nhom 2

            var li2 = list.Where(c => c.AccountCode != null && string.IsNullOrEmpty(c.ProductCode)).ToList();
            foreach (var x in li2.OrderBy(c => c.Priority))
            {
                li.Add(new ServiceConfiguationDto()
                {
                    Description = x.Description,
                    Priority = index,
                    Name = x.Name,
                    AccountCode = x.AccountCode,
                    CategoryCode = x.CategoryCode,
                    IsOpened = x.IsOpened,
                    ProviderCode = x.ProviderCode,
                    ProviderName = x.ProviderName,
                    ServiceCode = x.ServiceCode,
                    TransCodeConfig = x.TransCodeConfig,
                    IsSlowTrans = x.IsSlowTrans,

                    IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                    ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                    ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                    StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                    WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                    IsLastConfiguration = x.IsLastConfiguration,
                    IsRoundRobinAccount = x.IsRoundRobinAccount,
                    ParentProvider = x.ParentProvider,
                    MinBalance = x.MinBalance,
                    MinBalanceToDeposit = x.MinBalanceToDeposit,
                    DepositAmount = x.DepositAmount,
                    IsAutoDeposit = x.IsAutoDeposit,
                    AllowTopupReceiverType = x.AllowTopupReceiverType,
                    RateRunning = x.RateRunning,
                    WorkShortCode = x.WorkShortCode,
                });

                index = index + 1;
            }

            #endregion

            #region Nhom 3

            var li3 = list.Where(c => string.IsNullOrEmpty(c.AccountCode) && c.ProductCode != null).ToList();
            foreach (var x in li3.OrderBy(c => c.Priority))
            {
                li.Add(new ServiceConfiguationDto()
                {
                    Description = x.Description,
                    Priority = index,
                    Name = x.Name,
                    AccountCode = x.AccountCode,
                    CategoryCode = x.CategoryCode,
                    IsOpened = x.IsOpened,
                    ProviderCode = x.ProviderCode,
                    ProviderName = x.ProviderName,
                    ServiceCode = x.ServiceCode,
                    TransCodeConfig = x.TransCodeConfig,
                    IsSlowTrans = x.IsSlowTrans,
                    IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                    ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                    ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                    StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                    WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                    IsLastConfiguration = x.IsLastConfiguration,
                    IsRoundRobinAccount = x.IsRoundRobinAccount,
                    ParentProvider = x.ParentProvider,
                    MinBalance = x.MinBalance,
                    MinBalanceToDeposit = x.MinBalanceToDeposit,
                    DepositAmount = x.DepositAmount,
                    IsAutoDeposit = x.IsAutoDeposit,
                    AllowTopupReceiverType = x.AllowTopupReceiverType,
                    RateRunning = x.RateRunning,
                    WorkShortCode = x.WorkShortCode,
                });

                index = index + 1;
            }

            #endregion

            #region Nhom 4

            var li4 = list.Where(c => string.IsNullOrEmpty(c.AccountCode) && string.IsNullOrEmpty(c.ProductCode))
                .ToList();
            foreach (var x in li4.OrderBy(c => c.Priority))
            {
                li.Add(new ServiceConfiguationDto()
                {
                    Description = x.Description,
                    Priority = index,
                    Name = x.Name,
                    AccountCode = x.AccountCode,
                    CategoryCode = x.CategoryCode,
                    IsOpened = x.IsOpened,
                    ProviderCode = x.ProviderCode,
                    ProviderName = x.ProviderName,
                    ServiceCode = x.ServiceCode,
                    TransCodeConfig = x.TransCodeConfig,
                    IsSlowTrans = x.IsSlowTrans,
                    IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
                    ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
                    ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
                    StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
                    WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
                    IsLastConfiguration = x.IsLastConfiguration,
                    IsRoundRobinAccount = x.IsRoundRobinAccount,
                    ParentProvider = x.ParentProvider,
                    MinBalance = x.MinBalance,
                    MinBalanceToDeposit = x.MinBalanceToDeposit,
                    DepositAmount = x.DepositAmount,
                    IsAutoDeposit = x.IsAutoDeposit,
                    AllowTopupReceiverType = x.AllowTopupReceiverType,
                    RateRunning = x.RateRunning,
                    WorkShortCode = x.WorkShortCode,
                });

                index = index + 1;
            }

            #endregion

            return li.Count > limit ? li.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList() : li;
        }

        private async Task<bool> ClearCache(EntityDto<string> input)
        {
            try
            {
                var cache = _cacheManager.GetCache(input.Id);
                await cache.ClearAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ClearCache:{e}");
                return false;
            }
        }

        private async Task ClearCacheDelay(EntityDto<string> input)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                var cache = _cacheManager.GetCache(input.Id);
                await cache.ClearAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"ClearCacheDelay:{e}");
            }
        }


        public List<ServiceConfigurationStatusResponseLookupTableDto> GetStatusResponseForTableDropdown()
        {
            var listStatusDto = new List<ServiceConfigurationStatusResponseLookupTableDto>();
            listStatusDto.Add(new ServiceConfigurationStatusResponseLookupTableDto
            {
                ResponseCode = "1",
                ResponseMessage = L("TransactionIsSuccessful")
            });
            listStatusDto.Add(new ServiceConfigurationStatusResponseLookupTableDto
            {
                ResponseCode = "4000",
                ResponseMessage = L("TransactionIsReceived")
            });

            return listStatusDto;
        }
    }
}