using System.Threading.Tasks;
using HLS.Topup.Security.Dto;

namespace HLS.Topup.Security
{
    public interface IOtpAppService
    {
        Task SendOtpAuthAsync(OtpAuthRequestInput request);
        Task VerifyOtpAuthAsync(OtpAuthConfirmInput request);
        Task SendOtpAsync(OtpRequestInput request);
        Task VerifyOtpAsync(OtpConfirmInput request);
        Task<bool> CheckOdpAvailable(string phoneNumber);
    }
}
