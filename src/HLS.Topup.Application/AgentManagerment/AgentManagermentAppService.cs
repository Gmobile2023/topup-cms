using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Address;
using HLS.Topup.AgentManagerment.Exporting;
using HLS.Topup.AgentsManage.Dtos;
using HLS.Topup.Audit;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Audit;
using HLS.Topup.Dtos.Authentication;
using HLS.Topup.Dtos.Configuration;
using HLS.Topup.Dtos.Partner;
using HLS.Topup.Reports;
using HLS.Topup.RequestDtos;
using HLS.Topup.Sale;
using HLS.Topup.Security.HLS.Topup.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.AgentManagerment
{
    [AbpAuthorize(AppPermissions.Pages_AgentsManage)]
    public class AgentManagermentAppService : TopupAppServiceBase, IAgentManagermentAppService
    {
        private readonly IRepository<User, long> _lookupUserRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<SaleAssignAgent> _assignRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IReportsManager _reportManager;
        private readonly IAgentManagermenExport _agentManagermen;
        private readonly IAuditManger _auditManger;
        private readonly ILogger<AgentManagermentAppService> _logger;
        private readonly TopupAppSession _topupAppSession;

        private readonly IRepository<Odp> _odbRepository;

        //private readonly UserRegistrationManager _registrationManager;
        private readonly IAccountConfigurationManager _accountConfigurationManager;
        private readonly ICommonManger _commonManger;
        private readonly IUserEmailer _userEmailer;
        private readonly IStaffConfigurationManager _staffConfigurationManager;
        private readonly IAccountManager _accountManager;


        public AgentManagermentAppService(
            IRepository<User, long> lookupUserRepository, IRepository<UserProfile> userProfileRepository,
            IRepository<SaleAssignAgent> assignRepository,
            IAgentManagermenExport agentManagermen,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository,
            IRepository<Ward> wardRepository,
            IReportsManager reportManager, IAuditManger auditManger, ILogger<AgentManagermentAppService> logger,
            TopupAppSession topupAppSession, IRepository<Odp> odbRepository,
            //UserRegistrationManager registrationManager,
            IAccountConfigurationManager accountConfigurationManager,
            ICommonManger commonManger, IUserEmailer userEmailer, IStaffConfigurationManager staffConfigurationManager,
            IAccountManager accountManager)
        {
            _lookupUserRepository = lookupUserRepository;
            _userProfileRepository = userProfileRepository;
            _assignRepository = assignRepository;
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _agentManagermen = agentManagermen;
            _reportManager = reportManager;
            _auditManger = auditManger;
            _logger = logger;
            _topupAppSession = topupAppSession;
            _odbRepository = odbRepository;
            //_registrationManager = registrationManager;
            _accountConfigurationManager = accountConfigurationManager;
            _commonManger = commonManger;
            _userEmailer = userEmailer;
            _staffConfigurationManager = staffConfigurationManager;
            _accountManager = accountManager;
        }

        public async Task<PagedResultDto<AgentsDto>> GetAll(
            GetAllAgentsInput input)
        {
            var user = _lookupUserRepository.Get(AbpSession.UserId ?? 0);
            var query = from agent in _lookupUserRepository.GetAll()
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                join m in _assignRepository.GetAll() on agent.Id equals m.UserAgentId into mg
                from assign in mg.DefaultIfEmpty()
                join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                from sale in smg.DefaultIfEmpty()
                join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                from leader in lmg.DefaultIfEmpty()
                join c in _cityRepository.GetAll() on profile.CityId equals c.Id into cg
                from city in cg.DefaultIfEmpty()
                join d in _districtRepository.GetAll() on profile.DistrictId equals d.Id into dg
                from district in dg.DefaultIfEmpty()
                join w in _wardRepository.GetAll() on profile.WardId equals w.Id into wg
                from ward in wg.DefaultIfEmpty()
                join pr in _lookupUserRepository.GetAll() on agent.ParentId equals pr.Id into parent
                from agentGeneral in parent.DefaultIfEmpty()
                where (agent.AccountType == CommonConst.SystemAccountType.Agent
                       || agent.AccountType == CommonConst.SystemAccountType.MasterAgent)
                select new {agent, profile, assign, sale, leader, city, district, ward, agentGeneral};

            if (user != null && user.AccountType == CommonConst.SystemAccountType.SaleLead)
            {
                query = query.Where(c => c.sale != null && c.sale.UserSaleLeadId == user.Id);
            }
            else if (user != null && user.AccountType == CommonConst.SystemAccountType.Sale)
            {
                query = query.Where(c => c.assign != null && c.assign.SaleUserId == user.Id);
            }

            if (input.Filter != null)
                query = query.Where(c =>
                    c.agent.UserName.Contains(input.Filter) || c.agent.PhoneNumber.Contains(input.Filter) ||
                    c.agent.Surname.Contains(input.Filter) || c.agent.FullName.Contains(input.Filter) ||
                    c.agent.AccountCode.Contains(input.Filter));

            if (input.FromDateFilter != null)
                query = query.Where(c => c.agent.CreationTime >= input.FromDateFilter);

            if (input.ToDateFilter != null)
            {
                query = query.Where(
                    c => c.agent.CreationTime <= input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1));
            }


            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(c => c.agent.AccountCode.Contains(input.Filter)
                                         || c.agent.PhoneNumber.Contains(input.Filter)
                                         || c.agent.UserName.Contains(input.Filter));
            }

            var agentType = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            if (input.AgentTypeFilter != 99)
            {
                query = query.Where(c => c.agent != null && c.agent.AgentType == agentType);
            }

            if (input.ManagerFilter > 0)
            {
                query = query.Where(c => c.sale != null && c.sale.Id == input.ManagerFilter);
            }

            if (input.SaleLeadFilter > 0)
            {
                query = query.Where(c => c.leader != null && c.leader.Id == input.SaleLeadFilter);
            }

            if (input.AgentId > 0)
            {
                query = query.Where(c => c.agent.Id == input.AgentId.Value);
            }

            if (input.Province > 0)
                query = query.Where(c => c.profile != null && c.profile.CityId == input.Province);
            if (input.District > 0)
                query = query.Where(c => c.profile != null && c.profile.DistrictId == input.District);
            if (input.Village > 0)
                query = query.Where(c => c.profile != null && c.profile.WardId == input.Village);

            if (input.IsMapSale > -1 && input.IsMapSale == 0)
            {
                query = query.Where(c => c.sale == null);
            }
            else if (input.IsMapSale > -1 && input.IsMapSale == 1)
            {
                query = query.Where(c => c.sale != null);
            }

            if (input.Status.HasValue && input.Status > -1)
            {
                if (input.Status == 0)
                    query = query.Where(c => !c.agent.IsVerifyAccount);
                else if (input.Status == 1)
                    query = query.Where(c => c.agent.IsVerifyAccount && c.agent.IsActive);
                else if (input.Status == 2)
                    query = query.Where(c => c.agent.IsVerifyAccount && !c.agent.IsActive);
            }


            if (input.ExhibitFilter != null)
                query = query.Where(c => c.profile != null && c.profile.IdIdentity.Contains(input.ExhibitFilter));

            var totalCount = query.Count();
            var pagedAndFilteredAgents = query.OrderByDescending(c => c.agent.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);


            var agentsManage = from item in pagedAndFilteredAgents
                select new AgentsDto()
                {
                    AccountCode = item.agent.AccountCode,
                    PhoneNumber = item.agent.PhoneNumber,
                    FullName = item.agent.FullName,
                    AgentType = item.agent.AgentType,
                    ManagerName = item.sale != null
                        ? item.sale.UserName + " - " + item.sale.PhoneNumber + " - " + item.sale.FullName
                        : "",
                    SaleLeadName = item.leader != null
                        ? item.leader.UserName + " - " + item.leader.PhoneNumber + " - " + item.leader.FullName
                        : "",
                    CreationTime = item.agent.CreationTime,
                    Status = item.agent.IsVerifyAccount == false ? 0 : (item.agent.IsActive ? 1 : 2),
                    Address = item.profile != null
                        ? ((string.IsNullOrEmpty(item.profile.Address) ? "" : item.profile.Address + ", ")
                           + (item.ward != null ? item.ward.WardName + ", " : "")
                           + (item.district != null ? item.district.DistrictName + ", " : "")
                           + (item.city != null ? item.city.CityName + "" : "")
                        )
                        : "",
                    Exhibit = item.profile != null ? item.profile.IdIdentity : "",
                    IsMapSale = item.sale != null ? true : false,
                    Id = (int) item.agent.Id,
                    AgentGeneral = item.agentGeneral.AccountCode
                };

            return new PagedResultDto<AgentsDto>(
                totalCount,
                await agentsManage.ToListAsync()
            );
        }

        public async Task<AgentDetailView> GetAgentDetail(int userAgentId)
        {
            var info = (from agent in _lookupUserRepository.GetAll()
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                join m in _assignRepository.GetAll() on agent.Id equals m.UserAgentId into mg
                from assign in mg.DefaultIfEmpty()
                join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                from sale in smg.DefaultIfEmpty()
                join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                from leader in lmg.DefaultIfEmpty()
                join c in _cityRepository.GetAll() on profile.CityId equals c.Id into cg
                from city in cg.DefaultIfEmpty()
                join d in _districtRepository.GetAll() on profile.DistrictId equals d.Id into dg
                from district in dg.DefaultIfEmpty()
                join w in _wardRepository.GetAll() on profile.WardId equals w.Id into wg
                from ward in wg.DefaultIfEmpty()
                where agent.Id == userAgentId
                join ct in _lookupUserRepository.GetAll() on agent.CreatorUserId equals ct.Id into cu
                from creator in cu.DefaultIfEmpty()
                select new AgentDetailView()
                {
                    UserId = agent.Id,
                    AccountCode = agent.AccountCode,
                    PhoneNumber = agent.PhoneNumber,
                    FullName = agent.FullName,
                    Name = agent.Name,
                    Surname = agent.Surname,
                    AgentType = agent.AgentType,
                    ManagerName = sale != null ? sale.UserName + "- " + sale.PhoneNumber + " - " + sale.FullName : "",
                    SaleLeadName = leader != null
                        ? leader.UserName + " - " + leader.PhoneNumber + " - " + leader.FullName
                        : "",
                    CreationTime = agent.CreationTime,
                    IsMapSale = assign != null ? true : false,
                    AssignTime = assign.CreationTime,
                    VerifyTime = profile.CreationTime,
                    Status = agent.IsActive,
                    Address = profile.Address,
                    AddressView = profile != null
                        ? ((string.IsNullOrEmpty(profile.Address) ? "" : profile.Address + ", ")
                           + (ward != null ? ward.WardName + ", " : "")
                           + (district != null ? district.DistrictName + ", " : "")
                           + (city != null ? city.CityName + "" : "")
                        )
                        : "",
                    AgentName = agent.AgentName,
                    IdIdentityType = profile.IdType,
                    Exhibit = profile.IdIdentity,
                    IdIdentityFront = profile.FrontPhoto,
                    IdIdentityBack = profile.BackSitePhoto,
                    Province = profile.CityId,
                    District = profile.DistrictId,
                    Ward = profile.WardId,
                    IdentityIdExpireDate = profile.IdentityIdExpireDate,
                    CreatorName = creator.UserName,
                    ContractNumber = profile.ContractNumber,
                    SigDate = profile.SigDate,
                    MethodReceivePassFile = profile.MethodReceivePassFile,
                    ValueReceivePassFile = profile.ValueReceivePassFile
                }).FirstOrDefault();

            return info;
        }

        public async Task CreateOrEdit(CreateOrEditSaleAssignAgentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task<MappingSaleView> GetSaleAssignAgent(int userAgentId)
        {
            var assign = (from x in _assignRepository.GetAll()
                join s in _lookupUserRepository.GetAll() on x.SaleUserId equals s.Id
                join sl in _lookupUserRepository.GetAll() on s.UserSaleLeadId equals sl.Id
                where x.UserAgentId == userAgentId
                select new MappingSaleView()
                {
                    AgentId = (int) (x.UserAgentId ?? 0),
                    SaleUserId = x.SaleUserId,
                    UserSale = s.UserName + " - " + s.PhoneNumber + " - " + s.FullName,
                    UserSaleLeader = sl.UserName + " - " + sl.PhoneNumber + " - " + sl.FullName,
                    Id = x.Id,
                }).FirstOrDefault();

            return assign;
        }

        [AbpAuthorize(AppPermissions.Pages_AgentManager_Create)]
        private async Task Create(CreateOrEditSaleAssignAgentDto input)
        {
            try
            {
                var agent = await UserManager.GetUserByIdAsync(input.UserAgentId ?? 0);
                if (input.UserAgentId <= 0 || agent == null)
                    throw new UserFriendlyException("Không tồn tại User Agent");

                if (input.SaleUserId <= 0)
                    throw new UserFriendlyException("Không tồn tại User Sale");


                var assign = new SaleAssignAgent()
                {
                    Status = 1,
                    SaleUserId = input.SaleUserId,
                    UserAgentId = input.UserAgentId,
                };

                await _assignRepository.InsertAsync(assign);
                await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
                {
                    UserId = assign.UserAgentId ?? 0,
                });

                string textName = "Gán sale";
                var sale = await UserManager.GetUserByIdAsync(input.SaleUserId);
                if (sale != null)
                    textName = $"Gán sale {sale.UserName} - {sale.PhoneNumber} -{sale.FullName}";

                await AddActivities(agent, CommonConst.AccountActivityType.AssignSale, textName, input.ToJson(),
                    null, input.SaleUserId.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsManage_Unlock)]
        public async Task UnlockUser(BlockUnlockUserDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            user.Unblock();
            await AddActivities(user, CommonConst.AccountActivityType.UnLock, input.Note, input.ToJson(),
                "false", "true");
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsManage_Lock)]
        public async Task BlockUser(BlockUnlockUserDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            user.Block();
            await AddActivities(user, CommonConst.AccountActivityType.Lock, input.Note, input.ToJson(),
                "true", "false");
        }


        [AbpAuthorize(AppPermissions.Pages_AgentsManage_Edit)]
        private async Task Update(CreateOrEditSaleAssignAgentDto input)
        {
            var assign = await _assignRepository.FirstOrDefaultAsync((int) input.Id);
            if (assign == null)
                throw new UserFriendlyException("Không tồn tại thiết lập");

            var agent = await UserManager.GetUserByIdAsync(input.UserAgentId ?? 0);
            if (agent == null || input.UserAgentId <= 0)
                throw new UserFriendlyException("Không tồn tại User Agent");

            if (input.SaleUserId <= 0)
                throw new UserFriendlyException("Không tồn tại User Sale");
            var userSaleOld = assign.SaleUserId;
            assign.UserAgentId = input.UserAgentId;
            assign.SaleUserId = input.SaleUserId;

            await _assignRepository.UpdateAsync(assign);
            await _reportManager.ReportSyncAccountReport(new Report.SyncAccountRequest()
            {
                UserId = assign.UserAgentId ?? 0,
            });

            var saleNew = await UserManager.GetUserByIdAsync(input.SaleUserId);
            var saleOld = await UserManager.GetUserByIdAsync(userSaleOld);
            string textName = "Thay đổi sale";
            if (saleNew != null && saleOld != null)
                textName =
                    $"Sale cũ {saleOld.UserName} - {saleOld.PhoneNumber} -{saleOld.FullName}, Sale mới {saleNew.UserName} - {saleNew.PhoneNumber} -{saleNew.FullName}";

            await AddActivities(agent, CommonConst.AccountActivityType.ConvertSale, textName, input.ToJson(),
                assign.SaleUserId.ToString(), input.SaleUserId.ToString());
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsManage_Edit)]
        public async Task UpdateUserName(UpdateUserNameInputDto input)
        {
            var userUpdate = await UserManager.GetUserByIdAsync(input.UserId);
            if (userUpdate == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            var usernameOld = userUpdate.UserName;

            var update = await UserManager.UpdateUserNameAsync(input);
            if (update)
            {
                await AddActivities(userUpdate, CommonConst.AccountActivityType.ChangeUserName,
                    $"Thay đổi số điện thoại đăng nhập từ {usernameOld} sang {input.UserName}", input.ToJson(),
                    usernameOld, input.UserName, input.Attachment);
            }
        }

        public async Task CreateOrEditAgentPartner(CreateOrUpdateAgentPartnerInput input)
        {
            try
            {
                if (input.PartnerConfig == null || string.IsNullOrEmpty(input.PartnerConfig.ClientId) ||
                    string.IsNullOrEmpty(input.PartnerConfig.SecretKey) ||
                    string.IsNullOrEmpty(input.PartnerConfig.PrivateKeyFile) ||
                    string.IsNullOrEmpty(input.PartnerConfig.PublicKeyFile))
                    throw new UserFriendlyException("Thông tin tài khoản api không hợp lệ");

                if (input.IdentityServerStorage?.AllowedScopes == null ||
                    !input.IdentityServerStorage.AllowedScopes.Any() ||
                    input.IdentityServerStorage.AllowedGrantTypes == null ||
                    !input.IdentityServerStorage.AllowedGrantTypes.Any())
                    throw new UserFriendlyException("Thông tin Identity server không hợp lệ");

                if (input.Id.HasValue)
                {
                    await UpdateAgentPartner(input);
                }
                else
                {
                    await CreateAgentPartner(input);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateOrEditAgentPartner error:{e}");
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsSupper_Create)]
        private async Task CreateAgentPartner(CreateOrUpdateAgentPartnerInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Password))
                    throw new UserFriendlyException("Vui lòng nhập mật khẩu");
                var createUser = await _accountManager.CreateUserAsync(new CreateAccountDto
                {
                    Channel = CommonConst.Channel.WEB,
                    Name = input.Name,
                    Surname = input.Surname,
                    Password = input.Password,
                    AccountType = CommonConst.SystemAccountType.MasterAgent,
                    //ParentAccount = _topupAppSession.AccountCode, //Parent account code
                    AgentType = CommonConst.AgentType.AgentApi,
                    PhoneNumber = input.PhoneNumber,
                    IsEmailConfirmed = true,
                    IsActive = input.IsActive,
                    IsVerifyAccount = true,
                    AgentName = input.Name,
                    EmailAddress = input.EmailTech
                });
                if (createUser == null)
                    throw new UserFriendlyException("Tạo tài khoản Agent lỗi");

                var profile = new UserProfileDto
                {
                    AgentName = input.Name,
                    CityId = input.CityId,
                    DistrictId = input.DistrictId,
                    WardId = input.WardId,
                    Address = input.Address,
                    UserId = createUser.Id,
                    TenantId = AbpSession.TenantId,
                    Desscription = input.Description,
                    ContactInfos = input.ContactInfos.ToJson(),
                    ContractNumber = input.ContractNumber,
                    EmailReceives = input.EmailReceives,
                    PeriodCheck = input.PeriodCheck,
                    SigDate = input.SigDate,
                    TaxCode = input.TaxCode,
                    ChatId = input.ChatId,
                    LimitChannel = input.LimitChannel,
                    IsApplySlowTrans = input.IsApplySlowTrans,
                    EmailTech = input.EmailTech,
                    FolderFtp = input.FolderFtp
                };
                await UserManager.CreateOrUpdateUserProfile(profile);

                await _staffConfigurationManager.CreateUserStaff(new CreateStaffUserInput
                {
                    Name = input.Name,
                    Surname = input.Surname,
                    Password = input.Password,
                    IsActive = input.IsActive,
                    PhoneNumber = input.PhoneNumber,
                    UserName = input.UserName + "_nv",
                    ParentUserId = createUser.Id
                    //EmailAddress = input.UserName + "_nv@gmail.com"
                });

                var request = input.PartnerConfig.ConvertTo<CreateOrUpdatePartnerRequest>();
                request.IsActive = input.IsActive;
                request.PartnerCode = createUser.AccountCode;
                request.PartnerName = createUser.FullName;
                request.CategoryConfigs = input.PartnerConfig.CategoryConfigList;
                request.ProductConfigsNotAllow = input.PartnerConfig.ProductConfigsNotAllowList;
                request.ServiceConfigs = input.PartnerConfig.ServiceConfigList;
                var rs = await _accountConfigurationManager.CreateOrUpdatePartnerConfig(request);
                if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success && input.IdentityServerStorage != null &&
                    input.PartnerConfig.ClientId != null)
                {
                    input.IdentityServerStorage.ClientName = createUser.FullName;
                    input.IdentityServerStorage.ClientId = input.PartnerConfig.ClientId;
                    input.IdentityServerStorage.AccountCode = createUser.AccountCode;
                    input.IdentityServerStorage.IsActive = createUser.IsActive;
                    input.IdentityServerStorage.ClientSecrets = new List<string> {input.PartnerConfig.SecretKey};
                    await _commonManger.CreateOrUpdateClientId(input.IdentityServerStorage);
                    await Task.Run(() =>
                    {
                        _userEmailer.SendEmailCreateAgentApi(createUser, input.EmailTech,
                            input.Password, input.IdentityServerStorage.ClientId,
                            input.IdentityServerStorage.ClientSecrets?.FirstOrDefault());
                    }).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsSupper_Edit)]
        private async Task UpdateAgentPartner(CreateOrUpdateAgentPartnerInput input)
        {
            try
            {
                var user = await UserManager.GetUserByIdAsync(input.Id ?? 0);
                if (user == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");
                user.Name = input.Name;
                user.Surname = input.Surname;
                user.IsActive = input.IsActive;

                var request = input.PartnerConfig.ConvertTo<CreateOrUpdatePartnerRequest>();
                request.IsActive = input.IsActive;
                request.PartnerCode = user.AccountCode;
                request.PartnerName = user.FullName;
                request.CategoryConfigs = input.PartnerConfig.CategoryConfigList;
                request.ProductConfigsNotAllow = input.PartnerConfig.ProductConfigsNotAllowList;
                request.ServiceConfigs = input.PartnerConfig.ServiceConfigList;
                var updateGw = await _accountConfigurationManager.CreateOrUpdatePartnerConfig(request);
                if (updateGw.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                    throw new UserFriendlyException("Không cập nhật được thông tin cấu hình đại lý");

                CheckErrors(await UserManager.UpdateAsync(user));
                if (!input.Password.IsNullOrEmpty())
                {
                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
                }

                var profile = new UserProfileDto
                {
                    UserId = user.Id,
                    AgentName = input.Name,
                    CityId = input.CityId,
                    DistrictId = input.DistrictId,
                    WardId = input.WardId,
                    Address = input.Address,
                    TenantId = AbpSession.TenantId,
                    Desscription = input.Description,
                    ContactInfos = input.ContactInfos.ToJson(),
                    ContractNumber = input.ContractNumber,
                    EmailReceives = input.EmailReceives,
                    PeriodCheck = input.PeriodCheck,
                    SigDate = input.SigDate,
                    TaxCode = input.TaxCode,
                    ChatId = input.ChatId,
                    LimitChannel = input.LimitChannel,
                    IsApplySlowTrans = input.IsApplySlowTrans,
                    EmailTech = input.EmailTech,
                    FolderFtp = input.FolderFtp
                    
                };
                await UserManager.CreateOrUpdateUserProfile(profile);
                if (input.IdentityServerStorage != null && input.PartnerConfig.ClientId != null)
                {
                    input.IdentityServerStorage.ClientName = user.FullName;
                    input.IdentityServerStorage.ClientId = input.PartnerConfig.ClientId;
                    input.IdentityServerStorage.AccountCode = user.AccountCode;
                    input.IdentityServerStorage.IsActive = user.IsActive;
                    input.IdentityServerStorage.ClientSecrets = new List<string> {input.PartnerConfig.SecretKey};
                    await _commonManger.CreateOrUpdateClientId(input.IdentityServerStorage);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Update tài khoản không thành công");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsSupper_SendMailTech)]
        public async Task ResendEmailTech(EntityDto<long> input)
        {
            var profile = await _userProfileRepository.GetAllIncluding(x => x.UserFk)
                .FirstOrDefaultAsync(x => x.UserId == input.Id);
            if (profile == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            var client = await _commonManger.GetClientId(profile.UserFk.AccountCode);
            if (client == null)
                throw new UserFriendlyException("Tài khoản chưa có thông tin kết nối");
            await Task.Run(() =>
            {
                _userEmailer.SendEmailCreateAgentApi(profile.UserFk, profile.EmailTech,
                    null, client.ClientId,
                    client.ClientSecrets.FromJson<List<string>>().FirstOrDefault());
            }).ConfigureAwait(false);
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsManage_Edit)]
        public async Task ResetOdp(ResetOdpInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            if (input.ResetCount < 0)
                throw new UserFriendlyException("Thông tin không hợp lệ");

            var check = await _odbRepository.FirstOrDefaultAsync(x => x.PhoneNumber == user.PhoneNumber);
            if (check == null)
                throw new UserFriendlyException("Không tồn tại thông tin ODP với tài khoản này. Vui lòng kiểm tra lại");
            check.CountSend = input.ResetCount;
            await _odbRepository.UpdateAsync(check);
        }

        public async Task<FileDto> GetAllListToExcel(GetAllAgentsInput input)
        {
            input.MaxResultCount = int.MaxValue;
            input.SkipCount = 0;
            //var user = await _userRepository.GetAsync(AbpSession.UserId ?? 0);
            //request.AccountType = GetAccountTypeNumber(user.AccountType);
            //request.LoginCode = user.AccountCode;
            var user = _lookupUserRepository.Get(AbpSession.UserId ?? 0);

            var query = from agent in _lookupUserRepository.GetAll()
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                join m in _assignRepository.GetAll() on agent.Id equals m.UserAgentId into mg
                from assign in mg.DefaultIfEmpty()
                join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                from sale in smg.DefaultIfEmpty()
                join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                from leader in lmg.DefaultIfEmpty()
                join c in _cityRepository.GetAll() on profile.CityId equals c.Id into cg
                from city in cg.DefaultIfEmpty()
                join d in _districtRepository.GetAll() on profile.DistrictId equals d.Id into dg
                from district in dg.DefaultIfEmpty()
                join w in _wardRepository.GetAll() on profile.WardId equals w.Id into wg
                from ward in wg.DefaultIfEmpty()
                join pr in _lookupUserRepository.GetAll() on agent.ParentId equals pr.Id into parent
                from agentGeneral in parent.DefaultIfEmpty()
                where (agent.AccountType == CommonConst.SystemAccountType.Agent
                       || agent.AccountType == CommonConst.SystemAccountType.MasterAgent)
                select new {agent, profile, assign, sale, leader, city, district, ward, agentGeneral};


            if (user != null && user.AccountType == CommonConst.SystemAccountType.SaleLead)
            {
                query = query.Where(c => c.sale != null && c.sale.UserSaleLeadId == user.Id);
            }
            else if (user != null && user.AccountType == CommonConst.SystemAccountType.Sale)
            {
                query = query.Where(c => c.assign != null && c.assign.SaleUserId == user.Id);
            }

            if (input.Filter != null)
                query = query.Where(c =>
                    c.agent.UserName.Contains(input.Filter) || c.agent.PhoneNumber.Contains(input.Filter) ||
                    c.agent.Surname.Contains(input.Filter) || c.agent.FullName.Contains(input.Filter) ||
                    c.agent.AccountCode.Contains(input.Filter));

            if (input.FromDateFilter != null)
                query = query.Where(c => c.agent.CreationTime >= input.FromDateFilter);

            if (input.ToDateFilter != null)
            {
                query = query.Where(
                    c => c.agent.CreationTime <= input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1));
            }


            if (!string.IsNullOrEmpty(input.Filter))
            {
                query = query.Where(c => c.agent.AccountCode.Contains(input.Filter)
                                         || c.agent.PhoneNumber.Contains(input.Filter)
                                         || c.agent.UserName.Contains(input.Filter));
            }

            var agentType = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            if (input.AgentTypeFilter != 99)
            {
                query = query.Where(c => c.agent != null && c.agent.AgentType == agentType);
            }


            if (input.ManagerFilter > 0)
            {
                query = query.Where(c => c.sale != null && c.sale.Id == input.ManagerFilter);
            }

            if (input.SaleLeadFilter > 0)
            {
                query = query.Where(c => c.sale != null && c.leader.Id == input.SaleLeadFilter);
            }

            if (input.AgentId > 0)
            {
                query = query.Where(c => c.agent.Id == input.AgentId.Value);
            }

            if (input.Province > 0)
                query = query.Where(c => c.profile != null && c.profile.CityId == input.Province);
            if (input.District > 0)
                query = query.Where(c => c.profile != null && c.profile.DistrictId == input.District);
            if (input.Village > 0)
                query = query.Where(c => c.profile != null && c.profile.WardId == input.Village);

            if (input.Status.HasValue && input.Status > -1)
            {
                if (input.Status == 0)
                    query = query.Where(c => !c.agent.IsVerifyAccount);
                else if (input.Status == 1)
                    query = query.Where(c => c.agent.IsVerifyAccount && c.agent.IsActive);
                else if (input.Status == 2)
                    query = query.Where(c => c.agent.IsVerifyAccount && !c.agent.IsActive);
            }


            if (input.ExhibitFilter != null)
                query = query.Where(c => c.profile != null && c.profile.IdIdentity.Contains(input.ExhibitFilter));


            var pagedAndFilteredAgents = await query.OrderByDescending(c => c.agent.CreationTime).ToListAsync();


            var agentsManage = (from item in pagedAndFilteredAgents
                select new AgentsDto()
                {
                    AccountCode = item.agent.AccountCode,
                    PhoneNumber = item.agent.PhoneNumber,
                    FullName = item.agent.FullName,
                    AgentType = item.agent.AgentType,
                    ManagerName = item.sale != null
                        ? item.sale.UserName + " - " + item.sale.PhoneNumber + " - " + item.sale.FullName
                        : "",
                    SaleLeadName = item.leader != null
                        ? item.leader.UserName + " - " + item.leader.PhoneNumber + " - " + item.leader.FullName
                        : "",
                    CreationTime = item.agent.CreationTime,
                    Status = item.agent.IsVerifyAccount == false ? 0 : (item.agent.IsActive ? 1 : 2),
                    Address = item.profile != null
                        ? ((string.IsNullOrEmpty(item.profile.Address) ? "" : item.profile.Address + ", ")
                           + (item.ward != null ? item.ward.WardName + ", " : "")
                           + (item.district != null ? item.district.DistrictName + ", " : "")
                           + (item.city != null ? item.city.CityName + "" : "")
                        )
                        : "",
                    Exhibit = item.profile != null ? item.profile.IdIdentity : "",
                    IsMapSale = item.sale != null ? true : false,
                    Id = (int) item.agent.Id,
                    AgentGeneral = item.agentGeneral != null ? item.agentGeneral.AccountCode : ""
                }).ToList();

            return _agentManagermen.ExportToFile(agentsManage);
        }

        private async Task AddActivities(User user, CommonConst.AccountActivityType type, string note, string payload,
            string srcval, string desval, string attachment = "")
        {
            await _auditManger.AddAccountActivities(new AccountActivityHistoryRequest
            {
                Note = note,
                AccountCode = user.AccountCode,
                AccountType = (byte) user.AccountType,
                AgentType = (byte) user.AgentType,
                DesValue = desval,
                SrcValue = srcval,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                UserName = _topupAppSession.UserName,
                Payload = payload,
                AccountActivityType = type,
                Attachment = attachment,
            });
        }

        [AbpAuthorize(AppPermissions.Pages_AgentsSupper)]
        public async Task<PagedResultDto<AgentsSupperDto>> GetSupperAll(
            GetAllAgentSupperInput input)
        {
            var query = from agent in _lookupUserRepository.GetAll()
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                where (agent.AccountType == CommonConst.SystemAccountType.MasterAgent &&
                       agent.AgentType == CommonConst.AgentType.AgentApi)
                select new {agent, profile};


            if (!string.IsNullOrEmpty(input.AccountAgent))
            {
                query = query.Where(c => c.agent.AccountCode.Contains(input.AccountAgent)
                                         || c.agent.PhoneNumber.Contains(input.AccountAgent)
                                         || c.agent.UserName.Contains(input.AccountAgent));
            }

            if (input.CrossCheckPeriod > 0)
                query = query.Where(c => c.profile.PeriodCheck == input.CrossCheckPeriod);

            if (input.Status > 0)
            {
                if (input.Status == 1)
                    query = query.Where(c => c.agent.IsActive == true);
                else query = query.Where(c => c.agent.IsActive == false);
            }

            var totalCount = query.Count();
            var pagedAndFilteredAgents = query.OrderByDescending(c => c.agent.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);


            var agentsManage = from item in pagedAndFilteredAgents
                select new AgentsSupperDto()
                {
                    AccountCode = item.agent.AccountCode,
                    PhoneNumber = item.agent.PhoneNumber,
                    FullName = item.agent.FullName,
                    CreatedDate = item.agent.CreationTime,
                    Status = item.agent.IsActive ? 1 : 2,
                    StatusName = item.agent.IsActive ? "Hoạt động" : "Khóa",
                    CrossCheckPeriod = item.profile.PeriodCheck,
                    UserId = item.agent.Id,
                };

            return new PagedResultDto<AgentsSupperDto>(
                totalCount,
                await agentsManage.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Pages_AgentsSupper)]
        public async Task<AgentSupperDetailView> GetAgentSupperDetail(int userAgentId)
        {
            try
            {
                var info = (from agent in _lookupUserRepository.GetAll()
                    join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                    from profile in pg.DefaultIfEmpty()
                    join m in _assignRepository.GetAll() on agent.Id equals m.UserAgentId into mg
                    from assign in mg.DefaultIfEmpty()
                    join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                    from sale in smg.DefaultIfEmpty()
                    join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                    from leader in lmg.DefaultIfEmpty()
                    join c in _cityRepository.GetAll() on profile.CityId equals c.Id into cg
                    from city in cg.DefaultIfEmpty()
                    join d in _districtRepository.GetAll() on profile.DistrictId equals d.Id into dg
                    from district in dg.DefaultIfEmpty()
                    join w in _wardRepository.GetAll() on profile.WardId equals w.Id into wg
                    from ward in wg.DefaultIfEmpty()
                    where agent.Id == userAgentId
                    select new AgentSupperDetailView()
                    {
                        UserId = agent.Id,
                        AccountCode = agent.AccountCode,
                        PhoneNumber = agent.PhoneNumber,
                        FullName = agent.FullName,
                        Name = agent.Name,
                        Surname = agent.Surname,
                        AgentType = agent.AgentType,
                        CreationTime = agent.CreationTime,
                        Status = agent.IsActive ? 1 : 2,
                        StatusName = agent.IsActive ? "Hoạt động" : "Khóa",
                        Address = profile.Address,
                        AddressView = profile != null
                            ? ((string.IsNullOrEmpty(profile.Address) ? "" : profile.Address + ", ")
                               + (ward != null ? ward.WardName + ", " : "")
                               + (district != null ? district.DistrictName + ", " : "")
                               + (city != null ? city.CityName + "" : "")
                            )
                            : "",
                        AgentName = agent.AgentName,
                        Province = profile.CityId,
                        District = profile.DistrictId,
                        Ward = profile.WardId,
                        Contract = profile.ContractNumber,
                        CrossCheckPeriod = profile.PeriodCheck,
                        EmailCompare = profile.EmailReceives,
                        FolderFtp = profile.FolderFtp,
                        ContractRegister = profile.SigDate,
                        Content = profile.ContactInfos,
                        ChatId = profile.ChatId,
                        LimitChannel = profile.LimitChannel,
                        EmailTech = profile.EmailTech,
                        IsApplySlowTrans = profile.IsApplySlowTrans ?? false
                    }).FirstOrDefault();

                if (info != null && !string.IsNullOrEmpty(info.Content))
                {
                    try
                    {
                        var content = info.Content.FromJson<List<AgentPartnerContactInfo>>();
                        if (content != null)
                        {
                            var director = content.FirstOrDefault(c =>
                                c.ContactType == CommonConst.AgentPartnerContactInfoType.Director);
                            if (director != null)
                            {
                                info.ManagerFullName = director.FullName;
                                info.ManagerPhone = director.PhoneNumber;
                                info.ManagerEmail = director.Email;
                            }

                            var technical = content.FirstOrDefault(c =>
                                c.ContactType == CommonConst.AgentPartnerContactInfoType.Technical);
                            if (technical != null)
                            {
                                info.TechnicalFullName = technical.FullName;
                                info.TechnicalPhone = technical.PhoneNumber;
                                info.TechnicalEmail = technical.Email;
                            }

                            var comparator = content.FirstOrDefault(c =>
                                c.ContactType == CommonConst.AgentPartnerContactInfoType.Comparator);
                            if (comparator != null)
                            {
                                info.CompareFullName = comparator.FullName;
                                info.ComparePhone = comparator.PhoneNumber;
                                info.CompareEmail = comparator.Email;
                            }

                            var accountant = content.FirstOrDefault(c =>
                                c.ContactType == CommonConst.AgentPartnerContactInfoType.Accountant);
                            if (accountant != null)
                            {
                                info.AccountancyFullName = accountant.FullName;
                                info.AccountancyPhone = accountant.PhoneNumber;
                                info.AccountancyEmail = accountant.Email;
                            }
                        }
                    }
                    catch (Exception convet)
                    {
                        _logger.LogInformation(
                            $"GetAgentSupperDetail Convert Exception : {convet.Message}|{convet.StackTrace}|{convet.InnerException}");
                    }
                }

                if (info == null) return null;
                var partner = await _accountConfigurationManager.GetPartnerConfig(new GetPartnerRequest
                {
                    PartnerCode = info.AccountCode
                });
                if (partner == null)
                {
                    info.PartnerConfig = new PartnerConfigTransDto();
                    return info;
                }

                info.IdentityServerStorage = new IdentityServerStorageInputDto();
                var clientInfo = await _commonManger.GetClientId(info.AccountCode);
                if (clientInfo != null)
                {
                    var client = clientInfo.ConvertTo<IdentityServerStorageInputDto>();
                    client.AllowedScopes = clientInfo.AllowedScopes.FromJson<List<string>>();
                    client.ClientSecrets = clientInfo.ClientSecrets.FromJson<List<string>>();
                    client.AllowedGrantTypes = clientInfo.AllowedGrantTypes.FromJson<List<string>>();
                    info.IdentityServerStorage = client;
                }

                partner.ServiceConfigList = partner.ServiceConfig.FromJson<List<string>>();
                partner.CategoryConfigList = partner.CategoryConfigs.FromJson<List<string>>();
                //add category config not allow
                partner.ProductConfigsNotAllowList = partner.ProductConfigsNotAllow.FromJson<List<string>>();
                info.PartnerConfig = partner;

                return info;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"GetAgentSupperDetail Exception : {ex.Message}|{ex.StackTrace}|{ex.InnerException}");
                return new AgentSupperDetailView();
            }
        }


        [AbpAuthorize(AppPermissions.Pages_AgentsSupper)]
        public async Task<FileDto> GetAllAgentSupperListToExcel(GetAllAgentSupperInput input)
        {
            var user = _lookupUserRepository.Get(AbpSession.UserId ?? 0);
            var query = from agent in _lookupUserRepository.GetAll()
                join p in _userProfileRepository.GetAll() on agent.Id equals p.UserId into pg
                from profile in pg.DefaultIfEmpty()
                where (agent.AccountType == CommonConst.SystemAccountType.MasterAgent
                       && agent.AgentType == CommonConst.AgentType.AgentApi)
                select new {agent, profile};


            if (!string.IsNullOrEmpty(input.AccountAgent))
            {
                query = query.Where(c => c.agent.AccountCode.Contains(input.AccountAgent)
                                         || c.agent.PhoneNumber.Contains(input.AccountAgent)
                                         || c.agent.UserName.Contains(input.AccountAgent));
            }

            if (input.CrossCheckPeriod > 0)
                query = query.Where(c => c.profile.PeriodCheck == input.CrossCheckPeriod);

            if (input.Status > 0)
            {
                if (input.Status == 1)
                    query = query.Where(c => c.agent.IsActive == true);
                else query = query.Where(c => c.agent.IsActive == false);
            }


            var pagedAndFilteredAgents = await query.OrderByDescending(c => c.agent.CreationTime).ToListAsync();

            var agentsSupper = (from item in pagedAndFilteredAgents
                select new AgentsSupperDto()
                {
                    AccountCode = item.agent.AccountCode,
                    PhoneNumber = item.agent.PhoneNumber,
                    FullName = item.agent.FullName,
                    CreatedDate = item.agent.CreationTime,
                    Status = item.agent.IsActive ? 1 : 2,
                    StatusName = item.agent.IsActive ? "Hoạt động" : "Khóa",
                    CrossCheckPeriod = item.profile != null ? item.profile.PeriodCheck : 0,
                    UserId = item.agent.Id,
                }).ToList();

            return _agentManagermen.ExportAgentSupplerToFile(agentsSupper);
        }
    }
}
