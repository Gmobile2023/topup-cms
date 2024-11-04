using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Address;
using HLS.Topup.AgentManagerment.Exporting;
using HLS.Topup.Audit;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.PostManagement.Dtos;
using HLS.Topup.Reports;
using HLS.Topup.Sale;
using HLS.Topup.Security.HLS.Topup.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.PostManagement
{
    [AbpAuthorize(AppPermissions.Pages_PostManagement)]
    public class PostManagementAppService : TopupAppServiceBase, IPostManagementAppService
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly ILogger<PostManagementAppService> _logger;
        private readonly TopupAppSession _topupAppSession;
        //private readonly UserRegistrationManager _registrationManager;
        private readonly IAccountManager _accountManager;

        public PostManagementAppService(IRepository<UserProfile> userProfileRepository,
            ILogger<PostManagementAppService> logger,
            TopupAppSession topupAppSession, IAccountManager accountManager)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
            _topupAppSession = topupAppSession;
            _accountManager = accountManager;
            //_registrationManager = registrationManager;
        }

        public async Task<PagedResultDto<PostManagementDto>> GetAll(GetPostsInput input)
        {
            var filter = UserManager.Users
                .Where(x => x.AccountType == CommonConst.SystemAccountType.Agent)
                .Where(x => x.AgentType == CommonConst.AgentType.Agent)
                .Where(x => x.ParentId == AbpSession.UserId)
                .WhereIf(
                    !string.IsNullOrEmpty(input.Filter),
                    x => x.PhoneNumber == input.Filter || x.UserName == input.Filter ||
                         x.AccountCode == input.Filter ||
                         x.Name.Contains(input.Filter) || x.Surname.Contains(input.Filter))
                .WhereIf(input.Status != null, x => x.IsActive == input.Status);

            var pagedAndFiltered = filter
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.Name)
                .PageBy(input);

            var users = pagedAndFiltered.Select(x => new PostManagementDto
            {
                Name = x.Name,
                Surname = x.Name,
                Id = x.Id,
                IsActive = x.IsActive,
                AccountCode = x.AccountCode,
                FullName = x.FullName,
                AgentType = x.AgentType,
                AgentName = x.AgentName,
                PhoneNumber = x.PhoneNumber,
                CreationTime = x.CreationTime
            });
            var totalCount = await users.CountAsync();

            return new PagedResultDto<PostManagementDto>(
                totalCount,
                await users.ToListAsync()
            );
        }

        public async Task<PostManagementDto> GetAgentDetail(long userId)
        {
            var info = (from agent in UserManager.Users.Where(x => x.Id == userId)
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                select new PostManagementDto()
                {
                    Id = agent.Id,
                    AccountCode = agent.AccountCode,
                    PhoneNumber = agent.PhoneNumber,
                    FullName = agent.FullName,
                    Name = agent.Name,
                    Surname = agent.Surname,
                    CreationTime = agent.CreationTime,
                    IsActive = agent.IsActive,
                    Address = profile.Address,
                    AgentName = agent.AgentName,
                    CityId = profile.CityId ?? 0,
                    DistrictId = profile.DistrictId ?? 0,
                    WardId = profile.WardId ?? 0,
                    Description = profile.Desscription,
                }).FirstOrDefault();
            return info;
        }

        [AbpAuthorize(AppPermissions.Pages_PostManagement_Create, AppPermissions.Pages_PostManagement_Edit)]
        public async Task CreateOrEdit(PostManagementDto input)
        {
            if (input.Id.HasValue)
            {
                await Update(input);
            }
            else
            {
                await Create(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PostManagement_Edit)]
        private async Task Update(PostManagementDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id ?? 0);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.IsActive = input.IsActive;
            user.AgentName = input.AgentName;
            CheckErrors(await UserManager.UpdateAsync(user));
            var profile = new UserProfileDto
            {
                AgentName = input.AgentName,
                CityId = input.CityId,
                DistrictId = input.DistrictId,
                WardId = input.WardId,
                Address = input.Address,
                UserId = user.Id,
                TenantId = AbpSession.TenantId,
                Desscription = input.Description
            };
            await UserManager.CreateOrUpdateUserProfile(profile);
        }

        [AbpAuthorize(AppPermissions.Pages_PostManagement_Create)]
        private async Task Create(PostManagementDto input)
        {
            if (string.IsNullOrEmpty(input.Password))
                throw new UserFriendlyException("Vui lòng nhập mật khẩu");
            var createUser = await _accountManager.CreateUserAsync(new CreateAccountDto
            {
                Channel = CommonConst.Channel.WEB,
                Name = input.Name,
                Surname = input.Surname,
                Password = input.Password,
                AccountType = CommonConst.SystemAccountType.Agent,
                ParentAccount = _topupAppSession.AccountCode, //Parent account code
                AgentType = CommonConst.AgentType.Agent,
                PhoneNumber = input.PhoneNumber,
                IsEmailConfirmed = true,
                IsActive = input.IsActive,
                IsVerifyAccount = true,
                AgentName = input.AgentName,
                AccountCode = input.AccountCode
            });
            if (createUser == null)
                throw new UserFriendlyException("Tạo tài khoản Agent lỗi");

            var profile = new UserProfileDto
            {
                AgentName = input.AgentName,
                CityId = input.CityId,
                DistrictId = input.DistrictId,
                WardId = input.WardId,
                Address = input.Address,
                UserId = createUser.Id,
                TenantId = AbpSession.TenantId,
                Desscription = input.Description
            };
            await UserManager.CreateOrUpdateUserProfile(profile);
        }
    }
}
