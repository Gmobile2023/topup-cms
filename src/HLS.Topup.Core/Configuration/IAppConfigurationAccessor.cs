using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
