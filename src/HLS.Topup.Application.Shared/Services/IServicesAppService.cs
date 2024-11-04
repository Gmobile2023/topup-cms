using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Dto;


namespace HLS.Topup.Services
{
    public interface IServicesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetServiceForViewDto>> GetAll(GetAllServicesInput input);

        Task<GetServiceForViewDto> GetServiceForView(int id);

		Task<GetServiceForEditOutput> GetServiceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditServiceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetServicesToExcel(GetAllServicesForExcelInput input);

		
    }
}