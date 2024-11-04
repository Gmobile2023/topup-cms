using HLS.Topup.Dtos.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;

namespace HLS.Topup.StockManagement
{
    public interface IStocksAirtimesAppService : IApplicationService
    {
        Task<PagedResultDto<StocksAirtimeDto>> GetAll(GetAllStocksAirtimesInput input);
        Task<StocksAirtimeDto> GetStocksAirtimeForView(string providerCode);
        Task<StocksAirtimeDto> GetStocksAirtimeForEdit(string providerCode);
		Task CreateOrEdit(StocksAirtimeDto input);
		Task<FileDto> GetStocksAirtimesToExcel(GetAllStocksAirtimesForExcelInput input);

		Task<List<CommonLookupTableDto>> GetAllProvider();
		Task<ResponseMessages> Query(string providerCode);

        Task<ResponseMessages> DepositAirtimeViettel(decimal amount,string providerCode);
    }
}
