using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Runtime.Session;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Settings;
using HLS.Topup.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

using Abp.Application.Services.Dto; 
using Abp.UI;

namespace HLS.Topup.Common
{
    public class CommonAppService : TopupAppServiceBase, ICommonAppService
    {
        private readonly TopupAppSession _topupAppSession;
        private readonly ILogger<CommonAppService> _logger;
        private readonly ISettingManger _settingManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationRoot _appConfiguration;

        public CommonAppService(TopupAppSession topupAppSession, ILogger<CommonAppService> logger,
            ISettingManger settingManger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _topupAppSession = topupAppSession;
            _logger = logger;
            _settingManger = settingManger;
            _httpContextAccessor = httpContextAccessor;
            _appConfiguration = env.GetAppConfiguration();
        }

        [AbpAuthorize]
        public async Task<bool> CheckAccountActivities(CheckAccountActivityInput input)
        {
            if (input.Channel == 0)
                input.Channel = CommonConst.Channel.WEB;
            _logger.LogInformation($"CheckAccountActivities_Request:{input.ToJson()}");
            var rs = await _settingManger.CheckAccountActivities(input, _topupAppSession.UserId ?? 0,
                _topupAppSession.AccountCode, AbpSession.ToUserIdentifier());
            _logger.LogInformation($"CheckAccountActivities_Return:{rs.ToJson()}");
            return rs;
        }


        public async Task<object> AppSetting()
        {
            return new
            {
                Hotline = "0522553333",
                TermsOfUse = "https://cms.sandbox-topup.gmobile.vn/dieu-khoan-su-dung",
                PrivacyPolicy = "https://cms.sandbox-topup.gmobile.vn/chinh-sach-bao-mat",
                VerifyPaymentMethod =
                    await _settingManger.GetPaymentVerifyMethod(CommonConst.Channel.APP,
                        AbpSession.ToUserIdentifier()), ////Hình thức xác thực khi thanh toán
                IsUseOdpRegister =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpRegister), //Dùng ODP khi đăng ký nếu fase thì dùng OTP
                IsUseOdpResetPass =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpResetPass), //Dùng ODP khi resetPass nếu fase thì dùng OTP
                IsUseOdpLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpLogin), //Dùng ODP khi login nếu fase thì dùng OTP
                IsOdpVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOdpVerificationEnabled), //Các TH còn lại. Nếu true dùng ODP. fale dùng OTP
                IsOtpVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOtpVerificationEnabled)
            };
        }

        public async Task<object> GetAppSetting()
        {
            return new
            {
                Hotline = "0522553333",
                TermsOfUse = "https://cms.sandbox-topup.gmobile.vn/dieu-khoan-su-dung",
                PrivacyPolicy = "https://cms.sandbox-topup.gmobile.vn/chinh-sach-bao-mat",
                VerifyPaymentMethod =
                    await _settingManger.GetPaymentVerifyMethod(CommonConst.Channel.APP,
                        AbpSession.ToUserIdentifier()), ////Hình thức xác thực khi thanh toán
                IsUseOdpRegister =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpRegister), //Dùng ODP khi đăng ký nếu fase thì dùng OTP
                IsUseOdpResetPass =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpResetPass), //Dùng ODP khi resetPass nếu fase thì dùng OTP
                IsUseOdpLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsUseOdpLogin), //Dùng ODP khi login nếu fase thì dùng OTP
                IsOdpVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOdpVerificationEnabled), //Các TH còn lại. Nếu true dùng ODP. fale dùng OTP
                IsOtpVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOtpVerificationEnabled)
            };
        }


        [AbpAuthorize]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id != AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }
    }
}
