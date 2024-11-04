using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Zero.Configuration;
using HLS.Topup.AccountManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using HLS.Topup.Authentication.TwoFactor.Google;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Accounts;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Authorization.Impersonation;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Debugging;
using HLS.Topup.Identity;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Net.Sms;
using HLS.Topup.Notifications;
using HLS.Topup.Web.Models.Account;
using HLS.Topup.Security;
using HLS.Topup.Security.Dto;
using HLS.Topup.Security.Recaptcha;
using HLS.Topup.Sessions;
using HLS.Topup.Url;
using HLS.Topup.Web.Authentication.External;
using HLS.Topup.Web.Authentication.JwtBearer;
using HLS.Topup.Web.Filters;
using HLS.Topup.Web.Security.Recaptcha;
using HLS.Topup.Web.Session;
using HLS.Topup.Web.Views.Shared.Components.TenantChange;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using StringExtensions = ServiceStack.StringExtensions;

namespace HLS.Topup.Web.Controllers
{
    //[ForgeryExceptionFilter]
    public class AccountController : TopupControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWebUrlService _webUrlService;
        private readonly IAppUrlService _appUrlService;
        private readonly IAppNotifier _appNotifier;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IUserLinkManager _userLinkManager;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly ITenantCache _tenantCache;

        private readonly IAccountAppService _accountAppService;

        //private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;
        private readonly IdentityOptions _identityOptions;
        private readonly ISessionAppService _sessionAppService;
        private readonly ExternalLoginInfoManagerFactory _externalLoginInfoManagerFactory;
        private readonly ISettingManager _settingManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IJwtSecurityStampHandler _securityStampHandler;
        private readonly IAccountManager _accountManager;

        //private readonly Logger _logger = LogManager.GetLogger("AccountController");
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IWebHostEnvironment env,
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            IAppNotifier appNotifier,
            IWebUrlService webUrlService,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            LogInManager logInManager,
            SignInManager signInManager,
            IRecaptchaValidator recaptchaValidator,
            ITenantCache tenantCache,
            IAccountAppService accountAppService,
            //UserRegistrationManager userRegistrationManager,
            IImpersonationManager impersonationManager,
            IAppUrlService appUrlService,
            IPerRequestSessionCache sessionCache,
            IEmailSender emailSender,
            ISmsSender smsSender,
            IPasswordComplexitySettingStore passwordComplexitySettingStore,
            IOptions<IdentityOptions> identityOptions,
            ISessionAppService sessionAppService,
            ExternalLoginInfoManagerFactory externalLoginInfoManagerFactory,
            ISettingManager settingManager,
            IUserDelegationManager userDelegationManager, ILogger<AccountController> logger,
            IJwtSecurityStampHandler securityStampHandler, IAccountManager accountManager)
        {
            _userManager = userManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _webUrlService = webUrlService;
            _appNotifier = appNotifier;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _recaptchaValidator = recaptchaValidator;
            _tenantCache = tenantCache;
            _accountAppService = accountAppService;
            //_userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _appUrlService = appUrlService;
            _sessionCache = sessionCache;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
            _identityOptions = identityOptions.Value;
            _sessionAppService = sessionAppService;
            _externalLoginInfoManagerFactory = externalLoginInfoManagerFactory;
            _settingManager = settingManager;
            _userDelegationManager = userDelegationManager;
            _logger = logger;
            _securityStampHandler = securityStampHandler;
            _accountManager = accountManager;
            _appConfiguration = env.GetAppConfiguration();
        }

        #region Login / Logout

        //[ForgeryExceptionFilter]
        public async Task<ActionResult> Login(string userNameOrEmailAddress = "", string returnUrl = "",
            string successMessage = "", string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                AbpSession.UserId > 0)
            {
                var updateUserSignInTokenOutput = await _sessionAppService.UpdateUserSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, updateUserSignInTokenOutput.SignInToken,
                    AbpSession.UserId.Value, AbpSession.TenantId);
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            ViewBag.SingleSignIn = ss;
            ViewBag.UseCaptcha = UseCaptchaOnLogin();
            var link = _appConfiguration["App:DownloadApp"];
            ViewBag.Android = link.Split("|")[1];
            ViewBag.Ios = link.Split("|")[0];
            return View(
                new LoginFormViewModel
                {
                    IsSelfRegistrationEnabled = IsSelfRegistrationEnabled(),
                    IsTenantSelfRegistrationEnabled = IsTenantSelfRegistrationEnabled(),
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        [HttpPost]
        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        //[ForgeryExceptionFilter]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "",
            string returnUrlHash = "", string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            if (UseCaptchaOnLogin())
            {
                await _recaptchaValidator.ValidateAsync(
                    HttpContext.Request.Form[RecaptchaValidator.RecaptchaResponseKey]);
            }

            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password,
                GetTenancyNameOrNull());

            if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                loginResult.Result == AbpLoginResultType.Success)
            {
                loginResult.User.SetSignInToken();
                returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                    loginResult.User.Id, loginResult.User.TenantId);
            }

            if (_settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser))
            {
                await _userManager.UpdateSecurityStampAsync(loginResult.User);
                await _securityStampHandler.SetSecurityStampCacheItem(loginResult.User.TenantId, loginResult.User.Id,
                    loginResult.User.SecurityStamp);
                loginResult.Identity.ReplaceClaim(new Claim(AppConsts.SecurityStampKey,
                    loginResult.User.SecurityStamp));
            }

            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                loginResult.User.SetNewPasswordResetCode();
                if (AccountTypeHepper.IsAccountBackend(loginResult.User.AccountType) ||
                    loginResult.User.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    var link =
                        $"/Account/ResetPassword?userId={loginResult.User.Id}&tenantId={AbpSession.TenantId}&resetCode={loginResult.User.PasswordResetCode}&singleSignIn={ss}&returnUrl={returnUrl}";
                    link = EncryptQueryParameters(link);
                    return Json(new AjaxResponse
                    {
                        TargetUrl = link
                    });
                    // return Json(new AjaxResponse
                    // {
                    //     TargetUrl = Url.Action(
                    //         "ResetPassword",
                    //         new ResetPasswordViewModel
                    //         {
                    //             TenantId = AbpSession.TenantId,
                    //             UserId = loginResult.User.Id,
                    //             ResetCode = loginResult.User.PasswordResetCode,
                    //             ReturnUrl = returnUrl,
                    //             SingleSignIn = ss,
                    //         })
                    // });
                }
                else
                {
                    var link =
                        $"/Account/ConfirmResetPassword?userId={loginResult.User.Id}&resetCode={loginResult.User.PasswordResetCode}&returnUrl={returnUrl}";
                    link = EncryptQueryParameters(link);
                    await _accountAppService.SendResetCode(new SendResetCodeInput
                    {
                        PhoneNumber = loginResult.User.PhoneNumber,
                        ConfirmCode = new Random().Next(0, 9999).ToString("0000")
                    });
                    return Json(new AjaxResponse
                    {
                        TargetUrl = link
                    });
                }
            }

            if (loginResult.User.AccountType != CommonConst.SystemAccountType.System &&
                returnUrl.ToLower().Contains("/app"))
                returnUrl = "/";
            //HoangLT nếu enable OTP thì confirm otp trước khi đăng nhập
            //todo confirm otp bằng poup trên giao diện luôn
            var isVerify =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsUseVerifyLogin);
            if (isVerify && !AccountTypeHepper.IsAccountBackend(loginResult.User.AccountType))
            {
                await _accountAppService.SendOtp(new OtpRequestInput
                {
                    Type = CommonConst.OtpType.Login,
                    PhoneNumber = loginModel.UsernameOrEmailAddress
                });

                var link =
                    $"/Account/ConfirmLogin?ConfirmId={loginResult.User.Id}&TenantId={AbpSession.TenantId}&PhoneNumber={loginModel.UsernameOrEmailAddress}&SingleSignIn={ss}&ReturnUrl={returnUrl}";
                link = EncryptQueryParameters(link);
                return Json(new AjaxResponse
                {
                    // TargetUrl = Url.Action(
                    //     "ConfirmLogin",
                    //     new ConfirmLoginViewModel()
                    //     {
                    //         TenantId = AbpSession.TenantId,
                    //         ConfirmId = loginResult.User.Id,
                    //         ReturnUrl = returnUrl,
                    //         SingleSignIn = ss,
                    //         PhoneNumber = loginModel.UsernameOrEmailAddress
                    //     })
                    TargetUrl = link
                });
            }


            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, loginModel.RememberMe);
            if (signInResult.RequiresTwoFactor)
            {
                return Json(new AjaxResponse
                {
                    TargetUrl = Url.Action(
                        "SendSecurityCode",
                        new
                        {
                            returnUrl = returnUrl,
                            rememberMe = loginModel.RememberMe
                        })
                });
            }

            //Debug.Assert(signInResult.Succeeded);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse {TargetUrl = returnUrl});
        }

        public async Task<ActionResult> Logout(string returnUrl = "")
        {
            await _signInManager.SignOutAsync();
            var userIdentifier = AbpSession.ToUserIdentifier();

            if (userIdentifier != null &&
                _settingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowOneConcurrentLoginPerUser))
            {
                var user = await _userManager.GetUserAsync(userIdentifier);
                await _userManager.UpdateSecurityStampAsync(user);
                await _securityStampHandler.RemoveSecurityStampCacheItem(AbpSession.TenantId,
                    AbpSession.GetUserId());
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = NormalizeReturnUrl(returnUrl);
                return Redirect(returnUrl);
            }

            return RedirectToAction("Login");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress,
            string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result,
                        usernameOrEmailAddress, tenancyName);
            }
        }

        private string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId,
            int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }

        public ActionResult SessionLockScreen()
        {
            ViewBag.UseCaptcha = UseCaptchaOnLogin();
            return View();
        }

        #endregion

        #region Two Factor Auth

        public async Task<ActionResult> SendSecurityCode(string returnUrl, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var userProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

            var factorOptions = userProviders.Select(
                userProvider =>
                    new SelectListItem
                    {
                        Text = userProvider,
                        Value = userProvider
                    }).ToList();

            return View(
                new SendSecurityCodeViewModel
                {
                    Providers = factorOptions,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> SendSecurityCode(SendSecurityCodeViewModel model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            if (model.SelectedProvider != GoogleAuthenticatorProvider.Name)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
                var message = L("EmailSecurityCodeBody", code);

                if (model.SelectedProvider == "Email")
                {
                    await _emailSender.SendAsync(await _userManager.GetEmailAsync(user), L("EmailSecurityCodeSubject"),
                        message);
                }
                else if (model.SelectedProvider == "Phone")
                {
                    await _smsSender.SendAsync(await _userManager.GetPhoneNumberAsync(user), code,
                        CommonConst.OtpType.Login);
                }
            }

            return RedirectToAction(
                "VerifySecurityCode",
                new
                {
                    provider = model.SelectedProvider,
                    returnUrl = model.ReturnUrl,
                    rememberMe = model.RememberMe
                }
            );
        }

        public async Task<ActionResult> VerifySecurityCode(string provider, string returnUrl, bool rememberMe)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new UserFriendlyException(L("VerifySecurityCodeNotLoggedInErrorMessage"));
            }

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var isRememberBrowserEnabled = await IsRememberBrowserEnabledAsync();

            return View(
                new VerifySecurityCodeViewModel
                {
                    Provider = provider,
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe,
                    IsRememberBrowserEnabled = isRememberBrowserEnabled
                }
            );
        }

        [HttpPost]
        public async Task<JsonResult> VerifySecurityCode(VerifySecurityCodeViewModel model)
        {
            model.ReturnUrl = NormalizeReturnUrl(model.ReturnUrl);

            CheckCurrentTenant(await _signInManager.GetVerifiedTenantIdAsync());

            var result = await _signInManager.TwoFactorSignInAsync(
                model.Provider,
                model.Code,
                model.RememberMe,
                await IsRememberBrowserEnabledAsync() && model.RememberBrowser
            );

            if (result.Succeeded)
            {
                return Json(new AjaxResponse {TargetUrl = model.ReturnUrl});
            }

            if (result.IsLockedOut)
            {
                throw new UserFriendlyException(L("UserLockedOutMessage"));
            }

            throw new UserFriendlyException(L("InvalidSecurityCode"));
        }

        private Task<bool> IsRememberBrowserEnabledAsync()
        {
            return SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                .IsRememberBrowserEnabled);
        }

        #endregion

        #region Register

        public async Task<ActionResult> Register(string returnUrl = "", string ss = "")
        {
            return RegisterView(new RegisterViewModel
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync(),
                ReturnUrl = returnUrl,
                SingleSignIn = ss
            });
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            CheckSelfRegistrationIsEnabled();

            ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();
            Logger.Info($"ViewBag.UseCaptcha:{ViewBag.UseCaptcha}");

            return View("Register", model);
        }

        [HttpPost]
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                _logger.LogInformation($"Regisger request: {StringExtensions.ToJson(model)}");
                // if (!model.IsExternalLogin && UseCaptchaOnRegistration())
                // {
                //     await _recaptchaValidator.ValidateAsync(
                //         HttpContext.Request.Form[RecaptchaValidator.RecaptchaResponseKey]);
                // }
                // var isVerify =
                //     await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                //         .IsUseVerifyRegister);
                // if (isVerify)
                // {
                //     await _accountAppService.VerifyOtp(new OtpConfirmInput
                //     {
                //         PhoneNumber = model.UserName,
                //         Otp = model.Otp,
                //         Type = CommonConst.OtpType.Register
                //     });
                //     _logger.LogInformation($"Done verify OTP");
                // }

                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new Exception("Can not external login!");
                    }

                    using (var providerManager =
                        _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.LoginProvider))
                    {
                        model.UserName =
                            providerManager.Object.GetUserNameFromClaims(externalLoginInfo.Principal.Claims.ToList());
                    }

                    model.Password = await _userManager.CreateRandomPassword();
                }
                else
                {
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValidMessage"));
                    }
                }

                var user = await _accountManager.CreateUserAsync(new CreateAccountDto
                    {
                        Channel = CommonConst.Channel.WEB,
                        Name = model.Name,
                        Surname = model.Surname,
                        Password = model.Password,
                        IsActive = true,
                        PhoneNumber = model.UserName,
                        AccountType = CommonConst.SystemAccountType.MasterAgent,
                        AgentType = CommonConst.AgentType.Agent,
                        EmailAddress = model.EmailAddress,
                        //UserName = model.PhoneNumber,
                        IsEmailConfirmed = true,
                        EmailActivationLink = _appUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
                    }
                );
                _logger.LogInformation($"Done create user:{StringExtensions.ToJson(user)}");
                //Getting tenant-specific settings
                var isEmailConfirmationRequiredForLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .IsEmailConfirmationRequiredForLogin);
                _logger.LogInformation($"isEmailConfirmationRequiredForLogin:{isEmailConfirmationRequiredForLogin}");
                if (model.IsExternalLogin)
                {
                    _logger.LogInformation($"IsExternalLogin:{model.IsExternalLogin}");
                    //Debug.Assert(externalLoginInfo != null);

                    if (string.Equals(externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email), model.EmailAddress,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            LoginProvider = externalLoginInfo.LoginProvider,
                            ProviderKey = externalLoginInfo.ProviderKey,
                            TenantId = user.TenantId
                        }
                    };
                }

                //_logger.LogInformation("Begin SaveChangesAsync");
                await _unitOfWorkManager.Current.SaveChangesAsync();
                //Logger.Info($"SaveChangesAsync done");
                //Debug.Assert(user.TenantId != null);
                string tenancyName = null;
                if (user.TenantId != null)
                {
                    tenancyName = (await _tenantManager.GetByIdAsync(user.TenantId ?? 0)).TenancyName;
                }
                //Logger.Info($"Begin check done");

                //Directly login if possible
                if (user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin))
                {
                    _logger.LogInformation($"Process login user");
                    AbpLoginResult<Tenant, User> loginResult;
                    if (externalLoginInfo != null)
                    {
                        loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenancyName);
                    }
                    else
                    {
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenancyName);
                    }

                    _logger.LogInformation($"Login user return:{loginResult.Result}");
                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await _signInManager.SignInAsync(loginResult.Identity, false);
                        if (!string.IsNullOrEmpty(model.SingleSignIn) &&
                            model.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                            loginResult.Result == AbpLoginResultType.Success)
                        {
                            var returnUrl = NormalizeReturnUrl(model.ReturnUrl);
                            loginResult.User.SetSignInToken();
                            returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                                loginResult.User.Id, loginResult.User.TenantId);
                            return Redirect(returnUrl);
                        }

                        return Redirect(GetHomeUrl());
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " +
                                loginResult.Result);
                }

                _logger.LogInformation($"RegisterResultViewModel");
                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive,
                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.UseCaptcha = !model.IsExternalLogin && UseCaptchaOnRegistration();
                ViewBag.ErrorMessage = ex.Message;

                model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();

                return View("Register", model);
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            //todo HoangLT mở cho đăng ký ở host
            // if (!AbpSession.TenantId.HasValue)
            // {
            //     //Host users can not register
            //     throw new InvalidOperationException();
            // }

            var check = SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
            Logger.Info($"CheckCapccha ruturn:{check}");
            return check;
        }

        private bool UseCaptchaOnLogin()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnLogin);
        }


        private async Task<bool> IsOdbEnable()
        {
            return await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                .IsOdpVerificationEnabled);
        }

        private async Task<bool> CheckOdpAvailable(string phoneNumber)
        {
            return await _accountAppService.CheckOdpavailable(phoneNumber);
        }


        private void CheckSelfRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            //todo HoangLT mở cho đăng ký ở host
            // if (!AbpSession.TenantId.HasValue)
            // {
            //     return false; //No registration enabled for host users!
            // }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration);
        }

        private bool IsTenantSelfRegistrationEnabled()
        {
            if (AbpSession.TenantId.HasValue)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        #endregion

        #region ForgotPassword / ResetPassword

        public ActionResult ForgotPassword()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendPasswordResetLink(SendPasswordResetLinkViewModel model)
        {
            await _accountAppService.SendPasswordResetCode(
                new SendPasswordResetCodeInput
                {
                    EmailAddress = model.EmailAddress
                });

            return Json(new AjaxResponse());
        }

        public async Task<ActionResult> SendOtpResetPass(SendOtpResetPassViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserByUserNameAsync(model.PhoneNumber);
                if (user == null)
                    throw new UserFriendlyException(
                        $"Không tồn tại thông tin tài khoản với số điện thoại: {model.PhoneNumber}");
                if (!user.IsActive)
                    throw new UserFriendlyException(L("Message_LockAccount"));

                const string returnUrl = "/";
                user.SetNewPasswordResetCode();
                var link =
                    $"/Account/ConfirmResetPassword?userId={user.Id}&resetCode={user.PasswordResetCode}&returnUrl={returnUrl}";
                link = EncryptQueryParameters(link);
                //await _userManager.UpdateAsync(user);
                await _accountAppService.SendResetCode(new SendResetCodeInput
                {
                    PhoneNumber = user.PhoneNumber,
                    ConfirmCode = new Random().Next(0, 9999).ToString("0000")
                });
                //return Redirect(link);
                return Json(new AjaxResponse
                {
                    TargetUrl = link
                });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                //return RedirectToAction("ForgotPassword", "Account");
                return Json(new AjaxResponse
                {
                    TargetUrl = Url.Action("ForgotPassword")
                });
            }
        }

        public async Task<ActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                await SwitchToTenantIfNeeded(model.TenantId);
                var user = await _userManager.GetUserByIdAsync(model.UserId);
                if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
                {
                    throw new UserFriendlyException("Thông tin reset password không hợp lệ");
                }

                model.IsObpEnable =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpResetPass);
                model.IsUseVerify =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseVerifyResetPass);
                var times = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting
                    .OtpTimeOut);
                //const string param = "<span style=\"color: red\" id=\"timeOtp\"></span>";
                model.Message = L("Message_OTP_Description", times);
                if (model.IsObpEnable)
                {
                    var timesOdp =
                        await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting
                            .OdpAvailable);
                    model.Message = L("Message_ODP_Description", timesOdp / 60);
                }

                model.ReturnUrl = "/home";
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                ViewBag.PhoneNumber = user.UserName;
                model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmResetPassword(ResetPasswordInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            try
            {
                // var isUseVerify =
                //     await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                //         .IsUseVerifyResetPass);
                // if (isUseVerify)
                // {
                //     await _accountAppService.VerifyOtp(new OtpConfirmInput
                //     {
                //         Otp = input.Otp,
                //         Type = CommonConst.OtpType.ResetPass,
                //         PhoneNumber = user.PhoneNumber
                //     });
                // }
                input.Channel = CommonConst.Channel.WEB;
                input.IsVerify = true;
                var output = await _accountAppService.ResetPassword(input);

                if (output.CanLogin)
                {
                    await _signInManager.SignInAsync(user, false);
                    if (!string.IsNullOrEmpty(input.SingleSignIn) &&
                        input.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        user.SetSignInToken();
                        var returnUrl =
                            AddSingleSignInParametersToReturnUrl(input.ReturnUrl, user.SignInToken, user.Id,
                                user.TenantId);
                        return Redirect(returnUrl);
                    }
                }

                return Redirect(NormalizeReturnUrl(input.ReturnUrl));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                // return RedirectToAction("ConfirmResetPassword", "Account", new ResetPasswordViewModel
                // {
                //     TenantId = AbpSession.TenantId,
                //     UserId = user.Id,
                //     ReturnUrl = input.ReturnUrl,
                //     SingleSignIn = input.SingleSignIn,
                //     ResetCode = input.ResetCode
                // });
                var link =
                    $"/Account/ConfirmResetPassword?userId={user.Id}&resetCode={input.ResetCode}&returnUrl={input.ReturnUrl}";
                link = EncryptQueryParameters(link);
                return Redirect(link);
                // return Json(new AjaxResponse
                // {
                //     TargetUrl = link
                // });
            }
        }

        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            await SwitchToTenantIfNeeded(model.TenantId);

            var user = await _userManager.GetUserByIdAsync(model.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            model.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordInput input)
        {
            input.Channel = CommonConst.Channel.WEB;
            var output = await _accountAppService.ResetPassword(input);

            if (output.CanLogin)
            {
                var user = await _userManager.GetUserByIdAsync(input.UserId);
                await _signInManager.SignInAsync(user, false);

                if (!string.IsNullOrEmpty(input.SingleSignIn) &&
                    input.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    user.SetSignInToken();
                    var returnUrl =
                        AddSingleSignInParametersToReturnUrl(input.ReturnUrl, user.SignInToken, user.Id, user.TenantId);
                    return Redirect(returnUrl ?? "/");
                }
            }

            return Redirect(NormalizeReturnUrl(input.ReturnUrl ?? "/"));
        }

        //[ForgeryExceptionFilter]
        public async Task<ActionResult> ConfirmLogin(ConfirmLoginViewModel model)
        {
            await SwitchToTenantIfNeeded(model.TenantId);

            var user = await _userManager.GetUserByIdAsync(model.ConfirmId);
            if (user == null)
            {
                throw new UserFriendlyException("Đăng nhập không thành công");
            }

            model.IsOdpEnable =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsUseOdpLogin);
            // var times = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting
            //     .OtpTimeOut);
            //const string param = "<span style=\"color: red\" id=\"timeOtp\"></span>";
            var timeOtp =
                await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.OtpTimeOut);
            model.Message = L("Message_OTP_Description", timeOtp);
            if (model.IsOdpEnable)
            {
                var timesOdp =
                    await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.OdpAvailable);
                model.Message = L("Message_ODP_Description", timesOdp / 60);
            }
            //if (model.IsOdpEnable)
            //{
            //    model.IsOdpAvailable = await _accountAppService.CheckOdpavailable(user.PhoneNumber);
            //}

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(model);
        }

        [HttpPost]
        //[ForgeryExceptionFilter]
        public async Task<ActionResult> ConfirmLogin(ConfirmLoginInputModel model)
        {
            var user = await _userManager.GetUserByIdAsync(model.ConfirmId);
            try
            {
                _logger.LogInformation($"ConfirmLogin request: {StringExtensions.ToJson(model)}");
                await _accountAppService.VerifyOtp(new OtpConfirmInput
                {
                    Otp = model.Otp,
                    Type = CommonConst.OtpType.Login,
                    PhoneNumber = user.UserName
                });
                _logger.LogInformation($"Verify code done: {StringExtensions.ToJson(model)}");
                await _signInManager.SignInAsync(user, false);

                if (!string.IsNullOrEmpty(model.SingleSignIn) &&
                    model.SingleSignIn.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    user.SetSignInToken();
                    var returnUrl =
                        AddSingleSignInParametersToReturnUrl(model.ReturnUrl, user.SignInToken, user.Id, user.TenantId);
                    return Redirect(returnUrl);
                }

                return Redirect(NormalizeReturnUrl(model.ReturnUrl));
            }
            catch (Exception ex)
            {
                _logger.LogError($"ConfirmLogin error:{ex}");
                TempData["ErrorMessage"] = ex.Message;
                var link =
                    $"/Account/ConfirmLogin?ConfirmId={model.ConfirmId}&TenantId={AbpSession.TenantId}&PhoneNumber={user.UserName}&SingleSignIn={model.SingleSignIn}&ReturnUrl={model.ReturnUrl}";
                link = EncryptQueryParameters(link);
                return Redirect(link);
            }
        }

        #endregion

        #region Email activation / confirmation

        public ActionResult EmailActivation()
        {
            return View();
        }

        [HttpPost]
        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<JsonResult> SendEmailActivationLink(SendEmailActivationLinkInput model)
        {
            await _accountAppService.SendEmailActivationLink(model);
            return Json(new AjaxResponse());
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<ActionResult> EmailConfirmation(EmailConfirmationViewModel input)
        {
            await SwitchToTenantIfNeeded(input.TenantId);
            await _accountAppService.ActivateEmail(input);
            return RedirectToAction(
                "Login",
                new
                {
                    successMessage = L("YourEmailIsConfirmedMessage"),
                    userNameOrEmailAddress = (await _userManager.GetUserByIdAsync(input.UserId)).UserName
                });
        }

        #endregion

        #region External Login

        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl, string ss = "")
        {
            var redirectUrl = Url.Action(
                "ExternalLoginCallback",
                "Account",
                new
                {
                    ReturnUrl = returnUrl,
                    authSchema = provider,
                    ss = ss
                });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null,
            string ss = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (remoteError != null)
            {
                Logger.Error("Remote Error in ExternalLoginCallback: " + remoteError);
                throw new UserFriendlyException(L("CouldNotCompleteLoginOperation"));
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                Logger.Warn("Could not get information from external login.");
                return RedirectToAction(nameof(Login));
            }

            var tenancyName = GetTenancyNameOrNull();

            var loginResult = await _logInManager.LoginAsync(externalLoginInfo, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                {
                    await _signInManager.SignInAsync(loginResult.Identity, false);

                    if (!string.IsNullOrEmpty(ss) && ss.Equals("true", StringComparison.OrdinalIgnoreCase) &&
                        loginResult.Result == AbpLoginResultType.Success)
                    {
                        loginResult.User.SetSignInToken();
                        returnUrl = AddSingleSignInParametersToReturnUrl(returnUrl, loginResult.User.SignInToken,
                            loginResult.User.Id, loginResult.User.TenantId);
                    }

                    return Redirect(returnUrl);
                }
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterForExternalLogin(externalLoginInfo);
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                        loginResult.Result,
                        externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? externalLoginInfo.ProviderKey,
                        tenancyName
                    );
            }
        }

        private async Task<ActionResult> RegisterForExternalLogin(ExternalLoginInfo externalLoginInfo)
        {
            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            (string name, string surname) nameInfo;
            using (var providerManager =
                _externalLoginInfoManagerFactory.GetExternalLoginInfoManager(externalLoginInfo.LoginProvider))
            {
                nameInfo = providerManager.Object.GetNameAndSurnameFromClaims(
                    externalLoginInfo.Principal.Claims.ToList(), _identityOptions);
            }

            var viewModel = new RegisterViewModel
            {
                EmailAddress = email,
                Name = nameInfo.name,
                Surname = nameInfo.surname,
                IsExternalLogin = true,
                ExternalLoginAuthSchema = externalLoginInfo.LoginProvider
            };

            if (nameInfo.name != null &&
                nameInfo.surname != null &&
                email != null)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        #endregion

        #region Impersonation

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        public virtual async Task<JsonResult> Impersonate([FromBody] ImpersonateInput input)
        {
            var output = await _accountAppService.Impersonate(input);

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/ImpersonateSignIn?tokenId=" + output.ImpersonationToken
            });
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<ActionResult> ImpersonateSignIn(string tokenId)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(tokenId);
            await _signInManager.SignInAsync(result.Identity, false);
            return RedirectToAppHome();
        }

        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> DelegatedImpersonate([FromBody] DelegatedImpersonateInput input)
        {
            var userDelegation = await _userDelegationManager.GetAsync(input.UserDelegationId);
            if (userDelegation.TargetUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("User delegation error...");
            }

            var output = await _accountAppService.Impersonate(new ImpersonateInput
            {
                TenantId = AbpSession.TenantId,
                UserId = userDelegation.SourceUserId
            });

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/DelegatedImpersonateSignIn?userDelegationId=" + input.UserDelegationId +
                            "&tokenId=" + output.ImpersonationToken
            });
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<ActionResult> DelegatedImpersonateSignIn(long userDelegationId, string tokenId)
        {
            var userDelegation = await _userDelegationManager.GetAsync(userDelegationId);
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(tokenId);

            if (userDelegation.SourceUserId != result.User.Id)
            {
                throw new UserFriendlyException("User delegation error...");
            }

            await _signInManager.SignInWithClaimsAsync(result.User, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = userDelegation.EndTime.ToUniversalTime()
            }, result.Identity.Claims);

            return RedirectToAppHome();
        }

        public virtual JsonResult IsImpersonatedLogin()
        {
            return Json(new AjaxResponse {Result = AbpSession.ImpersonatorUserId.HasValue});
        }

        public virtual async Task<JsonResult> BackToImpersonator()
        {
            var output = await _accountAppService.BackToImpersonator();

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/ImpersonateSignIn?tokenId=" + output.ImpersonationToken
            });
        }

        #endregion

        #region Linked Account

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> SwitchToLinkedAccount([FromBody] SwitchToLinkedAccountInput model)
        {
            var output = await _accountAppService.SwitchToLinkedAccount(model);

            await _signInManager.SignOutAsync();

            return Json(new AjaxResponse
            {
                TargetUrl = _webUrlService.GetSiteRootAddress(output.TenancyName) +
                            "Account/SwitchToLinkedAccountSignIn?tokenId=" + output.SwitchAccountToken
            });
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<ActionResult> SwitchToLinkedAccountSignIn(string tokenId)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(tokenId);

            await _signInManager.SignInAsync(result.Identity, false);
            return RedirectToAppHome();
        }

        #endregion

        #region Change Tenant

        public async Task<ActionResult> TenantChangeModal()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            return View("/Views/Shared/Components/TenantChange/_ChangeModal.cshtml", new ChangeModalViewModel
            {
                TenancyName = loginInfo.Tenant?.TenancyName
            });
        }

        #endregion

        #region Common

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private void CheckCurrentTenant(int? tenantId)
        {
            if (AbpSession.TenantId != tenantId)
            {
                throw new Exception(
                    $"Current tenant is different than given tenant. AbpSession.TenantId: {AbpSession.TenantId}, given tenantId: {tenantId}");
            }
        }

        private async Task SwitchToTenantIfNeeded(int? tenantId)
        {
            if (tenantId != AbpSession.TenantId)
            {
                if (_webUrlService.SupportsTenancyNameInUrl)
                {
                    throw new InvalidOperationException($"Given tenantid ({tenantId}) does not match to tenant's URL!");
                }

                SetTenantIdCookie(tenantId);
                CurrentUnitOfWork.SetTenantId(tenantId);
                await _signInManager.SignOutAsync();
            }
        }

        #endregion

        #region Helpers

        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home", new {area = "App"});
        }

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home", new {area = "App"});
        }

        public string GetHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetAppHomeUrl;
            }

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            if (Url.IsLocalUrl(returnUrl) ||
                _webUrlService.GetRedirectAllowedExternalWebSites().Any(returnUrl.Contains))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion

        #region Etc

        [AbpMvcAuthorize]
        public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            await _appNotifier.SendMessageAsync(
                AbpSession.ToUserIdentifier(),
                message,
                severity.ToPascalCase().ToEnum<NotificationSeverity>()
            );

            return Content("Sent notification: " + message);
        }

        #endregion

        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" +
                   HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }
    }
}
