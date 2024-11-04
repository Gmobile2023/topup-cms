using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using System;
using System.Threading.Tasks;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServiceStack;

namespace HLS.Topup.Authorization.Organization
{
    public class OrganizationsUnitCustomManager : TopupDomainServiceBase, IOrganizationsUnitCustomManager
    {
        private readonly IRepository<OrganizationsUnitCustom, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<User, long> _userRepository;
        //private readonly Logger _logger = LogManager.GetLogger("OrganizationsUnitCustomManager");
        private readonly ILogger<OrganizationsUnitCustomManager> _logger;

        public OrganizationsUnitCustomManager(IRepository<OrganizationsUnitCustom, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IRepository<User, long> userRepository, ILogger<OrganizationsUnitCustomManager> logger)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task CreateOrganizationUnit(string displayName, long? parentUserId, long? userId,
            int? tenantId)
        {
            try
            {
                if (userId != null && !await CheckIsNode(userId ?? 0))
                {
                    throw new UserFriendlyException("Nốt mạng đã tồn tại");
                }

                long? parentId = null;
                if (parentUserId != null)
                {
                    var orgParent =
                        await _organizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == parentUserId);
                    if (orgParent != null)
                        parentId = orgParent.Id;
                }

                var organizationUnit = new OrganizationsUnitCustom
                {
                    TenantId = tenantId,
                    DisplayName = displayName,
                    ParentId = parentId,
                    UserId = userId,
                    Status = 1
                };

                await _organizationUnitManager.CreateAsync(organizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrganizationUnit error: {ex}");
                throw new UserFriendlyException("Tạo nốt mạng không thành công");
            }
        }

        public async Task<bool> CheckIsNode(long usserId)
        {
            var root = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == usserId);
            if (root != null)
                return false;
            var userInRoot = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == usserId);
            return userInRoot == null;
        }

        public async Task<User> GetAccountNetworkOrg(long usserId)
        {
            var root = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == usserId);
            if (root != null)
                return await _userRepository.FirstOrDefaultAsync(usserId);
            var userInOrg = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == usserId);
            if (userInOrg == null)
                return null;
            var orgRoot =
                await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == userInOrg.OrganizationUnitId);
            if (orgRoot == null)
                return null;

            return await _userRepository.FirstOrDefaultAsync(orgRoot.UserId ?? 0);
        }


        public async Task<UserInfoDto> GetAccountNetwork(long userId)
        {
            var ogrUser = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (ogrUser == null)
                return null;
            var ognetwork =
                await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == ogrUser.OrganizationUnitId);
            if (ognetwork?.UserId == null)
                return null;
            var userNetwork = await _userRepository.FirstOrDefaultAsync(ognetwork.UserId ?? 0);
            var info = userNetwork?.ConvertTo<UserInfoDto>();
            if (info == null) return null;
            info.OrganizationUnitId = ognetwork.Id;
            return info;
        }

        public async Task<OrganizationsUnitCustom> GetOrgUnit(long userId)
        {
            var ognetwork =
                await _organizationUnitRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            return ognetwork;
        }
    }
}
