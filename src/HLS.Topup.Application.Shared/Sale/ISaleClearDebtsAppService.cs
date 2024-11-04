using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.Sale
{
    public interface ISaleClearDebtsAppService : IApplicationService 
    {
        Task<PagedResultDtoReport<GetSaleClearDebtForViewDto>> GetAll(GetAllSaleClearDebtsInput input);

        Task<GetSaleClearDebtForViewDto> GetSaleClearDebtForView(int id);

		Task<GetSaleClearDebtForEditOutput> GetSaleClearDebtForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSaleClearDebtDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSaleClearDebtsToExcel(GetAllSaleClearDebtsForExcelInput input);

		
		Task<PagedResultDto<SaleClearDebtUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<List<SaleClearDebtBankLookupTableDto>> GetAllBankForTableDropdown();

		Task Cancel(string transcode);


	}
}