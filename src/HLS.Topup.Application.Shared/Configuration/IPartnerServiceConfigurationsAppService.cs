using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HLS.Topup.Configuration
{
    public interface IPartnerServiceConfigurationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPartnerServiceConfigurationForViewDto>> GetAll(GetAllPartnerServiceConfigurationsInput input);

        Task<GetPartnerServiceConfigurationForViewDto> GetPartnerServiceConfigurationForView(int id);

        Task<GetPartnerServiceConfigurationForEditOutput> GetPartnerServiceConfigurationForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPartnerServiceConfigurationDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPartnerServiceConfigurationsToExcel(GetAllPartnerServiceConfigurationsForExcelInput input);

        Task<List<PartnerServiceConfigurationServiceLookupTableDto>> GetAllServiceForTableDropdown();

        Task<List<PartnerServiceConfigurationProviderLookupTableDto>> GetAllProviderForTableDropdown();

        Task<List<PartnerServiceConfigurationCategoryLookupTableDto>> GetAllCategoryForTableDropdown();

        Task<PagedResultDto<PartnerServiceConfigurationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<PartnerServiceConfiguationDto>> GetPartnerServiceConfiguations(GetPartnerServiceConfigurationInput input);

        List<PartnerServiceConfigurationStatusResponseLookupTableDto> GetStatusResponseForTableDropdown();
    }
}