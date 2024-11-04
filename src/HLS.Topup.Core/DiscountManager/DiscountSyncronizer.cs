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

namespace HLS.Topup.DiscountManager
{
    public class DiscountSyncronizer :
        IEventHandler<EntityCreatedEventData<Discount>>,
        IEventHandler<EntityUpdatedEventData<Discount>>,
        IEventHandler<EntityDeletedEventData<Discount>>,

        IEventHandler<EntityCreatedEventData<DiscountDetail>>,
        IEventHandler<EntityUpdatedEventData<DiscountDetail>>,
        IEventHandler<EntityDeletedEventData<DiscountDetail>>,

        IEventHandler<EntityCreatedEventData<ServiceConfiguration>>,
        IEventHandler<EntityUpdatedEventData<ServiceConfiguration>>,
        IEventHandler<EntityDeletedEventData<ServiceConfiguration>>,

        IEventHandler<EntityCreatedEventData<Product>>,
        IEventHandler<EntityUpdatedEventData<Product>>,
        IEventHandler<EntityDeletedEventData<Product>>,

        IEventHandler<EntityCreatedEventData<Category>>,
        IEventHandler<EntityUpdatedEventData<Category>>,
        IEventHandler<EntityDeletedEventData<Category>>,

        IEventHandler<EntityCreatedEventData<Provider>>,
        IEventHandler<EntityUpdatedEventData<Provider>>,
        IEventHandler<EntityDeletedEventData<Provider>>,
        ITransientDependency,
        
        IEventHandler<EntityCreatedEventData<PartnerServiceConfiguration>>,
        IEventHandler<EntityUpdatedEventData<PartnerServiceConfiguration>>,
        IEventHandler<EntityDeletedEventData<PartnerServiceConfiguration>>
    {
        private readonly ILogger<DiscountSyncronizer> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IRedisCache _redisCache;
        private readonly INotificationSender _appNotifier;
        public DiscountSyncronizer(ILogger<DiscountSyncronizer> logger, ICacheManager cacheManager, IRedisCache redisCache, INotificationSender appNotifier)
        {
            _logger = logger;
            _cacheManager = cacheManager;
            _redisCache = redisCache;
            _appNotifier = appNotifier;
        }

        public void HandleEvent(EntityCreatedEventData<Discount> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được thêm mới",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });

                _logger.LogInformation("DiscountEventCreated-ClearCache");
                ClearDiscountCache();


            }
            catch (Exception e)
            {
                _logger.LogError("DiscountEventCreated error:{e}");
            }
        }

        public void HandleEvent(EntityUpdatedEventData<Discount> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được cập nhật",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("DiscountEventUpdated-ClearCache");
                ClearDiscountCache();
            }
            catch (Exception e)
            {
                _logger.LogError("DiscountEventUpdated error:{e}");
            }
        }

        public void HandleEvent(EntityDeletedEventData<Discount> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được xóa",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("DiscountEventDeleted-ClearCache");
                ClearDiscountCache();
            }
            catch (Exception e)
            {
                _logger.LogError("DiscountEventDeleted error:{e}");
            }
        }

        public void HandleEvent(EntityCreatedEventData<DiscountDetail> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được thêm mới",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("DiscountDetailEventCreated-ClearCache");
                ClearDiscountCache();
            }
            catch (Exception e)
            {
                _logger.LogError("DiscountDetailEventCreated error:{e}");
            }
        }

        public void HandleEvent(EntityUpdatedEventData<DiscountDetail> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được cập nhật",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("DiscountDetailEventUpdated-ClearCache");
                ClearDiscountCache();
            }
            catch (Exception e)
            {
                _logger.LogError("DiscountDetailEventUpdated error:{e}");
            }
        }

        public void HandleEvent(EntityDeletedEventData<DiscountDetail> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Chính sách được xóa",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("DiscountDetailEventDeleted-ClearCache");
                ClearDiscountCache();
            }
            catch (Exception e)
            {
                _logger.LogError("DiscountDetailEventDeleted error:{e}");
            }
        }

        public void HandleEvent(EntityCreatedEventData<Product> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Sản phẩm được thêm mới",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("ProductEventCreated-ClearCache");
                ClearDiscountCache();
                ClearServiceConfiguationsCache();
                ClearProductCache();
            }
            catch (Exception e)
            {
                _logger.LogError("ProductEventCreated error:{e}");
            }
        }

        public void HandleEvent(EntityUpdatedEventData<Product> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Sản phẩm được cập nhật",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("ProductEventUpdated-ClearCache");
                ClearDiscountCache();
                ClearServiceConfiguationsCache();
                ClearProductCache();
            }
            catch (Exception e)
            {
                _logger.LogError("ProductEventUpdated error:{e}");
            }
        }

        public void HandleEvent(EntityDeletedEventData<Product> eventData)
        {
            try
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Message,
                    Title = "Sản phẩm được xóa",
                    Message = eventData.Entity.ToJson(),
                    Module = "WEB",
                });
                _logger.LogInformation("ProductEventDeleted-ClearCache");
                ClearDiscountCache();
                ClearServiceConfiguationsCache();
                ClearProductCache();
            }
            catch (Exception e)
            {
                _logger.LogError("ProductEventDeleted error:{e}");
            }
        }

        public void HandleEvent(EntityCreatedEventData<Category> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Loại sản phẩm được thêm mới",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            _logger.LogInformation("CategoryEventCreated-ClearCache");
            ClearDiscountCache();
            ClearServiceConfiguationsCache();
            ClearProductCache();
        }

        public void HandleEvent(EntityUpdatedEventData<Category> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Loại sản phẩm được cập nhật",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            _logger.LogInformation("CategoryEventCreated-ClearCache");
            ClearDiscountCache();
            ClearServiceConfiguationsCache();
            ClearProductCache();
        }

        public void HandleEvent(EntityDeletedEventData<Category> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Loại sản phẩm được xóa",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            _logger.LogInformation("CategoryEventDeleted-ClearCache");
            ClearDiscountCache();
            ClearServiceConfiguationsCache();
            ClearProductCache();
        }

        private void ClearDiscountCache()
        {
            try
            {
                _cacheManager.GetCache(CacheConst.DiscountCache).Clear();
                _redisCache.RemoveByPatternAsync("PayGate_ProductDiscount:*");
            }
            catch (Exception e)
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Error,
                    Title = "ClearServiceConfiguationsCache Error ",
                    Message = e.Message

                });
                throw e;
            }

        }
        private void ClearServiceConfiguationsCache()
        {
            try
            {
                //_cacheManager.GetCache(CacheConst.ServiceConfiguations).Clear();
                _redisCache.RemoveByPatternAsync("PayGate_ServiceConfiguations:*");
                _redisCache.RemoveByPatternAsync("PayGate_RatingTrans:*");
            }
            catch (Exception e)
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Error,
                    Title = "ClearServiceConfiguationsCache Error ",
                    Message = e.Message

                });
                throw e;
            }

        }
        private void ClearProductCache()
        {
            try
            {
                _cacheManager.GetCache(CacheConst.ProductInfo).Clear();
                _redisCache.RemoveByPatternAsync("PayGate_ProductInfo:*");
            }
            catch (Exception e)
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Error,
                    Title = "ClearProductCache Error ",
                    Message = e.Message
                });
                throw e;
            }

        }

        private void ClearProvidersCache()
        {
            try
            {
                _redisCache.RemoveByPatternAsync("PayGate_ProviderInfo:*");
            }
            catch (Exception e)
            {
                _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
                {
                    BotType = (byte)BotType.Dev,
                    MessageType = (byte)BotMessageType.Error,
                    Title = "ClearProvidersCache Error ",
                    Message = e.Message

                });
                throw e;
            }

        }

        public void HandleEvent(EntityCreatedEventData<ServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình dịch vụ được thêm mới",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }

        public void HandleEvent(EntityUpdatedEventData<ServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình dịch vụ được cập nhật",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }

        public void HandleEvent(EntityDeletedEventData<ServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình dịch vụ được xóa",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }

        public void HandleEvent(EntityCreatedEventData<Provider> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Nhà cung cấp được thêm mới",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
            //ClearProvidersCache();
        }

        public void HandleEvent(EntityUpdatedEventData<Provider> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Nhà cung cấp được cập nhật",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
            //ClearProvidersCache();
        }

        public void HandleEvent(EntityDeletedEventData<Provider> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Nhà cung cấp được xóa",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
            //ClearProvidersCache();
        }

        public void HandleEvent(EntityCreatedEventData<PartnerServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình đóng/mở dịch vụ được thêm mới",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }

        public void HandleEvent(EntityUpdatedEventData<PartnerServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình đóng/mở dịch vụ được cập nhật",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }

        public void HandleEvent(EntityDeletedEventData<PartnerServiceConfiguration> eventData)
        {
            _appNotifier.PublishTeleMessage(new RequestDtos.SendTeleMessageRequest
            {
                BotType = (byte)BotType.Dev,
                MessageType = (byte)BotMessageType.Message,
                Title = "Cấu hình đóng/mở dịch vụ được xóa",
                Message = eventData.Entity.ToJson(),
                Module = "WEB",
            });
            ClearServiceConfiguationsCache();
        }
    }
}