using System;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using Microsoft.Extensions.Logging;
using ServiceStack;
using HLS.Topup.Dtos.PayBacks;
using System.Collections.Generic;
using System.Threading;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Transactions;

namespace HLS.Topup.Transactions
{
    public partial class TransactionManager
    {
        public async Task<ResponseMessageApi<AccountBalanceInfo>> GetBalanceAccountInfoRequest(
            AccountBalanceInfoCheckRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut(),
                    ReadWriteTimeout = _tokenHepper.GetTimeOut()
                };
                //_logger.LogInformation($"AccountBalanceInfoCheckRequest request: {request.ToJson()}");
                var rs = await client.GetAsync<ResponseMessageApi<AccountBalanceInfo>>(request);
                //_logger.LogInformation($"AccountBalanceInfoCheckRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountBalanceInfoCheckRequest error: {ex}");
                return new ResponseMessageApi<AccountBalanceInfo>
                {
                    Success = false,
                    Result = null
                };
            }
        }

        public async Task<ApiResponseDto<AccountBalanceInfo>> BlockBalanceAsync(BlockBalanceRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut(),
                    ReadWriteTimeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"BlockBalanceAsync request: {request.ToJson()}");
                var rs = await client.PostAsync<ApiResponseDto<AccountBalanceInfo>>(request);
                _logger.LogInformation($"BlockBalanceAsync return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BlockBalanceAsync error: {ex}");
                return new ApiResponseDto<AccountBalanceInfo>
                {
                    ResponseCode = ResponseCodeConst.Error
                };
            }
        }

        public async Task<ApiResponseDto<AccountBalanceInfo>> UnBlockBalanceAsync(UnBlockBalanceRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut(),
                    ReadWriteTimeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"UnBlockBalanceAsync request: {request.ToJson()}");
                var rs = await client.PostAsync<ApiResponseDto<AccountBalanceInfo>>(request);
                _logger.LogInformation($"UnBlockBalanceAsync return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnBlockBalanceAsync error: {ex}");
                return new ApiResponseDto<AccountBalanceInfo>
                {
                    ResponseCode = ResponseCodeConst.Error
                };
            }
        }

        public async Task<ResponseMessageApi<decimal>> GetLimitAmountBalance(GetAvailableLimitAccount input)
        {
            _logger.LogInformation($"GetLimitAmountBalance request: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ResponseMessageApi<decimal>>(input);
                _logger.LogInformation($"GetLimitAmountBalance return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetLimitAmountBalance error: {ex}");
                return new ResponseMessageApi<decimal>();
            }
        }

        public async Task<TransactionResponse> AdjustmentRequest(AdjustmentRequest request)
        {
            try
            {
                _logger.LogInformation($"AdjustmentRequest request: {request.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"AdjustmentRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AdjustmentRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Giao dịch không thành công"
                };
            }
        }

        public async Task<TransactionResponse> ClearDebtRequest(ClearDebtRequest request)
        {
            try
            {
                _logger.LogInformation($"ClearDebtRequest request: {request.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"ClearDebtRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClearDebtRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Lỗi nạp tiền không thành công"
                };
            }
        }

        public async Task<TransactionResponse> SaleDepositRequest(SaleDepositRequest request)
        {
            try
            {
                _logger.LogInformation($"SaleDepositRequest request: {request.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"SaleDepositRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SaleDepositRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Lỗi nạp tiền không thành công"
                };
            }
        }

        public async Task<ResponseMessageApi<List<PaybatchAccount>>> PayBacksRequest(PaybatchRequest request)
        {
            try
            {
                _logger.LogInformation($"PayBacksRequest request: {request.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                var rs = await client.PostAsync<ResponseMessageApi<List<PaybatchAccount>>>(request);
                _logger.LogInformation($"PayBacksRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"PayBacksRequest error: {ex}");
                return new ResponseMessageApi<List<PaybatchAccount>>
                {
                    Error = new ErrorMessage
                    {
                        Message = "Giao dịch không thành công"
                    }
                };
            }
        }

        public async Task<TransactionResponse> TransferRequest(TransferRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"TransferRequest request: {request.ToJson()}");
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"TransferRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"TransferRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Lỗi chuyển tiền không thành công"
                };
            }
        }

        public async Task<ResponseMessageApi<decimal>> GetBalanceRequest(GetBalanceRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut(),
                    ReadWriteTimeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"GetBalanceRequest request: {request.ToJson()}");
                var rs = await client.GetAsync<ResponseMessageApi<decimal>>(request);
                _logger.LogInformation($"GetBalanceRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetBlance error: {ex}");
                return new ResponseMessageApi<decimal>
                {
                    Success = false,
                    Result = 0
                };
            }
        }

        public async Task<TransactionResponse> DepositRequest(DepositRequest request)
        {
            try
            {
                _logger.LogInformation($"DepositRequest request: {request.ToJson()}");
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"DepositRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DepositRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Lỗi nạp tiền không thành công"
                };
            }
        }

        public async Task<TransactionResponse> TransferSystemRequest(TransferSystemRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"TransferSystemRequest request: {request.ToJson()}");
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"TransferSystemRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"TransferSystemRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = ResponseCodeConst.Error,
                    ResponseMessage = "Lỗi chuyển tiền không thành công"
                };
            }
        }

        public async Task<ApiResponseDto<List<BatchItemDto>>> GetBatchLotListRequest(
            BatchListGetRequest request)
        {
            _logger.LogInformation($"GetBatchLotListRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<BatchItemDto>>>(request);
                _logger.LogInformation($"GetBatchLotListRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetBatchLotListRequest error: {ex}");
                return null;
            }
        }


        public async Task<BatchItemDto> GetBatchSingleRequest(
          BatchSingleGetRequest request)
        {
            _logger.LogInformation($"{request.BatchCode} GetBatchSingleRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<BatchItemDto>(request);
                //_logger.LogInformation($"{request.BatchCode} GetBatchLotListRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"{request.BatchCode} GetBatchLotListRequest error: {ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<BatchDetailDto>>> GetBatchLotDetaiListLRequest(
            BatchDetailGetRequest request)
        {
            _logger.LogInformation($"GetBatchLotDetaiListLRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<BatchDetailDto>>>(request);
                _logger.LogInformation($"GetBatchLotDetaiListLRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetBatchLotDetaiListLRequest error: {ex}");
                return null;
            }
        }
    }
}
