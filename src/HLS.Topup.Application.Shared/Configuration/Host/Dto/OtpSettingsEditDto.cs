namespace HLS.Topup.Configuration.Host.Dto
{
    public class OtpSettingsEditDto
    {
        public bool IsEnabled { get; set; }
        public bool IsOtpIsEncrypted { get; set; }
        public bool IsOtpPass { get; set; }
        public bool IsUseOdpRegister { get; set; }
        public bool IsUseOdpResetPass { get; set; }
        public bool IsUseOdpLogin { get; set; }
        public bool IsUseVerifyLogin { get; set; }
        public bool IsUseVerifyRegister { get; set; }
        public bool IsUseVerifyResetPass { get; set; }
        public int OtpTimeOut { get; set; }
        public int MaxFailed { get; set; }
        public int OdpAvailable { get; set; }
        public bool IsOtpVerificationEnabled { get; set; }
        public bool IsOdpVerificationEnabled { get; set; }
        public bool IsLevelPassVerificationEnabled { get; set; }
        public byte DefaultVerifyTransId { get; set; }
        public int OdpMaxSend { get; set; }
    }
}
