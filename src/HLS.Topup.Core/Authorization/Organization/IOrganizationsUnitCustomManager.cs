using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;

namespace HLS.Topup.Authorization.Organization
{
    public interface IOrganizationsUnitCustomManager
    {
        Task CreateOrganizationUnit(string displayName, long? parentUserId, long? userId, int? tenantId);
        Task<bool> CheckIsNode(long usserId);
        Task<User> GetAccountNetworkOrg(long usserId);

        Task<UserInfoDto> GetAccountNetwork(long userId);

        Task<OrganizationsUnitCustom> GetOrgUnit(long userId);
    }
}
