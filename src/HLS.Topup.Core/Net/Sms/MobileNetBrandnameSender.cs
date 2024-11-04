using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Net.Sms
{
    public class MobileNetBrandnameSender : TopupServiceBase, ITransientDependency
    {
        private readonly MobileNetBrandNameSenderConfiguration _mobileNetSenderConfiguration;
        //private readonly Logger _logger = LogManager.GetLogger("MobileNetBrandnameSender");
        private readonly ILogger<MobileNetBrandnameSender> _logger;

        public MobileNetBrandnameSender(MobileNetBrandNameSenderConfiguration mobileNetSenderConfiguration, ILogger<MobileNetBrandnameSender> logger)
        {
            _mobileNetSenderConfiguration = mobileNetSenderConfiguration;
            _logger = logger;
        }

        public async Task<string> SendAsync(string number, string sms,string transCode)
        {
            try
            {
                if (_mobileNetSenderConfiguration.IsSendSms)
                {
                    _logger.LogInformation($"MobileNetBrandnameSender request: {transCode}-{number}-{sms}");
                    var username = _mobileNetSenderConfiguration.UserName;
                    var pass = _mobileNetSenderConfiguration.Password;
                    var brand = _mobileNetSenderConfiguration.Brandname;
                    var type = _mobileNetSenderConfiguration.Type;
                    var sv = new ServiceReference1.sendClient();
                    var result = await sv.sendAsync(username, pass, brand, sms, type, number, transCode);
                    _logger.LogInformation($"MobileNetBrandnameSender response: {transCode}-{number}-{result.@return.ToJson()}");
                    return result.@return.ToJson();
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MobileNetBrandnameSender error: {transCode}-{number}-{ex}");
                return null;
            }
        }
    }
    public class MobileGoSmsBrandNameResponse
    {
        public string detail { get; set; }
        public string idTran { get; set; }
        public string receipt { get; set; }
        public long result { get; set; }
    }
}
