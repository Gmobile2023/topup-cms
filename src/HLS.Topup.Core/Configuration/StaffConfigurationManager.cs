using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.RequestDtos;
using HLS.Topup.Sale;
using HLS.Topup.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServiceStack;
using Task = System.Threading.Tasks.Task;

namespace HLS.Topup.Configuration
{
    public class StaffConfigurationManager : TopupDomainServiceBase, IStaffConfigurationManager
    {
        private readonly IRepository<StaffConfiguration> _staffRepository;
        private readonly UserRegistrationManager _registrationManager;
        private readonly TopupAppSession _topupAppSession;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly UserManager _userManager;
        private readonly IAccountManager _accountManager;

        //private readonly Logger _logger = LogManager.GetLogger("StaffConfigurationManager");
        private readonly ILogger<StaffConfigurationManager> _logger;
        private readonly string _serviceApi;

        public StaffConfigurationManager(IRepository<StaffConfiguration> staffRepository,
            UserRegistrationManager registrationManager,
            TopupAppSession topupAppSession, UserManager userManager, IWebHostEnvironment env,
            ILogger<StaffConfigurationManager> logger,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, IAccountManager accountManager)
        {
            _staffRepository = staffRepository;
            _registrationManager = registrationManager;
            _topupAppSession = topupAppSession;
            _userManager = userManager;
            _logger = logger;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _accountManager = accountManager;
            var appConfiguration = env.GetAppConfiguration();
            _serviceApi = appConfiguration["TopupService:ServiceApi"];
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task CreateUserStaff(CreateStaffUserInput input)
        {
            var userNetwork = input.ParentUserId != null
                ? _userManager.GetAccountInfoById(input.ParentUserId ?? 0)
                : _userManager.GetAccountInfo();

            if (userNetwork?.NetworkInfo == null || userNetwork.NetworkInfo.OrganizationUnitId <= 0)
                throw new UserFriendlyException("Thông tin mạng lưới không chính xác");
            if (userNetwork.NetworkInfo.AgentType != CommonConst.AgentType.AgentApi)
                if (!input.Days.Any())
                    throw new UserFriendlyException("Chưa chọn thông tin ngày");

            // if (input.FromTime.FromTime > input.ToTime.FromTime)
            //     throw new UserFriendlyException("Khoảng thời gian không hợp lệ");
            // if (input.FromTime.ToTime > input.ToTime.ToTime)
            //     throw new UserFriendlyException("Khoảng thời gian không hợp lệ");
            var checkTotal = await GetTotalStaffUser(userNetwork);
            var total = checkTotal + 1;
            var createUser = await _accountManager.CreateUserAsync(new CreateAccountDto
            {
                AccountCodeRef = userNetwork.NetworkInfo.AccountCode + "_" + total.ToString("D2"),
                Channel = CommonConst.Channel.WEB,
                Name = input.Name,
                Surname = input.Surname,
                Password = input.Password,
                AccountType = userNetwork.NetworkInfo.AgentType == CommonConst.AgentType.AgentApi
                    ? CommonConst.SystemAccountType.StaffApi
                    : CommonConst.SystemAccountType.Staff,
                PhoneNumber = input.PhoneNumber,
                UserName = input.UserName,
                IsEmailConfirmed = true,
                IsActive = input.IsActive,
                IsCheckStatus = true,
                EmailAddress = input.EmailAddress,
                ParentAccount = userNetwork.NetworkInfo.AccountCode
            });
            if (createUser == null)
                throw new UserFriendlyException("Tạo nhân viên không thành công");

            await _staffRepository.InsertAsync(new StaffConfiguration
            {
                Days = input.Days.ToJson(),
                FromTime = input.FromTime.ToJson(),
                ToTime = input.ToTime.ToJson(),
                LimitAmount = input.LimitAmount,
                LimitPerTrans = input.LimitPerTrans,
                UserId = createUser.Id,
                Description = input.Description
            });
            //Add staff vào nốt mạng của user
            await _userManager.AddToOrganizationUnitAsync(createUser.Id, userNetwork.NetworkInfo.OrganizationUnitId);
            await CurrentUnitOfWork.SaveChangesAsync();
            if (userNetwork.NetworkInfo.AgentType != CommonConst.AgentType.AgentApi)
                await SetLimitTransAccount(new CreateOrUpdateLimitAccountTransRequest
                {
                    AccountCode = createUser.AccountCode,
                    LimitPerDay = input.LimitAmount,
                    LimitPerTrans = input.LimitPerTrans
                });
        }

        public async Task<UserStaffDto> GetUserStaffInfo(long userId)
        {
            var config = await _staffRepository.GetAllIncluding(x => x.UserFk)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (config == null)
                throw new UserFriendlyException("Thông tin tài khoản không tồn tại");
            var info = config.UserFk.ConvertTo<UserStaffDto>();
            info.Days = config.Days.FromJson<List<int>>();
            info.FromTime = config.FromTime.FromJson<StaffTime>();
            info.ToTime = config.ToTime.FromJson<StaffTime>();
            info.Description = config.Description;
            info.LimitAmount = config.LimitAmount;
            info.LimitPerTrans = config.LimitPerTrans;
            return info;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task UpdateUserStaff(UpdateStaffUserInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId ?? 0);
            if (user == null)
                throw new UserFriendlyException("Thông tin tài khoản không tồn tại");
            if (!input.Days.Any())
                throw new UserFriendlyException("Chưa chọn thông tin ngày");

            var config = await _staffRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (config == null)
                throw new UserFriendlyException("Thông tin cấu hình nhân viên không tồn tại");

            // if (input.FromTime.FromTime > input.ToTime.FromTime)
            //     throw new UserFriendlyException("Khoảng thời gian không hợp lệ");
            // if (input.FromTime.ToTime > input.ToTime.ToTime)
            //     throw new UserFriendlyException("Khoảng thời gian không hợp lệ");

            config.Days = input.Days.ToJson();
            config.Description = input.Description;
            config.FromTime = input.FromTime.ToJson();
            config.ToTime = input.ToTime.ToJson();
            config.LimitAmount = input.LimitAmount;
            config.LimitPerTrans = input.LimitPerTrans;

            if (!string.IsNullOrEmpty(input.Name) && input.Name != user.Name)
                user.Name = input.Name;
            if (!string.IsNullOrEmpty(input.Surname) && input.Surname != user.Surname)
                user.Surname = input.Surname;
            if (input.IsActive != user.IsActive)
                user.IsActive = input.IsActive;
            await _staffRepository.UpdateAsync(config);
            await _userManager.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();
            //todo gọi sang core thiết lập hạn mức
            await SetLimitTransAccount(new CreateOrUpdateLimitAccountTransRequest
            {
                AccountCode = user.AccountCode,
                LimitPerDay = input.LimitAmount,
                LimitPerTrans = input.LimitPerTrans
            });
        }

        public async Task<bool> CheckStaffAction(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user.AccountType != CommonConst.SystemAccountType.Staff)
                return true;
            var staffConfig = await _staffRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (staffConfig == null)
                return false;
            var days = staffConfig.Days.FromJson<List<int>>();
            var date = DateTime.Now;
            var currentDay = DateTime.Today.DayOfWeek.ToString();
            var currentHour = int.Parse(date.Hour.ToString());
            var currentMinute = int.Parse(date.Minute.ToString());
            var fromTime = staffConfig.FromTime.FromJson<StaffTime>();
            var toTime = staffConfig.ToTime.FromJson<StaffTime>();
            var lstDay = days.Select(item => item == 8 ? "Sunday" : Enum.GetName(typeof(DayOfWeek), item - 1)).ToList();
            return lstDay.Contains(currentDay) && CampareTime(fromTime, toTime, currentHour, currentMinute);
        }

        private bool CampareTime(StaffTime fromTime, StaffTime toTime, int hour, int minute)
        {
            var checkTime = hour * 60 + minute;

            var fromHour = fromTime.FromTime;
            var fromMinute = fromTime.ToTime;
            var checkFrom = fromHour * 60 + fromMinute;

            var toHour = toTime.FromTime;
            var toMinute = toTime.ToTime;
            var checkTo = toHour * 60 + toMinute;

            return checkTime >= checkFrom && checkTime <= checkTo;
        }

        private async Task<ResponseMessageApi<object>> SetLimitTransAccount(
            CreateOrUpdateLimitAccountTransRequest input)
        {
            _logger.LogInformation($"SetLimitTransAccount request: {input.ToJson()}");
            var client = new JsonServiceClient(_serviceApi)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            try
            {
                var rs = await client.PostAsync<ResponseMessageApi<object>>(input);
                _logger.LogInformation($"SetLimitTransAccount return: {rs.ToJson()}");
                return rs;
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"SetLimitTransAccount error: {ex}");
                return null;
            }
        }

        private async Task<int> GetTotalStaffUser(UserAccountInfoDto networkInfo)
        {
            try
            {
                return await _userOrganizationUnitRepository.GetAll()
                    .Where(x => x.OrganizationUnitId == networkInfo.NetworkInfo.OrganizationUnitId).CountAsync();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
