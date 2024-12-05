using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Limitations;
using HLS.Topup.Dtos.Reports;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.FeeManager;
using HLS.Topup.Notifications;
using HLS.Topup.Products;
using HLS.Topup.RequestDtos;
using HLS.Topup.Topup;
using HLS.Topup.Topup.ResponseDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;
using static HLS.Topup.Common.CommonConst;

namespace HLS.Topup.Transactions
{
    public partial class TransactionManager : TopupDomainServiceBase, ITransactionManager
    {
        //private readonly Logger _logger1 = LogManager.GetLogger("TransactionManager");
        private readonly ILogger<TransactionManager> _logger;
        private readonly string _serviceApi;
        private readonly TokenHepper _tokenHepper;
        private readonly ICommonManger _commonManger;
        private readonly INotificationSender _appNotifier;
        private readonly IDiscountManger _discountManger;
        private readonly IFeeManager _feeManager;
        private readonly IRepository<Product> _product;

        public TransactionManager(IWebHostEnvironment env, TokenHepper commonManager,
            ILogger<TransactionManager> logger, ICommonManger commonManger,
            INotificationSender appNotifier, IDiscountManger discountManger, IFeeManager feeManager,
            IRepository<Product> product)
        {
            _tokenHepper = commonManager;
            _logger = logger;
            _commonManger = commonManger;
            _appNotifier = appNotifier;
            _discountManger = discountManger;
            _feeManager = feeManager;
            _product = product;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        public async Task<ApiResponseDto<List<TransactionReportDto>>> TransactionReportsGetRequest(
            TransactionReportsRequest request)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<TransactionReportDto>>>(request);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<List<BalanceHistoryResponseDto>>> BalanceHistoriesGetRequest(
            BalanceHistoriesRequest request)
        {
            _logger.LogInformation($"BalanceHistoriesGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<BalanceHistoryResponseDto>>>(request);
                _logger.LogInformation($"BalanceHistoriesGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"BalanceHistoriesGetRequest error: {ex}");
                return null;
            }
        }


        public async Task<ApiResponseDto<List<TransactionsHistoryResponseDTO>>> TransactionHistoriesGetRequest(
            TransactionsHistoryRequest request)
        {
            _logger.LogInformation($"TransactionHistoriesGetRequest request: {request.ToJson()}");
            // connect server BalanceUrl:http://localhost:6797
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client
                    .GetAsync<ApiResponseDto<List<TransactionsHistoryResponseDTO>>>(request);
                _logger.LogInformation($"TransactionHistoriesGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"TransactionHistoriesGetRequest error: {e}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<TopupDetailResponseDTO>>> GetTopupDetailsRequest(
            GetTopupDetailRequest request)
        {
            _logger.LogInformation($"GetTopupDetailsRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<TopupDetailResponseDTO>>>(request);
                _logger.LogInformation($"GetTopupDetailsRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetTopupDetailsRequest error: {e}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<TopupDetailResponseDTO>>> GetTopupItemsRequest(
            TopupsListItemsRequest request)
        {
            _logger.LogInformation($"GetTopupItemsRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<TopupDetailResponseDTO>>>(request);
                _logger.LogInformation($"GetTopupItemsRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetTopupItemsRequest error: {e}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<LevelDiscountResponseDto>>> GetLevelDiscountsRequest(
            GetLevelDiscountsRequest request)
        {
            _logger.LogInformation($"GetLevelDiscountsRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<LevelDiscountResponseDto>>>(request);
                _logger.LogInformation($"GetLevelDiscountsRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetLevelDiscountsRequest error: {e}");
                return null;
            }
        }

        public async Task<TransactionResponse> GetDiscountAvailableRequest(GetDiscountAvailableRequest request)
        {
            _logger.LogInformation($"GetDiscountAvailableRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<TransactionResponse>(request);
                _logger.LogInformation($"GetDiscountAvailableRequest return: {rs.ResponseCode} - {rs.Payload}");
                return rs;
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetLevelDiscountsRequest error: {e}");
                return new TransactionResponse
                {
                    ResponseCode = "0"
                };
            }
        }

        public async Task<TransactionResponse> CollectDiscountRequest(CollectDiscountRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                    Timeout = _tokenHepper.GetTimeOut()
                };
                _logger.LogInformation($"CollectDiscountRequest request: {request.ToJson()}");
                var rs = await client.PostAsync<TransactionResponse>(request);
                _logger.LogInformation($"CollectDiscountRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CollectDiscountRequest error: {ex}");
                return new TransactionResponse
                {
                    ResponseCode = "0",
                    ResponseMessage = "Lỗi chuyển tiền không thành công"
                };
            }
        }

        public async Task<ApiResponseDto<List<TopupRequestResponseDto>>> TopupListRequestAsync(TopupsListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<TopupRequestResponseDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<TopupRequestResponseDto> GetDetailsRequestAsync(GetSaleRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                return await client.GetAsync<TopupRequestResponseDto>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        public async Task<ResponseMessages> UpdateStatusRequestAsync(TopupsUpdateStatusRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                return await client.PatchAsync<ResponseMessages>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<NewMessageReponseBase<object>> TopupRequestAsync(TopupRequest input)
        {
            _logger.LogInformation($"TopupRequest request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/gateway/topup", data);
                _logger.LogInformation($"TopupRequestReturn:{json}-{response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"TopupRequestReturn:{result}");
                    return result.FromJson<NewMessageReponseBase<object>>();

                }

                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"TopupRequest error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }


            // var client = new JsonServiceClient(_serviceApi)
            // {
            //     BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
            //     Timeout = _tokenHepper.GetTimeOut()
            // };
            // try
            // {
            //     var rs = await client.PostAsync<ResponseMessageApi<object>>(input);
            //
            //     _logger.LogInformation($"TopupRequest return: {rs.ToJson()}");
            //     return rs;
            // }
            // catch (System.Exception ex)
            // {
            //     _logger.LogInformation($"TopupRequest error: {ex}");
            //     return new ResponseMessageApi<object>
            //     {
            //         Success = false,
            //         Error = new ErrorMessage
            //         {
            //             Details = "Giao dịch thất bại"
            //         }
            //     };
            // }
        }

        public async Task<NewMessageReponseBase<string>> PayBatchRequestAsync(PayBatchRequest input)
        {
            _logger.LogInformation($"TopupManyRequest request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/gateway/paybatch", data);
                _logger.LogInformation($"TopupListRequestReturn:{response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadAsStringAsync();
                    var msg = new NewMessageReponseBase<string>()
                    {
                        ResponseStatus = new ResponseStatus(ResponseCodeConst.Success, "Thành công"),
                        Results = input.TransRef
                    };

                    return msg;
                    //return result.FromJson<ResponseMessageApi<object>>();
                }

                return new NewMessageReponseBase<string>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"TopupRequestList error: {ex}");
                return new NewMessageReponseBase<string>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
        }

        public async Task<ResponseMessages> StopBatchLotAsync(BatchLotStopRequest input)
        {
            _logger.LogInformation($"StopBatchLotAsync request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/gateway/batchLot_Stop", data);
                _logger.LogInformation($"StopBatchLot Return:{response.ToJson()}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.FromJson<ResponseMessages>();
                }

                return new ResponseMessages
                {
                    ResponseMessage = "Yêu cầu dừng lô chưa chưa thành công.",
                    ResponseCode = "0"
                };
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError($"StopBatchLot  TaskCanceledException error: {ex}");
                return new ResponseMessages
                {
                    ResponseMessage = "Yêu cầu dừng lô chưa có kết quả",
                    ResponseCode = "1"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"StopBatchLot error: {ex}");
                return new ResponseMessages
                {
                    ResponseMessage = "Yêu cầu dừng lô chưa chưa thành công.",
                    ResponseCode = "0"
                };
            }
        }

        public async Task<ApiResponseDto<List<CheckChargesHistoryResponseDto>>> CheckChargesHistoryGetRequest(
            CheckChargesRequest request)
        {
            _logger.LogError($"CheckChargesHistoryGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CheckChargesHistoryResponseDto>>>(request);
                _logger.LogError($"CheckChargesHistoryGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (Exception e)
            {
                _logger.LogError($"CheckChargesHistoryGetRequest error: {e}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CheckChargesDetaiResponselDto>>> CheckChargesDetailGetRequest(
            CheckChargesDetailRequest request)
        {
            _logger.LogError($"CheckChargesHistoryDetailGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<CheckChargesDetaiResponselDto>>>(request);
                _logger.LogError($"CheckChargesHistoryDetailGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (Exception e)
            {
                _logger.LogError($"CheckChargesHistoryDetailGetRequest error: {e}");
                return null;
            }
        }

        public async Task<ResponseMessages> TopupCancelRequestAsync(TopupCancelRequest input)
        {
            _logger.LogInformation($"TopupCancelRequestAsync request: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.DeleteAsync<ResponseMessages>(input);
                _logger.LogInformation($"TopupCancelRequestAsync return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"TopupCancelRequestAsync error: {ex}");
                return new ResponseMessages
                {
                    ResponseCode = "0",
                    ResponseMessage = "Giao dịch chưa có kết quả. Xin vui lòng chờ kết quả"
                };
            }
        }

        public async Task<NewMessageReponseBase<List<CardResponseDto>>> PinCodeAsync(CardSaleRequest input)
        {
            _logger.LogInformation($"PincodeRequest: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/gateway/card/card_sale", data);
                _logger.LogInformation($"PincodeReturn:{response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var resultConvert = result.FromJson<NewMessageReponseBase<List<CardResponseDto>>>();
                    _logger.LogInformation($"PinCodeAsync return: {input.ToJson()}|ErrorCode= {resultConvert?.ResponseStatus?.ErrorCode}|Message= {resultConvert?.ResponseStatus?.Message}");
                    return result.FromJson<NewMessageReponseBase<List<CardResponseDto>>>();
                }

                return new NewMessageReponseBase<List<CardResponseDto>>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"PinCodeAsync error: {ex}");
                return new NewMessageReponseBase<List<CardResponseDto>>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
        }

        public async Task<NewMessageReponseBase<BalanceResponseDto>> TransactionRefundRequestAsync(TransactionRefundRequest input)
        {
            _logger.LogInformation($"TopupRefundRequestAsync request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/backend/refund", data);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.FromJson<NewMessageReponseBase<BalanceResponseDto>>();
                }
                return new NewMessageReponseBase<BalanceResponseDto>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_Failed, "Giao dịch chưa có kết quả. Xin vui lòng chờ kết quả")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"TopupRefundRequestAsync error: {ex}");
                return new NewMessageReponseBase<BalanceResponseDto>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_Failed, "Giao dịch chưa có kết quả. Xin vui lòng chờ kết quả")
                };
            }
        }

        public async Task<NewMessageReponseBase<InvoiceResultDto>> BillQueryRequestAsync(BillQueryRequest input)
        {
            _logger.LogInformation($"BillQueryRequestAsync request: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<NewMessageReponseBase<InvoiceResultDto>>(input);
                _logger.LogInformation($"BillQueryRequestAsync return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"BillQueryRequestAsync error: {ex}");
                return new NewMessageReponseBase<InvoiceResultDto>()
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error, "Truy vấn thông tin không thành công. Vui lòng thử lại sau")
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> PayBillRequestAsync(PayBillRequest input)
        {
            _logger.LogInformation($"PayBillRequestAsync request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _tokenHepper.GetAccessTokenViaCredentialsAsync());
                var response = await client.PostAsync("/api/v1/gateway/pay_bill", data);
                _logger.LogInformation($"BillQueryRequestAsync return: {json}{response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"BillQueryRequestAsync return: {result}");
                    return result.FromJson<NewMessageReponseBase<object>>();
                }

                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"PayBillRequestAsync error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_WaitForResult,
                        "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để biết thông tin chi tiết")
                };
            }
        }

        public async Task<ResponseMessageApi<AccountProductLimitDto>> GetTotalPerDayProduct(
            GetTotalPerDayProductRequest input)
        {
            _logger.LogInformation($"GetTotalPerDayProduct request: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<ResponseMessageApi<AccountProductLimitDto>>(input);
                _logger.LogInformation($"GetTotalPerDayProduct return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetTotalPerDayProduct error: {ex}");
                return new ResponseMessageApi<AccountProductLimitDto>();
            }
        }

        public async Task<int> GetTotalWaitingBillRequest(GetTotalWaitingBillRequest input)
        {
            _logger.LogInformation($"GetTotalWaitingBillRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<int>(input);
                _logger.LogInformation($"GetTotalWaitingBillRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetTotalWaitingBillRequest error: {ex}");
                return 0;
            }
        }

        public async Task<ResponseMessageApi<object>> RemoveSavePayBillRequest(RemoveSavePayBillRequest input)
        {
            _logger.LogInformation($"RemoveSavePayBillRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.DeleteAsync<ResponseMessageApi<object>>(input);
                _logger.LogInformation($"RemoveSavePayBillRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"RemoveSavePayBillRequest error: {ex}");
                return new ResponseMessageApi<object>();
            }
        }

        public async Task<NewMessageReponseBase<ResponseProvider>> ProviderCheckTransRequest(
            ProviderCheckTransStatusRequest input)
        {
            _logger.LogInformation($"ProviderCheckTransRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<NewMessageReponseBase<ResponseProvider>>(input);
                _logger.LogInformation($"ProviderCheckTransRequestRreturn: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"ProviderCheckTransRequest error: {ex}");
                return new NewMessageReponseBase<ResponseProvider>
                {
                    ResponseStatus = new ResponseStatus("0", "Check giao dịch không thành công"),
                    Results = new ResponseProvider()
                };
            }
        }

        public async Task<NewMessageReponseBase<string>> CheckTransRequest(CheckTransStatusRequest input)
        {
            _logger.LogInformation($"CheckTransStatusRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<NewMessageReponseBase<string>>(input);
                _logger.LogInformation($"CheckTransStatusRequestReturn: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"CheckTransStatusRequestError: {ex}");
                return new NewMessageReponseBase<string>
                {
                    ResponseStatus = new ResponseStatus("0", "Check giao dịch không thành công")
                };
            }
        }

        public async Task<List<PayBillAccountDto>> GetSavePayBillRequest(GetSavePayBillRequest input)
        {
            _logger.LogInformation($"GetSavePayBillRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.GetAsync<List<PayBillAccountDto>>(input);
                _logger.LogInformation($"GetSavePayBillRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"GetSavePayBillRequest error: {ex}");
                return new List<PayBillAccountDto>();
            }
        }

        public async Task<ApiResponseDto<List<SaleOffsetReponseDto>>> GetOffsetTopupListRequestAsync(OffsetTopupRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<SaleOffsetReponseDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        public async Task<NewMessageReponseBase<string>> OffsetTopupRequestAsync(OffsetBuRequest input)
        {
            _logger.LogInformation($"OffsetTopupRequestAsync request: {input.ToJson()}");
            try
            {
                var json = input.ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                    BaseAddress = new Uri(_serviceApi),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync("/api/v1/backend/OffsetTopup", data);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.FromJson<NewMessageReponseBase<string>>();
                }
                return new NewMessageReponseBase<string>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_Failed, "Giao dịch chưa có kết quả. Xin vui lòng chờ kết quả")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"OffsetTopupRequestAsync error: {ex}");
                return new NewMessageReponseBase<string>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.ResponseCode_Failed, "Giao dịch chưa có kết quả. Xin vui lòng chờ kết quả")
                };
            }
        }

        public async Task<NewMessageReponseBase<string>> UpdateCardCodeRequest(UpdateCardCodeRequest input)
        {
            _logger.LogInformation($"UpdateCardCodeRequest: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var rs = await client.PostAsync<NewMessageReponseBase<string>>(input);
                _logger.LogInformation($"TransCode= {input.TransCode}|UpdateCardCodeRequestRreturn: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"TransCode= {input.TransCode}|UpdateCardCodeRequest error: {ex}");
                return new NewMessageReponseBase<string>()
                {
                    ResponseStatus = new ResponseStatus()
                    {
                        ErrorCode = ResponseCodeConst.Error,
                        Message = "Cập nhật mã thẻ không thành công"
                    }
                };                
            }
        }
    }
}
