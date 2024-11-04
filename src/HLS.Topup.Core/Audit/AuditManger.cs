using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Audit;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Paygate.Contracts.Commands.Commons;
using ServiceStack;

namespace HLS.Topup.Audit
{
    public class AuditManger : TopupDomainServiceBase, IAuditManger
    {
        private readonly ILogger<AuditManger> _logger;
        private readonly string _serviceApi;
        private readonly TokenHepper _tokenHepper;
        private readonly IBus _bus;

        public AuditManger(TokenHepper tokenHepper, ILogger<AuditManger> logger, IWebHostEnvironment env, IBus bus)
        {
            _tokenHepper = tokenHepper;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
            _logger = logger;
            _bus = bus;
        }

        public async Task<ApiResponseDto<List<AccountActivityHistoryDto>>> GetAccountActivityHistoryRequest(
            GetAccountActivityHistoryRequest request)
        {
            _logger.LogInformation($"GetAccountActivityHistoryRequest request: {request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.GetAsync<ApiResponseDto<List<AccountActivityHistoryDto>>>(request);
                _logger.LogInformation($"GetAccountActivityHistoryRequest return: {rs.ResponseCode} - {rs.Total}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetAccountActivityHistoryRequest error: {ex}");
                return null;
            }
        }

        public async Task AddAccountActivities(AccountActivityHistoryRequest request)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await _bus.Publish<AccountActivitiesCommand>(new
                    {
                        TimeStamp = DateTime.Now,
                        CorrelationId = Guid.NewGuid(),
                        request.AccountCode,
                        request.FullName,
                        request.AccountType,
                        request.AgentType,
                        request.PhoneNumber,
                        request.UserName,
                        request.Note,
                        Payload = request.Payload,
                        request.SrcValue,
                        request.DesValue,
                        request.Attachment,
                        AccountActivityType = (byte) request.AccountActivityType
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError($"AddAccountActivities eror:{e}");
                }
            }).ConfigureAwait(false);
        }
    }
}
