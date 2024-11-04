using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Report;
using HLS.Topup.Reports;
using HLS.Topup.Security;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Net.Sms
{
    public class MobileNetSender : TopupServiceBase, ISmsSender, ITransientDependency
    {
        private readonly MobileNetSenderConfiguration _mobileNetSenderConfiguration;
        private readonly IReportsManager _reportsManager;

        //private readonly IRepository<OtpMessage> _otpMessageRepository;

        //private readonly Logger _logger = LogManager.GetLogger("MobileNetSender");
        private readonly ILogger<MobileNetSender> _logger;
        private readonly MobileNetBrandnameSender _mobileNetBrandname;
        private readonly MobileGoSender _mobileGoSender;
        private readonly TelcoHepper _telcoHepper;
        private readonly IRepository<OtpMessage> _otpMessageRepository;

        public MobileNetSender(MobileNetSenderConfiguration mobileNetSenderConfiguration,
            MobileNetBrandnameSender mobileNetBrandname,
            TelcoHepper telcoHepper, ILogger<MobileNetSender> logger, MobileGoSender mobileGoSender,
            IReportsManager reportsManager, IRepository<OtpMessage> otpMessageRepository)
        {
            _mobileNetSenderConfiguration = mobileNetSenderConfiguration;
            //_otpMessageRepository = otpMessageRepository;
            _mobileNetBrandname = mobileNetBrandname;
            _telcoHepper = telcoHepper;
            _logger = logger;
            _mobileGoSender = mobileGoSender;
            _reportsManager = reportsManager;
            _otpMessageRepository = otpMessageRepository;
        }

        public async Task SendAsync(string number, string verifycode, CommonConst.OtpType type, bool isOtp = false)
        {
            try
            {
                _logger.LogInformation($"MobileNetSender: {number}");
                var transCode = "NT" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + number;
                var sms = L("Message_Sms", _mobileNetSenderConfiguration.Company, verifycode);
                var smsMsg = new SmsMessageRequest()
                {
                    Message = sms,
                    SmsChannel = CommonConst.SmsChannel.MobileNet,
                    PhoneNumber = number,
                    TransCode = transCode
                };
                _logger.LogInformation($"MobileNetSender request: {number}");
                if (_mobileNetSenderConfiguration.IsSendSms)
                {
                    if (_mobileNetSenderConfiguration.IsUseAllSmsMobileGo)
                    {
                        sms = L("Message_Sms_NT", verifycode);
                        var rs = await _mobileGoSender.SendAsync(number, sms);
                        smsMsg.Result = rs;
                        smsMsg.SmsChannel = CommonConst.SmsChannel.MogileGo;
                    }
                    else if (_mobileNetSenderConfiguration.IsUseAllSmsBrandName)
                    {
                        sms = await GetSmsOtp(verifycode, type, isOtp, true);
                        var rs = await SendBrandName(number, sms,transCode, type);
                        smsMsg.SmsChannel = CommonConst.SmsChannel.MobileNetBrandName;
                        smsMsg.Message = sms;
                        smsMsg.Result = rs;
                    }
                    else if (_mobileNetSenderConfiguration.IsUseAllSmsMobileNet)
                    {
                        var rs = await SendMobileNetSms(number, sms);
                        smsMsg.Result = rs;
                    }
                    else
                    {
                        string channel;
                        if (_mobileNetSenderConfiguration.IsUseVnm)
                        {
                            var telco = _telcoHepper.GetTelco(number);
                            channel = telco == CommonConst.TelcoConst.VietNammobile
                                ? _mobileNetSenderConfiguration.UseVnmChannel
                                : _mobileNetSenderConfiguration.SmsChannel;
                        }
                        else
                        {
                            channel = _mobileNetSenderConfiguration.SmsChannel;
                        }

                        if (channel == CommonConst.SmsChannel.MobileNetBrandName)
                        {
                            sms = await GetSmsOtp(verifycode, type, isOtp, true);
                            var rs = await SendBrandName(number, sms,transCode, type);
                            smsMsg.SmsChannel = CommonConst.SmsChannel.MobileNetBrandName;
                            smsMsg.Message = sms;
                            smsMsg.Result = rs;
                        }
                        else if (channel == CommonConst.SmsChannel.MogileGo)
                        {
                            sms = L("Message_Sms_NT", verifycode);
                            var rs = await _mobileGoSender.SendAsync(number, sms);
                            smsMsg.SmsChannel = CommonConst.SmsChannel.MogileGo;
                            smsMsg.Result = rs;
                        }
                        else
                        {
                            var rs = await SendMobileNetSms(number, sms);
                            smsMsg.SmsChannel = CommonConst.SmsChannel.MobileNet;
                            smsMsg.Result = rs;
                        }
                    }

                    // var request =
                    //     $"{_mobileNetSenderConfiguration.Url}/onsmsapi/sendsms.jsp?username={_mobileNetSenderConfiguration.UserName}&pass={_mobileNetSenderConfiguration.Password}&key={_mobileNetSenderConfiguration.Key}&phonesend={number}&smsid={_mobileNetSenderConfiguration.Smsid}&param={DateTime.Now:dd/MM/yyyy}__{message}__{DateTime.Now:hh:mm:ss}&sender={_mobileNetSenderConfiguration.SenderNumber}";
                    // var result = await request.GetJsonFromUrlAsync();
                }
                //Hàm này sau chạy ổn định bỏ lưu vào sql
                // await _otpMessageRepository.InsertAsync(new OtpMessage
                // {
                //     Channel = smsMsg.SmsChannel,
                //     Message = smsMsg.Message,
                //     Result = smsMsg.Result,
                //     PhoneNumer = smsMsg.PhoneNumber
                // });
                await SaveSmsMessage(smsMsg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Send sms error:" + ex);
            }
        }

        private async Task<string> SendMobileNetSms(string number, string sms)
        {
            try
            {
                _logger.LogInformation($"MobileNetSender request: {number}");
                var request =
                    $"{_mobileNetSenderConfiguration.Url}/onsmsapi/sendsms.jsp?username={_mobileNetSenderConfiguration.UserName}&pass={_mobileNetSenderConfiguration.Password}&key={_mobileNetSenderConfiguration.Key}&phonesend={number}&smsid={_mobileNetSenderConfiguration.Smsid}&param={sms}&sender={_mobileNetSenderConfiguration.SenderNumber}";
                var result = await request.GetJsonFromUrlAsync();
                _logger.LogInformation($"SMS sender mobile: {number} - Message: {sms} - Result: {result}");
                return result;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"MobileNetSender error: {e}");
                return null;
            }
        }

        private async Task<string> SendBrandName(string number, string sms,string transCode, CommonConst.OtpType type)
        {
            return await _mobileNetBrandname.SendAsync(number, sms,transCode);
        }

        private async Task<string> GetSmsOtp(string code, CommonConst.OtpType type, bool isOtp, bool isBrandName)
        {
            //Hàm này hiện tại chưa cần chia ra các loại tin nhắn khác khau
            // if (type == CommonConst.OtpType.Login)
            // {
            //     var isOdp = await SettingManager.GetSettingValueAsync<bool>(AppSettings
            //         .UserManagement
            //         .OtpSetting.IsUseOdpLogin);
            //     sms = isOdp
            //         ? L("Sms_Odp_Login",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"))
            //         : L("Sms_Otp_Login",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"));
            // }
            // else if (type == CommonConst.OtpType.ResetPass)
            // {
            //     var isOdp = await SettingManager.GetSettingValueAsync<bool>(AppSettings
            //         .UserManagement
            //         .OtpSetting.IsUseOdpResetPass);
            //     sms = isOdp
            //         ? L("Sms_Odp_ResetPass",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"))
            //         : L("Sms_Otp_ResetPass",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"));
            // }
            // else if (type == CommonConst.OtpType.Register)
            // {
            //     var isOdp = await SettingManager.GetSettingValueAsync<bool>(AppSettings
            //         .UserManagement
            //         .OtpSetting.IsUseOdpRegister);
            //     sms = isOdp
            //         ? L("Sms_Odp_Register",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"))
            //         : L("Sms_Otp_Register",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"));
            // }
            // else if (type == CommonConst.OtpType.Payment)
            // {
            //     var odbTimeout = await SettingManager.GetSettingValueAsync<int>(AppSettings
            //         .UserManagement
            //         .OtpSetting.OdpAvailable);
            //     var typeId = await SettingManager.GetSettingValueAsync<byte>(AppSettings
            //         .UserManagement
            //         .OtpSetting.DefaultVerifyTransId);
            //     sms = typeId == (byte) CommonConst.VerifyTransType.Odp
            //         ? L("Sms_OdpTrans_Message",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"),
            //             odbTimeout / 60)
            //         : L("Sms_OtpTrans_Message",
            //             _mobileNetSenderConfiguration.Company,
            //             code, DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"));
            // }
            // else
            // {
            //     var isOdp = await SettingManager.GetSettingValueAsync<bool>(AppSettings
            //         .UserManagement
            //         .OtpSetting.IsOdpVerificationEnabled);
            //     var odbTimeout = await SettingManager.GetSettingValueAsync<int>(AppSettings
            //         .UserManagement
            //         .OtpSetting.OdpAvailable);
            //     if (isOdp)
            //     {
            //         var hour = odbTimeout / 60;
            //         sms = L("Sms_Odp_Message", _mobileNetSenderConfiguration.Company, code,
            //             DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"),
            //             hour + "h");
            //     }
            //     else
            //     {
            //         sms = L("Sms_Otp_Message", _mobileNetSenderConfiguration.Company, code,
            //             DateTime.Now.ToString("hh:mm"), DateTime.Now.ToString("dd/MM/yyyy"));
            //     }
            // }
            if (!isBrandName)
                return L("Message_Sms", _mobileNetSenderConfiguration.Company, code);

            if (isOtp)
            {
                return L("Sms_Otp_Message", _mobileNetSenderConfiguration.Company, code,
                    DateTime.Now.ToString("hh:mm"),
                    DateTime.Now.ToString("dd/MM/yyyy"));
            }

            var odbTimeout = await SettingManager.GetSettingValueAsync<int>(AppSettings
                .UserManagement
                .OtpSetting.OdpAvailable);
            var hour = odbTimeout / 60;
            return L("Sms_Odp_Message", _mobileNetSenderConfiguration.Company, code,
                DateTime.Now.ToString("HH:mm"),
                DateTime.Now.ToString("dd/MM/yyyy"), hour + "h");
        }

        private async Task SaveSmsMessage(SmsMessageRequest request)
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (request.SmsChannel == CommonConst.SmsChannel.MobileNetBrandName)
                    {
                        if (!string.IsNullOrEmpty(request.Result))
                        {
                            var info = request.Result.FromJson<MobileGoSmsBrandNameResponse>();
                            if (info != null && info.result == 0)
                            {
                                request.Status = 1;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.Result) && request.Result.Equals("1"))
                        {
                            request.Status = 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                request.Channel = CommonConst.Channel.WEB; //Xem lại lấy đúng kênh
                await _reportsManager.InsertSmsMessage(request);
            }).ConfigureAwait(false);
        }
    }
}
