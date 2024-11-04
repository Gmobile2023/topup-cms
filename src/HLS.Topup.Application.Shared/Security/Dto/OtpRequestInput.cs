using HLS.Topup.Common;

namespace HLS.Topup.Security.Dto
{
    public class OtpAuthRequestInput
    {
        public CommonConst.OtpType Type { get; set; }
        public bool IsResend { get; set; }
    }

    public class OtpRequestInput
    {
        public CommonConst.OtpType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public bool IsResend { get; set; }
    }
    public class OtpResetPassRequestInput
    {
        public CommonConst.OtpType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
