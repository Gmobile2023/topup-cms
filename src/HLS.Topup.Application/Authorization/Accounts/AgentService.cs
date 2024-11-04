using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Dtos.Fees;
using HLS.Topup.FeeManager;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.LimitationManager;
using HLS.Topup.LimitationManager.Dtos;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Authorization.Accounts
{
    public class AgentService : TopupAppServiceBase, IAgentService
    {
        //private readonly UserRegistrationManager _registrationManager;
        private readonly TopupAppSession _topupAppSession;
        private readonly IDiscountManger _discountManger;
        private readonly IStaffConfigurationManager _staffConfigurationManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<StaffConfiguration> _staffRepository;
        private readonly ILogger<AccountAppService> _logger;
        private readonly IFeeManager _feeManager;
        private readonly ILimitationManager _limitationManager;
        private readonly TelcoHepper _telcoHepper;
        private readonly IAccountManager _accountManager;

        public AgentService(
            TopupAppSession topupAppSession,
            IDiscountManger discountManger, IStaffConfigurationManager staffConfigurationManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<StaffConfiguration> staffRepository, ILogger<AccountAppService> logger, IFeeManager feeManager,
            ILimitationManager limitationManager, TelcoHepper telcoHepper, IAccountManager accountManager)
        {
            //_registrationManager = registrationManager;
            _topupAppSession = topupAppSession;
            _discountManger = discountManger;
            _staffConfigurationManager = staffConfigurationManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _staffRepository = staffRepository;
            _logger = logger;
            _feeManager = feeManager;
            _limitationManager = limitationManager;
            _telcoHepper = telcoHepper;
            _accountManager = accountManager;
        }

        /// <summary>
        /// Hàm tạo tài khoản Agent cho tổng đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [AbpAuthorize(AppPermissions.Pages_AgentManager_Create)]
        public async Task<AgentInfoDto> CreateAgent(CreateAgentInput input)
        {
            var accountInfo = GetAccountInfo();
            if (accountInfo.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                accountInfo.UserInfo.AccountType == CommonConst.SystemAccountType.System)
                throw new UserFriendlyException("Tài khoản không có quyền tạo Agent");
            var createUser = await _accountManager.CreateUserAsync(new CreateAccountDto
            {
                Channel = CommonConst.Channel.API,
                Name = input.Name,
                Surname = input.SurName,
                Password = input.Password,
                AccountType = CommonConst.SystemAccountType.Agent,
                EmailAddress = input.EmailAddress,
                ParentAccount = _topupAppSession.AccountCode, //Parent account code
                AgentType = CommonConst.AgentType.AgentApi,
                PhoneNumber = input.PhoneNumber,
                //UserName = input.UserName,
                IsEmailConfirmed = true
            });
            if (createUser == null)
                throw new UserFriendlyException("Tạo tài khoản Agent lỗi");
            return createUser.ConvertTo<AgentInfoDto>();
        }

        [AbpAuthorize]
        public async Task<List<ProductDiscountDto>> GetProductDiscounts(ProductDiscountInput input)
        {
            var accountInfo = GetAccountInfo();
            if (string.IsNullOrEmpty(accountInfo.NetworkInfo.AccountCode))
                throw new UserFriendlyException("Tài khoản không hợp lệ");
            return await _discountManger.GetProductDiscounts(input.CategoryCode, accountInfo.NetworkInfo.AccountCode);
        }

        [AbpAuthorize(AppPermissions.Pages_AgentManager)]
        public async Task<PagedResultDto<AgentInfoDto>> GetNetworkAgent(GetAgentNetworkInput input)
        {
            try
            {
                var query = UserManager.Users.Where(x => x.ParentId == AbpSession.UserId)
                    .WhereIf(!string.IsNullOrEmpty(input.AccountCode), u => u.AccountCode == input.AccountCode)
                    .WhereIf(!string.IsNullOrEmpty(input.Email), u => u.EmailAddress == input.Email)
                    .WhereIf(!string.IsNullOrEmpty(input.PhoneNumber), u => u.PhoneNumber == input.PhoneNumber);
                var userCount = await query.CountAsync();
                if (userCount <= 0)
                    return new PagedResultDto<AgentInfoDto>(
                        0,
                        new List<AgentInfoDto>()
                    );
                var users = await query
                    .OrderByDescending(x=>x.Id)
                    .PageBy(input)
                    .ToListAsync();
                var userListDtos = users.ConvertTo<List<AgentInfoDto>>();
                return new PagedResultDto<AgentInfoDto>(
                    userCount,
                    userListDtos
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Lỗi");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_StaffManager_Create)]
        public async Task UpdateUserStaff(UpdateStaffUserInput input)
        {
            await _staffConfigurationManager.UpdateUserStaff(input);
        }

        [AbpAuthorize(AppPermissions.Pages_StaffManager)]
        public async Task<PagedResultDto<UserStaffDto>> GetListUserStaff(GetUserStaffInput input)
        {
            try
            {
                var networkInfo = UserManager.GetAccountInfo();
                var query = from ouUser in _userOrganizationUnitRepository.GetAll()
                    join ou in _organizationUnitRepository.GetAll() on ouUser.OrganizationUnitId equals ou.Id
                    join user in UserManager.Users on ouUser.UserId equals user.Id
                    join staff in _staffRepository.GetAll() on user.Id equals staff.UserId
                    where ouUser.OrganizationUnitId == networkInfo.NetworkInfo.OrganizationUnitId
                    select new UserStaffDto
                    {
                        Gender = user.Gender,
                        Name = user.Name,
                        Surname = user.Surname,
                        AccountCode = user.AccountCode,
                        AccountType = user.AccountType,
                        CreationTime = user.CreationTime,
                        Id = user.Id,
                        DoB = user.DoB,
                        LimitAmount = staff.LimitAmount,
                        EmailAddress = user.EmailAddress,
                        IsActive = user.IsActive,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        NetworkLevel = user.NetworkLevel,
                        TreePath = user.TreePath,
                        Days = staff.Days.FromJson<List<int>>(),
                        FromTime = staff.FromTime.FromJson<StaffTime>(),
                        ToTime = staff.ToTime.FromJson<StaffTime>(),
                        Description = staff.Description,
                        LimitPerTrans = staff.LimitPerTrans
                    };
                if (!string.IsNullOrEmpty(input.Search))
                {
                    query = query.Where(x =>
                        x.AccountCode.ToLower().Contains(input.Search.ToLower()) ||
                        x.PhoneNumber.Contains(input.Search) ||
                        x.UserName.ToLower().Contains(input.Search.ToLower())
                        || x.Name.ToLower().Contains(input.Search.ToLower())
                        || x.Surname.ToLower().Contains(input.Search.ToLower())
                    );
                }

                if (!string.IsNullOrEmpty(input.AccountCode))
                {
                    query = query.Where(x => x.AccountCode == input.AccountCode);
                }

                if (!string.IsNullOrEmpty(input.PhoneNumber))
                {
                    query = query.Where(x => x.PhoneNumber == input.PhoneNumber);
                }

                if (!string.IsNullOrEmpty(input.UserName))
                {
                    query = query.Where(x => x.UserName == input.UserName);
                }

                if (input.IsActive != null)
                {
                    query = query.Where(x => x.IsActive == input.IsActive);
                }

                var userCount = await query.CountAsync();
                if (userCount <= 0)
                    return new PagedResultDto<UserStaffDto>(
                        0,
                        new List<UserStaffDto>()
                    );
                var users = await query
                    .OrderByDescending(x=>x.Id)
                    .PageBy(input)
                    .ToListAsync();
                var userListDtos = users.ConvertTo<List<UserStaffDto>>();
                return new PagedResultDto<UserStaffDto>(
                    userCount,
                    userListDtos
                );
            }
            catch (Exception e)
            {
                return new PagedResultDto<UserStaffDto>(
                    0,
                    new List<UserStaffDto>()
                );
            }
        }


        [AbpAuthorize(AppPermissions.Pages_StaffManager)]
        public async Task<UserStaffDto> GetUserStaffInfo(GetUserStaffInfoInput input)
        {
            return await _staffConfigurationManager.GetUserStaffInfo(input.UserId);
        }

        [AbpAuthorize(AppPermissions.Pages_StaffManager_Create)]
        public async Task CreateUserStaff(CreateStaffUserInput input)
        {
            await _staffConfigurationManager.CreateUserStaff(input);
        }

        [AbpAuthorize]
        public async Task LockUnLockAccount(LockUnlockRequest input)
        {
            try
            {
                _logger.LogInformation($"LockUnlockRequest:{input.ToJson()}");
                var user = await UserManager.GetUserByIdAsync(input.UserId);
                if (user == null)
                    throw new UserFriendlyException("Tải khoản không tồn tại");
                user.IsActive = input.IsActive;
                await UserManager.UpdateAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError($"LockUnLockAccount error:{e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize]
        public async Task<int> GetTotalStaffUser(GetTotalUserStaffInput input)
        {
            try
            {
                var networkInfo = UserManager.GetAccountInfo();
                return await _userOrganizationUnitRepository.GetAll()
                    .Where(x => x.OrganizationUnitId == networkInfo.NetworkInfo.OrganizationUnitId).CountAsync();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        //Hàm này tạm chưa authen
        // public async Task<ProductDiscountDto> GetDiscount(GetProductDiscountDto input)
        // {
        //     return await _discountManger.GetDiscountAccount(input.ProductCode, input.AccountCode);
        // }

        public async Task<ProductDiscountDto> GetProductDiscount(GetProductDiscountDto input)
        {
            _logger.LogInformation($"GetProductDiscount request {input.ToJson()}");
            var rs= await _discountManger.GeProductDiscountAccount(input.ProductCode, input.AccountCode);
            _logger.LogInformation($"GetProductDiscount response {input.TransCode}-{rs?.DiscountValue}");
            return rs;
        }

        public async Task<ProductFeeDto> GetProductFee(GetFreeAccountInput input)
        {
            return await _feeManager.GetProductFee(input.ProductCode, input.AccountCode, input.Amount);
        }

        public async Task<LimitProductDetailDto> GetLimitProductPerDay(GetLimitProductPerDayInput input)
        {
            return await _limitationManager.GetLimitConfigProduct(input.ProductCode, input.AccountCode);
        }
    }
}