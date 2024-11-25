using System;
using System.Threading.Tasks;
using System.Web;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using HLS.Topup.AccountManager;
using Microsoft.AspNetCore.Identity;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Impersonation;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Configuration;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Security.Recaptcha;
using HLS.Topup.Url;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Common;
using HLS.Topup.Security;
using HLS.Topup.Security.Dto;
using Microsoft.Extensions.Logging;
using StringExtensions = ServiceStack.StringExtensions;
using Abp.Auditing;

namespace HLS.Topup.Authorization.Accounts
{
    public class AccountAppService : TopupAppServiceBase, IAccountAppService
    {
        private IAppUrlService AppUrlService { get; set; }
        private readonly ILogger<AccountAppService> _logger;
        private IRecaptchaValidator RecaptchaValidator { get; set; }
        private readonly IOtpAppService _otpAppService;
        private readonly IUserEmailer _userEmailer;
        //private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWebUrlService _webUrlService;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IAccountManager _accountManager;

        public AccountAppService(
            IUserEmailer userEmailer,
            //UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IPasswordHasher<User> passwordHasher,
            IWebUrlService webUrlService,
            IUserDelegationManager userDelegationManager, IOtpAppService otpAppService,
            ILogger<AccountAppService> logger, IAccountManager accountManager)
        {
            _userEmailer = userEmailer;
            //_userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _passwordHasher = passwordHasher;
            _webUrlService = webUrlService;

            AppUrlService = NullAppUrlService.Instance;
            RecaptchaValidator = NullRecaptchaValidator.Instance;
            _userDelegationManager = userDelegationManager;
            _otpAppService = otpAppService;
            _logger = logger;
            _accountManager = accountManager;
            //_otpAppService = otpAppService;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id,
                _webUrlService.GetServerRootAddress(input.TenancyName));
        }

        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }

        public async Task<RegisterOutput> Register(RegisterInput model)
        {
            // if (UseCaptchaOnRegistration())
            // {
            //     await RecaptchaValidator.ValidateAsync(input.CaptchaResponse);
            // }
            //
            // var user = await _userRegistrationManager.RegisterAsync(
            //     input.Name,
            //     input.Surname,
            //     input.EmailAddress,
            //     input.UserName,
            //     input.Password,
            //     false,
            //     AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            // );
            //
            // var isEmailConfirmationRequiredForLogin =
            //     await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
            //         .IsEmailConfirmationRequiredForLogin);
            //
            // return new RegisterOutput
            // {
            //     CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            // };
            _logger.LogInformation($"Register request:{StringExtensions.ToJson(model)}");
            var user = await _accountManager.CreateUserAsync(new CreateAccountDto
                {
                    Channel = CommonConst.Channel.APP,
                    Name = model.Name,
                    Surname = model.Surname,
                    Password = model.Password,
                    PhoneNumber = model.UserName,
                    AccountType = CommonConst.SystemAccountType.MasterAgent,
                    AgentType = CommonConst.AgentType.Agent,
                    EmailAddress = model.EmailAddress,
                    //UserName = model.PhoneNumber
                    IsEmailConfirmed = true,
                    //EmailActivationLink = _appUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
                }
            );
            _logger.LogInformation($"Register return:{user.UserName}");
            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed)
            };
        }

        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            if (!string.IsNullOrEmpty(input.PhoneNumber))
            {
                var user = await UserManager.GetUserByUserNameAsync(input.PhoneNumber);
                if (user == null)
                {
                    throw new UserFriendlyException("Tài khoản không tồn tại trên hệ thống");
                }

                user.PasswordResetCode = new Random().Next(0, 9999).ToString("0000");
                await _otpAppService.SendOtpAsync(new OtpRequestInput
                {
                    Code = user.PasswordResetCode,
                    Type = CommonConst.OtpType.ResetPass,
                    PhoneNumber = input.PhoneNumber
                });
            }
            else if (!string.IsNullOrEmpty(input.EmailAddress))
            {
                var user = await GetUserByChecking(input.EmailAddress);
                if (user == null)
                {
                    throw new UserFriendlyException("Tài khoản không tồn tại trên hệ thống");
                }
                user.SetNewPasswordResetCode();
                await _userEmailer.SendPasswordResetLinkAsync(
                    user,
                    AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                );
            }
        }

        public Task<bool> CheckOdpavailable(string phoneNumber)
        {
            return _otpAppService.CheckOdpAvailable(phoneNumber);
        }

        public async Task SendResetCode(SendResetCodeInput input)
        {
            await _otpAppService.SendOtpAsync(new OtpRequestInput
            {
                Code = input.ConfirmCode,
                Type = CommonConst.OtpType.ResetPass,
                PhoneNumber = input.PhoneNumber
            });
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            User user = null;
            if (input.UserId > 0)
            {
                user = await UserManager.GetUserByIdAsync(input.UserId);
            }
            else if (!string.IsNullOrEmpty(input.PhoneNumber))
            {
                user = await UserManager.GetUserByUserNameAsync(input.PhoneNumber);
            }

            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            if (user.PasswordResetCode != input.ResetCode && input.Channel == CommonConst.Channel.WEB)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            var isUseVerify =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                    .IsUseVerifyResetPass);
            if (input.IsVerify && isUseVerify)
            {
                await VerifyOtp(new OtpConfirmInput
                {
                    Otp = input.Channel == CommonConst.Channel.WEB ? input.Otp : input.ResetCode,
                    Type = CommonConst.OtpType.ResetPass,
                    PhoneNumber = user.PhoneNumber
                });
            }

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                UserName = user.UserName
            };
        }


        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(
                user,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            );
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user != null && user.IsEmailConfirmed)
            {
                return;
            }

            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() ||
                user.EmailConfirmationCode != input.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"),
                    L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await UserManager.UpdateAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
        {
            var userDelegation = await _userDelegationManager.GetAsync(input.UserDelegationId);
            if (userDelegation.TargetUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("User delegation error.");
            }

            return new ImpersonateOutput
            {
                ImpersonationToken =
                    await _impersonationManager.GetImpersonationToken(userDelegation.SourceUserId,
                        userDelegation.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(userDelegation.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        public virtual async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken =
                    await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TargetTenantId)
            };
        }

        /// <summary>
        /// Hàm kiểm tra thông tin số điện thoại hoặc email.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Trước khi gọi hàm Register cần gọi hàm ValidateAccount để kiểm tra xem SĐT hoặc email (nếu có) có hợp lệ không. Nếu SĐT hợp lệ hệ thống sẽ gửi OTP về SĐT để KH xác nhận trước khi tiếp tục đăng ký
        /// </remarks>GetReportDetailList
        public async Task ValidateAccount(VerifyAccountDto model)
        {
            var checkPhone = await UserManager.ValidateAccountRegister(model.PhoneNumber);
            if (checkPhone.ResponseCode != "1")
                throw new UserFriendlyException(checkPhone.ResponseMessage);
            if (model.IsSendCode)
                await _otpAppService.SendOtpAsync(new OtpRequestInput
                {
                    Type = CommonConst.OtpType.Register,
                    PhoneNumber = model.PhoneNumber
                });
        }

        public async Task SendOtp(OtpRequestInput model)
        {
            await _otpAppService.SendOtpAsync(model);
        }

        public async Task VerifyOtp(OtpConfirmInput model)
        {
            await _otpAppService.VerifyOtpAsync(model);
        }


        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
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

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        [DisableAuditing]
        public string GetPing()
        {
            return "OK";
        }
    }
}
