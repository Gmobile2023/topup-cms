using HLS.Topup.Configuration;
using HLS.Topup.Url;
using Microsoft.AspNetCore.Hosting;

namespace HLS.Topup.Common
{
    public class UrlExtentions:TopupDomainServiceBase
    {
        private readonly string _viewFile;
        public UrlExtentions(IWebHostEnvironment env)
        {
            var appConfiguration = env.GetAppConfiguration();
            _viewFile = appConfiguration["FtpServer:UrlViewFile"];
        }
        public string GetFullPath(string path)
        {
            if (path == null)
                return null;
            return !path.StartsWith("http") ? (_viewFile.TrimEnd('/') + path) : path;
        }
    }
}
