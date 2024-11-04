using HLS.Topup.Security;

namespace HLS.Topup.Configuration.Host.Dto
{
    public class SecuritySettingsEditDto
    {
        public bool AllowOneConcurrentLoginPerUser { get; set; }

        public bool UseDefaultPasswordComplexitySettings { get; set; }

        public PasswordComplexitySetting PasswordComplexity { get; set; }

        public PasswordComplexitySetting DefaultPasswordComplexity { get; set; }

        public UserLockOutSettingsEditDto UserLockOut { get; set; }

        public TwoFactorLoginSettingsEditDto TwoFactorLogin { get; set; }

        public OtpSettingsEditDto OtpSettings { get; set; }
        //public LevelPasswordSettingsEditDto LevelPasswordSetting { get; set; }
    }
}
