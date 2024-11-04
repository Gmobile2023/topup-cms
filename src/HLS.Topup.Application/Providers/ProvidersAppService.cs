using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Providers.Exporting;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Abp.UI;
using HLS.Topup.Configuration;
using HLS.Topup.RequestDtos;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.SystemManagerment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Providers
{
    [AbpAuthorize(AppPermissions.Pages_Providers)]
    public class ProvidersAppService : TopupAppServiceBase, IProvidersAppService
    {
        private readonly IRepository<Provider> _providerRepository;
        private readonly IProvidersExcelExporter _providersExcelExporter;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<ProvidersAppService> _logger;
        private readonly IAccountConfigurationManager _accountConfigurationManager;
        private readonly ISystemManager _systemManager;

        public ProvidersAppService(IRepository<Provider> providerRepository,
            IProvidersExcelExporter providersExcelExporter, ICacheManager cacheManager,
            ILogger<ProvidersAppService> logger, IAccountConfigurationManager accountConfigurationManager,
            ISystemManager systemManager)
        {
            _providerRepository = providerRepository;
            _providersExcelExporter = providersExcelExporter;
            _cacheManager = cacheManager;
            _logger = logger;
            _accountConfigurationManager = accountConfigurationManager;
            _systemManager = systemManager;
        }

        public async Task<PagedResultDto<GetProviderForViewDto>> GetAll(GetAllProvidersInput input)
        {
            var providerTypeFilter = input.ProviderTypeFilter.HasValue
                ? (CommonConst.ProviderType)input.ProviderTypeFilter
                : default;
            var providerStatusFilter = input.ProviderStatusFilter.HasValue
                ? (CommonConst.ProviderStatus)input.ProviderStatusFilter
                : default;

            var filteredProviders = _providerRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Images.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ParentProviderFilter),
                    e => e.ParentProvider == input.ParentProviderFilter)
                .WhereIf(input.ProviderTypeFilter.HasValue && input.ProviderTypeFilter > -1,
                    e => e.ProviderType == providerTypeFilter)
                .WhereIf(input.ProviderStatusFilter.HasValue && input.ProviderStatusFilter > -1,
                    e => e.ProviderStatus == providerStatusFilter);

            var pagedAndFilteredProviders = filteredProviders
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var providers = from o in pagedAndFilteredProviders
                select new GetProviderForViewDto()
                {
                    Provider = new ProviderDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        PhoneNumber = o.PhoneNumber,
                        ProviderType = o.ProviderType,
                        ProviderStatus = o.ProviderStatus,
                        TransCodeConfig = o.TransCodeConfig,
                        ParentProvider = o.ParentProvider,
                        Id = o.Id
                    }
                };

            var totalCount = await filteredProviders.CountAsync();

            return new PagedResultDto<GetProviderForViewDto>(
                totalCount,
                await providers.ToListAsync()
            );
        }

        public async Task<GetProviderForViewDto> GetProviderForView(int id)
        {
            var provider = await _providerRepository.GetAsync(id);

            var output = new GetProviderForViewDto { Provider = ObjectMapper.Map<ProviderDto>(provider) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_Edit)]
        public async Task<GetProviderForEditOutput> GetProviderForEdit(EntityDto input)
        {
            var provider = await _providerRepository.FirstOrDefaultAsync(input.Id);
            var getProvider = await _accountConfigurationManager.GetProviderInfo(new ProviderInfoGetRequest
            {
                ProviderCode = provider.Code
            });

            var output = new GetProviderForEditOutput
            {
                Provider = ObjectMapper.Map<CreateOrEditProviderDto>(provider),
                ProviderUpdate = getProvider
            };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProviderDto input)
        {
            if (string.IsNullOrEmpty(input.ParentProvider))
                input.ParentProvider = null;
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_Create)]
        protected virtual async Task Create(CreateOrEditProviderDto input)
        {
            var provider = ObjectMapper.Map<Provider>(input);
            if (AbpSession.TenantId != null)
            {
                provider.TenantId = (int?)AbpSession.TenantId;
            }

            await _providerRepository.InsertAsync(provider);
            var request = input.ProviderUpdateInfo.ConvertTo<ProviderInfoCreateRequest>();
            request.ProviderCode = provider.Code;
            request.ParentProvider = provider.ParentProvider;
            var create = await _accountConfigurationManager.ProviderInfoCreateRequest(request);
            _logger.LogInformation($"ProviderInfoCreateRequest return:{create.ToJson()}");
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_Edit)]
        protected virtual async Task Update(CreateOrEditProviderDto input)
        {
            var provider = await _providerRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, provider);
            await _providerRepository.UpdateAsync(provider);
            var updateRequest = input.ProviderUpdateInfo.ConvertTo<ProviderInfoUpdateRequest>();
            updateRequest.ProviderCode = provider.Code;
            updateRequest.ParentProvider = provider.ParentProvider;
            var update = await _accountConfigurationManager.ProviderInfoUpdateRequest(updateRequest);
            _logger.LogInformation($"ProviderInfoUpdateRequest return:{update.ToJson()}");
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_LockUnLock)]
        public virtual async Task Lock(EntityDto<string> input)
        {
            if (!await _systemManager.LockProvider(input.Id, 30))
                throw new UserFriendlyException("Thao tác không thành công");
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_LockUnLock)]
        public virtual async Task UnLock(EntityDto<string> input)
        {
            if (!await _systemManager.UnLockProvider(input.Id))
                throw new UserFriendlyException("Thao tác không thành công");
        }

        [AbpAuthorize(AppPermissions.Pages_Providers_Delete)]
        public async Task Delete(EntityDto input)
        {
            try
            {
                await _providerRepository.DeleteAsync(input.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FileDto> GetProvidersToExcel(GetAllProvidersForExcelInput input)
        {
            var providerTypeFilter = input.ProviderTypeFilter.HasValue
                ? (CommonConst.ProviderType)input.ProviderTypeFilter
                : default;
            var providerStatusFilter = input.ProviderStatusFilter.HasValue
                ? (CommonConst.ProviderStatus)input.ProviderStatusFilter
                : default;

            var filteredProviders = _providerRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Images.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) ||
                         e.EmailAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.ProviderTypeFilter.HasValue && input.ProviderTypeFilter > -1,
                    e => e.ProviderType == providerTypeFilter)
                .WhereIf(input.ProviderStatusFilter.HasValue && input.ProviderStatusFilter > -1,
                    e => e.ProviderStatus == providerStatusFilter);

            var query = (from o in filteredProviders
                select new GetProviderForViewDto()
                {
                    Provider = new ProviderDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        PhoneNumber = o.PhoneNumber,
                        ProviderType = o.ProviderType,
                        ProviderStatus = o.ProviderStatus,
                        Id = o.Id
                    }
                });


            var providerListDtos = await query.ToListAsync();

            return _providersExcelExporter.ExportToFile(providerListDtos);
        }

        public async Task<List<CommonLookupTableDto>> GetAllProvider()
        {
            return await _providerRepository.GetAll()
                .Select(p => new CommonLookupTableDto
                {
                    Id = p.Code,
                    DisplayName = p.Name
                }).ToListAsync();
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
    }
}