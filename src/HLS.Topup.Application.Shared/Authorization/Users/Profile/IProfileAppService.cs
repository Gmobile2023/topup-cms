using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Common;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Security.Dto;

namespace HLS.Topup.Authorization.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        Task UpdateUserById(UpdateProfileLimitDto input, int userId);

        Task ChangePassword(ChangePasswordInput input);

        Task UpdateProfilePicture(UpdateProfilePictureInput input);

        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();

        Task<GetProfilePictureOutput> GetProfilePicture();

        Task<GetProfilePictureOutput> GetProfilePictureByUser(long userId);

        Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username);

        Task<GetProfilePictureOutput> GetFriendProfilePicture(GetFriendProfilePictureInput input);

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey();
        Task UpdateProfileInfo(UpdateProfileInfoRequest input);

        Task SendVerificationSms(SendVerificationSmsInputDto input);

        Task VerifySmsCode(VerifySmsCodeInputDto input);

        Task PrepareCollectedData();


        Task UpdateMobileSendOtpAsync(UpdateMobileSendOtp input);
        Task UpdateLevel2PasswordAsync(UpdateLevel2PassDto input);
        Task VerifyLevel2PasswordAsync(VerifyLevel2PassDto input);
        Task ChangeLevel2PasswordAsync(ChangeLevel2PassDto input);

        Task<bool> CheckLevel2PasswordAsync();
        Task VerifyTransCodeAsync(VerifySmsCodeInputDto input);

        Task ChangePaymentVerifyMethod(PaymentMethodDto input);
        Task<CommonConst.VerifyTransType> GetPaymentVerifyMethod(GetPaymentMothodInput input);
    }
}
