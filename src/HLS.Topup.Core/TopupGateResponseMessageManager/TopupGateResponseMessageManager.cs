using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos.TopupGateResponseMessage;
using HLS.Topup.TopupGateResponseMessage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Logging;


namespace HLS.Topup.TopupGateResponseMessageManager
{
    public class TopupGateResponseMessageManager : TopupDomainServiceBase, ITopupGateResponseMessageManager
    {
        private readonly ILogger<TopupGateResponseMessageManager> _logger;
        private readonly string _serviceApi;

        public TopupGateResponseMessageManager(ILogger<TopupGateResponseMessageManager> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        public async Task<NewMessageReponseBase<object>> CreateTopupGateResponseMessageManager(
            CreateTopupGateResponseMessageRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("CreateTopupGateResponseMessageConfigRequest {@request}", request);
                var result = await client.PostAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation("Return CreateBankMessageConfigRequest{@result}", result.ToJson());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error CreateTopupGateResponseMessageManager{@Ex}", ex);
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> UpdateTopupGateResponseMessageManager(
            UpdateTopupGateResponseMessageRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("UpdateTopupGateResponseMessageConfigRequest{@request}", request);
                var result = await client.PutAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation("UpdateBankMessageConfigRequest{@result}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error UpdateTopupGateResponseMessageConfig{@Ex}", ex);
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<TopupGateResponseMessageDto>> GetTopupGateResponseMessageManager(
            GetTopupGateResponseMRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("GetTopupGateResponseMessageConfigRequest {@request}", request);
                var result = await client.GetAsync<NewMessageReponseBase<TopupGateResponseMessageDto>>(request);
                _logger.LogInformation("GetBankMessageConfigRequest return{@result}", result.Results.ToJson());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GetProviderResponse{@Ex}", ex);
                return null;
            }
        }

        public async Task<ApiResponseDto<List<TopupGateResponseMessageDto>>> GetListTopupGateResponseMessageAsync(
            GetListTopupGateResponseRMRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("GetListTopupGateResponseMessageConfigRequest {@request}", request);
                var result = await client.GetAsync<ApiResponseDto<List<TopupGateResponseMessageDto>>>(request);
                _logger.LogInformation("GetListMessageConfigRequest return{@result}", result.ResponseMessage.ToJson());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GetProviderResponse{@Ex}", ex);
                return null;
            }
        }


        public async Task<NewMessageReponseBase<object>> DeleteTopupGateResponseMessageManager(
            DeleteTopupGateResponseMessageRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("DeleteBankMessageConfigResquest {request}", request);
                var result = await client.DeleteAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation("DeleteBankMessageConfigRequest Return {result}", result.ToJson());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error DeleteProviderResponse{@Ex}", ex);
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> CreateListTopupGateResponseMessageAsync(
            CreateListTopupGateRMRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };
                _logger.LogInformation("CreateListTopupGateResponseMessage@{request}", request);
                var result = await client.PostAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation("GetListTopupGateResponseRMRequest @{result}", result.ToJson());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error CreateListTopUpGateResponseMessage @{ex}", ex);
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }
    }
}