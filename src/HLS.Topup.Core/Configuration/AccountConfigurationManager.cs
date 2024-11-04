using System;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Partner;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.RequestDtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Configuration
{
    public class AccountConfigurationManager : TopupDomainServiceBase, IAccountConfigurationManager
    {
        private readonly ILogger<AccountConfigurationManager> _logger;
        private readonly string _serviceApi;
        private readonly TokenHepper _tokenHepper;

        public AccountConfigurationManager(TokenHepper tokenHepper, IWebHostEnvironment env,
            ILogger<AccountConfigurationManager> logger)
        {
            _tokenHepper = tokenHepper;
            _logger = logger;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        public async Task<ProviderUpdateInfo> GetProviderInfo(ProviderInfoGetRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"GetProviderInfo request: {request.ToJson()}");
                var rs = await client.GetAsync<NewMessageReponseBase<ProviderUpdateInfo>>(request);
                _logger.LogInformation($"GetProviderInfo return: {rs.ResponseStatus.ToJson()}");
                return rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success ? rs.Results : new ProviderUpdateInfo();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProviderInfo error: {ex}");
                return new ProviderUpdateInfo();
            }
        }

        public async Task<NewMessageReponseBase<object>> ProviderInfoUpdateRequest(ProviderInfoUpdateRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"ProviderInfoUpdateRequest request: {request.ToJson()}");
                var rs = await client.PatchAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation($"ProviderInfoUpdateRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProviderInfoUpdateRequest error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> ProviderInfoCreateRequest(ProviderInfoCreateRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"ProviderInfoCreateRequest request: {request.ToJson()}");
                var rs = await client.PostAsync<NewMessageReponseBase<object>>(request);
                _logger.LogInformation($"ProviderInfoCreateRequest return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProviderInfoCreateRequest error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<PartnerConfigTransDto> GetPartnerConfig(GetPartnerRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"GetPartnerConfig request: {request.ToJson()}");
                var rs = await client.GetAsync(request);
                _logger.LogInformation($"GetPartnerConfig return: {rs.ToJson()}");
                return rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success && rs.Results != null
                    ? rs.Results
                    : new PartnerConfigTransDto();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetPartnerConfig error: {ex}");
                return new PartnerConfigTransDto();
            }
        }

        public async Task<NewMessageReponseBase<object>> UpdatePartnerConfig(UpdatePartnerRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"UpdatePartnerConfig request: {request.ToJson()}");
                var rs = await client.PutAsync(request);
                _logger.LogInformation($"UpdatePartnerConfig return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdatePartnerConfig error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> CreatePartnerConfig(CreatePartnerRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"CreatePartnerConfig request: {request.ToJson()}");
                var rs = await client.PostAsync(request);
                _logger.LogInformation($"CreatePartnerConfig return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreatePartnerConfig error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> CreateOrUpdatePartnerConfig(
            CreateOrUpdatePartnerRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"CreateOrUpdatePartnerConfig request: {request.ToJson()}");
                var rs = await client.PostAsync(request);
                _logger.LogInformation($"CreateOrUpdatePartnerConfig return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrUpdatePartnerConfig error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }

        public async Task<NewMessageReponseBase<object>> CreateOrUpdateServiceConfig(
            CreateOrUpdateServiceRequest request)
        {
            try
            {
                var client = new JsonServiceClient(_serviceApi)
                {
                    Timeout = _tokenHepper.GetTimeOut(),
                };
                _logger.LogInformation($"CreateOrUpdateServiceConfig request: {request.ToJson()}");
                var rs = await client.PostAsync(request);
                _logger.LogInformation($"CreateOrUpdateServiceConfig return: {rs.ToJson()}");
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrUpdatePartnerConfig error: {ex}");
                return new NewMessageReponseBase<object>
                {
                    ResponseStatus = new ResponseStatus(ResponseCodeConst.Error)
                };
            }
        }
    }
}
