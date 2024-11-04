using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using HLS.Topup.Address;
using HLS.Topup.Authorization.Organization;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Notifications;
using HLS.Topup.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.AccountManager
{
    public class AccountManager : TopupDomainServiceBase, IAccountManager
    {
        private IAbpSession AbpSession { get; set; }
        private IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly ILogger<AccountManager> _logger;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IOrganizationsUnitCustomManager _organizationsUnitCustomManager;
        private readonly INotificationManger _notificationManger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUserEmailer _userEmail;
        private readonly IAppNotifier _appNotifier;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IRepository<User, long> _lookupUserRepository;

        public AccountManager(UserManager userManager, RoleManager roleManager,
            IOrganizationsUnitCustomManager organizationsUnitCustomManager,
            INotificationManger notificationManger,
            IPasswordHasher<User> passwordHasher, IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILogger<AccountManager> logger, IRepository<City> cityRepository, IRepository<District> districtRepository,
            IRepository<Ward> wardRepository, IRepository<UserProfile> userProfileRepository, IUserEmailer userEmail,
            IAppNotifier appNotifier, INotificationSubscriptionManager notificationSubscriptionManager,
            IRepository<User, long> lookupUserRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organizationsUnitCustomManager = organizationsUnitCustomManager;
            _notificationManger = notificationManger;
            _passwordHasher = passwordHasher;
            _passwordValidators = passwordValidators;
            _logger = logger;
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _userProfileRepository = userProfileRepository;
            _userEmail = userEmail;
            _appNotifier = appNotifier;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _lookupUserRepository = lookupUserRepository;
        }

        public async Task<UserProfileDto> GetAccount(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
                return null;
            var output = user.ConvertTo<UserProfileDto>();
            output.UserId = user.Id;
            var profile = await _userProfileRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (profile != null)
            {
                output.IdIdentity = profile.IdIdentity;
                output.Address = profile.Address;
                output.Desscription = profile.Desscription;
                output.CityId = profile.CityId;
                output.DistrictId = profile.DistrictId;
                output.WardId = profile.WardId;
                output.ContractNumber = profile.ContractNumber;
                output.SigDate = profile.SigDate;
                output.EmailTech = profile.EmailTech;
                output.FolderFtp = profile.FolderFtp;
                output.IdType = profile.IdType;
                output.IdIdentity = profile.IdIdentity;
                output.IdentityIdExpireDate = profile.IdentityIdExpireDate;
                output.IdentityIdExpireDate = profile.IdentityIdExpireDate;
                output.FrontPhoto = profile.FrontPhoto;
                output.BackSitePhoto = profile.BackSitePhoto;
                if (profile.MethodReceivePassFile.HasValue)
                    output.MethodReceivePassFile = profile.MethodReceivePassFile;
                output.ValueReceivePassFile = profile.ValueReceivePassFile;
                if (user.ParentId != null)
                {
                    output.ParentId = user.ParentId;
                    output.ParentAccount = user.ParentCode;
                    var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) user.ParentId);
                    output.ParentName = _lookupUser != null
                        ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                        : "";
                }
            }

            return output;
        }

        public async Task<UserProfileDto> GetAccountByCode(string accountCode)
        {
            var user = await _userManager.GetUserByAccountCodeAsync(accountCode);
            if (user == null)
                return null;
            var output = user.ConvertTo<UserProfileDto>();
            output.UserId = user.Id;
            var profile = await _userProfileRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (profile != null)
            {
                output.IdIdentity = profile.IdIdentity;
                output.Address = profile.Address;
                output.Desscription = profile.Desscription;
                output.CityId = profile.CityId;
                output.DistrictId = profile.DistrictId;
                output.WardId = profile.WardId;
                output.ContractNumber = profile.ContractNumber;
                output.SigDate = profile.SigDate;
                output.EmailTech = profile.EmailTech;
                output.FolderFtp = profile.FolderFtp;
                output.IdType = profile.IdType;
                output.IdIdentity = profile.IdIdentity;
                output.IdentityIdExpireDate = profile.IdentityIdExpireDate;
                output.IdentityIdExpireDate = profile.IdentityIdExpireDate;
                output.FrontPhoto = profile.FrontPhoto;
                output.BackSitePhoto = profile.BackSitePhoto;
                if (profile.MethodReceivePassFile.HasValue)
                    output.MethodReceivePassFile = profile.MethodReceivePassFile;
                output.ValueReceivePassFile = profile.ValueReceivePassFile;
                if (user.ParentId != null)
                {
                    output.ParentId = user.ParentId;
                    var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) user.ParentId);
                    output.ParentName = _lookupUser != null
                        ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                        : "";
                }
            }

            return output;
        }

        public async Task<User> CreateUserAsync(CreateAccountDto input)
        {
            try
            {
                bool isDefaultEmail = false;
                if (string.IsNullOrEmpty(input.EmailAddress))
                {
                    isDefaultEmail = true;
                    input.EmailAddress = input.PhoneNumber + "@default.com";
                }

                var checkPhone = await _userManager.ValidateEmailPhone(input.PhoneNumber, input.EmailAddress, null);
                if (checkPhone.ResponseCode != "01")
                    throw new UserFriendlyException("Số điên thoại/Email không hợp lệ hoặc đã tồn tại");
                if (!string.IsNullOrEmpty(input.AccountCode))
                {
                    if (!ValidationHelper.IsAccountCode(input.AccountCode))
                        throw new UserFriendlyException("Mã tài khoản không hợp lệ");
                }

                User parentUser = null;
                if (!string.IsNullOrEmpty(input.ParentAccount) &&
                    (input.AccountType == CommonConst.SystemAccountType.Agent ||
                     input.AccountType == CommonConst.SystemAccountType.StaffApi))
                {
                    parentUser = await _userManager.GetUserByAccountCodeAsync(input.ParentAccount);
                }
                else if (input.AccountType == CommonConst.SystemAccountType.MasterAgent)
                {
                    parentUser = await _userManager.GetUserByAccountTypeAsync(CommonConst.SystemAccountType.Company);
                }
                else if (input.AccountType == CommonConst.SystemAccountType.Company)
                {
                    var checkUser = await _userManager.GetUserByAccountTypeAsync(CommonConst.SystemAccountType.Company);
                    if (checkUser != null)
                        throw new UserFriendlyException("Tài khoản công ty đã tồn tại.");
                }

                if (input.AccountType != CommonConst.SystemAccountType.System &&
                    input.AccountType != CommonConst.SystemAccountType.Staff &&
                    input.AccountType != CommonConst.SystemAccountType.Company)
                {
                    if (parentUser == null)
                        throw new UserFriendlyException("Chưa nhập cấp trên");
                }

                var user = new User
                {
                    TenantId = input.TenantId,
                    Name = input.Name,
                    Surname = input.Surname,
                    EmailAddress = input.EmailAddress,
                    IsActive = true,
                    UserName = !string.IsNullOrEmpty(input.UserName) ? input.UserName : input.PhoneNumber,
                    PhoneNumber = !string.IsNullOrEmpty(input.PhoneNumber) ? input.PhoneNumber : input.UserName,
                    IsEmailConfirmed = input.IsEmailConfirmed,
                    Roles = new List<UserRole>(),
                    IsDefaultEmail = isDefaultEmail,
                    AccountType = input.AccountType,
                    AgentType = input.AgentType,
                    IsVerifyAccount = input.IsVerifyAccount,
                    AgentName = input.AgentName
                };
                if (string.IsNullOrEmpty(input.Password))
                {
                    var randomPassword = await _userManager.CreateRandomPassword();
                    input.Password = randomPassword;
                }
                else
                {
                    await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                    foreach (var validator in _passwordValidators)
                    {
                        CheckErrors(await validator.ValidateAsync(_userManager, user, input.Password));
                    }

                    user.Password = _passwordHasher.HashPassword(user, input.Password);
                }

                user.SetNormalizedNames();

                var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
                foreach (var defaultRole in defaultRoles)
                {
                    user.Roles.Add(new UserRole(input.TenantId, user.Id, defaultRole.Id));
                }

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await _userManager.CreateAsync(user, input.Password));
                if ((user.AccountType == CommonConst.SystemAccountType.Staff ||
                     user.AccountType == CommonConst.SystemAccountType.StaffApi) &&
                    !string.IsNullOrEmpty(input.AccountCodeRef))
                {
                    user.AccountCode = input.AccountCodeRef;
                }
                else if (!string.IsNullOrEmpty(input.AccountCode))
                {
                    user.AccountCode = input.AccountCode;
                }
                else
                {
                    user.AccountCode = user.GenAccountCode();
                }

                user.TreePath = user.AccountCode;
                if (user.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    user.ParentCode = input.ParentAccount;
                    user.ParentId = parentUser?.Id;
                }
                else if (parentUser != null)
                {
                    user.TreePath = parentUser.TreePath + "-" + user.AccountCode;
                    user.ParentId = parentUser.Id;
                    user.ParentCode = parentUser.AccountCode;
                    user.NetworkLevel = parentUser.NetworkLevel + 1;
                }

                if (user.AccountType == CommonConst.SystemAccountType.Agent
                    || user.AccountType == CommonConst.SystemAccountType.Company
                    || user.AccountType == CommonConst.SystemAccountType.MasterAgent)
                {
                    await _organizationsUnitCustomManager.CreateOrganizationUnit(user.UserName + " - " + user.FullName,
                        user.ParentId, user.Id, user.TenantId);
                }

                if ((byte) user.AgentType > 0 && user.AgentType != CommonConst.AgentType.Agent)
                {
                    await _userManager.AddToRoleAsync(user, user.AgentType.ToString("G"));
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, user.AccountType.ToString("G"));
                }

                if (user.Id > 0)
                {
                    var request = input.ConvertTo<UserProfileDto>();
                    request.UserId = user.Id;
                    await _userManager.CreateOrUpdateUserProfile(request);
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                if (!user.IsEmailConfirmed)
                {
                    user.SetNewEmailConfirmationCode();
                    await _userEmail.SendEmailActivationLinkAsync(user, input.EmailActivationLink);
                }

                //Notifications
                //if (input.TenantId != null)
                    //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(
                        //user.ToUserIdentifier());
                //await _appNotifier.WelcomeToTheApplicationAsync(user);
                //await _appNotifier.NewUserRegisteredAsync(user);

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException(e.Message);
            }
        }


        public async Task<User> UpdateUserAsync(CreateAccountDto input)
        {
            try
            {
                _logger.LogInformation($"UpdateUserAsync:{input.ToJson()}");
                var user = await _userManager.GetUserByIdAsync(input.Id ?? 0);
                if (user == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");
                if (!string.IsNullOrEmpty(input.EmailAddress) && input.EmailAddress != user.EmailAddress)
                    user.EmailAddress = input.EmailAddress;
                if (!string.IsNullOrEmpty(input.Name) && input.Name != user.Name)
                    user.Name = input.Name;
                if (!string.IsNullOrEmpty(input.Surname) && input.Surname != user.Surname)
                    user.Surname = input.Surname;
                if (!string.IsNullOrEmpty(input.AgentName) && input.AgentName != user.AgentName)
                    user.AgentName = input.AgentName;
                if (!string.IsNullOrEmpty(input.ParentAccount) && input.ParentAccount != user.ParentCode)
                {
                    User parentUser = null;
                    if (!string.IsNullOrEmpty(input.ParentAccount) &&
                        input.AccountType == CommonConst.SystemAccountType.Agent)
                    {
                        parentUser = await _userManager.GetUserByAccountCodeAsync(input.ParentAccount);
                    }
                    else if (input.AccountType == CommonConst.SystemAccountType.MasterAgent)
                    {
                        parentUser = await _userManager.GetUserByAccountTypeAsync(CommonConst.SystemAccountType.Company);
                    }

                    user.TreePath = parentUser.TreePath + "-" + user.AccountCode;
                    user.ParentId = parentUser.Id;
                    user.ParentCode = parentUser.AccountCode;
                    user.NetworkLevel = parentUser.NetworkLevel + 1;
                }

                if (input.IsActive != user.IsActive)
                    user.IsActive = input.IsActive;
                user.Gender = input.Gender;
                user.DoB = input.DoB;

                var profile = await _userManager.GetUserProfile(user.Id) ?? new UserProfileDto();

                if (!string.IsNullOrEmpty(input.Address))
                    profile.Address = input.Address;

                if (input.CityId != null && input.CityId > 0)
                    profile.CityId = input.CityId;
                if (input.DistrictId != null && input.DistrictId > 0)
                    profile.DistrictId = input.DistrictId;
                if (input.WardId != null && input.WardId > 0)
                    profile.WardId = input.WardId;
                if (input.Description != null)
                    profile.Desscription = input.Description;
                if (input.FrontPhoto != null)
                    profile.FrontPhoto = input.FrontPhoto;
                if (input.BackSitePhoto != null)
                    profile.FrontPhoto = input.BackSitePhoto;
                if (input.IdIdentity != null)
                    profile.IdIdentity = input.IdIdentity;
                if (input.IdentityIdExpireDate != null)
                    profile.IdentityIdExpireDate = input.IdentityIdExpireDate;
                if (input.SigDate != null)
                    profile.SigDate = input.SigDate;
                if (input.ContractNumber != null)
                    profile.ContractNumber = input.ContractNumber;
                if (input.ContractRegister != null)
                    profile.ContractRegister = input.ContractRegister;
                if (input.MethodReceivePassFile != null)
                    profile.MethodReceivePassFile = input.MethodReceivePassFile;
                if (input.ValueReceivePassFile != null)
                    profile.ValueReceivePassFile = input.ValueReceivePassFile;

                profile.IdType = input.IdType ?? 0;
                profile.UserId = user.Id;

                await _userManager.CreateOrUpdateUserProfile(profile);
                CheckErrors(await _userManager.UpdateAsync(user));
                await CurrentUnitOfWork.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"UpdateUserAsync:{e}");
                throw new UserFriendlyException("Cập nhật tài khoản không thành công.");
            }
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
