using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;


namespace HLS.Topup.Sale
{
    public interface ISaleLimitDebtsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSaleLimitDebtForViewDto>> GetAll(GetAllSaleLimitDebtsInput input);

        Task<GetSaleLimitDebtForViewDto> GetSaleLimitDebtForView(int id);

		Task<GetSaleLimitDebtForEditOutput> GetSaleLimitDebtForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSaleLimitDebtDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSaleLimitDebtsToExcel(GetAllSaleLimitDebtsForExcelInput input);

		
		Task<PagedResultDto<SaleLimitDebtUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}