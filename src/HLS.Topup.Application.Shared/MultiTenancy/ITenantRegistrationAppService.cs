using System.Threading.Tasks;
using Abp.Application.Services;
using HLS.Topup.Editions.Dto;
using HLS.Topup.MultiTenancy.Dto;

namespace HLS.Topup.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}