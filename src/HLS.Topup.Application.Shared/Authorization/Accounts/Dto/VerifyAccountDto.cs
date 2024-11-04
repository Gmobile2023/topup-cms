using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class VerifyAccountDto
    {
        public string PhoneNumber { get; set; }
        public bool IsSendCode { get; set; }
    }
}
