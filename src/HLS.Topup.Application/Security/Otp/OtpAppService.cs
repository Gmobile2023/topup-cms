using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Security.Dto;
using HLS.Topup.Validation;
using Microsoft.Extensions.Logging;
using NLog;
using Org.BouncyCastle.Ocsp;
using ServiceStack;

namespace HLS.Topup.Security.Otp
{
    public class OtpAppService : TopupAppServiceBase, IOtpAppService
    {
        private readonly IOtpManager _manager;

        private readonly TopupAppSession _topupAppSession;

        //private readonly Logger _logger = LogManager.GetLogger("OtpAppService");
        private readonly ILogger<OtpAppService> _logger;

        public OtpAppService(IOtpManager manager, TopupAppSession topupAppSession, ILogger<OtpAppService> logger)
        {
            _manager = manager;
            _topupAppSession = topupAppSession;
            _logger = logger;
        }

        [AbpAuthorize]
        public async Task SendOtpAuthAsync(OtpAuthRequestInput request)
        {
            _logger.LogInformation($"SendOtpAuthAsync request:{request.ToJson()}");
            if (!ValidationHelper.IsPhone(_topupAppSession.UserName))
                throw new UserFriendlyException("Tài khoản không hợp lệ. Username phải là số điện thoại");
            await Task.Run(async () =>
            {
                await _manager.RequestVerifyCode(_topupAppSession.UserName, request.Type, null, AbpSession.UserId,
                    request.IsResend);
            }).ConfigureAwait(false);
        }

        [AbpAuthorize]
        public async Task VerifyOtpAuthAsync(OtpAuthConfirmInput request)
        {
            _logger.LogInformation($"VerifyOtpAuthAsync request:{request.ToJson()}");
            await _manager.VerifytCode(_topupAppSession.UserName, request.Otp, request.Type);
        }

        public async Task SendOtpAsync(OtpRequestInput request)
        {
            _logger.LogInformation($"SendOtpAsync request:{request.ToJson()}");
            if (string.IsNullOrEmpty(request.PhoneNumber))
                throw new UserFriendlyException("Vui lòng nhập số điện thoại");
            await Task.Run(async () =>
            {
                await _manager.RequestVerifyCode(request.PhoneNumber, request.Type, request.Code, null,
                    request.IsResend);
            }).ConfigureAwait(false);
        }

        public async Task VerifyOtpAsync(OtpConfirmInput request)
        {
            _logger.LogInformation($"VerifyOtpAsync request:{request.ToJson()}");
            if (string.IsNullOrEmpty(request.PhoneNumber))
                throw new UserFriendlyException("Vui lòng nhập số điện thoại");
            await _manager.VerifytCode(request.PhoneNumber, request.Otp, request.Type);
        }

        public async Task<bool> CheckOdpAvailable(string phoneNumber)
        {
            return await _manager.CheckOdpAvailable(phoneNumber);
        }
    }
}
