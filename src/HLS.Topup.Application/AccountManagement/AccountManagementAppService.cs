using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.AccountManagement.Dtos;
using HLS.Topup.AccountManagement.Exporting;
using HLS.Topup.Authorization;
using HLS.Topup.Dto;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Reports;

namespace HLS.Topup.AccountManagement
{
    [AbpAuthorize]
    public class AccountManagementAppService : TopupAppServiceBase, IAccountManagementAppService
    {
        private readonly IAccountManager _accountManager;
        private readonly IAccountManagementAppServiceExport _accountManagementAppServiceExport;
        private readonly TopupAppSession _topupAppSession;
        private readonly IReportsManager _reportManager;

        public AccountManagementAppService(IAccountManager accountManager, IAccountManagementAppServiceExport accountManagementAppServiceExport, TopupAppSession topupAppSession, IReportsManager reportManager)
        {
            _accountManager = accountManager;
            _topupAppSession = topupAppSession;
            _accountManagementAppServiceExport = accountManagementAppServiceExport;
            _reportManager = reportManager;
        }

        public async Task<PagedResultDto<UserProfileDto>> GetAllSubAgents(GetSubAgenstInput input)
        {
            var filter = UserManager.Users
                .Where(x => x.ParentId == AbpSession.UserId)
                .Where(x => x.AccountType == CommonConst.SystemAccountType.Agent)
                .Where(x => x.AgentType == CommonConst.AgentType.SubAgent)
                .WhereIf(
                    !string.IsNullOrEmpty(input.Filter),
                    x => x.PhoneNumber == input.Filter || x.UserName == input.Filter ||
                         x.AccountCode == input.Filter ||
                         x.Name.Contains(input.Filter) || x.Surname.Contains(input.Filter))
                .WhereIf(input.Status != null, x => x.IsActive == input.Status);

            var pagedAndFiltered = filter
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.Name)
                .PageBy(input);

            var users = pagedAndFiltered.Select(x => new UserProfileDto
            {
                Name = x.Name,
                Surname = x.Name,
                UserId = x.Id,
                IsActive = x.IsActive,
                AccountCode = x.AccountCode,
                FullName = x.FullName,
                AgentType = x.AgentType,
                AgentName = x.AgentName,
                PhoneNumber = x.PhoneNumber,
                CreationTime = x.CreationTime
            });
            var totalCount = await users.CountAsync();

            return new PagedResultDto<UserProfileDto>(
                totalCount,
                await users.ToListAsync()
            );
        }

        public async Task<UserProfileDto> GetAccount(EntityDto<long> input)
        {
            return await _accountManager.GetAccount(input.Id);
        }

        /// <summary>
        /// Hàm tạo đại lý con
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrEditSubAgent(CreateAccountDto input)
        {
            input.TenantId = AbpSession.TenantId;
            input.ParentAccount = !string.IsNullOrEmpty(input.ParentAccount) ? input.ParentAccount : _topupAppSession.AccountCode;
            input.AccountType = CommonConst.SystemAccountType.Agent;
            input.AgentType = CommonConst.AgentType.SubAgent;
            input.IsEmailConfirmed = true;
            input.IsVerifyAccount = true;
            if (input.Id.HasValue)
            {
                await Update(input);
            }
            else
            {
                await Create(input);
            }
        }


        /// <summary>
        /// Hàm tạo đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrEditAgent(CreateAccountDto input)
        {
            input.TenantId = AbpSession.TenantId;
            input.AccountType = CommonConst.SystemAccountType.MasterAgent;
            input.AgentType = input.AgentType;
            input.IsEmailConfirmed = true;
            input.IsVerifyAccount = true;
            if (input.Id.HasValue)
            {
                await Update(input);
            }
            else
            {
                await Create(input);
            }
        }

        /// <summary>
        /// Update đại lý con
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task Update(CreateAccountDto input)
        {
            var user = await _accountManager.UpdateUserAsync(input);
            if (user != null)
            {
                await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                {
                    UserId = user.Id,
                    AccountCode = user.AccountCode,
                });
            }
        }

        private async Task Create(CreateAccountDto input)
        {
            if (string.IsNullOrEmpty(input.Password))
                throw new UserFriendlyException("Vui lòng nhập mật khẩu");
            var user = await _accountManager.CreateUserAsync(input);
            if (user != null)
            {
                await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                {
                    UserId = user.Id,
                    AccountCode = user.AccountCode,
                });
            }
        }

        public async Task<FileDto> GetAllSubAgentsListToExcel(GetSubAgenstInput input)
        {
            var filter = UserManager.Users
                .Where(x => x.AccountType == CommonConst.SystemAccountType.Agent)
                .Where(x => x.AgentType == CommonConst.AgentType.SubAgent)
                .Where(x => x.ParentId == AbpSession.UserId)
                .WhereIf(
                    !string.IsNullOrEmpty(input.Filter),
                    x => x.PhoneNumber == input.Filter || x.UserName == input.Filter ||
                         x.AccountCode == input.Filter ||
                         x.Name.Contains(input.Filter) || x.Surname.Contains(input.Filter))
                .WhereIf(input.Status != null, x => x.IsActive == input.Status);

            var pagedAndFiltered = filter
                .OrderByDescending(x => x.Id).ThenBy(x => x.Name);


            var users = pagedAndFiltered.Select(x => new UserProfileDto
            {
                Name = x.Name,
                Surname = x.Name,
                UserId = x.Id,
                IsActive = x.IsActive,
                AccountCode = x.AccountCode,
                FullName = x.FullName,
                AgentType = x.AgentType,
                AgentName = x.AgentName,
                PhoneNumber = x.PhoneNumber,
                CreationTime = x.CreationTime
            });

            var data = await users.ToListAsync();

            return _accountManagementAppServiceExport.ExportToFile(data);
        }
    }
}
