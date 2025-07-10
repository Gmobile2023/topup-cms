using System;
using System.Threading.Tasks;
using Abp.Notifications;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.RequestDtos;
using MassTransit;
using Microsoft.Extensions.Logging;
using ServiceStack;
using Topup.Contracts.Commands.Commons;
using Topup.Contracts.Requests.Commons;

namespace HLS.Topup.Notifications
{
    public class NotificationSender : TopupDomainServiceBase, INotificationSender
    {
        private readonly INotificationManger _notificationManger;
        private readonly IBus _bus;
        private readonly ILogger<NotificationManger> _logger;

        public NotificationSender(INotificationManger notificationManger, IBus bus, ILogger<NotificationManger> logger)
        {
            _notificationManger = notificationManger;
            _bus = bus;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string notificationName, string accountcode, string message,
            string title,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            try
            {
                await _notificationManger.SendNotification(new SendNotificationRequest
                {
                    Body = message,
                    Title = title,
                    AppNotificationName = notificationName,
                    AccountCode = accountcode,
                    Severity = severity.ToString("G")
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SendNotificationDataAsync(string accountcode, string notificationName,
            SendNotificationData data, string message, string title,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            try
            {
                var notificationData = new NotificationAppData
                {
                    Properties = data,
                    Type = AppNotificationNames.Payment
                };
                await _notificationManger.SendNotification(new SendNotificationRequest
                {
                    Body = message,
                    Title = title,
                    Data = notificationData.ToJson(),
                    AccountCode = accountcode,
                    AppNotificationName = notificationName,
                    Severity = severity.ToString("G")
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SendNotificationDataAsync(SendNotificationRequest request)
        {
            try
            {
                await _notificationManger.SendNotification(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task PublishNotification(string accountcode, string notificationName, SendNotificationData data,
            string message, string title,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            try
            {
                _logger.LogInformation($"PublishNotification:{accountcode}-{title}-{message}");
                var notificationData = new NotificationAppData
                {
                    Properties = data,
                    Type = AppNotificationNames.Payment
                };
                await _bus.Publish<NotificationSendCommand>(new
                {
                    Body = message,
                    Data = notificationData.ToJson(),
                    Title = title,
                    ReceivingAccount = accountcode,
                    AppNotificationName = notificationName,
                    TimeStamp = DateTime.Now,
                    CorrelationId = Guid.NewGuid()
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"PublishNotification_ERROR:{e}");
            }
        }

        public async Task PublishTeleMessage(SendTeleMessageRequest request)
        {
            try
            {
                _logger.LogInformation($"PublishTeleMessage:{request.ToJson()}");

                await _bus.Publish<SendBotMessage>(new
                {
                    request.Title,
                    request.Code,
                    request.Message,
                    request.Module,
                    BotType=(BotType)request.BotType,
                    MessageType=(BotMessageType)request.MessageType,
                    TimeStamp=DateTime.Now,
                    CorrelationId=Guid.NewGuid()
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"PublishTeleMessage:{e}");
            }
        }

        public async Task PublishTeleToGroupMessage(SendTeleMessageRequest request)
        {
            try
            {
                _logger.LogInformation($"PublishTeleToGroupMessage:{request.ToJson()}");

                await _bus.Publish<SendBotMessageToGroup>(new
                {
                    request.Title,
                    request.Code,
                    request.Message,
                    request.Module,
                    BotType=(BotType)request.BotType,
                    MessageType=(BotMessageType)request.MessageType,
                    request.ChatId,
                    TimeStamp=DateTime.Now,
                    CorrelationId=Guid.NewGuid()
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"PublishTeleToGroupMessage:{e}");
            }
        }
    }
}
