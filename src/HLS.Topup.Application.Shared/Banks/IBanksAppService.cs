using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Dto;


namespace HLS.Topup.Banks
{
    public interface IBanksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankForViewDto>> GetAll(GetAllBanksInput input);

        Task<GetBankForViewDto> GetBankForView(int id);

		Task<GetBankForEditOutput> GetBankForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBankDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBanksToExcel(GetAllBanksForExcelInput input);

		
    }
}