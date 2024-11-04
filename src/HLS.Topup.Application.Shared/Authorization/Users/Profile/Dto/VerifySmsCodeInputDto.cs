using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Users.Profile.Dto
{
    public class VerifySmsCodeInputDto
    {
        public string Code { get; set; }

        public string PhoneNumber { get; set; }
        public CommonConst.OtpType Type { get; set; }
    }
}
