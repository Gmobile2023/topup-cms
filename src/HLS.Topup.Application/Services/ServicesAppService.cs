using HLS.Topup.Services;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Services.Exporting;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.RequestDtos;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Services
{
    [AbpAuthorize(AppPermissions.Pages_Services)]
    public class ServicesAppService : TopupAppServiceBase, IServicesAppService
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IServicesExcelExporter _servicesExcelExporter;
        private readonly IAccountConfigurationManager _accountConfigurationManager;


        public ServicesAppService(IRepository<Service> serviceRepository, IServicesExcelExporter servicesExcelExporter,
            IAccountConfigurationManager accountConfigurationManager)
        {
            _serviceRepository = serviceRepository;
            _servicesExcelExporter = servicesExcelExporter;
            _accountConfigurationManager = accountConfigurationManager;
        }

        public async Task<PagedResultDto<GetServiceForViewDto>> GetAll(GetAllServicesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (ServiceStatus) input.StatusFilter
                : default;

            var filteredServices = _serviceRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.ServiceCode.Contains(input.Filter) || e.ServicesName.Contains(input.Filter) ||
                         e.ServiceConfig.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceCodeFilter),
                    e => e.ServiceCode.Contains(input.ServiceCodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServicesNameFilter),
                    e => e.ServicesName.Contains(input.ServicesNameFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var pagedAndFilteredServices = filteredServices
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.ServicesName)
                .PageBy(input);

            var services = from o in pagedAndFilteredServices
                select new GetServiceForViewDto()
                {
                    Service = new ServiceDto
                    {
                        ServiceCode = o.ServiceCode,
                        ServicesName = o.ServicesName,
                        Status = o.Status,
                        Order = o.Order,
                        Id = o.Id
                    }
                };

            var totalCount = await filteredServices.CountAsync();

            return new PagedResultDto<GetServiceForViewDto>(
                totalCount,
                await services.ToListAsync()
            );
        }

        public async Task<GetServiceForViewDto> GetServiceForView(int id)
        {
            var service = await _serviceRepository.GetAsync(id);

            var output = new GetServiceForViewDto {Service = ObjectMapper.Map<ServiceDto>(service)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Services_Edit)]
        public async Task<GetServiceForEditOutput> GetServiceForEdit(EntityDto input)
        {
            var service = await _serviceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetServiceForEditOutput {Service = ObjectMapper.Map<CreateOrEditServiceDto>(service)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditServiceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Services_Create)]
        protected virtual async Task Create(CreateOrEditServiceDto input)
        {
            var service = ObjectMapper.Map<Service>(input);
            if (AbpSession.TenantId != null)
            {
                service.TenantId = (int?) AbpSession.TenantId;
            }
            var updateGw = await _accountConfigurationManager.CreateOrUpdateServiceConfig(
                new CreateOrUpdateServiceRequest
                {
                    IsActive = input.Status == ServiceStatus.Active,
                    Description = input.Description,
                    ServiceCode = input.ServiceCode,
                    ServiceName = input.ServicesName
                });
            if (updateGw.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                throw new UserFriendlyException("Thêm mới dịch vụ không thành công");
            await _serviceRepository.InsertAsync(service);
        }

        [AbpAuthorize(AppPermissions.Pages_Services_Edit)]
        protected virtual async Task Update(CreateOrEditServiceDto input)
        {
            var updateGw = await _accountConfigurationManager.CreateOrUpdateServiceConfig(
                new CreateOrUpdateServiceRequest
                {
                    IsActive = input.Status == ServiceStatus.Active,
                    Description = input.Description,
                    ServiceCode = input.ServiceCode,
                    ServiceName = input.ServicesName
                });
            if (updateGw.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                throw new UserFriendlyException("Cập nhật cấu hình dịch vụ không thành công");

            var service = await _serviceRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, service);
        }

        [AbpAuthorize(AppPermissions.Pages_Services_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _serviceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetServicesToExcel(GetAllServicesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (ServiceStatus) input.StatusFilter
                : default;

            var filteredServices = _serviceRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.ServiceCode.Contains(input.Filter) || e.ServicesName.Contains(input.Filter) ||
                         e.ServiceConfig.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceCodeFilter),
                    e => e.ServiceCode == input.ServiceCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServicesNameFilter),
                    e => e.ServicesName == input.ServicesNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var query = (from o in filteredServices
                select new GetServiceForViewDto()
                {
                    Service = new ServiceDto
                    {
                        ServiceCode = o.ServiceCode,
                        ServicesName = o.ServicesName,
                        Status = o.Status,
                        Order = o.Order,
                        Id = o.Id
                    }
                });


            var serviceListDtos = await query.ToListAsync();

            return _servicesExcelExporter.ExportToFile(serviceListDtos);
        }
    }
}
