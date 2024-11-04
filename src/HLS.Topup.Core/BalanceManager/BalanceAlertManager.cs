using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Stock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.BalanceManager
{
    public class BalanceAlertManager : TopupDomainServiceBase, IBalanceAlertManager
    {
        private readonly ILogger<BalanceAlertManager> _logger;
        private readonly string _serviceApi;
        
        public BalanceAlertManager(ILogger<BalanceAlertManager> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }
        
        public async Task<ApiResponseDto<List<LowBalanceAlertResponseDto>>> BalanceAlertGetAllRequest(BalanceAlertGetAllRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi);
                _logger.LogInformation($"BalanceAlertGetAllRequest request: {request.ToJson()}");
                var rs = await client.GetAsync<ApiResponseDto<List<LowBalanceAlertResponseDto>>>(request);
                _logger.LogInformation($"BalanceAlertGetAllRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BalanceAlertGetAllRequest error: {ex}");
                return null;
            }
        }
        
        public async Task<LowBalanceAlertResponseDto> BalanceAlertGetRequest(BalanceAlertGetRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi);
                _logger.LogInformation($"BalanceAlertGetRequest request: {request.ToJson()}");
                var rs = await client.GetAsync<LowBalanceAlertResponseDto>(request);
                _logger.LogInformation($"BalanceAlertGetRequest return: {rs.ToJson()}");
                return rs.ConvertTo<LowBalanceAlertResponseDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"BalanceAlertGetRequest error: {ex}");
                return null;
            }
        }
        
        public async Task<NewMessageReponseBase<object>> BalanceAlertAddRequest(BalanceAlertAddRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi);
                _logger.LogInformation($"BalanceAlertAddRequest request: {request.ToJson()}");
                var rs = await client.PostAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation($"BalanceAlertAddRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BalanceAlertAddRequest error: {ex}");
                return null;
            }
        }
        
        public async Task<NewMessageReponseBase<object>> BalanceAlertUpdateRequest(BalanceAlertUpdateRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi);
                _logger.LogInformation($"BalanceAlertUpdateRequest request: {request.ToJson()}");
                var rs = await client.PutAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation($"BalanceAlertUpdateRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BalanceAlertUpdateRequest error: {ex}");
                return null;
            }
        }
    }
}