using Abp.Dependency;
using HLS.Topup.Configuration;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Net.Sms
{
    public class MobileNetSenderConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string Url => _appConfiguration["SmsConfigs:MobileNetSms:Url"];
        public bool IsSendSms => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsSendSms"]);
        public bool IsSendMobileGo => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsSendMobileGo"]);
        public bool IsUseAllSmsMobileGo => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsUseAllSmsMobileGo"]);

        public bool IsUseAllSmsBrandName =>
            bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsUseAllSmsBrandName"]);

        public bool IsUseAllSmsMobileNet =>
            bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsUseAllSmsMobileNet"]);

        public bool IsSendBrandName => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:IsSendBrandName"]);
        public bool IsUseVnm => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSms:VNMConfig:IsUse"]);
        public string UseVnmChannel => _appConfiguration["SmsConfigs:MobileNetSms:VNMConfig:SmsChannel"];
        public string SmsChannel => _appConfiguration["SmsConfigs:SmsChannel"];

        public string UserName => _appConfiguration["SmsConfigs:MobileNetSms:UserName"];
        public string Password => _appConfiguration["SmsConfigs:MobileNetSms:Password"];
        public string Key => _appConfiguration["SmsConfigs:MobileNetSms:key"];
        public string Smsid => _appConfiguration["SmsConfigs:MobileNetSms:Smsid"];
        public string Company => _appConfiguration["SmsConfigs:MobileNetSms:Company"];

        public string SenderNumber => _appConfiguration["SmsConfigs:MobileNetSms:SenderNumber"];
        public string TelcoVnm => _appConfiguration["SmsConfigs:MobileNetSms:TelcoVNM"];

        public MobileNetSenderConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }

    public class MobileNetBrandNameSenderConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string Url => _appConfiguration["SmsConfigs:MobileNetSmsBrandName:Url"];
        public bool IsSendSms => bool.Parse(_appConfiguration["SmsConfigs:MobileNetSmsBrandName:IsSendSms"]);

        public string UserName => _appConfiguration["SmsConfigs:MobileNetSmsBrandName:UserName"];
        public string Password => _appConfiguration["SmsConfigs:MobileNetSmsBrandName:Password"];
        public string Brandname => _appConfiguration["SmsConfigs:MobileNetSmsBrandName:Brandname"];
        public string Type => _appConfiguration["SmsConfigs:MobileNetSmsBrandName:Type"];

        public MobileNetBrandNameSenderConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }

    public class MobileGoSenderConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string Url => _appConfiguration["SmsConfigs:MobileGoSms:Url"];
        public bool IsSendSms => bool.Parse(_appConfiguration["SmsConfigs:MobileGoSms:IsSendSms"]);

        public string UserName => _appConfiguration["SmsConfigs:MobileGoSms:UserName"];
        public string Password => _appConfiguration["SmsConfigs:MobileGoSms:Password"];

        public MobileGoSenderConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
