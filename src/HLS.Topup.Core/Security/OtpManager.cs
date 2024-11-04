using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Net.Sms;
using HLS.Topup.Security.HLS.Topup.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;

namespace HLS.Topup.Security
{
    public class OtpManager : TopupDomainServiceBase, IOtpManager
    {
        private readonly TopupAppSession _topupAppSession;
        private readonly IRepository<Otp> _otpRepository;
        private readonly IRepository<Odp> _odpRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<OtpMessage> _otpMessageRepository;
        private readonly ISmsSender _smsSender;

        private readonly ICacheManager _cacheManager;

        //private readonly Logger _logger = LogManager.GetLogger("OtpManager");
        private readonly ILogger<OtpManager> _logger;

        private const string Key = "75D3BDA3-3D60-48E0-89C4-F40227895AE3"; //fix tạm
        //private readonly IConfigurationRoot _appConfiguration;

        public OtpManager(ICacheManager cacheManager, ISmsSender smsSender, IRepository<Otp> otpRepository,
            TopupAppSession topupAppSession,
            //IConfigurationRoot appConfiguration,
            IRepository<OtpMessage> otpMessageRepository, IRepository<Odp> odpRepository, UserManager userManager,
            ILogger<OtpManager> logger)
        {
            _cacheManager = cacheManager;
            _smsSender = smsSender;
            _otpRepository = otpRepository;
            _topupAppSession = topupAppSession;
            //_appConfiguration = appConfiguration;
            _otpMessageRepository = otpMessageRepository;
            _odpRepository = odpRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task RequestVerifyCode(string phoneNumber, CommonConst.OtpType type, string code = null,
            long? userId = null, bool isResend = false)
        {
            var checkOdb = await IsSendOdp(type);
            if (checkOdb)
                await RequestOdp(phoneNumber, type, code, null, isResend);
            else
                await RequestOtp(phoneNumber, type, code);
        }

        public async Task VerifytCode(string phoneNumber, string otp, CommonConst.OtpType type)
        {
            // var user = await _userManager.GetUserByMobileAsync(phoneNumber);
            // if (user == null || user.AccountType == CommonConst.SystemAccountType.System || user.UserName == "admin")
            //     return;
            var checkOdb = await IsSendOdp(type);
            if (checkOdb)
                await VerifyOdp(phoneNumber, otp, type);
            else
                await VerifyOtp(phoneNumber, otp, type);
        }

        private async Task RequestOdp(string phoneNumber, CommonConst.OtpType type, string code = null,
            long? userId = null, bool isResend = false)
        {
            try
            {
                _logger.LogInformation($"ODP request:{phoneNumber}-{type}");
                var sendSms = false;
                var maxSend =
                    await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.OdpMaxSend);
                _logger.LogInformation($"{phoneNumber}-maxSend:{maxSend}");
                var odpAvailable =
                    await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.OdpAvailable);
                _logger.LogInformation($"{phoneNumber}-odpAvailable:{odpAvailable}");
                var check = await _odpRepository.GetAll().OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                var isEndcode =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOtpIsEncrypted);
                var otpVal = code ?? new Random().Next(0, 9999).ToString("0000");
                var hasOtp = isEndcode ? CryptUtils.HashSHA256(otpVal + Key) : otpVal;

                if (check != null)
                {
                    if (DateTime.Now.AddMinutes(-odpAvailable) < check.RequestDate)
                    {
                        if (isResend)
                        {
                            if (check.CountSend >= maxSend)
                            {
                                _logger.LogInformation(
                                    $"{phoneNumber} đã gửi yêu cầu nhận ODP quá số lần cho phép trong ngày");
                                throw new UserFriendlyException(
                                    $"{phoneNumber} đã gửi yêu cầu nhận ODP quá số lần cho phép trong ngày. Bạn vui lòng xem lại SMS");
                            }
                            else
                            {
                                check.Code = hasOtp;
                                check.CountSend += 1;
                                await _odpRepository.UpdateAsync(check);
                                sendSms = true;
                            }
                        }
                        else
                        {
                            if (check.CountSend >= 1)
                            {
                                _logger.LogInformation($"{phoneNumber} ODP đã được gửi ra");
                            }
                            else
                            {
                                check.Code = hasOtp;
                                check.CountSend += 1;
                                await _odpRepository.UpdateAsync(check);
                                sendSms = true;
                            }
                        }
                    }
                    else
                    {
                        check.Code = hasOtp;
                        check.CountSend = 1;
                        check.RequestDate = DateTime.Now;
                        check.UserId = userId;
                        await _odpRepository.UpdateAsync(check);
                        sendSms = true;
                    }
                }
                else
                {
                    _logger.LogInformation($"{phoneNumber}-New sms");
                    await _odpRepository.InsertAsync(new Odp
                    {
                        Code = hasOtp,
                        Type = type,
                        CountSend = 1,
                        PhoneNumber = phoneNumber,
                        RequestDate = DateTime.Now,
                        UserId = userId
                    });
                    sendSms = true;
                }

                if (sendSms)
                {
                    _logger.LogInformation($"{phoneNumber}-send sms:");
                    //send sms
                    _logger.LogInformation($"Send sms phoneNumber:{phoneNumber}-{otpVal}");
                    await _smsSender.SendAsync(phoneNumber, otpVal, type);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RequestOdp error" + ex);
                throw new UserFriendlyException(ex.Message);
            }
        }

        private async Task VerifyOdp(string phoneNumber, string otpRequest, CommonConst.OtpType type)
        {
            _logger.LogInformation($"ODP Verify:{phoneNumber}-{type}-{otpRequest}");
            var errorCode = 2; //Nếu =2 vẫn cho phép nhập lại otp
            var isIsIgnoreOtp =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsOtpPass);
            if (isIsIgnoreOtp)
            {
                var otpPass = (DateTime.Now.Day + 5).ToString("00") + (DateTime.Now.Month + 7).ToString("00");
                if (otpRequest.Trim() == otpPass)
                    return;
            }

            var isEncrypted =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsOtpIsEncrypted);
            // var countFailConf =
            //     int.Parse(await SettingManager.GetSettingValueAsync(AppSettings.UserManagement.OtpSetting.MaxFailed));
            var hasOtp = isEncrypted ? CryptUtils.HashSHA256(otpRequest + Key) : otpRequest;
            var otp = await _odpRepository.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Code == hasOtp);
            if (otp == null)
            {
                throw new UserFriendlyException(errorCode, "ODP không chính xác");
            }
        }


        private async Task RequestOtp(string phoneNumber, CommonConst.OtpType type, string code = null,
            long? userId = null)
        {
            try
            {
                _logger.LogInformation($"OTP request:{phoneNumber}-{type}");

                //var otpResendConfig = 3;
                //const int timeOut = 1;
                //const int timeAgain = 15;
                //if (DateTime.Now.AddMinutes(-timeOut) >= otp.CreationTime)
                // var currentDate = DateTime.Now;
                // var lastTime = _cacheManager.GetCache("OTP")
                //     .Get($"{_topupAppSession.TenantId ?? 0}:{phoneNumber}:{type:G}:OTP_TIME_AGAIN", () => currentDate);


                // var span = currentDate - lastTime;
                // var totalMinutes = span.TotalMinutes;
                // if (totalMinutes >= timeAgain)
                //     throw new UserFriendlyException(
                //         $"Bạn đã gửi OTP quá {otpResendConfig} lần cho liên tiếp. Vui lòng thử lại sau");

                // var checkOtp = await _otpRepository.GetAll().Where(x =>
                //         x.Status == CommonConst.OtpStatus.Init && DateTime.Now.AddMinutes(-timeOut) < x.CreationTime &&
                //         x.PhoneNumber == phoneNumber)
                //     .CountAsync();
                // if (checkOtp + 1 > otpResendConfig)
                // {
                //     throw new UserFriendlyException(
                //         $"Bạn đã gửi OTP quá {otpResendConfig} lần liên tiếp. Vui lòng thử lại sau");
                // }

                await _cacheManager.GetCache("OTP")
                    .RemoveAsync($"{_topupAppSession.TenantId ?? 0}:{phoneNumber}:{type:G}:OTP_TIME_AGAIN");

                var isEndcode =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOtpIsEncrypted);
                await _cacheManager.GetCache("OTP")
                    .RemoveAsync($"{_topupAppSession.TenantId ?? 0}:{phoneNumber}:{type:G}:OTP_TIME_AGAIN");
                var otpVal = code ?? new Random().Next(0, 9999).ToString("0000");
                var hasOtp = isEndcode ? CryptUtils.HashSHA256(otpVal + Key) : otpVal;
                var otp = new Otp
                {
                    Code = hasOtp,
                    Status = CommonConst.OtpStatus.Init,
                    PhoneNumber = phoneNumber,
                    TenantId = null,
                    UserId = userId,
                    Type = type
                };
                await _otpRepository.InsertAsync(otp);
                //send sms
                await _smsSender.SendAsync(phoneNumber, otpVal, type, true);
            }
            catch (Exception ex)
            {
                Logger.Error("InitOtp error" + ex);
                throw new UserFriendlyException(ex.Message);
            }
        }


        private async Task VerifyOtp(string phoneNumber, string otpRequest, CommonConst.OtpType type)
        {
            _logger.LogInformation($"OTP Verify:{phoneNumber}-{type}-{otpRequest}");
            var errorCode = 2; //Nếu =2 vẫn cho phép nhập lại otp
            var isIsIgnoreOtp =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsOtpPass);

            if (isIsIgnoreOtp)
            {
                var otpPass = (DateTime.Now.Day + 5).ToString("00") + (DateTime.Now.Month + 7).ToString("00");
                if (otpRequest.Trim() == otpPass)
                    return;
            }

            var isEncrypted =
                await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting.IsOtpIsEncrypted);
            var timeOut =
                await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.OtpTimeOut);
            var countFailConf =
                await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.OtpSetting.MaxFailed);
            var hasOtp = isEncrypted ? CryptUtils.HashSHA256(otpRequest + Key) : otpRequest;

            var otpMax = await _otpRepository.GetAll().OrderByDescending(x => x.CreationTime)
                .Where(x => x.PhoneNumber == phoneNumber && x.Type == type &&
                            x.Status == CommonConst.OtpStatus.Init).FirstOrDefaultAsync();

            var otp = await _otpRepository.GetAll().OrderByDescending(x => x.CreationTime).Where(x =>
                    x.PhoneNumber == phoneNumber && x.Code == hasOtp && x.Type == type &&
                    x.Status == CommonConst.OtpStatus.Init)
                .FirstOrDefaultAsync();

            var maxId = 0;
            if (otpMax != null)
            {
                maxId = otpMax.Id;
            }

            var countFail = _cacheManager.GetCache("AbpOtpCache").AsTyped<string, int>().Get(
                $"{_topupAppSession.TenantId ?? 0}_{phoneNumber}_{type:G}_OTP_FAIL_COUNT",
                () => 0);

            if (countFail >= countFailConf)
            {
                errorCode = 0;
                await RemoveOtpCountFail(phoneNumber, type);
                throw new UserFriendlyException(errorCode, "Bạn đã nhập sai quá số lần cho phép OTP");
            }

            if (otp == null)
            {
                countFail++;
                if (countFail >= countFailConf)
                {
                    errorCode = 0;
                    await RemoveOtpCountFail(phoneNumber, type);
                    throw new UserFriendlyException(errorCode,
                        $"Bạn đã nhập sai OTP quá {countFailConf} lần cho phép. Vui lòng thử lại sau");
                }

                await SetOtpCountFail(phoneNumber, type, countFail);
                throw new UserFriendlyException(errorCode, "OTP không chính xác");
            }

            if (maxId != 0 && otp.Id != maxId)
            {
                countFail++;
                if (countFail >= countFailConf)
                {
                    errorCode = 0;
                    await RemoveOtpCountFail(phoneNumber, type);
                    throw new UserFriendlyException(errorCode, "OTP không chính xác");
                }

                await SetOtpCountFail(phoneNumber, type, countFail);
                throw new UserFriendlyException(errorCode, "OTP không chính xác");
            }
            else if (otp.Status != CommonConst.OtpStatus.Init)
            {
                countFail++;
                if (countFail >= countFailConf)
                {
                    errorCode = 0;
                    await RemoveOtpCountFail(phoneNumber, type);
                    throw new UserFriendlyException(errorCode, "OTP không chính xác");
                }

                await SetOtpCountFail(phoneNumber, type, countFail);
                throw new UserFriendlyException(errorCode, "OTP không chính xác");
            }
            else if (DateTime.Now.AddSeconds(-timeOut) >= otp.CreationTime)
            {
                otp.Status = CommonConst.OtpStatus.Timeout;
                await _otpRepository.UpdateAsync(otp);
                await RemoveOtpCountFail(phoneNumber, type);
                throw new UserFriendlyException(errorCode, "OTP đã hết hạn");
            }
            else
            {
                otp.ConfirmDate = DateTime.Now;
                otp.Status = CommonConst.OtpStatus.Success;
                await RemoveOtpCountFail(phoneNumber, type);
            }
        }


        private async Task SetOtpCountFail(string phoneNumber, CommonConst.OtpType type, int countFail)
        {
            await _cacheManager.GetCache("AbpOtpCache")
                .SetAsync($"{_topupAppSession.TenantId ?? 0}_{phoneNumber}_{type:G}_OTP_FAIL_COUNT", countFail);
        }

        private async Task RemoveOtpCountFail(string phoneNumber, CommonConst.OtpType type)
        {
            await _cacheManager.GetCache("AbpOtpCache")
                .RemoveAsync($"{_topupAppSession.TenantId ?? 0}_{phoneNumber}_{type:G}_OTP_FAIL_COUNT");
        }

        public async Task SmsMessageInsertAsync(OtpMessage message)
        {
            try
            {
                await _otpMessageRepository.InsertAsync(message);
            }
            catch (Exception e)
            {
                Logger.Error("Insert sms message error: " + e.Message);
            }
        }

        public async Task<bool> CheckOdpAvailable(string phoneNumner)
        {
            try
            {
                var maxSend =
                    int.Parse(await SettingManager.GetSettingValueAsync(
                        AppSettings.UserManagement.OtpSetting.OdpMaxSend));
                var odpAvailable =
                    int.Parse(await SettingManager.GetSettingValueAsync(AppSettings.UserManagement.OtpSetting
                        .OdpAvailable));
                var check = await _odpRepository.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumner);
                if (check == null) return true;
                if (DateTime.Now.AddSeconds(-odpAvailable) < check.RequestDate)
                {
                    return maxSend < check.CountSend;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("CheckOdp error" + ex);
                return false;
            }
        }

        private async Task<bool> IsSendOdp(CommonConst.OtpType type)
        {
            switch (type)
            {
                case CommonConst.OtpType.Register:
                {
                    var register =
                        await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                            .IsUseOdpRegister);
                    return register;
                }
                case CommonConst.OtpType.ResetPass:
                {
                    var resets =
                        await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                            .IsUseOdpResetPass);
                    return resets;
                }
                case CommonConst.OtpType.Login:
                {
                    var resets =
                        await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                            .IsUseOdpLogin);
                    return resets;
                }
                case CommonConst.OtpType.Payment:
                {
                    return true;
                    //chỗ này bỏ tạm
                    // var check =
                    //     await SettingManager.GetSettingValueAsync<byte>(AppSettings.UserManagement.OtpSetting
                    //         .DefaultVerifyTransId);
                    // return check == (byte) CommonConst.VerifyTransType.Odp;
                }
                default:
                    return await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.OtpSetting
                        .IsOdpVerificationEnabled);
            }
        }
    }
}
