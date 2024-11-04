using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
namespace HLS.Topup.Deposits
{
    public interface IDepositsAppService : IApplicationService
    {
        Task<PagedResultDtoReport<GetDepositForViewDto>> GetAll(GetAllDepositsInput input);

        Task<GetDepositForViewDto> GetDepositForView(int id);

		Task<GetDepositForEditOutput> GetDepositForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepositDto input);

		Task CreateOrEditDebtSale(CreateOrEditDepositDto input);

		Task Delete(EntityDto input);
		Task Approval(ApprovalDepositDto request);
		Task Cancel(CancelDepositDto request);

		Task<FileDto> GetDepositsToExcel(GetAllDepositsForExcelInput input);

		Task<PagedResultDto<DepositUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

		Task<List<DepositBankLookupTableDto>> GetAllBankForTableDropdown();

		Task<List<DepositUserLookupTableDto>> GetAllUserForTableDropdown();

		Task<ResponseMessages> DepositRequest(DepositRequestDto input);
		Task<List<DepositRequestItemDto>> GetDepositRequest(GetTopRequestDeposit input);

		Task<DepositDto> GetDeposit(string transcode, long? userId = null);

		Task<decimal> GetLimitAvailability(long userId);

		Task<ResponseMessages> GetRandomRequestCode();
    }
}
