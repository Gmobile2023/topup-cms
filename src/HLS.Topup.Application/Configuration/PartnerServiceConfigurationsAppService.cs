using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Common;
using HLS.Topup.Configuration.Exporting;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Products;
using HLS.Topup.Providers;
using HLS.Topup.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HLS.Topup.Configuration
{
    public class PartnerServiceConfigurationsAppService : TopupAppServiceBase, IPartnerServiceConfigurationsAppService
    {
        private readonly IRepository<PartnerServiceConfiguration> _serviceConfigurationRepository;
        private readonly IPartnerServiceConfigurationsExcelExporter _serviceConfigurationsExcelExporter;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly IRepository<Provider, int> _lookup_providerRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IRepository<Product, int> _lookup_productRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IPartnerServiceConfigurationManager _serviceConfigurationManager;
        private readonly ILogger<PartnerServiceConfigurationsAppService> _logger;
        private readonly ICacheManager _cacheManager;

        public PartnerServiceConfigurationsAppService(IRepository<PartnerServiceConfiguration> serviceConfigurationRepository,
            IPartnerServiceConfigurationsExcelExporter serviceConfigurationsExcelExporter,
            IRepository<Service, int> lookup_serviceRepository, IRepository<Provider, int> lookup_providerRepository,
            IRepository<Category, int> lookup_categoryRepository, IRepository<Product, int> lookup_productRepository,
            IRepository<User, long> lookup_userRepository, IPartnerServiceConfigurationManager serviceConfigurationManager,
            ILogger<PartnerServiceConfigurationsAppService> logger, ICacheManager cacheManager,
            IWebHostEnvironment hostingEnvironment, IRepository<UserProfile> userProfileRepository)
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
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations)]
        public async Task<PagedResultDto<GetPartnerServiceConfigurationForViewDto>> GetAll(
            GetAllPartnerServiceConfigurationsInput input)
        {
            input.ServiceIds = input.ServiceIds.Where(c => c > 0).Select(c => c).ToList();
            input.CategoryIds = input.CategoryIds.Where(c => c > 0).Select(c => c).ToList();
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.PartnerServiceConfigurationStatus)input.StatusFilter
                : default;
            var filteredServiceConfigurations = _serviceConfigurationRepository.GetAll()
                .Include(e => e.ServiceFk)
                .Include(e => e.ProviderFk)
                .Include(e => e.CategoryFk)
                .Include(e => e.UserFk)
                .WhereIf(input.UserId != null, x => x.UserId == input.UserId)
                .WhereIf(input.ProviderId != null, x => x.ProviderId == input.ProviderId)
                .WhereIf(input.ServiceId != null, x => x.ServiceId == input.ServiceId)
                .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
                .WhereIf(input.ServiceIds != null && input.ServiceIds.Count > 0, x => input.ServiceIds.Contains(x.ServiceId ?? 0))
                .WhereIf(input.CategoryIds != null && input.CategoryIds.Count > 0, x => input.CategoryIds.Contains(x.CategoryId ?? 0))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),
                    e => e.Name.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                         e.UserFk.Name.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                         e.UserFk.Surname.ToUpper().Contains(input.NameFilter.ToUpper()) ||
                         e.ProviderFk.Code.ToUpper().Contains(input.NameFilter.ToUpper()))
                //.WhereIf((int)input.Status > -1,e => input.Status == e.Status)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                    e => e.ServiceFk != null && e.ServiceFk.ServicesName == input.ServiceServicesNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderNameFilter),
                    e => e.ProviderFk != null && e.ProviderFk.Name == input.ProviderNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryName == input.CategoryCategoryNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredServiceConfigurations = filteredServiceConfigurations
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.ProviderFk.Code)
                .PageBy(input);

            var serviceConfigurations = from o in pagedAndFilteredServiceConfigurations
                                        join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()
                                        join o2 in _lookup_providerRepository.GetAll() on o.ProviderId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()
                                        join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                                        from s3 in j3.DefaultIfEmpty()
                                        join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                                        from s5 in j5.DefaultIfEmpty()
                                        select new GetPartnerServiceConfigurationForViewDto()
                                        {
                                            ServiceConfiguration = new PartnerServiceConfigurationDto
                                            {
                                                Name = o.Name,
                                                Status = o.Status,
                                                Id = o.Id,
                                                Description = o.Description
                                            },
                                            ServiceServicesName = s1 == null || s1.ServicesName == null ? "" : s1.ServicesName.ToString(),
                                            ProviderName = s2 == null || s2.Name == null ? "" : s2.Code + "-" + s2.Name.ToString(),
                                            CategoryCategoryName = s3 == null || s3.CategoryName == null ? "" : s3.CategoryName.ToString(),
                                            UserName = s5 == null || s5.Name == null ? "" : s5.AccountCode + "-" + s5.Name.ToString(),
                                            AgentType = s5 == null ? "" : s5.AgentType.ToString(),
                                        };

            var totalCount = await filteredServiceConfigurations.CountAsync();

            return new PagedResultDto<GetPartnerServiceConfigurationForViewDto>(
                totalCount,
                await serviceConfigurations.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<GetPartnerServiceConfigurationForViewDto> GetPartnerServiceConfigurationForView(int id)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.GetAsync(id);

            var output = new GetPartnerServiceConfigurationForViewDto
            { ServiceConfiguration = ObjectMapper.Map<PartnerServiceConfigurationDto>(serviceConfiguration) };

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

            if (output.ServiceConfiguration.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long)output.ServiceConfiguration.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ServiceConfigurations_Edit)]
        public async Task<GetPartnerServiceConfigurationForEditOutput> GetPartnerServiceConfigurationForEdit(EntityDto input)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPartnerServiceConfigurationForEditOutput
            { ServiceConfiguration = ObjectMapper.Map<CreateOrEditPartnerServiceConfigurationDto>(serviceConfiguration) };

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

            if (output.ServiceConfiguration.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long)output.ServiceConfiguration.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPartnerServiceConfigurationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations_Create)]
        protected virtual async Task Create(CreateOrEditPartnerServiceConfigurationDto input)
        {
            var serviceConfiguration = ObjectMapper.Map<PartnerServiceConfiguration>(input);
            if (AbpSession.TenantId != null)
            {
                serviceConfiguration.TenantId = (int?)AbpSession.TenantId;
            }

            await _serviceConfigurationRepository.InsertAsync(serviceConfiguration);
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations_Edit)]
        protected virtual async Task Update(CreateOrEditPartnerServiceConfigurationDto input)
        {
            var serviceConfiguration = await _serviceConfigurationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, serviceConfiguration);
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _serviceConfigurationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPartnerServiceConfigurationsToExcel(GetAllPartnerServiceConfigurationsForExcelInput input)
        {
            input.ServiceIds = input.ServiceIds.Where(c => c > 0).Select(c => c).ToList();
            input.CategoryIds = input.CategoryIds.Where(c => c > 0).Select(c => c).ToList();
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.PartnerServiceConfigurationStatus)input.StatusFilter
                : default;
            var filteredServiceConfigurations = _serviceConfigurationRepository.GetAll()
                .Include(e => e.ServiceFk)
                .Include(e => e.ProviderFk)
                .Include(e => e.CategoryFk)
                .Include(e => e.UserFk)
                .WhereIf(input.UserId != null, x => x.UserId == input.UserId)
                .WhereIf(input.ProviderId != null, x => x.ProviderId == input.ProviderId)
                .WhereIf(input.ServiceId != null, x => x.ServiceId == input.ServiceId)
                .WhereIf(input.CategoryId != null, x => x.CategoryId == input.CategoryId)
                .WhereIf(input.ServiceIds != null && input.ServiceIds.Count > 0, x => input.ServiceIds.Contains(x.ServiceId ?? 0))
                .WhereIf(input.CategoryIds != null && input.CategoryIds.Count > 0, x => input.CategoryIds.Contains(x.CategoryId ?? 0))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                //.WhereIf((int)input.Status > -1,e => input.Status == e.Status)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                    e => e.ServiceFk != null && e.ServiceFk.ServicesName == input.ServiceServicesNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProviderNameFilter),
                    e => e.ProviderFk != null && e.ProviderFk.Name == input.ProviderNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryName == input.CategoryCategoryNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredServiceConfigurations
                         join o1 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _lookup_providerRepository.GetAll() on o.ProviderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _lookup_categoryRepository.GetAll() on o.CategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o5 in _lookup_userRepository.GetAll() on o.UserId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         select new GetPartnerServiceConfigurationForViewDto()
                         {
                             ServiceConfiguration = new PartnerServiceConfigurationDto
                             {
                                 Name = o.Name,
                                 Status = o.Status,
                                 Id = o.Id,
                                 Description = o.Description
                             },
                             ServiceServicesName = s1 == null || s1.ServicesName == null ? "" : s1.ServicesName.ToString(),
                             ProviderName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CategoryCategoryName = s3 == null || s3.CategoryName == null ? "" : s3.CategoryName.ToString(),
                             UserName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             AgentType = s5 == null ? "" : s5.AgentType.ToString(),
                         });

            var serviceConfigurationListDtos = await query.ToListAsync();

            return _serviceConfigurationsExcelExporter.ExportToFile(serviceConfigurationListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<List<PartnerServiceConfigurationServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookup_serviceRepository.GetAll()
                .Select(service => new PartnerServiceConfigurationServiceLookupTableDto
                {
                    Id = service.Id,
                    DisplayName = service == null || service.ServicesName == null ? "" : service.ServicesName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<List<PartnerServiceConfigurationProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookup_providerRepository.GetAll()
                .Select(provider => new PartnerServiceConfigurationProviderLookupTableDto
                {
                    Id = provider.Id,
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<List<PartnerServiceConfigurationCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAll()
                .Select(category => new PartnerServiceConfigurationCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<PagedResultDto<PartnerServiceConfigurationProductLookupTableDto>> GetAllProductForLookupTable(
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

            var lookupTableDtoList = new List<PartnerServiceConfigurationProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new PartnerServiceConfigurationProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.ProductName?.ToString()
                });
            }

            return new PagedResultDto<PartnerServiceConfigurationProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PartnerServiceConfigurations)]
        public async Task<PagedResultDto<PartnerServiceConfigurationUserLookupTableDto>> GetAllUserForLookupTable(
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

            var lookupTableDtoList = new List<PartnerServiceConfigurationUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new PartnerServiceConfigurationUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName?.ToString(),
                    AccountCode = user.AccountCode?.ToString(),
                    Phone = user.PhoneNumber?.ToString(),
                });
            }

            return new PagedResultDto<PartnerServiceConfigurationUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<PartnerServiceConfiguationDto>> GetPartnerServiceConfiguations(GetPartnerServiceConfigurationInput input)
        {
            try
            {
                _logger.LogInformation($"GetPartnerServiceConfiguations request: {input.ToJson()}");
                // if (input.AccountCode == null)
                //     return null;
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
                    var partner=await _userProfileRepository.GetAllIncluding(x => x.UserFk).FirstOrDefaultAsync(x => x.UserFk.AccountCode == input.AccountCode);
                    if (partner != null)
                    {
                        isApplySlowTrans = partner.IsApplySlowTrans ?? false;
                        if (partner.LimitChannel > 0)
                            limit = partner.LimitChannel;
                    }
                }

                return await _cacheManager.GetCache("PartnerServiceConfiguations").AsTyped<string, List<PartnerServiceConfiguationDto>>().GetAsync(
                    $"PartnerServiceConfiguations_{input.AccountCode}_{input.ServiceCode}_{input.CategoryCode}",
                    async () =>
                    {
                        List<PartnerServiceConfiguationDto> listView = new List<PartnerServiceConfiguationDto>();

                        #region 1.Có tài khoản

                        var list = await _serviceConfigurationManager.GetPartnerServiceConfiguations(input.AccountCode,
                            input.ServiceCode, input.CategoryCode);
                        if (list != null && list.Count > 0)
                            listView.AddRange(list);

                        #endregion 1.Có tài khoản

                        //if (listView.Count >= limit)
                        //    return listView.Count > limit
                        //        ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                        //        : listView;
                        //else
                        //{
                        //    #region 2.Có tài khoản, không sản phẩm

                        //    list = await _serviceConfigurationManager.GetPartnerServiceConfiguations(input.AccountCode,
                        //        input.ServiceCode, input.CategoryCode, isApplySlowTrans);
                        //    if (list != null && list.Count > 0)
                        //    {
                        //        var providers = listView.Select(c => c.ProviderCode).ToList();
                        //        listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                        //    }

                        //    if (listView.Count >= limit)
                        //        return listView.Count > limit
                        //            ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                        //            : listView;

                        //    #endregion 2.Có tài khoản, không sản phẩm

                        //    #region 3.Không tài khoản , có sản phẩm

                        //    list = await _serviceConfigurationManager.GetServiceConfiguations(string.Empty,
                        //        input.ServiceCode,
                        //        input.CategoryCode, input.ProductCode, isApplySlowTrans);

                        //    if (list != null && list.Count > 0)
                        //    {
                        //        var providers = listView.Select(c => c.ProviderCode).ToList();
                        //        listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                        //    }

                        //    if (listView.Count >= limit)
                        //        return listView.Count > limit
                        //            ? listView.OrderBy(c => c.Priority).Skip(0).Take(limit).ToList()
                        //            : listView;

                        //    #endregion 3.Không tài khoản , có sản phẩm

                        //    #region 4.Không tài khoản , không sản phẩm

                        //    list = await _serviceConfigurationManager.GetServiceConfiguations(string.Empty,
                        //        input.ServiceCode,
                        //        input.CategoryCode, isApplySlowTrans);
                        //    if (list != null && list.Count > 0)
                        //    {
                        //        var providers = listView.Select(c => c.ProviderCode).ToList();
                        //        listView.AddRange(list.Where(c => !providers.Contains(c.ProviderCode)));
                        //    }

                        //    #endregion 4.Không tài khoản , không sản phẩm
                        //}

                        return ConvertPartnerServiceConfiguationDto(listView, limit);
                    });
            }
            catch (Exception e)
            {
                _logger.LogError($"GetServiceConfiguations error:{e}");
                return null;
            }
        }

        private List<PartnerServiceConfiguationDto> ConvertPartnerServiceConfiguationDto(List<PartnerServiceConfiguationDto> list, int limit)
        {
            var li = new List<PartnerServiceConfiguationDto>();
            int index = 1;

            //#region Nhom 1

            //var li1 = list.Where(c => c.AccountCode != null && c.ProductCode != null).ToList();
            //foreach (var x in li1.OrderBy(c => c.Priority))
            //{
            //    li.Add(new ServiceConfiguationDto()
            //    {
            //        Description = x.Description,
            //        Priority = index,
            //        Name = x.Name,
            //        AccountCode = x.AccountCode,
            //        CategoryCode = x.CategoryCode,
            //        IsOpened = x.IsOpened,
            //        ProviderCode = x.ProviderCode,
            //        ProviderName = x.ProviderName,
            //        ServiceCode = x.ServiceCode,
            //        TransCodeConfig = x.TransCodeConfig,
            //        IsSlowTrans = x.IsSlowTrans,

            //        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
            //        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
            //        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
            //        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
            //        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
            //    });

            //    index = index + 1;
            //}

            //#endregion Nhom 1

            //#region Nhom 2

            //var li2 = list.Where(c => c.AccountCode != null && string.IsNullOrEmpty(c.ProductCode)).ToList();
            //foreach (var x in li2.OrderBy(c => c.Priority))
            //{
            //    li.Add(new ServiceConfiguationDto()
            //    {
            //        Description = x.Description,
            //        Priority = index,
            //        Name = x.Name,
            //        AccountCode = x.AccountCode,
            //        CategoryCode = x.CategoryCode,
            //        IsOpened = x.IsOpened,
            //        ProviderCode = x.ProviderCode,
            //        ProviderName = x.ProviderName,
            //        ServiceCode = x.ServiceCode,
            //        TransCodeConfig = x.TransCodeConfig,
            //        IsSlowTrans = x.IsSlowTrans,

            //        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
            //        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
            //        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
            //        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
            //        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
            //    });

            //    index = index + 1;
            //}

            //#endregion Nhom 2

            //#region Nhom 3

            //var li3 = list.Where(c => string.IsNullOrEmpty(c.AccountCode) && c.ProductCode != null).ToList();
            //foreach (var x in li3.OrderBy(c => c.Priority))
            //{
            //    li.Add(new ServiceConfiguationDto()
            //    {
            //        Description = x.Description,
            //        Priority = index,
            //        Name = x.Name,
            //        AccountCode = x.AccountCode,
            //        CategoryCode = x.CategoryCode,
            //        IsOpened = x.IsOpened,
            //        ProviderCode = x.ProviderCode,
            //        ProviderName = x.ProviderName,
            //        ServiceCode = x.ServiceCode,
            //        TransCodeConfig = x.TransCodeConfig,
            //        IsSlowTrans = x.IsSlowTrans,
            //        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
            //        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
            //        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
            //        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
            //        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
            //    });

            //    index = index + 1;
            //}

            //#endregion Nhom 3

            //#region Nhom 4

            //var li4 = list.Where(c => string.IsNullOrEmpty(c.AccountCode) && string.IsNullOrEmpty(c.ProductCode))
            //    .ToList();
            //foreach (var x in li4.OrderBy(c => c.Priority))
            //{
            //    li.Add(new ServiceConfiguationDto()
            //    {
            //        Description = x.Description,
            //        Priority = index,
            //        Name = x.Name,
            //        AccountCode = x.AccountCode,
            //        CategoryCode = x.CategoryCode,
            //        IsOpened = x.IsOpened,
            //        ProviderCode = x.ProviderCode,
            //        ProviderName = x.ProviderName,
            //        ServiceCode = x.ServiceCode,
            //        TransCodeConfig = x.TransCodeConfig,
            //        IsSlowTrans = x.IsSlowTrans,
            //        IsEnableResponseWhenJustReceived = x.IsEnableResponseWhenJustReceived,
            //        ProviderMaxWaitingTimeout = x.ProviderMaxWaitingTimeout,
            //        ProviderSetTransactionTimeout = x.ProviderSetTransactionTimeout,
            //        StatusResponseWhenJustReceived = x.StatusResponseWhenJustReceived,
            //        WaitingTimeResponseWhenJustReceived = x.WaitingTimeResponseWhenJustReceived,
            //    });

            //    index = index + 1;
            //}

            //#endregion Nhom 4

            return li.Count > limit ? li.OrderBy(c => c.Name).Skip(0).Take(limit).ToList() : li;
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

        public List<PartnerServiceConfigurationStatusResponseLookupTableDto> GetStatusResponseForTableDropdown()
        {
            var listStatusDto = new List<PartnerServiceConfigurationStatusResponseLookupTableDto>();
            listStatusDto.Add(new PartnerServiceConfigurationStatusResponseLookupTableDto
            {
                ResponseCode = "01",
                ResponseMessage = L("TransactionIsSuccessful")
            });
            listStatusDto.Add(new PartnerServiceConfigurationStatusResponseLookupTableDto
            {
                ResponseCode = "4000",
                ResponseMessage = L("TransactionIsReceived")
            });

            return listStatusDto;
        }
    }
}