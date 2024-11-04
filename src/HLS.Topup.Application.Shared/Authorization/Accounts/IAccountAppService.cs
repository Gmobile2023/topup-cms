using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Security.Dto;

namespace HLS.Topup.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<int?> ResolveTenantId(ResolveTenantIdInput input);

        Task<RegisterOutput> Register(RegisterInput input);

        Task SendPasswordResetCode(SendPasswordResetCodeInput input);

        Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input);

        Task SendEmailActivationLink(SendEmailActivationLinkInput input);

        Task ActivateEmail(ActivateEmailInput input);

        Task<ImpersonateOutput> Impersonate(ImpersonateInput input);

        Task<ImpersonateOutput> BackToImpersonator();

        Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input);
        Task ValidateAccount(VerifyAccountDto model);
        Task SendOtp(OtpRequestInput model);
        Task VerifyOtp(OtpConfirmInput model);
        Task<bool> CheckOdpavailable(string phoneNumber);
        Task SendResetCode(SendResetCodeInput input);
        string GetPing();
    }
}
