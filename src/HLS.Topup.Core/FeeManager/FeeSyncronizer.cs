using System;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using HLS.Topup.Categories;
using HLS.Topup.Common;
using HLS.Topup.Products;
using Microsoft.Extensions.Logging;
using HLS.Topup.Configuration;
using HLS.Topup.Notifications;
using ServiceStack;
using HLS.Topup.Providers;

namespace HLS.Topup.FeeManager
{
    public class FeeSyncronizer : ITransientDependency,
        IEventHandler<EntityCreatedEventData<Fee>>,
        IEventHandler<EntityUpdatedEventData<Fee>>,
        IEventHandler<EntityDeletedEventData<Fee>>,
        IEventHandler<EntityCreatedEventData<FeeDetail>>,
        IEventHandler<EntityUpdatedEventData<FeeDetail>>,
        IEventHandler<EntityDeletedEventData<FeeDetail>>
    {
        private readonly ILogger<FeeSyncronizer> _logger;
        private readonly IRedisCache _redisCache;
        private readonly INotificationSender _appNotifier;

        public FeeSyncronizer(ILogger<FeeSyncronizer> logger,
            IRedisCache redisCache, INotificationSender appNotifier)
        {
            _logger = logger;
            _redisCache = redisCache;
            _appNotifier = appNotifier;
        }

        public void HandleEvent(EntityCreatedEventData<Fee> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee  được thêm mới",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });

                _logger.LogInformation("FeeEventCreated-ClearCache");
            }
            catch (Exception e)
            {
                _logger.LogError("FeeEventCreated error:{e}");
            }
        }

        public void HandleEvent(EntityUpdatedEventData<Fee> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee được cập nhật",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("FeeEventUpdated-ClearCache");

            }
            catch (Exception e)
            {
                _logger.LogError("FeeEventUpdated error:{e}");
            }
        }

        public void HandleEvent(EntityDeletedEventData<Fee> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee được xóa",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("FeeEventDeleted-ClearCache");
            }
            catch (Exception e)
            {
                _logger.LogError("FeeEventDeleted error:{e}");
            }
        }

        public void HandleEvent(EntityCreatedEventData<FeeDetail> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee được thêm mới",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("FeeDetailEventCreated-ClearCache");
            }
            catch (Exception e)
            {
                _logger.LogError("FeeDetailEventCreated error:{e}");
            }
        }

        public void HandleEvent(EntityUpdatedEventData<FeeDetail> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee được cập nhật",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("FeeDetailEventUpdated-ClearCache");
            }
            catch (Exception e)
            {
                _logger.LogError("FeeDetailEventUpdated error:{e}");
            }
        }

        public void HandleEvent(EntityDeletedEventData<FeeDetail> eventData)
        {
            try
            {
                ClearFeeCache();
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách Fee được xóa",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("FeeDetailEventDeleted-ClearCache");
            }
            catch (Exception e)
            {
                _logger.LogError("FeeDetailEventDeleted error:{e}");
            }
        }
        private void ClearFeeCache()
        {
            try
            {
                _redisCache.RemoveByPatternAsync("PayGate_ProductFeeInfo:*");
            }
            catch (Exception e)
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Error,
                    Title = "Clear Fee cache Error ",
                    Message = e.Message
                });
                throw;
            }
        }

    }
}