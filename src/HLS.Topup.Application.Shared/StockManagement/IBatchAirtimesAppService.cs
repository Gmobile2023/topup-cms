using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.StockManagement
{
    public interface IBatchAirtimesAppService : IApplicationService 
    {
        Task<PagedResultDto<BatchAirtimeDto>> GetAll(GetAllBatchAirtimesInput input);

        Task<BatchAirtimeDto> GetBatchAirtimeForView(string stockCode);
        Task<BatchAirtimeDto> GetBatchAirtimeForEdit(string stockCode);
        Task CreateOrEdit(BatchAirtimeDto input);
        Task Approval(string code);
        Task Reject(string code);
        Task<FileDto> GetBatchAirtimesToExcel(GetAllBatchAirtimesForExcelInput input);
        Task Delete(string code);
    }
}