using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;


namespace HLS.Topup.BalanceManager
{
    public interface ISystemAccountTransfersAppService : IApplicationService
    {
        Task<PagedResultDto<GetSystemAccountTransferForViewDto>> GetAll(GetAllSystemAccountTransfersInput input);

        Task<GetSystemAccountTransferForViewDto> GetSystemAccountTransferForView(int id);

		Task<GetSystemAccountTransferForEditOutput> GetSystemAccountTransferForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSystemAccountTransferDto input);

		Task Delete(EntityDto input);
		Task Approval(EntityDto input);
		Task Cancel(EntityDto input);

		Task<FileDto> GetSystemAccountTransfersToExcel(GetAllSystemAccountTransfersForExcelInput input);


    }
}
