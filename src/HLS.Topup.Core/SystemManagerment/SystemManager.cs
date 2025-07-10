using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Hangfire;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Configs;
using HLS.Topup.Notifications;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Topup.Contracts.Commands.Backend;

namespace HLS.Topup.SystemManagerment
{
    public class SystemManager : TopupDomainServiceBase, ISystemManager
    {
        private readonly IRepository<Provider> _providerRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<SystemManager> _logger;
        private readonly IBus _bus;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly INotificationSender _appNotifier;

        public SystemManager(IRepository<Provider> providerRepository, ICacheManager cacheManager,
            ILogger<SystemManager> logger, IBus bus, IWebHostEnvironment env, INotificationSender appNotifier)
        {
            _providerRepository = providerRepository;
            _cacheManager = cacheManager;
            _logger = logger;
            _bus = bus;
            _appNotifier = appNotifier;
            _appConfiguration = env.GetAppConfiguration();
        }

        public async Task<bool> LockProvider(string providerCode, int timeLock = 30)
        {
            try
            {
                _logger.LogInformation($"LockProvider:{providerCode}");
                var provider = await _providerRepository.FirstOrDefaultAsync(x => x.Code == providerCode);
                if (provider.ProviderStatus != CommonConst.ProviderStatus.Active) return true;
                provider.ProviderStatus = CommonConst.ProviderStatus.Lock;
                await _providerRepository.UpdateAsync(provider);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await ClearCache(new EntityDto<string>("ServiceConfiguations"));
                //var config = new HangfireConfig();
                //_appConfiguration.GetSection("HangfireConfig").Bind(config);
                BackgroundJob.Schedule<ISystemManager>((x) => x.UnLockProvider(provider.Code, true),
                    TimeSpan.FromMinutes(timeLock));

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"LockProvider error:{e}");
                return false;
            }
        }

        public async Task<bool> UnLockProvider(string providerCode, bool isAuto = false)
        {
            try
            {
                _logger.LogInformation($"UnLockProvider:{providerCode}-{isAuto}");
                var provider = await _providerRepository.FirstOrDefaultAsync(x => x.Code == providerCode);
                if (provider.ProviderStatus != CommonConst.ProviderStatus.Lock) return true;
                provider.ProviderStatus = CommonConst.ProviderStatus.Active;
                await _providerRepository.UpdateAsync(provider);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await ClearCache(new EntityDto<string>("ServiceConfiguations"));
                await _bus.Publish<ResetAutoLockProviderCommand>(new
                {
                    CorrelationId = Guid.NewGuid(),
                    ProviderCode = provider.Code
                });
                if (isAuto)
                {
                    await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                    {
                        Message = $"Kênh {providerCode} đã được mở lại tự động. Vui lòng theo dõi tình trạng Kênh",
                        Module = "WEB",
                        Title = $"Mở kênh {provider.Code} tự động",
                        BotType = (byte) CommonConst.BotType.Channel,
                        MessageType = (byte) CommonConst.BotMessageType.Message
                    });
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"LockProvider error:{e}");
                return false;
            }
        }

        private async Task<bool> ClearCache(EntityDto<string> input)
        {
            try
            {
                var cache = _cacheManager.GetCache(input.Id);
                await cache.ClearAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ClearCache:{e}");
                return false;
            }
        }
    }
}