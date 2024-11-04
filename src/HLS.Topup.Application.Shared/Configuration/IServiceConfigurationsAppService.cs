using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using HLS.Topup.Dtos.Configuration;


namespace HLS.Topup.Configuration
{
    public interface IServiceConfigurationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetServiceConfigurationForViewDto>> GetAll(GetAllServiceConfigurationsInput input);

        Task<GetServiceConfigurationForViewDto> GetServiceConfigurationForView(int id);

		Task<GetServiceConfigurationForEditOutput> GetServiceConfigurationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServiceConfigurationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServiceConfigurationsToExcel(GetAllServiceConfigurationsForExcelInput input);


		Task<List<ServiceConfigurationServiceLookupTableDto>> GetAllServiceForTableDropdown();

		Task<List<ServiceConfigurationProviderLookupTableDto>> GetAllProviderForTableDropdown();

		Task<List<ServiceConfigurationCategoryLookupTableDto>> GetAllCategoryForTableDropdown();
		Task<List<ServiceConfigurationProductLookupTableDto>> GetAllProductForTableDropdown();

		Task<PagedResultDto<ServiceConfigurationProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<ServiceConfigurationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

		Task<List<ServiceConfiguationDto>> GetServiceConfiguations(GetServiceConfigurationInput input);

		List<ServiceConfigurationStatusResponseLookupTableDto> GetStatusResponseForTableDropdown();
	}
}
