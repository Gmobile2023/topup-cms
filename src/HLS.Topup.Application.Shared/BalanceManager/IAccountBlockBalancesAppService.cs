using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.RequestDtos;


namespace HLS.Topup.BalanceManager
{
    public interface IAccountBlockBalancesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAccountBlockBalanceForViewDto>> GetAll(GetAllAccountBlockBalancesInput input);

        Task<PagedResultDto<AccountBlockBalanceDetailDto>> GetListDetail(
            GetAllAccountBlockBalancesDetailInput input);

        Task<GetAccountBlockBalanceForViewDto> GetAccountBlockBalanceForView(int id);

        Task<GetAccountBlockBalanceForEditOutput> GetAccountBlockBalanceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAccountBlockBalanceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAccountBlockBalancesToExcel(GetAllAccountBlockBalancesForExcelInput input);


        Task<List<AccountBlockBalanceUserLookupTableDto>> GetAllUserForTableDropdown();
    }
}
