using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.BalanceManager
{
    public interface IPayBatchBillsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPayBatchBillForViewDto>> GetAll(GetAllPayBatchBillsInput input);

        Task<GetPayBatchBillForViewDto> GetPayBatchBillForView(int id);

        Task<GetPayBatchBillForEditOutput> GetPayBatchBillForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPayBatchBillDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetPayBatchBillsToExcel(GetAllPayBatchBillsForExcelInput input);


        Task<List<PayBatchBillProductLookupTableDto>> GetAllProductForTableDropdown();

        Task<PagedResultDtoReport<PayBatchBillItem>> PayBatchBillGetRequest(GetPayBatchSearchInput input);

        Task<PagedResultDtoReport<PayBatchBillItem>> GetPayBatchBillDetail(GetPayBatchSearchDetailInput input);

        Task<FileDto> GetPayBatchDetailToExcel(GetPayBatchSearchDetailInput input);

        Task<int> CheckPayBatchBill(CheckPayBatchBillInput input);

        Task ConfirmApproval(int id);

        Task ConfirmCancel(int id);
    }
}
