using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Notifications
{
    public class NotificationManger : TopupDomainServiceBase, INotificationManger
    {
        private readonly TokenHepper _tokenHepper;

        private readonly string _serviceApi;

        //private readonly Logger _logger = LogManager.GetLogger("CardManager");
        private readonly ILogger<NotificationManger> _logger;

        public NotificationManger(ILogger<NotificationManger> logger, TokenHepper tokenHepper, IWebHostEnvironment env)
        {
            _logger = logger;
            _tokenHepper = tokenHepper;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        public async Task<ResponseMessageApi<bool>> SendNotification(SendNotificationRequest request)
        {
            _logger.LogInformation($"SendNotification request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.PostAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"SendNotification return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SendNotification error:{ex}");
                return null;
            }
        }

        public async Task<ApiResponseDto<List<NotificationAppOutDto>>> GetUserNotifications(
            GetUserNotificationRequest request)
        {
            _logger.LogInformation($"GetUserNotifications request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ApiResponseDto<List<NotificationAppOutDto>>>(request);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetUserNotifications error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<NotificationAppOutDto>> GetNotification(GetNotificationRequest request)
        {
            _logger.LogInformation($"GetNotification request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ResponseMessageApi<NotificationAppOutDto>>(request);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetNotification error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<ShowNotificationDto>> GetLastNotificationRequest(
            GetLastNotificationRequest request)
        {
            _logger.LogInformation($"GetLastNotificationRequest request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                return await client.GetAsync<ResponseMessageApi<ShowNotificationDto>>(request);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"GetLastNotificationRequest error:{ex}");
                return new ResponseMessageApi<ShowNotificationDto>();
            }
        }

        public async Task<ResponseMessageApi<bool>> SetAllNotificationsAsRead(SetAllNotificationsAsReadRequest request)
        {
            _logger.LogInformation($"SetAllNotificationsAsRead request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.PostAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"SetAllNotificationsAsRead return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SetAllNotificationsAsRead error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<bool>> SetNotificationAsRead(SetNotificationAsReadRequest request)
        {
            _logger.LogInformation($"SetNotificationAsRead request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.PostAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"SetNotificationAsRead return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SetNotificationAsRead error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<bool>> DeleteNotification(DeleteNotificationRequest request)
        {
            _logger.LogInformation($"DeleteNotification request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.DeleteAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"DeleteNotification return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"DeleteNotificationRequest error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<bool>> SubscribeNotification(SubscribeNotificationRequest request)
        {
            _logger.LogInformation($"SubscribeNotification request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.PostAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"SubscribeNotification return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"SubscribeNotificationRequest error:{ex}");
                return null;
            }
        }

        public async Task<ResponseMessageApi<bool>> UnSubscribeNotification(UnSubscribeNotificationRequest request)
        {
            _logger.LogInformation($"UnSubscribeNotification request:{request.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs= await client.PostAsync<ResponseMessageApi<bool>>(request);
                _logger.LogInformation($"UnSubscribeNotification return:{rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"UnSubscribeNotification error:{ex}");
                return null;
            }
        }
    }
}
