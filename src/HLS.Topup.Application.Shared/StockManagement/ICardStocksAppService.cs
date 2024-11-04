using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Services.Dtos;


namespace HLS.Topup.StockManagement
{
    public interface ICardStocksAppService : IApplicationService 
    {
        Task<PagedResultDtoReport<CardStockDto>> GetAll(GetAllCardStocksInput input);

        Task<GetCardStockForViewDto> GetCardStockForView(string code, string stockType, decimal cardvalue);

		Task<GetCardStockForEditOutput> GetCardStockForEdit(string code, string stockType, decimal cardvalue);
        Task<ResponseMessages> UpdateEditQuantity(EditQuantityStockDto input);

        Task CreateOrEdit(CreateOrEditCardStockDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCardStocksToExcel(GetAllCardStocksForExcelInput input);

		Task<TransferCardStockDto> GetTransferStock(Guid id);
		Task<ResponseMessages> TransferStock(TransferCardStockDto input);
		Task<List<CommonLookupTableDto>> StockCodes();
		Task<ResponseMessages> StockTransferRequest(StockTransferInput input);
    }
}