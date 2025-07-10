using System;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using HLS.Topup.SystemManagerment;
using ServiceStack;
using Topup.Contracts.Commands.Backend;

namespace HLS.Topup.EventBus.Consumers
{
    public class ProviderActionConsumer : IConsumer<LockProviderCommand>
    {
        private readonly ILogger<ProviderActionConsumer> _logger;
        private readonly ISystemManager _systemManager;

        public ProviderActionConsumer(ILogger<ProviderActionConsumer> logger, ISystemManager systemManager)
        {
            _logger = logger;
            _systemManager = systemManager;
        }

        public async Task Consume(ConsumeContext<LockProviderCommand> context)
        {
            try
            {
                _logger.LogInformation("LockProviderCommand recevied: " + context.Message.ToJson());
                await _systemManager.LockProvider(context.Message.ProviderCode, context.Message.TimeClose);
            }
            catch (Exception e)
            {
                _logger.LogError($"LockProviderCommand error: {e}");
            }
        }
    }
}
