using System.Threading.Tasks;
using HLS.Topup.Common;

namespace HLS.Topup.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string code, CommonConst.OtpType type,bool isOtp=false);
    }
}
