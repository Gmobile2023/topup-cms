using System.Threading.Tasks;
using HLS.Topup.Common;

namespace HLS.Topup.Security
{
    public interface IOtpManager
    {
        Task RequestVerifyCode(string phoneNumber, CommonConst.OtpType type, string code = null, long? userId = null,bool isResend = false);
        Task VerifytCode(string phoneNumber, string otp, CommonConst.OtpType type);
        Task SmsMessageInsertAsync(OtpMessage message);
        Task<bool> CheckOdpAvailable(string phoneNumner);
    }
}
