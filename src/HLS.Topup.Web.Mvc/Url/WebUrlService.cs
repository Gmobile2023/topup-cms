using Abp.Dependency;
using HLS.Topup.Configuration;
using HLS.Topup.Url;

namespace HLS.Topup.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
        public override string ServerFileAddressFormatKey => "App:ServerFileAddress";
    }
}
