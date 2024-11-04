using System;
using System.Net;
using System.Reflection;
using Abp.Application.Services;
using HLS.Topup.Common;
using MassTransit;

namespace HLS.Topup.Web.AppVersion
{
    public class AlarmAppVersion : IApplicationService
    {
        private readonly IBus _bus;

        public AlarmAppVersion(IBus bus)
        {
            _bus = bus;
        }

        public void AlarmVersion()
        {
            var version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
            Console.WriteLine($"Starting WebApi with Version:{version}");
            _bus.Publish<SendBotMessage>(new
            {
                Message = $"Service WebApi đã được update với phiên bản:{version}",
                Module = Dns.GetHostName(),
                MessageType = CommonConst.BotMessageType.Message,
                Title = $"Starting WebApi Service",
                BotType = CommonConst.BotType.Dev,
                TimeStamp = DateTime.Now,
                CorrelationId = Guid.NewGuid()
            });
        }
    }
}
