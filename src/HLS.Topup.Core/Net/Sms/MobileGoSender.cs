using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Abp.Dependency;
using Microsoft.Extensions.Logging;
using ServiceStack;
using Twilio.Http;

namespace HLS.Topup.Net.Sms
{
    public class MobileGoSender : TopupServiceBase, ITransientDependency
    {
        private readonly MobileGoSenderConfiguration _mobileGoConfiguration;
        private readonly ILogger<MobileNetSender> _logger;

        public MobileGoSender(ILogger<MobileNetSender> logger,
            MobileGoSenderConfiguration mobileGoConfiguration)
        {
            _logger = logger;
            _mobileGoConfiguration = mobileGoConfiguration;
        }

        public async Task<string> SendAsync(string number, string sms)
        {
            try
            {
                if (_mobileGoConfiguration.IsSendSms)
                {
                    _logger.LogInformation($"NTSender request: {number}-{sms}");
                    //var request = $"{_mobileGoConfiguration.Url}/api?action=sendmessage&username={_mobileGoConfiguration.UserName}&password={_mobileGoConfiguration.Password}&recipient={number}&messagedata={sms}";
                    var request = $"{_mobileGoConfiguration.Url}/http/send-message?to={number}&message={sms}";
                    var result = await request.GetStringFromUrlAsync();
                    _logger.LogInformation($"NTSender: {number} - Message: {sms} - Result: {result}");
                    return "0";
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"NTSender error: {number}-{ex}");
                return null;
            }
        }
    }

    public class MobileSmsResult
    {
        [DataMember(Name = "response")] public MobileSmsResponse Response { get; set; }
    }

    public class MobileSmsResponse
    {
        [DataMember(Name = "action")] public string Action { get; set; }

        [DataMember(Name = "data")] public MobileSmsData Data { get; set; }
    }

    public class MobileSmsData
    {
        [DataMember(Name = "acceptreport")] public MobileSmsAcceptreport Acceptreport { get; set; }
    }

    public class MobileSmsAcceptreport
    {
        [DataMember(Name = "statuscode")] public long Statuscode { get; set; }

        [DataMember(Name = "statusmessage")] public string Statusmessage { get; set; }

        [DataMember(Name = "messageid")] public Guid Messageid { get; set; }

        [DataMember(Name = "originator")] public string Originator { get; set; }

        [DataMember(Name = "recipient")] public string Recipient { get; set; }

        [DataMember(Name = "messagetype")] public string Messagetype { get; set; }

        [DataMember(Name = "messagedata")] public string Messagedata { get; set; }
    }
}
