using System;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using HLS.Topup.Authentication.TwoFactor.Google;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Authorization.Users.Profile.Cache;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Friendships;
using HLS.Topup.Gdpr;
using HLS.Topup.Net.Sms;
using HLS.Topup.Security;
using HLS.Topup.Security.Dto;
using HLS.Topup.Storage;
using HLS.Topup.Timing;
using HLS.Topup.Validation;
using ServiceStack;
using Microsoft.Extensions.Logging;
using StringExtensions = ServiceStack.StringExtensions;
using HLS.Topup.Reports;
using HLS.Topup.Settings;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Authorization.Users.Profile
{
    [AbpAuthorize]
    public class ProfileAppService : TopupAppServiceBase, IProfileAppService
    {
        private const int MaxProfilPictureBytes = 5242880; //5MB
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IFriendshipManager _friendshipManager;
        private readonly GoogleTwoFactorAuthenticateService _googleTwoFactorAuthenticateService;
        private readonly ISmsSender _smsSender;
        private readonly TopupAppSession _topupAppSession;
        private readonly ICacheManager _cacheManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ProfileImageServiceFactory _profileImageServiceFactory;
        private readonly IOtpAppService _otpAppService;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IStaffConfigurationManager _staffConfigurationManager;
        private readonly ILogger<ProfileAppService> _logger;
        private readonly UrlExtentions _extentions;
        private readonly IReportsManager _reportManager;
        private readonly ISettingManger _settingManager;

        public ProfileAppService(
            IAppFolders appFolders,
            IBinaryObjectManager binaryObjectManager,
            ITimeZoneService timezoneService,
            IFriendshipManager friendshipManager,
            GoogleTwoFactorAuthenticateService googleTwoFactorAuthenticateService,
            ISmsSender smsSender,
            ICacheManager cacheManager,
            ITempFileCacheManager tempFileCacheManager,
            IBackgroundJobManager backgroundJobManager,
            ProfileImageServiceFactory profileImageServiceFactory, IOtpAppService otpAppService,
            IRepository<UserProfile> userProfileRepository,
            IStaffConfigurationManager staffConfigurationManager,
            TopupAppSession topupAppSession, ILogger<ProfileAppService> logger, UrlExtentions extentions,
            IReportsManager reportManager, ISettingManger settingManager)
        {
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timezoneService;
            _friendshipManager = friendshipManager;
            _googleTwoFactorAuthenticateService = googleTwoFactorAuthenticateService;
            _smsSender = smsSender;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
            _backgroundJobManager = backgroundJobManager;
            _profileImageServiceFactory = profileImageServiceFactory;
            _otpAppService = otpAppService;
            _topupAppSession = topupAppSession;
            _userProfileRepository = userProfileRepository;
            _staffConfigurationManager = staffConfigurationManager;
            _logger = logger;
            _extentions = extentions;
            _reportManager = reportManager;
            _settingManager = settingManager;
        }

        [DisableAuditing]
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);

                userProfileEditDto.QrCodeSetupImageUrl = user.GoogleAuthenticatorKey != null
                    ? _googleTwoFactorAuthenticateService.GenerateSetupCode("HLS.Topup",
                        user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300).QrCodeSetupImageUrl
                    : "";
                userProfileEditDto.IsGoogleAuthenticatorEnabled = user.GoogleAuthenticatorKey != null;

                if (Clock.SupportsMultipleTimezone)
                {
                    userProfileEditDto.Timezone =
                        await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                    var defaultTimeZoneId =
                        await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    if (userProfileEditDto.Timezone == defaultTimeZoneId)
                    {
                        userProfileEditDto.Timezone = string.Empty;
                    }
                }

                userProfileEditDto.ProfilePicture = (await GetProfilePicture()).ProfilePicture;
                var userId = AbpSession.UserId;
                if (user.AccountType == CommonConst.SystemAccountType.Staff || user.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    var network = UserManager.GetAccountInfo();
                    var staff = await _staffConfigurationManager.GetUserStaffInfo(user.Id);
                    userProfileEditDto.StaffInfo = staff.ConvertTo<StaffInfo>();
                    userProfileEditDto.AgentName = network.NetworkInfo.AgentName;
                    userProfileEditDto.AgentCode = network.NetworkInfo.AccountCode;
                    userProfileEditDto.IsVerifyAccount = network.NetworkInfo.IsVerifyAccount;
                    userProfileEditDto.AgentIsVerify = network.NetworkInfo.IsVerifyAccount;
                    userProfileEditDto.AgentIsActive = network.NetworkInfo.IsActive;
                    userId = network.NetworkInfo.Id;
                }
                else
                {
                    userProfileEditDto.IsVerifyAccount = user.IsVerifyAccount;
                    userProfileEditDto.AgentIsVerify = user.IsVerifyAccount;
                    userProfileEditDto.IsActive = user.IsActive;
                }

                //Lấy địa chỉ của thằng cha
                var profile = await _userProfileRepository.GetAllIncluding(x => x.CityFk).Include(x => x.DistrictFk)
                    .Include(x => x.WardFk).FirstOrDefaultAsync(x => x.UserId == userId);
                if (profile != null)
                {
                    userProfileEditDto.CityId = profile.CityId;
                    userProfileEditDto.DistrictId = profile.DistrictId;
                    userProfileEditDto.WardId = profile.WardId;
                    userProfileEditDto.Address = profile.Address;
                    userProfileEditDto.FrontPhoto = _extentions.GetFullPath(profile.FrontPhoto);
                    userProfileEditDto.BackSitePhoto = _extentions.GetFullPath(profile.BackSitePhoto);
                    userProfileEditDto.IdentityId = profile.IdIdentity;
                    userProfileEditDto.IdType = profile.IdType;
                    userProfileEditDto.AgentAddress =
                        $"{profile.Address}, {profile.WardFk?.WardName}, {profile.DistrictFk?.DistrictName}, {profile.CityFk?.CityName}";
                }

                return userProfileEditDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCurrentUserProfileForEdit error:{e}");
                return null;
            }
        }

        public async Task DisableGoogleAuthenticator()
        {
            var user = await GetCurrentUserAsync();
            user.GoogleAuthenticatorKey = null;
        }

        public async Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            user.GoogleAuthenticatorKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            CheckErrors(await UserManager.UpdateAsync(user));

            return new UpdateGoogleAuthenticatorKeyOutput
            {
                QrCodeSetupImageUrl = _googleTwoFactorAuthenticateService.GenerateSetupCode(
                    "HLS.Topup",
                    user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300).QrCodeSetupImageUrl
            };
        }

        public async Task UpdateProfileInfo(UpdateProfileInfoRequest input)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            if (input.Surname != user.Surname)
                user.Surname = input.Surname;
            if (input.Name != user.Name)
                user.Name = input.Name;
            if (user.AccountType != CommonConst.SystemAccountType.Staff && !string.IsNullOrEmpty(input.AgentName) &&
                input.AgentName != user.AgentName)
                user.AgentName = input.AgentName;

            CheckErrors(await UserManager.UpdateAsync(user));
        }

        public async Task SendVerificationSms(SendVerificationSmsInputDto input)
        {
            var code = RandomHelper.GetRandom(100000, 999999).ToString();
            var cacheKey = AbpSession.ToUserIdentifier().ToString();
            var cacheItem = new SmsVerificationCodeCacheItem {Code = code};

            _cacheManager.GetSmsVerificationCodeCache().Set(
                cacheKey,
                cacheItem
            );
            //L("SmsVerificationMessage", code)
            //await _smsSender.SendAsync(input.PhoneNumber, code);
        }

        public async Task VerifySmsCode(VerifySmsCodeInputDto input)
        {
            var cacheKey = AbpSession.ToUserIdentifier().ToString();
            var cash = await _cacheManager.GetSmsVerificationCodeCache().GetOrDefaultAsync(cacheKey);

            if (cash == null)
            {
                throw new Exception("Phone number confirmation code is not found in cache !");
            }

            if (input.Code != cash.Code)
            {
                throw new UserFriendlyException(L("WrongSmsVerificationCode"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.IsPhoneNumberConfirmed = true;
            user.PhoneNumber = input.PhoneNumber;
            await UserManager.UpdateAsync(user);
        }

        public async Task PrepareCollectedData()
        {
            await _backgroundJobManager.EnqueueAsync<UserCollectedDataPrepareJob, UserIdentifier>(
                AbpSession.ToUserIdentifier());
        }

        /// <summary>
        /// Hàm cập nhật số điện thoại nhận OTP
        /// </summary>
        public async Task UpdateMobileSendOtpAsync(UpdateMobileSendOtp input)
        {
            if (!string.IsNullOrEmpty(input.PhoneNumber))
                input.PhoneNumber = input.PhoneNumber.Trim();
            if (!ValidationHelper.IsPhone(input.PhoneNumber))
                throw new UserFriendlyException("Số điện thoại sai định dạng");
            var user = await GetCurrentUserAsync();

            var checkMobile = await UserManager.GetUserByMobileOtpAsync(input.PhoneNumber);
            if (checkMobile != null && checkMobile.Id != user.Id)
                throw new UserFriendlyException("Số điện thoại nhận OTP đã tồn tại");

            var checkMobileUser = await UserManager.GetUserByMobileAsync(input.PhoneNumber);
            if (checkMobileUser != null && checkMobileUser.Id != user.Id)
                throw new UserFriendlyException("Số điện thoại đã được sử dụng trong hệ thống");
            user.MobileOtp = input.PhoneNumber;
        }

        /// <summary>
        /// Hàm cập nhật hoặc tạo mới mật khẩu cấp 2
        /// </summary>
        public async Task UpdateLevel2PasswordAsync(UpdateLevel2PassDto input)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            var hasOtp = CryptUtils.HashSHA256(input.Password + user.Id);
            user.Level2Password = hasOtp;
        }

        public async Task ChangeLevel2PasswordAsync(ChangeLevel2PassDto input)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            var hasOtp = CryptUtils.HashSHA256(input.Password + user.Id);
            user.Level2Password = hasOtp;
        }

        public async Task VerifyLevel2PasswordAsync(VerifyLevel2PassDto input)
        {
            if (string.IsNullOrEmpty(input.Password))
                throw new UserFriendlyException("Vui lòng nhập MK cấp 2");
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");

            // var isCheckLock = CheckLockFailLevel2PassIsEnabled();
            // if (!isCheckLock)
            //     return;

            // var settingCount = GetCountFailLockFailLevel2Pass();
            // var seconds = GetSecondLockFailLevel2Pass();
            // if (await UserManager.IsLockedOutAsync(user))
            // {
            //     var time = TimeSpan.FromSeconds(seconds);
            //     await UserManager.ResetFailedLevel2PassCountAsync(user);
            //
            //     throw new UserFriendlyException(L("LimiCountFailLevel2Password", settingCount,
            //         $"{time.Minutes} phút"));
            // }

            var haspass = CryptUtils.HashSHA256(input.Password + user.Id);
            if (user.Level2Password != haspass)
            {
                // await UserManager.IncrementFailedLevel2PassCountAsync(user);
                // if (user.AccessFailedCountLevel2Pass != null && user.AccessFailedCountLevel2Pass >= settingCount)
                // {
                //     var time = TimeSpan.FromSeconds(seconds);
                //     await UserManager.SetLockLevel2PasswordEndDateAsync(user,
                //         DateTimeOffset.UtcNow.AddSeconds(GetSecondLockFailLevel2Pass()));
                //     throw new UserFriendlyException(L("LimiCountFailLevel2Password", settingCount,
                //         $"{time.Minutes} phút"));
                // }

                throw new UserFriendlyException("Mật khẩu cấp 2 không chính xác. Vui lòng thử lại");
            }

            //await UserManager.ResetFailedLevel2PassCountAsync(user);
        }

        public async Task VerifyLevel2PasswordAsync_BAK(VerifyLevel2PassDto input)
        {
            if (string.IsNullOrEmpty(input.Password))
                throw new UserFriendlyException("Vui lòng nhập MK cấp 2");
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");

            var isCheckLock = CheckLockFailLevel2PassIsEnabled();
            if (!isCheckLock)
                return;

            var settingCount = GetCountFailLockFailLevel2Pass();
            var seconds = GetSecondLockFailLevel2Pass();
            if (await UserManager.IsLockedOutAsync(user))
            {
                var time = TimeSpan.FromSeconds(seconds);
                await UserManager.ResetFailedLevel2PassCountAsync(user);

                throw new UserFriendlyException(L("LimiCountFailLevel2Password", settingCount,
                    $"{time.Minutes} phút"));
            }

            var haspass = CryptUtils.HashSHA256(input.Password + user.Id);
            if (user.Level2Password != haspass)
            {
                await UserManager.IncrementFailedLevel2PassCountAsync(user);
                if (user.AccessFailedCountLevel2Pass != null && user.AccessFailedCountLevel2Pass >= settingCount)
                {
                    var time = TimeSpan.FromSeconds(seconds);
                    await UserManager.SetLockLevel2PasswordEndDateAsync(user,
                        DateTimeOffset.UtcNow.AddSeconds(GetSecondLockFailLevel2Pass()));
                    throw new UserFriendlyException(L("LimiCountFailLevel2Password", settingCount,
                        $"{time.Minutes} phút"));
                }

                throw new UserFriendlyException("Sai mật khẩu cấp 2");
            }

            await UserManager.ResetFailedLevel2PassCountAsync(user);
        }

        /// <summary>
        /// Verify code khi thực hiện giao dịch
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task VerifyTransCodeAsync(VerifySmsCodeInputDto input)
        {
            var typeConfig =
                await SettingManager.GetSettingValueAsync<byte>(AppSettings.UserManagement.OtpSetting
                    .DefaultVerifyTransId);
            if (typeConfig == (byte) CommonConst.VerifyTransType.LevelPassword)
            {
                await VerifyLevel2PasswordAsync(new VerifyLevel2PassDto {Password = input.Code});
            }
            else
            {
                //OTP hoặc ODP
                await _otpAppService.VerifyOtpAsync(new OtpConfirmInput
                {
                    Otp = input.Code,
                    Type = input.Type,
                    PhoneNumber = _topupAppSession.PhoneNumber
                });
            }
        }

        public async Task<bool> CheckLevel2PasswordAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            return !string.IsNullOrEmpty(user.Level2Password);
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            try
            {
                _logger.LogInformation($"UpdateCurrentUserProfile request: {input.ToJson()}");
                var user = await GetCurrentUserAsync();
                if (user.PhoneNumber != input.PhoneNumber)
                {
                    input.IsPhoneNumberConfirmed = false;
                }
                else if (user.IsPhoneNumberConfirmed)
                {
                    input.IsPhoneNumberConfirmed = true;
                }

                if (!string.IsNullOrEmpty(input.Name) && input.Name != user.Name)
                    user.Name = input.Name;
                if (!string.IsNullOrEmpty(input.Surname) && input.Surname != user.Surname)
                    user.Surname = input.Surname;

                if (!string.IsNullOrEmpty(input.EmailAddress) && input.EmailAddress != user.EmailAddress)
                    user.EmailAddress = input.EmailAddress;

                if (!string.IsNullOrEmpty(input.PhoneNumber) && input.PhoneNumber != user.PhoneNumber)
                    user.PhoneNumber = input.PhoneNumber;

                if (input.IsUpdateVerify && (user.AccountType != CommonConst.SystemAccountType.Staff) &&
                    !user.IsVerifyAccount)
                {
                    try
                    {
                        var agentProfile = await UserManager.GetAgentProfile(user.Id);
                        if (!string.IsNullOrEmpty(input.AgentName))
                            user.AgentName = input.AgentName;
                        if (input.CityId.HasValue)
                            agentProfile.CityId = input.CityId.Value;
                        if (input.DistrictId.HasValue)
                            agentProfile.DistrictId = input.DistrictId.Value;
                        if (input.WardId.HasValue)
                            agentProfile.WardId = input.WardId.Value;
                        if (!string.IsNullOrEmpty(input.Address))
                            agentProfile.Address = input.Address;
                        agentProfile.IdType =
                            input.IdType.HasValue ? input.IdType.Value : CommonConst.IdType.IdentityCard;
                        if (!string.IsNullOrEmpty(input.IdentityId))
                            agentProfile.IdIdentity = input.IdentityId;
                        if (!string.IsNullOrEmpty(input.BackSitePhoto))
                            agentProfile.BackSitePhoto = input.BackSitePhoto;
                        if (!string.IsNullOrEmpty(input.FrontPhoto))
                            agentProfile.FrontPhoto = input.FrontPhoto;
                        if (!string.IsNullOrEmpty(input.ExtraInfo))
                            agentProfile.ExtraInfo = input.ExtraInfo;

                        await UserManager.CreateOrUpdateUserProfile(agentProfile);

                        if (agentProfile.CityId > 0
                            && agentProfile.DistrictId > 0
                            && agentProfile.WardId > 0
                            && !string.IsNullOrEmpty(agentProfile.Address)
                            && !string.IsNullOrEmpty(user.AgentName)
                            && !string.IsNullOrEmpty(agentProfile.IdIdentity)
                            && !string.IsNullOrEmpty(agentProfile.FrontPhoto))
                        {
                            // Passport chỉ cần 1 ảnh FrontPhoto
                            if (agentProfile.IdType == CommonConst.IdType.Passport)
                            {
                                user.IsVerifyAccount = true;
                            }
                            // != Passport chỉ cần 2 ảnh
                            else if (!string.IsNullOrEmpty(agentProfile.BackSitePhoto))
                            {
                                user.IsVerifyAccount = true;
                            }
                        }

                        await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                        {
                            AccountCode = user.AccountCode,
                            UserId = user.Id,
                        });
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"UpdateVerify error: {e}");
                    }
                }
                else if (input.IsUpdateVerify && user.IsVerifyAccount && user.AccountType != CommonConst.SystemAccountType.Staff)
                {
                    if (!string.IsNullOrEmpty(input.AgentName))
                        user.AgentName = input.AgentName;
                }

                CheckErrors(await UserManager.UpdateAsync(user));
                if (Clock.SupportsMultipleTimezone)
                {
                    if (StringExtensions.IsNullOrEmpty(input.Timezone))
                    {
                        var defaultValue =
                            await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                            TimingSettingNames.TimeZone, defaultValue);
                    }
                    else
                    {
                        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                            TimingSettingNames.TimeZone, input.Timezone);
                    }
                }


                await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                {
                    UserId = user.Id,
                    AccountCode = user.AccountCode,
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateProfile error: {e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task UpdateUserById(UpdateProfileLimitDto input, int userId)
        {
            try
            {
                _logger.LogInformation($"UpdateUserById request: {input.ToJson()}");
                var user = await UserManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");

                user.Id = userId;
                user.Name = input.Name;
                user.Surname = input.Surname;

                try
                {
                    var agentProfile = await UserManager.GetAgentProfile(user.Id);
                    if (!string.IsNullOrEmpty(input.AgentName))
                        user.AgentName = input.AgentName;
                    if (input.CityId.HasValue)
                        agentProfile.CityId = input.CityId.Value;
                    if (input.DistrictId.HasValue)
                        agentProfile.DistrictId = input.DistrictId.Value;
                    if (input.WardId.HasValue)
                        agentProfile.WardId = input.WardId.Value;
                    if (!string.IsNullOrEmpty(input.Address))
                        agentProfile.Address = input.Address;
                    agentProfile.IdType =
                        input.IdType.HasValue ? input.IdType.Value : CommonConst.IdType.IdentityCard;
                    if (!string.IsNullOrEmpty(input.IdentityId))
                        agentProfile.IdIdentity = input.IdentityId;
                    if (!string.IsNullOrEmpty(input.BackSitePhoto))
                        agentProfile.BackSitePhoto = input.BackSitePhoto;
                    if (!string.IsNullOrEmpty(input.FrontPhoto))
                        agentProfile.FrontPhoto = input.FrontPhoto;
                    if (!string.IsNullOrEmpty(input.ExtraInfo))
                        agentProfile.ExtraInfo = input.ExtraInfo;
                    if (input.IdentityIdExpireDate != null)
                        agentProfile.IdentityIdExpireDate = input.IdentityIdExpireDate;

                    await UserManager.CreateOrUpdateUserProfile(agentProfile);

                    if (agentProfile.CityId > 0
                        && agentProfile.DistrictId > 0
                        && agentProfile.WardId > 0
                        && !string.IsNullOrEmpty(agentProfile.Address)
                        && !string.IsNullOrEmpty(user.AgentName)
                        && !string.IsNullOrEmpty(agentProfile.IdIdentity)
                        && !string.IsNullOrEmpty(agentProfile.FrontPhoto))
                    {
                        // Passport chỉ cần 1 ảnh FrontPhoto
                        if (agentProfile.IdType == CommonConst.IdType.Passport)
                        {
                            user.IsVerifyAccount = true;
                        }
                        // != Passport chỉ cần 2 ảnh
                        else if (!string.IsNullOrEmpty(agentProfile.BackSitePhoto))
                        {
                            user.IsVerifyAccount = true;
                        }
                    }

                    user.AgentName = input.AgentName;

                    await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                    {
                        AccountCode = user.AccountCode
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError($"UpdateVerify error: {e}");
                }

                CheckErrors(await UserManager.UpdateAsync(user));
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateProfile error: {e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            try
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

                var user = await GetCurrentUserAsync();
                if (await UserManager.CheckPasswordAsync(user, input.CurrentPassword))
                {
                    CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
                }
                else
                {
                    CheckErrors(IdentityResult.Failed(new IdentityError
                    {
                        Description = "Incorrect password."
                    }));
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"ChangePassword error: {e}");
                if (e.Message == "[Identity.Password mismatch]")
                {
                    throw new UserFriendlyException("Mật khẩu cũ không chính xác!");
                }
            }
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            try
            {
                _logger.LogInformation($"UpdateProfilePictureInput request: {input.FileToken}");
                await SettingManager.ChangeSettingForUserAsync(
                    AbpSession.ToUserIdentifier(),
                    AppSettings.UserManagement.UseGravatarProfilePicture,
                    input.UseGravatarProfilePicture.ToString().ToLowerInvariant()
                );

                if (input.UseGravatarProfilePicture)
                {
                    return;
                }

                _logger.LogInformation($"UpdateProfilePictureInput1 request: {input.FileToken}");
                byte[] byteArray;

                var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

                if (imageBytes == null)
                {
                    throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
                }

                _logger.LogInformation($"UpdateProfilePictureInput2 request: {input.FileToken}");
                // using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
                // {
                //     var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                //     var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                //     var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);
                //
                //     using (var stream = new MemoryStream())
                //     {
                //         bmCrop.Save(stream, bmpImage.RawFormat);
                //         byteArray = stream.ToArray();
                //     }
                // }


                if (imageBytes.Length > MaxProfilPictureBytes)
                {
                    throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit",
                        AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
                }

                var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

                if (user.ProfilePictureId.HasValue)
                {
                    await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
                }

                var storedFile = new BinaryObject(AbpSession.TenantId, imageBytes);
                await _binaryObjectManager.SaveAsync(storedFile);
                user.ProfilePictureId = storedFile.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateProfilePictureInput error: {e}-{input.FileToken}");
                throw new UserFriendlyException("Cập nhật ảnh đại diện không thành công");
            }
        }

        [AbpAllowAnonymous]
        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity
                        .RequiredLength)
            };

            return new GetPasswordComplexitySettingOutput
            {
                Setting = passwordComplexitySetting
            };
        }

        [DisableAuditing]
        public async Task<GetProfilePictureOutput> GetProfilePicture()
        {
            using (var profileImageService = await _profileImageServiceFactory.Get(AbpSession.ToUserIdentifier()))
            {
                var profilePictureContent = await profileImageService.Object.GetProfilePictureContentForUser(
                    AbpSession.ToUserIdentifier()
                );

                return new GetProfilePictureOutput(profilePictureContent);
            }
        }

        [AbpAllowAnonymous]
        public async Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username)
        {
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            var userIdentifier = new UserIdentifier(AbpSession.TenantId, user.Id);
            using (var profileImageService = await _profileImageServiceFactory.Get(userIdentifier))
            {
                var profileImage = await profileImageService.Object.GetProfilePictureContentForUser(userIdentifier);
                return new GetProfilePictureOutput(profileImage);
            }
        }

        public async Task<GetProfilePictureOutput> GetFriendProfilePicture(GetFriendProfilePictureInput input)
        {
            var friendUserIdentifier = input.ToUserIdentifier();
            var friendShip = await _friendshipManager.GetFriendshipOrNullAsync(
                AbpSession.ToUserIdentifier(),
                friendUserIdentifier
            );

            if (friendShip == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }


            using (var profileImageService = await _profileImageServiceFactory.Get(friendUserIdentifier))
            {
                var image = await profileImageService.Object.GetProfilePictureContentForUser(friendUserIdentifier);
                return new GetProfilePictureOutput(image);
            }
        }


        [AbpAllowAnonymous]
        public async Task<GetProfilePictureOutput> GetProfilePictureByUser(long userId)
        {
            var userIdentifier = new UserIdentifier(AbpSession.TenantId, userId);
            using (var profileImageService = await _profileImageServiceFactory.Get(userIdentifier))
            {
                var profileImage = await profileImageService.Object.GetProfilePictureContentForUser(userIdentifier);
                return new GetProfilePictureOutput(profileImage);
            }
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        public async Task ChangePaymentVerifyMethod(PaymentMethodDto input)
        {
            await _settingManager.ChangePaymentVerifyMethod(input, AbpSession.ToUserIdentifier());
        }

        public async Task<CommonConst.VerifyTransType> GetPaymentVerifyMethod(GetPaymentMothodInput input)
        {
            try
            {
                return await _settingManager.GetPaymentVerifyMethod(input.Channel, AbpSession.ToUserIdentifier());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return null;
            }

            return file.Bytes;
        }

        private async Task<GetProfilePictureOutput> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            return new GetProfilePictureOutput(Convert.ToBase64String(bytes));
        }

        private bool CheckLockFailLevel2PassIsEnabled()
        {
            return true;
            //return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.IsEnabledLevl2Password);
        }

        private int GetSecondLockFailLevel2Pass()
        {
            return 60;
            //return SettingManager.GetSettingValue<int>(AppSettings.UserManagement.DefaultLockTransSeconds);
        }

        private int GetCountFailLockFailLevel2Pass()
        {
            return 3;
            //return SettingManager.GetSettingValue<int>(AppSettings.UserManagement.MaxFailedLelve2Password);
        }
    }
}
