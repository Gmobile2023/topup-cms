using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.Address
{
    public interface IDistrictsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input);

        Task<GetDistrictForViewDto> GetDistrictForView(int id);

		Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDistrictDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input);
		
		Task<List<DistrictCityLookupTableDto>> GetAllCityForTableDropdown();

	    Task<List<DistrictCityLookupTableDto>> GetDistrictByCity(int cityId);

    }
}