using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;


namespace HLS.Topup.Address
{
    public interface ICountriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input);

        Task<GetCountryForViewDto> GetCountryForView(int id);

		Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCountryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input);

		
    }
}