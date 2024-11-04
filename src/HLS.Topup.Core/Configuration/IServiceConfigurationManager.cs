using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Configuration;

namespace HLS.Topup.Configuration
{
    public interface IServiceConfigurationManager
    {
        Task<List<ServiceConfiguationDto>> GetServiceConfiguations(string accountCode, string serviceCode,
            string categoryCode, bool isApplySlowTrans = false, bool masterAccount = false);

        Task<List<ServiceConfiguationDto>> GetServiceConfiguations(string accountCode, string serviceCode,
            string categoryCode,
            string productCode, bool isApplySlowTrans = false, bool masterAccount = false);
    }
}