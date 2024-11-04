using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using ServiceStack;

namespace HLS.Topup.StockManagement
{
    public class CardManager : TopupDomainServiceBase, ICardManager
    {
        private readonly TokenHepper _tokenHepper;
        private readonly string _serviceApi;
        //private readonly Logger _logger = LogManager.GetLogger("CardManager");
        private readonly ILogger<CardManager> _logger;

        public CardManager(IWebHostEnvironment env, TokenHepper commonManager, ILogger<CardManager> logger)
        {
            _tokenHepper = commonManager;
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        public async Task<ResponseMessages> CardBatchCreateRequest(CardBatchCreateRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
            try
            {
                var result = await client.PostAsync<ResponseMessages>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardBatchDeleteRequest(CardBatchDeleteRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var result = await client.DeleteAsync<ResponseMessages>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CardBatchResponseDto>>> CardBatchGetListRequest(
            CardBatchGetListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<CardBatchResponseDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }


        public async Task<ApiResponseDto<CardBatchResponseDto>> CardBatchGetRequest(CardBatchGetRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<CardBatchResponseDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardBatchUpdateRequest(CardBatchUpdateRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return await client.PatchAsync<ResponseMessages>(input);
        }

        public async Task<ApiResponseDto<CardResponseDto>> CardGetFullRequest(CardGetFullRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<CardResponseDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<List<CardResponseDto>>> CardGetListRequest(CardGetListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<CardResponseDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        // public async Task<ApiResponseDto<List<CardRequestResponseDto>>> CardRequestGetListRequest(
        //     CardRequestGetListRequest input)
        // {
        //     var client = new JsonServiceClient(_serviceApi)
        //     {
        //         BearerToken = await _commonManager.GetAccessTokenViaCredentialsAsync()
        //     };
        //     try
        //     {
        //         return await client.GetAsync<ApiResponseDto<List<CardRequestResponseDto>>>(input);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return null;
        //     }
        // }

        public async Task<ApiResponseDto<CardResponseDto>> CardGetRequest(CardGetRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<CardResponseDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardImportListRequest(CardImportListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var result = await client.PostAsync<ResponseMessages>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<NewMessageReponseBase<string>> CardImportApiRequest(CardImportApiRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)         
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var result = await client.PostAsync<NewMessageReponseBase<string>>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CardImportApiRequest error: {ex}");
                return null;
            }
        }

        public async Task<ResponseMessages> CardImportFileRequest(CardImportFileModel input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var result = await client.PostAsync<ResponseMessages>(input);
                _logger.LogInformation($"CardImportFileRequest result: {result.ResponseCode}|{result.ResponseMessage}");
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CardImportFileRequest error: {ex}");
                return new ResponseMessages()
                {
                    ResponseCode = "00",
                    ResponseMessage = ex.Message,
                };
            }
        }

        public async Task<ResponseMessages> CardImportRequest(CardImportRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var result = await client.PostAsync<ResponseMessages>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardStockCreateRequest(CardStockCreateRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return await client.PostAsync<ResponseMessages>(input);
        }

        public async Task<ResponseMessages> CardStockTransferRequest(CardStockTransferRequest input)
        {
            input.Status = 1;
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return await client.PostAsync<ResponseMessages>(input);
        }

        public async Task<ResponseMessages> CardsStockTransferRequest(Guid id)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync(),
                Timeout = _tokenHepper.GetTimeOut()
            };
            return await client.PostAsync<ResponseMessages>(new CardsStockTransferRequest() {Id = id});
        }

        public async Task<ApiResponseDto<List<StockResponseDto>>> CardStockGetListRequest(
            CardStockGetListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<StockResponseDto>>>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CardStockGetListRequest error: {ex}");
                return new ApiResponseDto<List<StockResponseDto>>
                {
                    ResponseCode = "00"
                };
            }
        }

        public async Task<ResponseMessages> CardStockUpdateRequest(CardStockUpdateRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return await client.PutAsync<ResponseMessages>(input);
        }

        public async Task<ResponseMessages> CardStockUpdateQuantityRequest(CardStockUpdateQuantityRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            return await client.PutAsync<ResponseMessages>(input);
        }

        public async Task<ResponseMessages> SimCreateRequest(SimCreateRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PostAsync<ResponseMessages>(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> SimCreateManyRequest(SimCreateManyRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PostAsync<ResponseMessages>(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<List<SimResponseDto>>> SimGetListRequest(SimGetListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<SimResponseDto>>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<SimResponseDto>> SimGetRequest(SimGetRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<SimResponseDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> SimUpdateRequest(SimUpdateRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PatchAsync<ResponseMessages>(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResponseDto<StockResponseDto>> CardStockGetRequest(CardStockGetRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<StockResponseDto>>(input);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardUpdateRequest(CardUpdateRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PatchAsync<ResponseMessages>(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ResponseMessages> CardUpdateStatusRequest(CardUpdateStatusRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.PatchAsync<ResponseMessages>(input);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ApiResponseDto<List<StockTransferItemInfoRespond>>> GetCardInfoTransferRequest(GetCardInfoTransferRequest input)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                return await client.GetAsync<ApiResponseDto<List<StockTransferItemInfoRespond>>>(input);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<ResponseMessages> StockTransferRequest(StockTransferCardRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                //BearerToken = await _tokenHepper.GetAccessTokenViaCredentialsAsync()
            };
            return await client.PostAsync<ResponseMessages>(input);
        }


        public async Task<ApiResponseDto<List<StockTransRequestDto>>> CardStockTransListAsync(CardStockTransListRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<StockTransRequestDto>>>(input);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"CardStockTransListAsync Exception: {ex}");
                return null;
            }
        }

        public async Task<NewMessageReponseBase<string>> StockCardApiCheckTransRequest(StockCardApiCheckTransRequest input)
        {
            var client = new JsonServiceClient(_serviceApi)
            {               
                Timeout = _tokenHepper.GetTimeOut()
            };
            try
            {
                var result = await client.PostAsync<NewMessageReponseBase<string>>(input);
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"StockCardApiCheckTransRequest error: {ex}");
                return null;
            }
        }

    }
}
