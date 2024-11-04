using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Hangfire;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Notifications;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.Notifications
{
    public class NotificationScheduleManager : TopupDomainServiceBase, INotificationScheduleManager
    {
        private readonly IRepository<NotificationSchedule> _notificationrepository;
        private readonly INotificationSender _notificationSender;
        private readonly ILogger<NotificationScheduleManager> _logger;
        private readonly UserManager _userManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NotificationScheduleManager(IRepository<NotificationSchedule> notificationrepository,
            INotificationSender notificationSender, ILogger<NotificationScheduleManager> logger,
            UserManager userManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _notificationrepository = notificationrepository;
            _notificationSender = notificationSender;
            _logger = logger;
            _userManager = userManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task PublishNotifications(int messageId)
        {
            using var uow = _unitOfWorkManager.Begin();
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                try
                {
                    _logger.LogInformation($"ExcuteSendNotifications:{messageId}");
                    var message = await _notificationrepository.FirstOrDefaultAsync(x =>
                        x.Id == messageId && x.Status == CommonConst.SendNotificationStatus.Approved);
                    if (message != null)
                    {
                        //Update trạng thái đã gửi trước.
                        message.DateSend = DateTime.Now;
                        message.Status = CommonConst.SendNotificationStatus.Published;
                        await _notificationrepository.UpdateAsync(message);

                        var listUser = new List<User>();
                        if (message.UserId != null)
                        {
                            var user = await _userManager.GetUserByIdAsync(message.UserId ?? 0);
                            listUser.Add(user);
                        }
                        else if (message.AgentType > 0)
                        {
                            var userTypes = await _userManager.GetUserByAgentTypeAsync(message.AgentType);
                            listUser.AddRange(userTypes);
                        }

                        if (!listUser.Any())
                        {
                            message.Status = CommonConst.SendNotificationStatus.Failed;
                            message.ExtraInfo = "Không lấy được danh sách tài khoản";
                            await _notificationrepository.UpdateAsync(message);
                            return;
                        }

                        await SendNotificationToAccount(listUser, message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"ExcuteSendNotificationsError:{ex}");
                }
            }
            await uow.CompleteAsync();
        }


        public async Task ScheduleNotification(NotificationSchedule message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message.ScheduleId))
                {
                    BackgroundJob.Delete(message.ScheduleId);
                    var newJobId =
                        BackgroundJob.Schedule<INotificationScheduleManager>((x) => x.PublishNotifications(message.Id),
                            message.DateSchedule);
                    if (!string.IsNullOrEmpty(newJobId))
                    {
                        message.ScheduleId = newJobId;
                    }
                }
                else
                {
                    var jobId = BackgroundJob.Schedule<INotificationScheduleManager>(
                        (x) => x.PublishNotifications(message.Id), message.DateSchedule);
                    if (!string.IsNullOrEmpty(jobId))
                    {
                        message.ScheduleId = jobId;
                    }
                }

                await _notificationrepository.UpdateAsync(message);
            }
            catch (Exception e)
            {
                _logger.LogError($"ScheduleNotification_Error:{e}");
            }
        }

        public async Task SendNowNotification(int messageId)
        {
            try
            {
                var message = await _notificationrepository.FirstOrDefaultAsync(messageId);
                if (message != null)
                {
                    BackgroundJob.Enqueue<INotificationScheduleManager>((x) => x.PublishNotifications(messageId));
                    //Nếu gửi ngay thì xóa  job đã lên lịch trước đó
                    if (!string.IsNullOrEmpty(message.ScheduleId))
                    {
                        BackgroundJob.Delete(message.ScheduleId);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"SendNowNotification_Error:{messageId}");
            }
        }

        private async Task SendNotificationToAccount(IEnumerable<User> listUser, NotificationSchedule message)
        {
            try
            {
                foreach (var item in listUser)
                {
                    if (item == null) continue;
                    var stringReplaces = new Dictionary<string, string>
                    {
                        {"{firt_name}", item.Surname},
                        {"{last_name}", item.Name},
                        {"{full_name}", item.FullName},
                        {"{email}", item.EmailAddress},
                        {"{phone}", item.PhoneNumber}
                    };
                    var bodyMessage = ReplaceStringWithToken(stringReplaces, message.Body);
                    var dataNotifi = new SendNotificationData
                    {
                    };
                    await _notificationSender.PublishNotification(item.AccountCode,
                        AppNotificationNames.SystemSchedule, dataNotifi, bodyMessage, message.Title);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendNotificationToAccountError:{ex}");
            }
        }

        private static string ReplaceStringWithToken(Dictionary<string, string> tokens, string input)
        {
            if (string.IsNullOrEmpty(input) || tokens == null || tokens.Count == 0) return input;
            var b = new StringBuilder(input);
            foreach (var (key, value) in tokens.Where(token => b.ToString().Contains(token.Key)))
            {
                b.Replace(key, value);
            }
            return b.ToString();
        }
    }
}
