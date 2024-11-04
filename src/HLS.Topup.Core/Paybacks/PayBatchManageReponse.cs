using HLS.Topup.BalanceManager;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HLS.Topup.Paybacks
{
    public class PayBatchManageReponse : TopupDomainServiceBase, IPayBatchManageReponse
    {
        private readonly ILogger<PayBatchManageReponse> _logger;
        private readonly string _serviceApi;
        private readonly TokenHepper _tokenHepper;
        public PayBatchManageReponse(IWebHostEnvironment env, TokenHepper commonManager, ILogger<PayBatchManageReponse> logger)
        {
            _tokenHepper = commonManager;
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }
        public async Task<ApiResponseDto<List<PayBatchBillItem>>> PayBatchBillGetRequest(
          PayBatchBillRequest request)
        {
            _logger.LogInformation($"ReportDetailGetRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<PayBatchBillItem>>>(request);
                _logger.LogInformation($"PayBatchBillGetRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"PayBatchBillGetRequest error: {ex}");
                return null;
            }
        }
    }
}
