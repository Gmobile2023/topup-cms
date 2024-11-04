using HLS.Topup.Common;

namespace HLS.Topup.Security.Dto
{
    public class OtpConfirmInput
    {
        public string Otp { get; set; }
        public string PhoneNumber { get; set; }
        public CommonConst.OtpType Type { get; set; }
    }

    public class OtpAuthConfirmInput
    {
        public string Otp { get; set; }
        public CommonConst.OtpType Type { get; set; }
    }
}