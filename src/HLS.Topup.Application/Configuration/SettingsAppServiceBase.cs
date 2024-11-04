using System;
using System.Threading.Tasks;
using Abp.Net.Mail;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using HLS.Topup.Configuration.Dto;
using HLS.Topup.Configuration.Host.Dto;

namespace HLS.Topup.Configuration
{
    public abstract class SettingsAppServiceBase : TopupAppServiceBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IAppConfigurationAccessor _configurationAccessor;

        protected SettingsAppServiceBase(
            IEmailSender emailSender,
            IAppConfigurationAccessor configurationAccessor)
        {
            _emailSender = emailSender;
            _configurationAccessor = configurationAccessor;
        }

        #region Send Test Email

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            try
            {
                await _emailSender.SendAsync(
                    input.EmailAddress,
                    L("TestEmail_Subject"),
                    L("TestEmail_Body")
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Gửi mail lỗi: " + e.Message);
            }
        }

        public ExternalLoginSettingsDto GetEnabledSocialLoginSettings()
        {
            var dto = new ExternalLoginSettingsDto();
            if (!bool.Parse(_configurationAccessor.Configuration["Authentication:AllowSocialLoginSettingsPerTenant"]))
            {
                return dto;
            }

            if (IsSocialLoginEnabled("Facebook"))
            {
                dto.EnabledSocialLoginSettings.Add("Facebook");
            }

            if (IsSocialLoginEnabled("Google"))
            {
                dto.EnabledSocialLoginSettings.Add("Google");
            }

            if (IsSocialLoginEnabled("Twitter"))
            {
                dto.EnabledSocialLoginSettings.Add("Twitter");
            }

            if (IsSocialLoginEnabled("Microsoft"))
            {
                dto.EnabledSocialLoginSettings.Add("Microsoft");
            }

            if (IsSocialLoginEnabled("WsFederation"))
            {
                dto.EnabledSocialLoginSettings.Add("WsFederation");
            }

            if (IsSocialLoginEnabled("OpenId"))
            {
                dto.EnabledSocialLoginSettings.Add("OpenId");
            }

            return dto;
        }

        private bool IsSocialLoginEnabled(string name)
        {
            return _configurationAccessor.Configuration.GetSection("Authentication:" + name).Exists() &&
                   bool.Parse(_configurationAccessor.Configuration["Authentication:" + name + ":IsEnabled"]);
        }

        #endregion
    }
}