using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Configuration;

namespace HLS.Topup.Configuration
{
    public interface IPartnerServiceConfigurationManager
    {
        Task<List<PartnerServiceConfiguationDto>> GetPartnerServiceConfiguations(string accountCode, string serviceCode, string categoryCode);
    }
}
