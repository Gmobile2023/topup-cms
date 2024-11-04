using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.Address
{
    public interface ICitiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input);

        Task<GetCityForViewDto> GetCityForView(int id);

		Task<GetCityForEditOutput> GetCityForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCityDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input);

		
		Task<List<CityCountryLookupTableDto>> GetAllCountryForTableDropdown();
		
    }
}