using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using HLS.Topup.Authorization.Organization;
using Microsoft.AspNetCore.Identity;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Debugging;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Notifications;
using HLS.Topup.Validation;
using Microsoft.Extensions.Logging;
using NLog;

namespace HLS.Topup.Authorization.Users
{
    public class UserRegistrationManager : TopupDomainServiceBase
    {
        private IAbpSession AbpSession { get; set; }

        private IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        //private readonly Logger _logger = LogManager.GetLogger("UserRegistrationManager");
        private readonly ILogger<UserRegistrationManager> _logger;
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private IOrganizationsUnitCustomManager _organizationsUnitCustomManager;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy, IOrganizationsUnitCustomManager organizationsUnitCustomManager,
            ILogger<UserRegistrationManager> logger)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;
            _organizationsUnitCustomManager = organizationsUnitCustomManager;
            _logger = logger;

            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName,
            string plainPassword, bool isEmailConfirmed, string emailActivationLink)
        {
            CheckForTenant();
            CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
            var isNewRegisteredUserActiveByDefault =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                    .IsNewRegisteredUserActiveByDefault);

            await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = isNewRegisteredUserActiveByDefault,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
            foreach (var defaultRole in defaultRoles)
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }


        public async Task<User> CreateUserAsync(CreateAccountDto input)
        {
            try
            {
                bool isDefaultEmail = false;
                if (AbpSession.TenantId != null)
                    CheckForTenant();
                CheckSelfRegistrationIsEnabled();
                if (string.IsNullOrEmpty(input.EmailAddress))
                {
                    isDefaultEmail = true;
                    input.EmailAddress = input.PhoneNumber + "@default.com";
                }

                var checkPhone = await _userManager.ValidateEmailPhone(input.PhoneNumber, input.EmailAddress, null);
                if (checkPhone.ResponseCode != ResponseCodeConst.ResponseCode_Success)
                    throw new UserFriendlyException("Số điên thoại/Email không hợp lệ hoặc đã tồn tại");
                if (!string.IsNullOrEmpty(input.AccountCode))
                {
                    if (!ValidationHelper.IsAccountCode(input.AccountCode))
                        throw new UserFriendlyException("Mã tài khoản không hợp lệ");
                }

                var tenant = await GetActiveTenantAsync();
                bool isNewRegisteredUserActiveByDefault;
                if (tenant != null)
                    isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings
                        .UserManagement
                        .IsNewRegisteredUserActiveByDefault);
                else isNewRegisteredUserActiveByDefault = true;

                User parentUser = null;
                //user.ParentId = AbpSession.UserId;
                if (!string.IsNullOrEmpty(input.ParentAccount) &&
                    input.AccountType == CommonConst.SystemAccountType.Agent)
                {
                    parentUser = await _userManager.GetUserByAccountCodeAsync(input.ParentAccount);
                }
                else if (input.AccountType == CommonConst.SystemAccountType.MasterAgent)
                {
                    parentUser = await _userManager.GetUserByAccountTypeAsync(CommonConst.SystemAccountType.Company);
                }
                else if (input.AccountType == CommonConst.SystemAccountType.Company)
                {
                    var checkUser = await _userManager.GetUserByAccountTypeAsync(CommonConst.SystemAccountType.Company);
                    if (checkUser != null)
                        throw new UserFriendlyException("Tài khoản công ty đã tồn tại.");
                }

                if (input.AccountType != CommonConst.SystemAccountType.System &&
                    input.AccountType != CommonConst.SystemAccountType.Staff &&
                    input.AccountType != CommonConst.SystemAccountType.Company)
                {
                    if (parentUser == null)
                        throw new UserFriendlyException("Chưa nhập cấp trên");
                }

                if (tenant != null)
                    await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

                var user = new User
                {
                    TenantId = tenant?.Id,
                    Name = input.Name,
                    Surname = input.Surname,
                    EmailAddress = input.EmailAddress,
                    IsActive = input.IsCheckStatus ? input.IsActive : isNewRegisteredUserActiveByDefault,
                    UserName = input.PhoneNumber,
                    PhoneNumber = input.PhoneNumber,
                    IsEmailConfirmed = input.IsEmailConfirmed,
                    Roles = new List<UserRole>(),
                    IsDefaultEmail = isDefaultEmail,
                    AccountType = input.AccountType,
                    AgentType = input.AgentType,
                    IsVerifyAccount = input.IsVerifyAccount,
                    AgentName = input.AgentName
                };
                if (string.IsNullOrEmpty(input.Password))
                {
                    var randomPassword = await _userManager.CreateRandomPassword();
                    //user.Password = _passwordHasher.HashPassword(user, randomPassword);
                    input.Password = randomPassword;
                }

                user.SetNormalizedNames();

                var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
                foreach (var defaultRole in defaultRoles)
                {
                    user.Roles.Add(new UserRole(tenant?.Id, user.Id, defaultRole.Id));
                }

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await _userManager.CreateAsync(user, input.Password));
                if ((user.AccountType == CommonConst.SystemAccountType.Staff ||
                     user.AccountType == CommonConst.SystemAccountType.StaffApi) &&
                    !string.IsNullOrEmpty(input.AccountCodeRef))
                {
                    user.AccountCode = input.AccountCodeRef;
                }
                else if (!string.IsNullOrEmpty(input.AccountCode))
                {
                    user.AccountCode = input.AccountCode;
                }
                else
                {
                    user.AccountCode = user.GenAccountCode();
                }

                user.TreePath = user.AccountCode;
                if (parentUser != null)
                {
                    user.TreePath = parentUser.TreePath + "-" + user.AccountCode;
                    user.ParentId = parentUser.Id;
                    //user.AccountType = CommonConst.SystemAccountType.Agent;
                    user.NetworkLevel = parentUser.NetworkLevel + 1;
                }

                if (user.AccountType == CommonConst.SystemAccountType.Agent
                    || user.AccountType == CommonConst.SystemAccountType.Company
                    || user.AccountType == CommonConst.SystemAccountType.MasterAgent)
                {
                    await _organizationsUnitCustomManager.CreateOrganizationUnit(user.UserName + " - " + user.FullName,
                        user.ParentId, user.Id, user.TenantId);
                }

                if (user.AgentType == CommonConst.AgentType.AgentApi)
                {
                    await _userManager.AddToRoleAsync(user, user.AgentType.ToString("G"));
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, user.AccountType.ToString("G"));
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                if (!user.IsEmailConfirmed)
                {
                    user.SetNewEmailConfirmationCode();
                    await _userEmailer.SendEmailActivationLinkAsync(user, input.EmailActivationLink);
                }

                //Notifications
                if (tenant != null)
                    await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(
                        user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);
                await _appNotifier.NewUserRegisteredAsync(user);

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException(e.Message);
            }
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
