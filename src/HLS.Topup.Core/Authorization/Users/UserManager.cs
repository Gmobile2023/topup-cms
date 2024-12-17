using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using HLS.Topup.AgentsManager;
using HLS.Topup.Authorization.Organization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Security;
using HLS.Topup.Validation;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.Authorization.Users
{
    /// <summary>
    /// User manager.
    /// Used to implement domain logic for users.
    /// Extends <see cref="AbpUserManager{TRole,TUser}"/>.
    /// </summary>
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISettingManager _settingManager;
        private readonly IOrganizationsUnitCustomManager _organizationsUnitCustomManager;
        private readonly IRepository<UserProfile> _userProfile;
        private readonly IRepository<ChangeUserNameHistories> _chaneUserNameHistories;
        private readonly ICacheManager _cacheManager;

        public UserManager(
            UserStore userStore, Microsoft.Extensions.Options.IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager> logger,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager,
            ILocalizationManager localizationManager, IOrganizationsUnitCustomManager organizationsUnitCustomManager,
            IRepository<UserProfile> userProfile, IRepository<ChangeUserNameHistories> chaneUserNameHistories,
            IRepository<UserLogin, long> userLoginRepository)
            : base(
                roleManager,
                userStore,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager,
                userLoginRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _cacheManager = cacheManager;
            _settingManager = settingManager;
            _localizationManager = localizationManager;
            _organizationsUnitCustomManager = organizationsUnitCustomManager;
            _userProfile = userProfile;
            _chaneUserNameHistories = chaneUserNameHistories;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        public override Task<IdentityResult> SetRolesAsync(User user, string[] roleNames)
        {
            if (user.Name == "admin" && !roleNames.Contains(StaticRoleNames.Host.Admin))
            {
                throw new UserFriendlyException(L("AdminRoleCannotRemoveFromAdminUser"));
            }

            return base.SetRolesAsync(user, roleNames);
        }

        public override async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(user, permissions);

            await base.SetGrantedPermissionsAsync(user, permissions);
        }

        public async Task<string> CreateRandomPassword()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequiredLength)
            };

            var upperCaseLetters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var lowerCaseLetters = "abcdefghijkmnopqrstuvwxyz";
            var digits = "0123456789";
            var nonAlphanumerics = "!@$?_-";

            string[] randomChars =
            {
                upperCaseLetters,
                lowerCaseLetters,
                digits,
                nonAlphanumerics
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (passwordComplexitySetting.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    upperCaseLetters[rand.Next(0, upperCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    lowerCaseLetters[rand.Next(0, lowerCaseLetters.Length)]);
            }

            if (passwordComplexitySetting.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    digits[rand.Next(0, digits.Length)]);
            }

            if (passwordComplexitySetting.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    nonAlphanumerics[rand.Next(0, nonAlphanumerics.Length)]);
            }

            for (var i = chars.Count; i < passwordComplexitySetting.RequiredLength; i++)
            {
                var rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public async Task<bool> CheckEmailNotExistAsync(string email, long? userId = null)
        {
            var check = await Users.FirstOrDefaultAsync(p => p.EmailAddress == email);
            if (userId == null)
            {
                return check != null;
            }

            if (check == null)
                return false;
            return check.Id != userId;
        }

        public async Task<bool> CheckPhoneNotExistAsync(string phone, long? userId = null)
        {
            var check = await Users.FirstOrDefaultAsync(p => p.PhoneNumber == phone);
            if (userId == null)
            {
                return check != null;
            }

            if (check == null)
                return false;
            return check.Id != userId;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return Users.FirstOrDefaultAsync(p => p.EmailAddress == email);
        }

        public Task<User> GetUserByMobileAsync(string mobile)
        {
            return Users.FirstOrDefaultAsync(p => p.PhoneNumber == mobile);
        }

        public Task<User> GetUserByMobileOtpAsync(string mobile)
        {
            return Users.FirstOrDefaultAsync(p => p.MobileOtp == mobile);
        }

        public Task<User> GetUserByAccountCodeAsync(string accountcode)
        {
            return Users.FirstOrDefaultAsync(p => p.AccountCode == accountcode);
        }

        public Task<User> GetUserAnyFieldAsync(string search)
        {
            return Users.FirstOrDefaultAsync(p =>
                p.AccountCode == search || p.PhoneNumber == search || p.UserName == search);
        }

        public Task<User> GetUserByAccountTypeAsync(CommonConst.SystemAccountType type)
        {
            return Users.FirstOrDefaultAsync(p => p.AccountType == type);
        }

        public async Task<List<User>> GetUserByAgentTypeAsync(CommonConst.AgentType type)
        {
            return await Users.Where(p => p.AgentType == type).ToListAsync();
        }

        //Todo Hàm này k viết Async
        public UserAccountInfoDto GetAccountInfo()
        {
            return GetAccountInfoById(AbpSession.UserId ?? 0);
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public UserAccountInfoDto GetAccountInfoById(long userId)
        {
            var user = Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
                throw new UserFriendlyException("Thông tin tài khoản không tồn tại");
            if (user.AccountType == CommonConst.SystemAccountType.Staff ||
                user.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                // var network = Users.FirstOrDefault(p => p.Id == user.ParentId);
                // if (network == null)
                //     throw new UserFriendlyException("Thông tin tài khoản nốt mạng không hợp lệ");
                return new UserAccountInfoDto
                {
                    NetworkInfo = _organizationsUnitCustomManager.GetAccountNetwork(user.Id).Result,
                    UserInfo = user.ConvertTo<UserInfoDto>(),
                };
            }


            if (user.AccountType != CommonConst.SystemAccountType.System)
            {
                var org = _organizationsUnitCustomManager.GetOrgUnit(user.Id).Result;
                var networkInfo = user.ConvertTo<UserInfoDto>();
                networkInfo.OrganizationUnitId = org.Id;
                return new UserAccountInfoDto
                {
                    NetworkInfo = networkInfo,
                    UserInfo = user.ConvertTo<UserInfoDto>(),
                };
            }

            return new UserAccountInfoDto
            {
                NetworkInfo = user.ConvertTo<UserInfoDto>(),
                UserInfo = user.ConvertTo<UserInfoDto>(),
            };
        }

        public Task<User> GetUserSearchAsync(string search)
        {
            return Users.FirstOrDefaultAsync(p =>
                p.AccountCode == search || p.PhoneNumber == search || p.EmailAddress == search || p.UserName == search);
        }

        public async Task<List<User>> GetListUserSearch(GetUserQueryRequest request)
        {
            return await Users.Where(p =>
                    p.PhoneNumber.Contains(request.Search) || p.UserName.Contains(request.Search) ||
                    p.AccountCode.Contains(request.Search))
                .WhereIf((request.AgentType != CommonConst.AgentType.Default && (byte)request.AgentType > 0),
                    x => x.AgentType == request.AgentType)
                .WhereIf(request.IsAccountAgent, x => x.AccountType == CommonConst.SystemAccountType.Agent ||
                                                      x.AccountType == CommonConst.SystemAccountType.MasterAgent ||
                                                      x.AccountType == CommonConst.SystemAccountType.MasterAgent)
                .WhereIf(
                    (request.AccountType != CommonConst.SystemAccountType.Default && (byte)request.AccountType > 0),
                    x => x.AccountType == request.AccountType)
                .Take(100)
                .ToListAsync();
        }

        public async Task<List<User>> GetListUserSaleSearch(string search)
        {
            return await Users.Where(p =>
                    p.PhoneNumber.Contains(search) || p.UserName.Contains(search) || p.AccountCode.Contains(search))
                .Where(x => x.AccountType == CommonConst.SystemAccountType.Sale)
                .Take(100)
                .ToListAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await Users.FirstOrDefaultAsync(p => p.UserName == userName);
        }

        public virtual Task SetLockLevel2PasswordEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockTransDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public virtual Task<int> IncrementFailedLevel2PassCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCountLevel2Pass++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetFailedLevel2PassCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCountLevel2Pass = 0;
            return Task.FromResult(0);
        }

        public virtual async Task CreateOrUpdateUserProfile(UserProfileDto input)
        {
            var profile = await _userProfile.GetAllIncluding(x => x.UserFk)
                .FirstOrDefaultAsync(x => x.UserId == input.UserId);
            if (profile == null)
            {
                var item = input.ConvertTo<UserProfile>();
                await _userProfile.InsertAsync(item);
            }
            else
            {
                profile.CityId = input.CityId;
                profile.DistrictId = input.DistrictId;
                profile.WardId = input.WardId;
                profile.Address = input.Address;
                profile.FrontPhoto = input.FrontPhoto;
                profile.BackSitePhoto = input.BackSitePhoto;
                profile.IdIdentity = input.IdIdentity;
                profile.IdType = input.IdType;
                profile.Desscription = input.Desscription;
                profile.ExtraInfo = input.ExtraInfo;
                profile.IdentityIdExpireDate = input.IdentityIdExpireDate;
                profile.ContactInfos = input.ContactInfos;
                profile.ContractNumber = input.ContractNumber;
                profile.EmailReceives = input.EmailReceives;
                profile.PeriodCheck = input.PeriodCheck;
                profile.SigDate = input.SigDate;
                profile.TaxCode = input.TaxCode;
                profile.ChatId = input.ChatId;
                profile.EmailTech = input.EmailTech;
                profile.FolderFtp = input.FolderFtp;
                profile.MethodReceivePassFile = input.MethodReceivePassFile;
                profile.ValueReceivePassFile = input.ValueReceivePassFile;
                if (profile.LimitChannel != input.LimitChannel || profile.IsApplySlowTrans != input.IsApplySlowTrans)
                {
                    profile.LimitChannel = input.LimitChannel;
                    profile.IsApplySlowTrans = input.IsApplySlowTrans;
                    await _userProfile.UpdateAsync(profile);
                    //await ClearCache(profile.UserFk.AccountCode);
                }
            }
        }

        public async Task<bool> UpdateUserNameAsync(UpdateUserNameInputDto input)
        {
            var userUpdate = await GetUserByIdAsync(input.UserId);
            if (userUpdate == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");

            if (input.UserName.Trim().Length != 10)
                throw new UserFriendlyException("Số điện thoại không hợp lệ. Vui lòng kiểm tra lại.");

            var check = await GetUserByUserNameAsync(input.UserName);
            if (check != null)
                throw new UserFriendlyException("Số điện thoại đã tồn tại. Vui lòng chọn số điện thoại khác");

            var oldUsername = userUpdate.UserName;

            userUpdate.UserName = input.UserName;
            userUpdate.PhoneNumber = input.UserName;
            await UpdateAsync(userUpdate);
            var history = new ChangeUserNameHistories
            {
                Attachment = input.Attachment,
                Description = input.Description,
                Status = 0,
                UserId = input.UserId,
                NewUserName = input.UserName,
                OldUserName = oldUsername
            };
            await _chaneUserNameHistories.InsertAsync(history);
            await _unitOfWorkManager.Current.SaveChangesAsync();
            return true;
        }

        public async Task<ResponseMessages> ValidateAccountRegister(string userName)
        {
            var rs = new ResponseMessages();
            if (string.IsNullOrEmpty(userName))
            {
                rs.ResponseCode = ResponseCodeConst.Error;
                rs.ResponseMessage = "Chưa nhập username";
                return rs;
            }

            var check = await GetUserByUserNameAsync(userName);
            if (check != null)
            {
                rs.ResponseCode = "2000";
                rs.ResponseMessage = "Tài khoản đã tồn tại trên hệ thống";
                return rs;
            }

            if (ValidationHelper.IsPhone(userName) == false)
            {
                rs.ResponseCode = ResponseCodeConst.Error;
                rs.ResponseMessage = "Số điện thoại không hợp lệ";
                return rs;
            }


            rs.ResponseCode = ResponseCodeConst.ResponseCode_Success;
            rs.ResponseMessage = "Success";
            return rs;
        }

        public async Task<ResponseMessages> ValidateEmailPhone(string mobile, string email,
            long? userId = null)
        {
            var rs = new ResponseMessages();
            if (string.IsNullOrEmpty(mobile))
            {
                rs.ResponseCode = ResponseCodeConst.Error;
                rs.ResponseMessage = L("Required_PhoneNumber");
                return rs;
            }

            // if (string.IsNullOrEmpty(email))
            // {
            //     rs.ResponseCode = ResponseCodeConst.Error;
            //     rs.ResponseMessage = L("Required_Email");
            //     return rs;
            // }

            // if (await CheckPhoneNotExistAsync(mobile, userId))
            // {
            //     rs.ResponseCode = "2000";
            //     rs.ResponseMessage = "Số điện thoại đã tồn tại";
            //     return rs;
            // }

            if (ValidationHelper.IsPhone(mobile) == false)
            {
                rs.ResponseCode = ResponseCodeConst.Error;
                rs.ResponseMessage = L("MobileInvalid");
                return rs;
            }


            if (!string.IsNullOrEmpty(email) && ValidationHelper.IsEmail(email) == false)
            {
                rs.ResponseCode = "2012";
                rs.ResponseMessage = L("EmailInValid");
                return rs;
            }

            if (!string.IsNullOrEmpty(email) && await CheckEmailNotExistAsync(email, userId))
            {
                rs.ResponseCode = "2008";
                rs.ResponseMessage = "Email đã tồn tại";
                return rs;
            }

            rs.ResponseCode = ResponseCodeConst.ResponseCode_Success;
            rs.ResponseMessage = "Success";
            return rs;
        }

        private void CheckPermissionsToUpdate(User user, IEnumerable<Permission> permissions)
        {
            if (user.Name == AbpUserBase.AdminUserName &&
                (!permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Roles_Edit) ||
                 !permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Users_ChangePermissions)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminUser"));
            }
        }

        private new string L(string name)
        {
            return _localizationManager.GetString(TopupConsts.LocalizationSourceName, name);
        }

        public async Task<UserProfileDto> GetUserProfile(long userId)
        {
            var profile = await _userProfile.FirstOrDefaultAsync(x => x.UserId == userId);
            return profile?.ConvertTo<UserProfileDto>();
        }

        public virtual async Task<UserProfileDto> GetAgentProfile(long userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                long agentId;
                string agentName;
                if (user.AccountType == CommonConst.SystemAccountType.Staff ||
                    user.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    var agent = await _organizationsUnitCustomManager.GetAccountNetwork(user.Id);
                    agentId = agent.Id;
                    agentName = agent.AgentName;
                }
                else
                {
                    agentId = user.Id;
                    agentName = user.AgentName;
                }

                var profile = await _userProfile.FirstOrDefaultAsync(x => x.UserId == agentId);
                var agency = profile?.ConvertTo<UserProfileDto>();
                if (agency == null)
                {
                    agency = new UserProfileDto();
                }

                // gán lại userId = id đại lý
                agency.UserId = agentId;
                agency.AgentName = agentName;

                return agency;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<bool> ClearCache(string accountCode)
        {
            try
            {
                var cache = _cacheManager.GetCache("PartnerInfo");
                var cacheConfig = _cacheManager.GetCache("ServiceConfiguations");
                await cache.ClearAsync();
                await cacheConfig.ClearAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}