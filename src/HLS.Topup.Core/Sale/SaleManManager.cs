using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.UI;
using HLS.Topup.Address;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Sale;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Sale
{
    public class SaleManManager : TopupDomainServiceBase, ISaleManManager
    {
        private readonly ILogger<SaleManManager> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<SaleManLocation> _saleManLocationRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<UserProfile> _userProfile;
        private readonly IRepository<User, long> _userRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<SaleAssignAgent> _assignRepository;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly UserManager _userManager;
        private readonly IAddressManager _addressManager;

        public SaleManManager(ILogger<SaleManManager> logger,
            IRepository<SaleManLocation> saleManLocationRepository,
            UserManager userManager, IAddressManager addressManager,
            IEnumerable<IPasswordValidator<User>> passwordValidators, IPasswordHasher<User> passwordHasher,
            RoleManager roleManager, IRepository<UserProfile> userProfile, IRepository<District> districtRepository,
            IRepository<Ward> wardRepository, IRepository<City> cityRepository,
            IRepository<User, long> userRepository,
            IRepository<SaleAssignAgent> assignRepository)
        {
            _logger = logger;
            _saleManLocationRepository = saleManLocationRepository;
            _userManager = userManager;
            _addressManager = addressManager;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _userProfile = userProfile;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _cityRepository = cityRepository;
            _userRepository = userRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _assignRepository = assignRepository;
        }

        public async Task CreateSale(CreateOrUpdateSaleDto input)
        {
            try
            {
                var checkPhone =
                    await _userManager.ValidateEmailPhone(input.PhoneNumber, input.EmailAddress, null);
                if (checkPhone.ResponseCode != ResponseCodeConst.ResponseCode_Success)
                    throw new UserFriendlyException(checkPhone.ResponseMessage);
                var checkUsername = await _userManager.ValidateAccountRegister(input.UserName);
                if (checkUsername.ResponseCode != ResponseCodeConst.ResponseCode_Success && checkUsername.ResponseCode != "0")
                    throw new UserFriendlyException(checkUsername.ResponseMessage);
                if (input?.TenantId == 0)
                    input.TenantId = null;
                if (input?.SaleLeadUserId == 0)
                    input.SaleLeadUserId = null;
                var user = new User
                {
                    Name = input.Name,
                    Surname = input.SurName,
                    AccountType = input.SaleType,
                    IsActive = input.IsActive,
                    IsEmailConfirmed = true,
                    EmailAddress = input.UserName + "@gmail.com",
                    PhoneNumber = input.PhoneNumber,
                    IsDefaultEmail = true,
                    UserName = input.UserName,
                    UserSaleLeadId = input.SaleLeadUserId,
                    Description = input.Description
                };
                user.SetNormalizedNames();
                //Passwords is not mapped (see mapping configuration)
                user.TenantId = input.TenantId;
                //Set password
                await _userManager.InitializeOptionsAsync(input.TenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(_userManager, user, input.Password));
                }

                user.Password = _passwordHasher.HashPassword(user, input.Password);

                CheckErrors(await _userManager.CreateAsync(user));
                user.AccountCode = user.GenAccountCode();
                await _userManager.AddToRoleAsync(user, "USER");
                await _userManager.AddToRoleAsync(user, user.AccountType.ToString("G"));

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateSale error:{e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task UpdateSale(CreateOrUpdateSaleDto input)
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(input.Id ?? 0);
                if (user == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");
                var checkPhone =
                    await _userManager.ValidateEmailPhone(input.PhoneNumber, input.EmailAddress, user.Id);
                if (checkPhone.ResponseCode != ResponseCodeConst.ResponseCode_Success)
                    throw new UserFriendlyException("Số điên thoại/Email không hợp lệ hoặc đã tồn tại");

                if (!string.IsNullOrEmpty(input.PhoneNumber) && input.PhoneNumber != user.PhoneNumber)
                    user.PhoneNumber = input.PhoneNumber;
                if (!string.IsNullOrEmpty(input.EmailAddress) && input.EmailAddress != user.EmailAddress)
                    user.EmailAddress = input.EmailAddress;
                if (!string.IsNullOrEmpty(input.Name) && input.Name != user.Name)
                    user.Name = input.Name;
                if (!string.IsNullOrEmpty(input.SurName) && input.SurName != user.Surname)
                    user.Surname = input.SurName;
                if (!string.IsNullOrEmpty(input.Password))
                    user.Password = _passwordHasher.HashPassword(user, input.Password);

                if (input.IsActive != user.IsActive)
                    user.IsActive = input.IsActive;
                user.Description = input.Description;
                user.UserSaleLeadId = input.SaleLeadUserId;
                CheckErrors(await _userManager.UpdateAsync(user));

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateSale error:{e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// Lấy ra danh sách SaleLearder
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<UserInfoDto>> GetUserSaleLeader(UserInfoSearch search)
        {
            try
            {
                IQueryable<UserInfoDto> query = null;
                if (search.AccountType == CommonConst.SystemAccountType.System)
                {
                    #region 1.Admin

                    query = from u in _userRepository.GetAll()
                            .Where(x => (x.AccountType == CommonConst.SystemAccountType.SaleLead)
                                        && (x.AccountCode.Contains(search.Search)
                                            || x.PhoneNumber.Contains(search.Search)
                                            || x.UserName.Contains(search.Search)))
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };

                    #endregion
                }
                else if (search.AccountType == CommonConst.SystemAccountType.SaleLead)
                {
                    #region 2.SaleLead

                    query = from u in _userRepository.GetAll()
                            .Where(x => (x.AccountCode == search.LoginCode
                                         && x.AccountType == CommonConst.SystemAccountType.SaleLead)
                                        && (x.AccountCode.Contains(search.Search)
                                            || x.PhoneNumber.Contains(search.Search)
                                            || x.UserName.Contains(search.Search)))
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };

                    #endregion
                }
                else if (search.AccountType == CommonConst.SystemAccountType.Sale)
                {
                    #region 3.Sale

                    query = from u in _userRepository.GetAll()
                            .Where(x => x.AccountType == CommonConst.SystemAccountType.SaleLead
                                        && (x.AccountCode.Contains(search.Search)
                                            || x.PhoneNumber.Contains(search.Search)
                                            || x.UserName.Contains(search.Search)))
                        join s in _userRepository.GetAll() on u.Id equals s.UserSaleLeadId
                        where s.AccountCode == search.LoginCode
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };

                    #endregion
                }

                return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy ra danh sách Sale
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<UserInfoDto>> GetUserSaleBySaleLeader(UserInfoSearch search)
        {
            try
            {
                IQueryable<UserInfoDto> query = null;
                if (search.AccountType == CommonConst.SystemAccountType.System)
                {
                    if (search.SaleLeadId > 0)
                    {
                        query = from u in _userRepository.GetAll().Where(x =>
                                x.UserSaleLeadId == search.SaleLeadId
                                && (x.AccountType == CommonConst.SystemAccountType.Sale)
                                && (x.AccountCode.Contains(search.Search)
                                    || x.PhoneNumber.Contains(search.Search)
                                    || x.UserName.Contains(search.Search)))
                            select new UserInfoDto
                            {
                                UserName = u.UserName,
                                Name = u.Name,
                                Surname = u.Surname,
                                PhoneNumber = u.PhoneNumber,
                                AccountCode = u.AccountCode,
                                AgentName = u.AgentName,
                                Id = u.Id,
                                AccountType = u.AccountType,
                                FullName = u.FullName,
                            };
                    }

                    if (!string.IsNullOrEmpty(search.SaleLeadCode))
                    {
                        query = from u in _userRepository.GetAll().Where(x =>
                                (x.AccountType == CommonConst.SystemAccountType.Sale)
                                && (x.AccountCode.Contains(search.Search)
                                    || x.PhoneNumber.Contains(search.Search)
                                    || x.UserName.Contains(search.Search)))
                            join l in _userRepository.GetAll().Where(c => c.AccountCode == search.SaleLeadCode)
                                on u.UserSaleLeadId equals l.Id
                            select new UserInfoDto
                            {
                                UserName = u.UserName,
                                Name = u.Name,
                                Surname = u.Surname,
                                PhoneNumber = u.PhoneNumber,
                                AccountCode = u.AccountCode,
                                AgentName = u.AgentName,
                                Id = u.Id,
                                AccountType = u.AccountType,
                                FullName = u.FullName,
                            };
                    }
                    else
                    {
                        query = from u in _userRepository.GetAll().Where(x =>
                                (x.AccountType == CommonConst.SystemAccountType.Sale)
                                && (x.AccountCode.Contains(search.Search)
                                    || x.PhoneNumber.Contains(search.Search)
                                    || x.UserName.Contains(search.Search)))
                            select new UserInfoDto
                            {
                                UserName = u.UserName,
                                Name = u.Name,
                                Surname = u.Surname,
                                PhoneNumber = u.PhoneNumber,
                                AccountCode = u.AccountCode,
                                AgentName = u.AgentName,
                                Id = u.Id,
                                AccountType = u.AccountType,
                                FullName = u.FullName,
                            };
                    }
                }
                else if (search.AccountType == CommonConst.SystemAccountType.SaleLead)
                {
                    query = from u in _userRepository.GetAll().Where(x =>
                            x.UserSaleLeadId == search.SaleLeadId
                            && (x.AccountType == CommonConst.SystemAccountType.Sale)
                            && (x.AccountCode.Contains(search.Search)
                                || x.PhoneNumber.Contains(search.Search)
                                || x.UserName.Contains(search.Search)))
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                }
                else if (search.AccountType == CommonConst.SystemAccountType.Sale)
                {
                    query = from u in _userRepository.GetAll().Where(x =>
                            x.Id == search.SaleId
                            && (x.AccountType == CommonConst.SystemAccountType.Sale)
                            && (x.AccountCode.Contains(search.Search)
                                || x.PhoneNumber.Contains(search.Search)
                                || x.UserName.Contains(search.Search)))
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                }

                return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<UserInfoDto>> GetUserAgentBy(UserInfoSearch search)
        {
            try
            {
                IQueryable<UserInfoDto> query = null;
                if (search.SaleId > 0)
                {
                    var querySeach = from u in _userRepository.GetAll().Where(x =>
                            (x.AccountType == CommonConst.SystemAccountType.Agent
                             || x.AccountType == CommonConst.SystemAccountType.MasterAgent
                             || x.AccountType == CommonConst.SystemAccountType.Company)
                            && (x.AccountCode.Contains(search.Search)
                                || x.PhoneNumber.Contains(search.Search)
                                || x.Surname.ToUpper().Contains(search.Search.ToUpper())
                                || x.Name.ToUpper().Contains(search.Search.ToUpper())
                                || x.UserName.ToUpper().Contains(search.Search.ToUpper())))
                        join f in _assignRepository.GetAll().Where(c => c.SaleUserId == search.SaleId)
                            on u.Id equals f.UserAgentId
                        select u;

                    if (search.AgentType == CommonConst.AgentType.Agent
                        || search.AgentType == CommonConst.AgentType.AgentApi
                        || search.AgentType == CommonConst.AgentType.AgentGeneral
                        || search.AgentType == CommonConst.AgentType.AgentCampany
                       )
                    {
                        querySeach = querySeach.Where(c => c.AgentType == search.AgentType);
                    }

                    query = from u in querySeach
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                }
                else if (search.SaleLeadId > 0)
                {
                    var querySeach = from u in _userRepository.GetAll().Where(x =>
                            (x.AccountType == CommonConst.SystemAccountType.Agent
                             || x.AccountType == CommonConst.SystemAccountType.MasterAgent
                             || x.AccountType == CommonConst.SystemAccountType.Company)
                            && (x.AccountCode.Contains(search.Search)
                                || x.PhoneNumber.Contains(search.Search)
                                || (x.Surname ?? "").Contains(search.Search)
                                || (x.Name ?? "").Contains(search.Search)
                                || x.UserName.Contains(search.Search)))
                        join f in _assignRepository.GetAll() on u.Id equals f.UserAgentId
                        join s in _userRepository.GetAll() on f.SaleUserId equals s.Id
                        where s.UserSaleLeadId == search.SaleLeadId
                        select u;


                    if (search.AgentType == CommonConst.AgentType.Agent
                        || search.AgentType == CommonConst.AgentType.AgentApi
                        || search.AgentType == CommonConst.AgentType.AgentGeneral
                        || search.AgentType == CommonConst.AgentType.AgentCampany)
                    {
                        querySeach = querySeach.Where(c => c.AgentType == search.AgentType);
                    }

                    query = from u in querySeach
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                }
                else
                {
                    var querySeach = from u in _userRepository.GetAll().Where(x =>
                            (x.AccountType == CommonConst.SystemAccountType.Agent
                             || x.AccountType == CommonConst.SystemAccountType.MasterAgent
                             || x.AccountType == CommonConst.SystemAccountType.Company)
                            && (x.AccountCode.Contains(search.Search)
                                || x.PhoneNumber.Contains(search.Search)
                                || (x.Surname ?? "").Contains(search.Search)
                                || (x.Name ?? "").Contains(search.Search)
                                || x.UserName.Contains(search.Search)))
                        select u;


                    if (search.AgentType == CommonConst.AgentType.Agent
                        || search.AgentType == CommonConst.AgentType.AgentApi
                        || search.AgentType == CommonConst.AgentType.AgentGeneral
                        || search.AgentType == CommonConst.AgentType.AgentCampany)
                    {
                        querySeach = querySeach.Where(c => c.AgentType == search.AgentType);
                    }

                    query = from u in querySeach
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                }

                return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserInfoDto> GetUserSaleLeaderBySale(int userId)
        {
            try
            {
                if (userId != null)
                {
                    var usersale = _userRepository.FirstOrDefault(c => c.Id == userId);
                    var userLeader = _userRepository.FirstOrDefault(c => c.Id == (usersale.UserSaleLeadId ?? 0));
                    var query = new UserInfoDto()
                    {
                        UserName = userLeader.UserName,
                        Name = userLeader.Name,
                        Surname = userLeader.Surname,
                        PhoneNumber = userLeader.PhoneNumber,
                        AccountCode = userLeader.AccountCode,
                        AgentName = userLeader.AgentName,
                        Id = userLeader.Id,
                        AccountType = userLeader.AccountType,
                        FullName = userLeader.FullName,
                    };

                    return query;
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Danh sách nhân viên kinh doanh và đại lý
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<UserInfoDto>> GetUserAgentStaff(UserInfoSearch search)
        {
            try
            {
                IQueryable<UserInfoDto> query = null;
                if (search.AccountType == CommonConst.SystemAccountType.System)
                {
                    query = from u in _userRepository.GetAll().Where(x => x.IsVerifyAccount
                                                                          && (x.AccountType ==
                                                                              CommonConst.SystemAccountType.Agent
                                                                              || x.AccountType ==
                                                                              CommonConst.SystemAccountType.MasterAgent
                                                                              || x.AccountType ==
                                                                              CommonConst.SystemAccountType.Staff
                                                                              || x.AccountType ==
                                                                              CommonConst.SystemAccountType.StaffApi
                                                                          )
                                                                          && (x.AccountCode.Contains(search.Search) ||
                                                                              x.PhoneNumber.Contains(search.Search) ||
                                                                              x.UserName.Contains(search.Search)))
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                    return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
                }
                else if (search.AccountType == CommonConst.SystemAccountType.SaleLead)
                {
                    query = from u in _userRepository.GetAll().Where(x =>
                            (x.AccountType == CommonConst.SystemAccountType.Agent
                             || x.AccountType == CommonConst.SystemAccountType.MasterAgent
                             || x.AccountType == CommonConst.SystemAccountType.Staff
                             || x.AccountType == CommonConst.SystemAccountType.StaffApi
                            )
                            && (x.AccountCode.Contains(search.Search) ||
                                x.PhoneNumber.Contains(search.Search) ||
                                x.UserName.Contains(search.Search)))
                        join s in _assignRepository.GetAll() on u.Id equals s.UserAgentId
                        join us in _userRepository.GetAll() on s.SaleUserId equals us.Id
                        where us.UserSaleLeadId == search.SaleLeadId
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };
                    return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
                }
                else if (search.AccountType == CommonConst.SystemAccountType.Sale)
                {
                    query = from u in _userRepository.GetAll().Where(x =>
                            (x.AccountType == CommonConst.SystemAccountType.Agent
                             || x.AccountType == CommonConst.SystemAccountType.MasterAgent
                             || x.AccountType == CommonConst.SystemAccountType.Staff
                             || x.AccountType == CommonConst.SystemAccountType.StaffApi
                            )
                            && (x.AccountCode.Contains(search.Search) ||
                                x.PhoneNumber.Contains(search.Search) ||
                                x.UserName.Contains(search.Search)))
                        join s in _assignRepository.GetAll() on u.Id equals s.UserAgentId
                        where s.SaleUserId == search.SaleId
                        select new UserInfoDto
                        {
                            UserName = u.UserName,
                            Name = u.Name,
                            Surname = u.Surname,
                            PhoneNumber = u.PhoneNumber,
                            AccountCode = u.AccountCode,
                            AgentName = u.AgentName,
                            Id = u.Id,
                            AccountType = u.AccountType,
                            FullName = u.FullName,
                        };

                    return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
                }

                return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<List<UserInfoDto>> GetUserAgentLevel(UserInfoSearch search)
        {
            try
            {
                IQueryable<UserInfoDto> query = null;
                var single = _userRepository.Single(c => c.AccountCode == search.SaleLeadCode);
                query = from u in _userRepository.GetAll().Where(x => x.IsVerifyAccount
                                                                      && (x.AccountType == CommonConst.SystemAccountType
                                                                              .Agent
                                                                          || x.AccountType ==
                                                                          CommonConst.SystemAccountType.MasterAgent
                                                                          || x.AccountType ==
                                                                          CommonConst.SystemAccountType.Staff
                                                                          || x.AccountType ==
                                                                          CommonConst.SystemAccountType.StaffApi
                                                                      )
                                                                      && (x.AccountCode.Contains(search.Search) ||
                                                                          x.PhoneNumber.Contains(search.Search) ||
                                                                          x.UserName.Contains(search.Search))
                                                                      && x.ParentId == single.Id)
                    select new UserInfoDto
                    {
                        UserName = u.UserName,
                        Name = u.Name,
                        Surname = u.Surname,
                        PhoneNumber = u.PhoneNumber,
                        AccountCode = u.AccountCode,
                        AgentName = u.AgentName,
                        Id = u.Id,
                        AccountType = u.AccountType,
                        FullName = u.FullName,
                    };

                return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CreateOrUpdateSaleDto> GetSaleInfo(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
                return new CreateOrUpdateSaleDto();
            var item = user.ConvertTo<CreateOrUpdateSaleDto>();
            item.SaleLeadUserId = user.UserSaleLeadId;
            item.SaleType = user.AccountType;
            item.SurName = user.Surname;

            return item;
        }

        public async Task<UserInfoSaleDto> GetSaleAssignInfo(int userId)
        {
            var li = await (from assign in _assignRepository.GetAll()
                join s in _userRepository.GetAll() on assign.SaleUserId equals s.Id
                join sl in _userRepository.GetAll() on s.UserSaleLeadId equals sl.Id
                where assign.UserAgentId == userId
                select new UserInfoSaleDto()
                {
                    SaleCode = s.AccountCode,
                    SaleLeaderCode = sl.AccountCode,
                    UserSaleId = s.Id,
                    UserLeaderId = sl.Id,
                }).FirstOrDefaultAsync();
            return li;
        }

        public async Task<UserInfoDto> GetSaleAssignDetail(int userId)
        {
            var li = await (from assign in _assignRepository.GetAll()
                join s in _userRepository.GetAll() on assign.SaleUserId equals s.Id
                join sl in _userRepository.GetAll() on s.UserSaleLeadId equals sl.Id
                where assign.UserAgentId == userId
                select new UserInfoDto()
                {
                    UserName = s.UserName,
                    AccountCode = s.AccountCode,
                    FullName = s.UserName,
                    PhoneNumber = s.PhoneNumber
                }).FirstOrDefaultAsync();

            return li;
        }

        public async Task<List<UserInfoDto>> GetAgentLocationSale(long saleId, string search)
        {
            try
            {
                //var locationSale = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleId)
                //    .Select(x => x.WardId);
                //var query = from u in _userRepository.GetAll().Where(x => x.IsVerifyAccount
                //                                                          && (x.AccountType ==
                //                                                              CommonConst.SystemAccountType.Agent ||
                //                                                              x.AccountType ==
                //                                                              CommonConst.SystemAccountType.MasterAgent)
                //                                                          && x.AccountCode.Contains(search) ||
                //                                                          x.PhoneNumber.Contains(search) ||
                //                                                          x.UserName.Contains(search))
                //            join a in _userProfile.GetAll()
                //                .Where(x => x.WardId != null && locationSale.Contains(x.WardId.Value)) on u.Id equals a
                //                .UserId
                //            select new UserInfoDto
                //            {
                //                UserName = u.UserName,
                //                Name = u.Name,
                //                Surname = u.Surname,
                //                PhoneNumber = u.PhoneNumber,
                //                AccountCode = u.AccountCode,
                //                AgentName = u.AgentName,
                //                Id = u.Id,
                //                AccountType = u.AccountType,
                //                FullName = u.FullName,
                //            };
                //return await query.OrderBy(x => x.AccountCode).Take(100).ToListAsync();

                var locationSale = from x in _userRepository.GetAll()
                    join y in _assignRepository.GetAll() on x.Id equals y.UserAgentId
                    where y.SaleUserId == saleId
                          // && x.IsVerifyAccount
                          && (x.AccountType == CommonConst.SystemAccountType.Agent ||
                              x.AccountType == CommonConst.SystemAccountType.MasterAgent)
                          && (x.AccountCode.Contains(search) ||
                              x.PhoneNumber.Contains(search) ||
                              x.UserName.Contains(search))
                    select new UserInfoDto
                    {
                        UserName = x.UserName,
                        Name = x.Name,
                        Surname = x.Surname,
                        PhoneNumber = x.PhoneNumber,
                        AccountCode = x.AccountCode,
                        AgentName = x.AgentName,
                        Id = x.Id,
                        AccountType = x.AccountType,
                        FullName = x.FullName,
                    };

                return await locationSale.OrderBy(x => x.AccountCode).Take(100).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<List<AddressSaleItemDto>> GetListCitySale(long? userId = null, long? saleLeadId = null)
        {
            List<AddressSaleItemDto> list;
            var listCity = await _addressManager.GetListCitiesCacheAsync();

            if (saleLeadId == null)
            {
                list = listCity.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    CityId = x.Id,
                    CityName = x.CityName,
                    DisplayName = x.CityName
                }).ToList();
            }
            else
            {
                var citySaleLead = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleLeadId)
                    .Select(x => x.CityId);
                if (listCity.Any())
                    listCity = listCity.Where(x => citySaleLead.Contains(x.Id))
                        .ToList(); //Chỉ lấy những city thuộc saleLead

                list = listCity.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    CityId = x.Id,
                    CityName = x.CityName,
                    DisplayName = x.CityName
                }).ToList();
            }

            var selected = _saleManLocationRepository.GetAll().Select(x => new SelectedAddress
            {
                CityId = x.CityId ?? 0,
                UserId = x.UserId
            });
            foreach (var item in list.Where(item => selected.Count(x => x.CityId == item.Id) > 0))
            {
                item.Selected = true;
                if (userId != null)
                {
                    var dis = selected.Where(x => x.UserId != userId);
                    if (dis.Count(x => x.CityId == item.Id) > 0)
                        item.Disabled = true;
                }
                else
                {
                    item.Disabled = true;
                }
            }

            return list;
        }

        public async Task<List<AddressSaleItemDto>> GetListDistrictSale(int? cityId = null, long? userId = null,
            long? saleLeadId = null)
        {
            List<AddressSaleItemDto> list;
            var listDistricts = new List<AddressSaleItemDto>();
            if (cityId != null && userId == null)
            {
                var cities = _districtRepository.GetAllIncluding(x => x.CityFk).Where(x => x.CityId == cityId);
                listDistricts = cities.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.Id,
                    DisplayName = x.DistrictName,
                    DistrictName = x.DistrictName,
                    CityId = x.CityId,
                    CityName = x.CityFk.CityName
                }).ToList();
            }
            else if (userId != null)
            {
                listDistricts = (from d in _districtRepository.GetAllIncluding(x => x.CityFk)
                    join a in _saleManLocationRepository.GetAll().Where(x => x.UserId == userId) on d.Id equals a
                        .DistrictId
                    select d).Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.Id,
                    DistrictName = x.DistrictName,
                    DisplayName = x.DistrictName,
                    CityId = x.CityId,
                    CityName = x.CityFk.CityName
                }).ToList();
            }

            if (!listDistricts.Any())
                return null;
            if (saleLeadId == null && userId != null)
            {
                list = listDistricts.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DisplayName = x.DistrictName,
                    DistrictId = x.Id,
                    DistrictName = x.DistrictName,
                    CityId = x.CityId,
                    CityName = x.CityName
                }).ToList();
            }
            else
            {
                var districtLead = _saleManLocationRepository.GetAllIncluding(x => x.CityFk)
                    .Where(x => x.UserId == saleLeadId)
                    .Select(x => x.DistrictId);
                if (districtLead.Any())
                    listDistricts = listDistricts.Where(x => districtLead.Contains(x.Id))
                        .ToList(); //Chỉ lấy những District thuộc saleLead

                list = listDistricts.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DisplayName = x.DistrictName,
                    DistrictId = x.Id,
                    DistrictName = x.DistrictName,
                    CityId = x.CityId,
                    CityName = x.CityName
                }).ToList();
            }

            var selected = _saleManLocationRepository.GetAll().Select(x => new SelectedAddress
            {
                DistrictId = x.DistrictId ?? 0,
                UserId = x.UserId
            });
            foreach (var item in list.Where(item => selected.Count(x => x.DistrictId == item.Id) > 0))
            {
                item.Selected = true;
                if (userId != null)
                {
                    var dis = selected.Where(x => x.UserId != userId);
                    if (dis.Count(x => x.DistrictId == item.Id) > 0)
                        item.Disabled = true;
                }
                else
                {
                    item.Disabled = true;
                }
            }

            return list;
        }

        public async Task<List<AddressSaleItemDto>> GetListWardSale(int? districtId = null, long? userId = null,
            long? saleLeadId = null)
        {
            List<AddressSaleItemDto> list;
            var listWards = new List<AddressSaleItemDto>();
            if (districtId != null && userId == null)
            {
                var wards = _wardRepository.GetAllIncluding(x => x.DistrictFk).Where(x => x.DistrictId == districtId);
                listWards = wards.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.DistrictId,
                    DisplayName = x.DistrictFk.DistrictName,
                    DistrictName = x.WardName
                }).ToList();
            }
            else if (userId != null)
            {
                listWards = (from d in _wardRepository.GetAllIncluding(x => x.DistrictFk)
                    join a in _saleManLocationRepository.GetAll().Where(x => x.UserId == userId) on d.Id equals a.WardId
                    select d).Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.DistrictId,
                    DisplayName = x.DistrictFk.DistrictName,
                    DistrictName = x.WardName
                }).ToList();
            }

            if (saleLeadId == null)
            {
                list = listWards.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.DistrictId,
                    DisplayName = x.DistrictName,
                    DistrictName = x.DistrictName
                }).ToList();
            }
            else
            {
                var wardLead = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleLeadId)
                    .Select(x => x.WardId);
                if (wardLead.Any())
                    listWards = listWards.Where(x => wardLead.Contains(x.Id))
                        .ToList(); //Chỉ lấy những Ward thuộc saleLead

                list = listWards.Select(x => new AddressSaleItemDto
                {
                    Id = x.Id,
                    DistrictId = x.DistrictId,
                    DisplayName = x.DistrictName,
                    DistrictName = x.DistrictName
                }).ToList();
            }

            var selected = _saleManLocationRepository.GetAll().Select(x => new SelectedAddress
            {
                WardId = x.WardId,
                UserId = x.UserId
            });
            foreach (var item in list.Where(item => selected.Count(x => x.WardId == item.Id) > 0))
            {
                item.Selected = true;
                if (userId != null)
                {
                    var dis = selected.Where(x => x.UserId != userId);
                    if (dis.Count(x => x.WardId == item.Id) > 0)
                        item.Disabled = true;
                }
                else
                {
                    item.Disabled = true;
                }
            }

            return list;
        }

        public async Task<AddressSaleSelected> GetAddressSelected(long userId)
        {
            return new AddressSaleSelected
            {
                Cities = await GetListCitySale(userId),
                Districts = await GetListDistrictSale(userId: userId),
                Wards = await GetListWardSale(userId: userId),
            };
        }

        public async Task<AddressSaleSelected> GetAddressSelected(long? userId = null, long? saleLeadId = null,
            int? cityId = null, int? districtId = null, int? wardId = null)
        {
            var citySelected = await _saleManLocationRepository.GetAll().Select(x =>
                new SelectedAddress
                {
                    CityId = x.CityId ?? 0,
                    UserId = x.UserId
                }).Distinct().ToListAsync();
            var cities = await _cityRepository.GetAll().ToListAsync();
            if (saleLeadId != null)
            {
                var citySaleLead = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleLeadId)
                    .Select(x => x.CityId).Distinct();
                cities = cities.Where(x => citySaleLead.Contains(x.Id))
                    .ToList();
            }

            var lstCity = new List<AddressSaleItemDto>();
            foreach (var item in cities)
            {
                var add = new AddressSaleItemDto
                {
                    DisplayName = item.CityName,
                    Id = item.Id,
                    CityId = item.Id,
                    CityName = item.CityName
                };
                if (citySelected.Count(x => x.CityId == item.Id) > 0)
                {
                    add.Selected = true;
                    if (userId != null)
                    {
                        var dis = citySelected.Where(x => x.UserId != userId);
                        if (dis.Count(x => x.CityId == item.Id) > 0)
                            add.Disabled = true;
                    }
                }

                lstCity.Add(add);
            }

            //district
            var districtSelected = _saleManLocationRepository.GetAll()
                .WhereIf(userId != null, x => x.UserId == userId).Select(x =>
                    new SelectedAddress
                    {
                        DistrictId = x.DistrictId ?? 0,
                        UserId = x.UserId
                    }).Distinct().ToList();
            var districts = _districtRepository.GetAllIncluding(x => x.CityFk)
                .WhereIf(cityId != null, x => x.CityId == cityId)
                .Where(x => citySelected.Select(x => x.CityId).Contains(x.CityId)).ToList();
            if (saleLeadId != null)
            {
                var districtSaleLead = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleLeadId)
                    .Select(x => x.DistrictId).Distinct();
                districts = districts.Where(x => districtSaleLead.Contains(x.Id))
                    .ToList();
            }

            var listDitrist = new List<AddressSaleItemDto>();
            foreach (var item in districts)
            {
                var add = new AddressSaleItemDto
                {
                    DisplayName = item.DistrictName,
                    Id = item.Id,
                    CityId = item.Id,
                    CityName = item.CityFk.CityName,
                    DistrictName = item.DistrictName,
                    DistrictId = item.Id
                };
                if (districtSelected.Count(x => x.DistrictId == item.Id) > 0)
                {
                    add.Selected = true;
                    if (userId != null)
                    {
                        var dis = districtSelected.Where(x => x.UserId != userId);
                        if (dis.Count(x => x.DistrictId == item.Id) > 0)
                            add.Disabled = true;
                    }
                }

                listDitrist.Add(add);
            }

            var wardSelected = _saleManLocationRepository.GetAll()
                .WhereIf(userId != null, x => x.UserId == userId)
                .Select(x =>
                    new SelectedAddress
                    {
                        WardId = x.WardId,
                        UserId = x.UserId
                    }).Distinct().ToList();
            var wards = _wardRepository.GetAllIncluding(x => x.DistrictFk)
                .WhereIf(districtId != null, x => x.DistrictId == districtId)
                .Where(x => districtSelected.Select(x => x.DistrictId).Contains(x.DistrictId)).ToList();
            if (saleLeadId != null)
            {
                var wardSaleLead = _saleManLocationRepository.GetAll().Where(x => x.UserId == saleLeadId)
                    .Select(x => x.DistrictId).Distinct();
                wards = wards.Where(x => wardSaleLead.Contains(x.Id))
                    .ToList();
            }

            var listWard = new List<AddressSaleItemDto>();
            foreach (var item in wards)
            {
                var add = new AddressSaleItemDto
                {
                    DisplayName = item.WardName,
                    Id = item.Id,
                    WardName = item.WardName,
                    DistrictName = item.DistrictFk.DistrictName,
                    DistrictId = item.Id
                };
                if (wardSelected.Count(x => x.WardId == item.Id) > 0)
                {
                    add.Selected = true;
                    if (userId != null)
                    {
                        var dis = wardSelected.Where(x => x.UserId != userId);
                        if (dis.Count(x => x.WardId == item.Id) > 0)
                            add.Disabled = true;
                    }
                }

                listWard.Add(add);
            }

            return new AddressSaleSelected
            {
                Cities = lstCity,
                Districts = listDitrist,
                Wards = listWard
            };
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        private class SelectedAddress
        {
            public int WardId { get; set; }
            public int CityId { get; set; }
            public int DistrictId { get; set; }
            public long UserId { get; set; }
        }
    }
}