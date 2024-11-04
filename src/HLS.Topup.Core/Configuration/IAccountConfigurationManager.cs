using System.Threading.Tasks;
using HLS.Topup.Dtos.Partner;
using HLS.Topup.Dtos.Provider;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.Configuration
{
    public interface IAccountConfigurationManager
    {
        Task<ProviderUpdateInfo> GetProviderInfo(ProviderInfoGetRequest request);
        Task<NewMessageReponseBase<object>> ProviderInfoUpdateRequest(ProviderInfoUpdateRequest request);
        Task<NewMessageReponseBase<object>> ProviderInfoCreateRequest(ProviderInfoCreateRequest request);

        Task<PartnerConfigTransDto> GetPartnerConfig(GetPartnerRequest request);
        Task<NewMessageReponseBase<object>> UpdatePartnerConfig(UpdatePartnerRequest request);
        Task<NewMessageReponseBase<object>> CreatePartnerConfig(CreatePartnerRequest request);
        Task<NewMessageReponseBase<object>> CreateOrUpdatePartnerConfig(CreateOrUpdatePartnerRequest request);
        Task<NewMessageReponseBase<object>> CreateOrUpdateServiceConfig(CreateOrUpdateServiceRequest request);
    }
}
