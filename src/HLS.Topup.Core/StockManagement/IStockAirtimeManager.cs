using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.StockManagement
{
    public interface IStockAirtimeManager
    {
        Task<ApiResponseDto<List<StocksAirtimeDto>>> GetAllStockAirtime(GetAllStockAirtimeRequest input);
        Task<ApiResponseDto<StocksAirtimeDto>> GetStockAirtime(GetStockAirtimeRequest input);
        Task<ResponseMessages> CreateStockAirtime(CreateStockAirtimeRequest input);
        Task<ResponseMessages> UpdateStockAirtime(UpdateStockAirtimeRequest input);
        Task<ReponseMessageResultBase<string>> GetAvailableStockAirtime(GetAvailableStockAirtimeRequest input);

        Task<ReponseMessageResultBase<string>> DepositStockAirtime(ViettelDepositRequest input);
        Task<ApiResponseDto<List<BatchAirtimeDto>>> GetAllBatchAirtime(GetAllBatchAirtimeRequest input);
        Task<ApiResponseDto<BatchAirtimeDto>>  GetBatchAirtime(GetBatchAirtimeRequest input);
        Task<ResponseMessages> CreateBatchAirtime(CreateBatchAirtimeRequest input);
        Task<ResponseMessages> UpdateBatchAirtime(UpdateBatchAirtimeRequest input);
        Task<ResponseMessages> DateteBatchAirtime(DeleteBatchAirtimeRequest input);
        Task AutoCheckBalanceProvider();



    }
}
