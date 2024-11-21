using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Deposits.Exporting;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServiceStack;
using HLS.Topup.Sale;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Dtos.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Deposits
{
    [AbpAuthorize]
    public class DepositsAppService : TopupAppServiceBase, IDepositsAppService
    {
        private readonly IRepository<Deposit> _depositRepository;
        private readonly IDepositsExcelExporter _depositsExcelExporter;
        private readonly IRepository<User, long> _lookupUserRepository;
        private readonly IRepository<Bank, int> _lookupBankRepository;
        private readonly IRepository<SaleLimitDebt, int> _saleLimitDebtRepository;
        private readonly IRepository<SaleClearDebtHistory, int> _clearDebtHistoryRepository;
        private readonly IRepository<SaleAssignAgent> _assignAgentRepository;
        private readonly ICommonManger _commonManger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<SaleClearDebt> _saleClearDebtRepository;
        private readonly UrlExtentions _extentions;
        private readonly TopupAppSession _appSession;

        private readonly INotificationSender _appNotifier;

        //private readonly Logger _logger = LogManager.GetLogger("DepositsAppService");
        private readonly ILogger<DepositsAppService> _logger;


        public DepositsAppService(IRepository<Deposit> depositRepository, IDepositsExcelExporter depositsExcelExporter,
            IRepository<User, long> lookupUserRepository, IRepository<Bank, int> lookupBankRepository,
            ICommonManger commonManger, ITransactionManager transactionManager, INotificationSender appNotifier,
            IRepository<SaleLimitDebt, int> saleLimitDebtRepository,
            IRepository<SaleClearDebtHistory, int> clearDebtHistoryRepository,
            IRepository<SaleAssignAgent> assignAgentRepository,
            IRepository<SaleClearDebt> saleClearDebtRepository,
            ILogger<DepositsAppService> logger, UrlExtentions extentions, TopupAppSession appSession)
        {
            _depositRepository = depositRepository;
            _depositsExcelExporter = depositsExcelExporter;
            _lookupUserRepository = lookupUserRepository;
            _lookupBankRepository = lookupBankRepository;
            _commonManger = commonManger;
            _transactionManager = transactionManager;
            _saleLimitDebtRepository = saleLimitDebtRepository;
            _clearDebtHistoryRepository = clearDebtHistoryRepository;
            _assignAgentRepository = assignAgentRepository;
            _appNotifier = appNotifier;
            _logger = logger;
            _extentions = extentions;
            _appSession = appSession;
            _saleClearDebtRepository = saleClearDebtRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits)]
        public async Task<PagedResultDtoReport<GetDepositForViewDto>> GetAll(GetAllDepositsInput input)
        {
            var user = await _lookupUserRepository.GetAsync(AbpSession.UserId ?? 0);
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DepositStatus) input.StatusFilter
                : default;

            List<long> listUserByNvkd = new List<long>();
            if (input.SaleLeadFilter.HasValue)
            {
                var lsSale = await _lookupUserRepository.GetAll()
                    .Where(x => x.UserSaleLeadId == input.SaleLeadFilter.Value).ToListAsync();
                if (lsSale.Any())
                    listUserByNvkd = lsSale.Select(x => x.Id).ToList();
            }

            var filteredDeposits = _depositRepository.GetAll()
                .Include(e => e.UserFk)
                //.Include(e => e.UserSaleFk)
                .Include(e => e.BankFk)
                .Include(e => e.ApproverFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter) ||
                         e.ExtraInfo.Contains(input.Filter))
                .WhereIf(input.MinApprovedDateFilter != null,
                    e => e.CreationTime >= input.MinApprovedDateFilter || e.ApprovedDate >= input.MinApprovedDateFilter)
                .WhereIf(input.MaxApprovedDateFilter != null,
                    e => e.CreationTime <= input.MaxApprovedDateFilter || e.ApprovedDate <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Pending,
                    e => e.CreationTime >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Pending,
                    e => e.CreationTime <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Approved,
                    e => e.ApprovedDate >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Approved,
                    e => e.ApprovedDate <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Canceled,
                    e => e.LastModificationTime >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Canceled,
                    e => e.LastModificationTime <= input.MaxApprovedDateFilter)
                .WhereIf(input.UserId != null, e => e.UserId == (long) input.UserId)
                .WhereIf(input.ApproverId != null, e => e.ApproverId == input.ApproverId)
                .WhereIf(input.BankId != null, e => e.BankId == input.BankId)
                .WhereIf(input.TransCodeFilter != null, e => e.TransCode.Contains(input.TransCodeFilter))
                .WhereIf(input.TransCodeBankFilter != null, e => e.TransCodeBank.Contains(input.TransCodeBankFilter))
                .WhereIf(input.SaleLeadFilter.HasValue && listUserByNvkd.Any(),
                    e => listUserByNvkd.Contains(e.UserSaleId.Value))
                .WhereIf(input.SaleManFilter != null, e => e.UserSaleId == input.SaleManFilter)
                .WhereIf(input.RequestCodeFilter != null, e => e.RequestCode.Contains(input.RequestCodeFilter))
                //.WhereIf(input.SaleLeadFilter != null, e => e.UserSaleFk.UserSaleLeadFk.Id == input.SaleLeadFilter)
                //.WhereIf(input.SaleManFilter != null, e => e.UserSaleFk != null && e.UserSaleFk.Id == input.SaleManFilter)
                .WhereIf(input.AgentTypeFilter.HasValue,
                    e => e.UserFk != null && e.UserFk.AgentType == input.AgentTypeFilter)
                .WhereIf(input.DepositTypeFilter != null, e => e.Type == input.DepositTypeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter),
                    e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter),
                    e => e.ApproverFk != null && e.ApproverFk.Name == input.UserName2Filter);

            if (user.AccountType == CommonConst.SystemAccountType.SaleLead)
            {
                filteredDeposits = from x in filteredDeposits
                    join y in _assignAgentRepository.GetAll() on x.UserId equals y.UserAgentId
                    join s in _lookupUserRepository.GetAll() on y.SaleUserId equals s.Id
                    where s.UserSaleLeadId == user.Id
                    select x;
            }
            else if (user.AccountType == CommonConst.SystemAccountType.Sale)
            {
                filteredDeposits = from x in filteredDeposits
                    join y in _assignAgentRepository.GetAll() on x.UserId equals y.UserAgentId
                    where y.SaleUserId == user.Id
                    select x;
            }
            else if (user.AccountType != CommonConst.SystemAccountType.System)
            {
                filteredDeposits = filteredDeposits.Where(c => c.UserId == user.Id);
            }

            var pagedAndFilteredDeposits = filteredDeposits
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var deposits = from o in pagedAndFilteredDeposits
                join o1 in _lookupUserRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookupBankRepository.GetAll() on o.BankId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookupUserRepository.GetAll() on o.ApproverId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _assignAgentRepository.GetAll() on s1.Id equals o4.UserAgentId into j4
                from assign in j4.DefaultIfEmpty()
                join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                from sale in smg.DefaultIfEmpty()
                join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                from leader in lmg.DefaultIfEmpty()
                select new GetDepositForViewDto()
                {
                    Deposit = new DepositDto
                    {
                        Status = o.Status,
                        Amount = o.Amount,
                        ApprovedDate = o.ApprovedDate,
                        TransCode = o.TransCode,
                        Id = o.Id,
                        Description = o.Description,
                        Type = o.Type,
                        RecipientInfo = o.RecipientInfo,
                        CreationTime = o.CreationTime,
                        TransCodeBank = o.TransCodeBank,
                        RequestCode = o.RequestCode,
                    },
                    UserName = s1 == null || s1.Name == null
                        ? ""
                        : s1.AccountCode + " - " + s1.PhoneNumber + " - " + s1.FullName,
                    BankId = s2.Id,
                    BankBankName = s2 == null || s2.ShortName == null ? "" : s2.ShortName,
                    UserName2 = s3 == null ? "" : s3.UserName,
                     AgentType = s1.AgentType,
                     AgentName = s1!=null?s1.AccountCode:"" + " - " + s1!=null?s1.UserName:"" + " - " + s1!=null?s1.FullName:"",
                    SaleMan = sale != null ? sale.UserName + " - " + sale.PhoneNumber + " - " + sale.FullName : "",
                    SaleLeader = leader != null
                        ? leader.UserName + " - " + leader.PhoneNumber + " - " + leader.FullName
                        : "",
                };


            var totalCount = await filteredDeposits.CountAsync();
            var totalAmount = filteredDeposits.Sum(c => c.Amount);

            return new PagedResultDtoReport<GetDepositForViewDto>(
                totalCount,
                new GetDepositForViewDto() {Deposit = new DepositDto() {Amount = totalAmount}},
                await deposits.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits)]
        public async Task<GetDepositForViewDto> GetDepositForView(int id)
        {
            var deposit = await _depositRepository.GetAsync(id);

            var output = new GetDepositForViewDto {Deposit = ObjectMapper.Map<DepositDto>(deposit)};

            if (output.Deposit.UserId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.UserId);
                output.UserName = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : "";
                output.AgentType = _lookupUser != null ? _lookupUser.AgentType : CommonConst.AgentType.Default;
            }

            if (output.Deposit.BankId != null)
            {
                var _lookupBank = await _lookupBankRepository.FirstOrDefaultAsync((int) output.Deposit.BankId);
                output.BankBankName = _lookupBank?.ShortName?.ToString();
            }

            if (output.Deposit.ApproverId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.ApproverId);
                output.UserName2 = _lookupUser?.UserName?.ToString();
            }

            if (output.Deposit.CreatorUserId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.CreatorUserId);
                output.CreatorName = _lookupUser?.UserName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits_Edit)]
        public async Task<GetDepositForEditOutput> GetDepositForEdit(EntityDto input)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDepositForEditOutput {Deposit = ObjectMapper.Map<CreateOrEditDepositDto>(deposit)};

            if (output.Deposit.UserId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.UserId);
                output.UserName = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : "";
            }

            if (output.Deposit.BankId != null)
            {
                var _lookupBank = await _lookupBankRepository.FirstOrDefaultAsync((int) output.Deposit.BankId);
                output.BankBankName = _lookupBank?.ShortName?.ToString();
            }

            if (output.Deposit.ApproverId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.ApproverId);
                output.UserName2 = _lookupUser?.UserName?.ToString();
            }

            if (output.Deposit.UserSaleId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long) output.Deposit.UserSaleId);
                output.UserNameSale = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : "";
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepositDto input)
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

        public async Task CreateOrEditDebtSale(CreateOrEditDepositDto input)
        {
            if (input.Id == null)
            {
                await CreateDebtSale(input);
            }
            else
            {
                await UpdateDebtSale(input);
            }
        }

        public async Task CreateOrEditDepositCash(CreateOrEditDepositDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Deposits_Create)]
        protected virtual async Task Create(CreateOrEditDepositDto input)
        {
            try
            {
                if (input.Amount <= 0)
                    throw new UserFriendlyException("Số tiền không hợp lệ");

                if (input.Type == CommonConst.DepositType.Deposit)
                {
                    if (input.BankId == null)
                        throw new UserFriendlyException("Vui lòng chon ngân hàng");
                    if (input.Amount < 50000)
                        throw new UserFriendlyException("Số tiền nạp tối thiểu là 50.000đ");
                }
                else if (input.Type == CommonConst.DepositType.SaleDeposit)
                {
                    if (input.Amount < 0)
                        throw new UserFriendlyException("Số tiền nạp tối thiểu là 50.000đ");

                    if (input.UserSaleId <= 0 || input.UserSaleId == null)
                        throw new UserFriendlyException("Chưa có tài khoản nhân viên sale");
                }
                else if (input.Type == CommonConst.DepositType.Cash)
                {
                    if (input.Amount < 50000)
                        throw new UserFriendlyException("Số tiền nạp tối thiểu là 50.000đ");

                    if (input.UserId <= 0 || input.UserId == null)
                        throw new UserFriendlyException("Vui lòng chọn tài khoản nạp tiền");
                }

                var deposit = ObjectMapper.Map<Deposit>(input);
                deposit.RequestCode = await GeRequestCode();
                var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
                if (checkDes.IsAccountSystem())
                    throw new UserFriendlyException("Tài khoản nhận không hợp lệ");
                deposit.TransCode = await _commonManger.GetIncrementCodeAsync("D");
                await _depositRepository.InsertAsync(deposit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits_Edit)]
        protected virtual async Task Update(CreateOrEditDepositDto input)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync((int) input.Id);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (deposit.Status != CommonConst.DepositStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
            if (checkDes.IsAccountSystem())
                throw new UserFriendlyException("Tài khoản nhận không hợp lệ");
            if (input.Amount != deposit.Amount)
                deposit.Amount = input.Amount;
            if (input.BankId != null && input.BankId != deposit.BankId)
                deposit.BankId = input.BankId;
            if (input.Type != deposit.Type)
                deposit.Type = input.Type;
            if (input.UserId != deposit.UserId)
                deposit.UserId = input.UserId;
            if (input.Attachment != null)
                deposit.Attachment = input.Attachment;
            if (!string.IsNullOrEmpty(deposit.Description) && deposit.Description != input.Description)
                deposit.Description = input.Description;
            await _depositRepository.UpdateAsync(deposit);
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits_DebtSale)]
        protected virtual async Task CreateDebtSale(CreateOrEditDepositDto input)
        {
            try
            {
                if (input.Amount <= 0)
                    throw new UserFriendlyException("Số tiền không hợp lệ");

                if (input.Type == CommonConst.DepositType.Deposit)
                {
                    if (input.BankId == null)
                        throw new UserFriendlyException("Vui lòng chon ngân hàng");
                    if (input.Amount < 50000)
                        throw new UserFriendlyException("Số tiền nạp tối thiểu là 50.000đ");
                }
                else if (input.Type == CommonConst.DepositType.SaleDeposit)
                {
                    if (input.Amount < 0)
                        throw new UserFriendlyException("Số tiền nạp phải lớn hơn 0đ");

                    if (input.UserSaleId <= 0 || input.UserSaleId == null)
                        throw new UserFriendlyException("Chưa có tài khoản nhân viên sale");

                    var userSale = await UserManager.GetUserByIdAsync(input.UserSaleId ?? 0);
                    if (userSale == null)
                        throw new UserFriendlyException("Tài khoản sale không tồn tại.");
                    if (userSale.IsActive == false)
                        throw new UserFriendlyException("Tài khoản nhân viên sale đang khóa.");

                    DateTime date = DateTime.Now;
                    var saleLimit =
                        await _saleLimitDebtRepository.FirstOrDefaultAsync(c => c.UserId == (input.UserSaleId ?? 0));
                    if (saleLimit == null)
                        throw new UserFriendlyException("Nhân viên sale chưa có hạn mức công nợ.");
                    if (saleLimit.Status == CommonConst.DebtLimitAmountStatus.Lock)
                        throw new UserFriendlyException("Hạn mức công nợ của nhân viên sale đang khóa.");

                    if (saleLimit.Status == CommonConst.DebtLimitAmountStatus.Init)
                        throw new UserFriendlyException("Hạn mức công nợ của nhân viên sale chưa được duyệt.");

                    var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                    {
                        AccountCode = userSale.AccountCode,
                        CurrencyCode = "DEBT"
                    });
                    var balanceView = balance.Result;

                    if (saleLimit.LimitAmount - balanceView < input.Amount)
                    {
                        throw new UserFriendlyException("Nhân viên sale không đủ hạn mức.");
                    }
                }

                var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
                if (checkDes.IsAccountSystem())
                    throw new UserFriendlyException("Tài khoản nhận không hợp lệ");

                var deposit = ObjectMapper.Map<Deposit>(input);
                deposit.TransCode = await _commonManger.GetIncrementCodeAsync("D");
                await _depositRepository.InsertAsync(deposit);

                await Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"Begin send bot");
                        var sale = await UserManager.GetUserByIdAsync(input.UserSaleId ?? 0);
                        var message = L("Bot_Send_SaleDeposit", sale.UserName + "-" + sale.FullName,
                            checkDes.AccountCode + "-" + checkDes.FullName,
                            deposit.Amount.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                            deposit.Description,
                            "https://sandbox-topup.gmobile.vn/App/Deposits");
                        await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                        {
                            Message = message,
                            Module = "WEB",
                            Title = "Thông báo Đại lý nạp tiền vào tài khoản",
                            BotType = (byte) CommonConst.BotType.Deposit,
                            MessageType = (byte) CommonConst.BotMessageType.Message
                        });
                        _logger.LogInformation($"Done send bot:Message:{message}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Send bot deposit request eror:{e}");
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits_DebtSale)]
        protected virtual async Task UpdateDebtSale(CreateOrEditDepositDto input)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync((int) input.Id);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (deposit.Status != CommonConst.DepositStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
            if (checkDes.IsAccountSystem())
                throw new UserFriendlyException("Tài khoản nhận không hợp lệ");

            if (input.Amount < 0)
                throw new UserFriendlyException("Số tiền nạp phải lớn hơn 0đ");

            if (input.UserSaleId <= 0 || input.UserSaleId == null)
                throw new UserFriendlyException("Chưa có tài khoản nhân viên sale");

            var userSale = await UserManager.GetUserByIdAsync(input.UserSaleId ?? 0);
            if (userSale == null)
                throw new UserFriendlyException("Tài khoản sale không tồn tại.");
            if (userSale.IsActive == false)
                throw new UserFriendlyException("Tài khoản nhân viên sale đang khóa.");

            DateTime date = DateTime.Now;
            var saleLimit = await _saleLimitDebtRepository.SingleAsync(c => c.UserId == (input.UserSaleId ?? 0));
            if (saleLimit == null)
                throw new UserFriendlyException("Nhân viên sale chưa có hạn mức công nợ.");
            if (saleLimit.Status == CommonConst.DebtLimitAmountStatus.Lock)
                throw new UserFriendlyException("Hạn mức công nợ của nhân viên sale đang khóa.");

            if (saleLimit.Status == CommonConst.DebtLimitAmountStatus.Init)
                throw new UserFriendlyException("Hạn mức công nợ của nhân viên sale chưa được duyệt.");

            var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
            {
                AccountCode = userSale.AccountCode,
                CurrencyCode = "DEBT"
            });
            var balanceView = balance.Result;

            if (saleLimit.LimitAmount - balanceView < input.Amount)
            {
                throw new UserFriendlyException("Nhân viên sale không đủ hạn mức.");
            }

            if (input.Amount != deposit.Amount)
                deposit.Amount = input.Amount;
            if (input.BankId != null && input.BankId != deposit.BankId)
                deposit.BankId = input.BankId;
            if (input.Type != deposit.Type)
                deposit.Type = input.Type;
            if (input.UserId != deposit.UserId)
                deposit.UserId = input.UserId;
            if (!string.IsNullOrEmpty(deposit.Description) && deposit.Description != input.Description)
                deposit.Description = input.Description;

            await _depositRepository.UpdateAsync(deposit);
        }


        [AbpAuthorize(AppPermissions.Pages_Deposits_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _depositRepository.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_Deposits_Approval)]
        public async Task Approval(ApprovalDepositDto request)
        {
            // var checkTransCodeBank = await _depositRepository.FirstOrDefaultAsync(x => x.TransCodeBank == request.TransCodeBank);
            // var checkTransBank = _saleClearDebtRepository.FirstOrDefaultAsync(c => c.TransCodeBank == request.TransCodeBank);
            // if (checkTransBank != null)
            //     throw new UserFriendlyException("Mã giao dịch ngân hàng đã tồn tại!.");
            var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.TransCode == request.TransCode);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (deposit.BankId != null && string.IsNullOrEmpty(request.TransCodeBank))
                throw new UserFriendlyException("Vui lòng nhập mã giao dịch ngân hàng!");
            // if (checkTransCodeBank != null && request.TransCodeBank != null)
            //     throw new UserFriendlyException("Mã giao dịch ngân hàng đã tồn tại!");
            if (deposit.Status != CommonConst.DepositStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            var user = await UserManager.GetUserByIdAsync(deposit.UserId);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            deposit.TransCodeBank = request.TransCodeBank;
            deposit.ApprovedDate = DateTime.Now;
            deposit.ApproverId = AbpSession.UserId ?? 0;
            // deposit.Description = deposit.Type == CommonConst.DepositType.Deposit ? "Duyệt giao dịch nạp tiền" :
            //     deposit.Type == CommonConst.DepositType.Increase ? "Duyệt giao dịch điều chỉnh tăng" :
            //     deposit.Type == CommonConst.DepositType.SaleDeposit ? "Duyệt giao dịch sale nạp tiền cho đại lý." :
            //     deposit.Type == CommonConst.DepositType.Decrease ? "Duyệt giao dịch điều chỉnh giảm" : "";
            deposit.Status = CommonConst.DepositStatus.Processing;

            await _depositRepository.UpdateAsync(deposit);
            await CurrentUnitOfWork.SaveChangesAsync();

            TransactionResponse requestDeposit;
            string extraInfo = string.Empty;
            if (deposit.BankId > 0)
            {
                var bank = await _lookupBankRepository.FirstOrDefaultAsync(deposit.BankId ?? 0);
                if (bank != null)
                {
                    extraInfo = (new BankResponseDto()
                    {
                        BankName = bank?.BankName,
                        BranchName = bank?.BranchName,
                        BankAccountName = bank?.BankAccountName,
                        BankAccountCode = bank?.BankAccountCode,
                    }).ToJson();
                }
            }


            if (deposit.Type == CommonConst.DepositType.Deposit || deposit.Type == CommonConst.DepositType.Cash)
            {
                requestDeposit = await _transactionManager.DepositRequest(new DepositRequest
                {
                    AccountCode = user.AccountCode,
                    Amount = deposit.Amount,
                    CurrencyCode = "VND",
                    TransRef = deposit.TransCode,
                    Description = deposit.Description,
                    TransNote = deposit.Description,
                    ExtraInfo = extraInfo,
                });
                //deposit.ExtraInfo = requestDeposit.ToJson();
                deposit.ExtraInfo =$"{requestDeposit.ResponseCode}|{requestDeposit.ResponseMessage}";
                if (requestDeposit.ResponseCode == "01")
                {
                    deposit.Status = CommonConst.DepositStatus.Approved;
                    await Task.Run(async () =>
                    {
                        try
                        {
                            _logger.LogInformation($"Begin send notifi to: {user.AccountCode}");
                            var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                            {
                                AccountCode = user.AccountCode,
                                CurrencyCode = "VND"
                            });
                            var message = L("Notifi_Recive_Deposit", deposit.Amount.ToFormat("đ"),
                                balance.Result.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            await _appNotifier.PublishNotification(
                                user.AccountCode,
                                AppNotificationNames.Deposit,
                                new SendNotificationData
                                {
                                    TransCode = deposit.TransCode,
                                    Amount = deposit.Amount,
                                    PartnerCode = user.AccountCode,
                                    ServiceCode = CommonConst.ServiceCodes.DEPOSIT,
                                    TransType = CommonConst.TransactionType.Deposit.ToString("G")
                                },
                                message,
                                L("Notifi_Recive_Deposit_Title")
                            );
                            _logger.LogInformation($"Done notifi to: {user.AccountCode}-Message:{message}");
                            if (user.AgentType == CommonConst.AgentType.AgentApi)
                            {
                                var botMessage = L("Bot_Send_DepositToAgentApi", user.AccountCode + "-" + user.FullName,
                                    deposit.Amount.ToFormat("đ"), balance.Result.ToFormat("đ"),
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), deposit.Description,
                                    deposit.TransCode);
                                var userProfile = await UserManager.GetUserProfile(user.Id);
                                if (userProfile != null && !string.IsNullOrEmpty(userProfile.ChatId))
                                {
                                    await _appNotifier.PublishTeleToGroupMessage(new SendTeleMessageRequest
                                    {
                                        Message = botMessage,
                                        Module = "Balance",
                                        Title = "Thông báo Nạp tiền TK",
                                        BotType = (byte) CommonConst.BotType.Deposit,
                                        MessageType = (byte) CommonConst.BotMessageType.Message,
                                        ChatId = userProfile.ChatId
                                    });
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"SendNotifi deposit approval eror:{e}");
                        }
                    }).ConfigureAwait(false);
                }
            }
            else if (deposit.Type == CommonConst.DepositType.SaleDeposit)
            {
                var userSale = await UserManager.GetUserByIdAsync(deposit.UserSaleId ?? 0);
                if (userSale == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");

                DateTime date = DateTime.Now;
                var saleLimit = _saleLimitDebtRepository.GetAll()
                    .Where(c => c.UserId == (deposit.UserSaleId ?? 0)
                                && c.Status == CommonConst.DebtLimitAmountStatus.Active
                                && c.CreationTime <= date)
                    .OrderByDescending(c => c.CreationTime)
                    .FirstOrDefault();

                if (saleLimit == null)
                    throw new UserFriendlyException("Tài khoản chưa có hạn mức và tuổi nợ");

                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                {
                    AccountCode = userSale.AccountCode,
                    CurrencyCode = "DEBT"
                });
                var balanceView = balance.Result;

                if (saleLimit.LimitAmount - balanceView < deposit.Amount)
                {
                    throw new UserFriendlyException("Tài khoản không đủ hạn mức để nạp tiền cho đại lý.");
                }

                var debtHistory = await _clearDebtHistoryRepository.GetAll().Where(c => c.UserId == userSale.Id)
                    .FirstOrDefaultAsync();
                if (debtHistory != null)
                {
                    var checkDate = debtHistory.StartDate.AddDays(saleLimit.DebtAge).AddDays(-1);
                    var dateNow = DateTime.Now.Date;
                    if (balanceView > 0 && checkDate < dateNow)
                    {
                        throw new UserFriendlyException(
                            $"Tài khoản nhân viên còn nợ {balanceView.ToString("N0")}đ chưa được xóa hạn mức.");
                    }
                    else if (balanceView == 0) debtHistory.StartDate = dateNow;
                }

                requestDeposit = await _transactionManager.SaleDepositRequest(new SaleDepositRequest()
                {
                    AccountCode = user.AccountCode,
                    SaleCode = userSale.AccountCode,
                    TransNote = deposit.Description,
                    Amount = deposit.Amount,
                    TransRef = request.TransCode,
                });

                if (requestDeposit.ResponseCode == "01")
                {
                    deposit.Status = CommonConst.DepositStatus.Approved;
                    if (debtHistory != null)
                    {
                        debtHistory.Amount += deposit.Amount;
                        await _clearDebtHistoryRepository.UpdateAsync(debtHistory);
                    }
                    else
                    {
                        await _clearDebtHistoryRepository.InsertAsync(new SaleClearDebtHistory()
                        {
                            CreationTime = DateTime.Now,
                            StartDate = DateTime.Now.Date,
                            Amount = deposit.Amount,
                            UserId = userSale.Id,
                        });
                    }
                    
                    await Task.Run(async () =>
                    {
                        try
                        {
                            _logger.LogInformation($"Begin send notifi to: {user.AccountCode}");
                            var userBalance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                            {
                                AccountCode = user.AccountCode,
                                CurrencyCode = "VND"
                            });
                            
                            var message = L($"Notifi_SaleDeposit_Body", 
                                deposit.Amount.ToFormat("đ"),
                                saleLimit.UserFk.FullName,
                                userBalance.Result.ToFormat("đ"), 
                                deposit.Description,
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            
                            await _appNotifier.PublishNotification(
                                user.AccountCode,
                                AppNotificationNames.Deposit,
                                new SendNotificationData
                                {
                                    TransCode = deposit.TransCode,
                                    Amount = deposit.Amount,
                                    PartnerCode = user.AccountCode,
                                    TransType = CommonConst.TransactionType.Deposit.ToString("G")
                                },
                                message,
                                L("Notifi_SaleDeposit_Title")
                            );
                            _logger.LogInformation($"Done notifi to: {user.AccountCode}-Message:{message}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"SendNotifi deposit approval eror:{e}");
                        }
                    }).ConfigureAwait(false);
                }
            }
            else
            {
                //Call giao dịch điều chỉnh
                requestDeposit = await _transactionManager.AdjustmentRequest(new AdjustmentRequest
                {
                    AccountCode = user.AccountCode,
                    Amount = deposit.Amount,
                    CurrencyCode = "VND",
                    TransRef = deposit.TransCode,
                    AdjustmentType = deposit.Type == CommonConst.DepositType.Decrease
                        ? CommonConst.AdjustmentType.Decrease
                        : CommonConst.AdjustmentType.Increase,
                    TransNote = deposit.Description
                });
                //deposit.ExtraInfo = requestDeposit.ToJson();
                deposit.ExtraInfo = $"{requestDeposit.ResponseCode}|{requestDeposit.ResponseMessage}";
                if (requestDeposit.ResponseCode == "01")
                {
                    deposit.Status = CommonConst.DepositStatus.Approved;
                    await Task.Run(async () =>
                    {
                        try
                        {
                            _logger.LogInformation($"Begin send notifi to: {user.AccountCode}");
                            var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                            {
                                AccountCode = user.AccountCode,
                                CurrencyCode = "VND"
                            });
                            var key = deposit.Type == CommonConst.DepositType.Increase
                                ? "Notifi_Adjustment_Increases_Body"
                                : "Notifi_Adjustment_Decreases_Body";
                            var message = L($"{key}", deposit.Amount.ToFormat("đ"),
                                balance.Result.ToFormat("đ"), deposit.Description,
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            await _appNotifier.PublishNotification(
                                user.AccountCode,
                                AppNotificationNames.System,
                                new SendNotificationData
                                {
                                    TransCode = deposit.TransCode,
                                    Amount = deposit.Amount,
                                    PartnerCode = user.AccountCode,
                                    TransType = deposit.Type == CommonConst.DepositType.Increase
                                        ? CommonConst.TransactionType.AdjustmentIncrease.ToString("G")
                                        : CommonConst.TransactionType.AdjustmentDecrease.ToString("G")
                                },
                                message,
                                deposit.Type == CommonConst.DepositType.Increase
                                    ? L("Notifi_Adjustment_Increases_Title")
                                    : L("Notifi_Adjustment_Decreases_Title")
                            );
                            _logger.LogInformation($"Done notifi to: {user.AccountCode}-Message:{message}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"SendNotifi deposit approval eror:{e}");
                        }
                    }).ConfigureAwait(false);
                }
            }

            await _depositRepository.UpdateAsync(deposit);
            if (requestDeposit.ResponseCode != "01")
                throw new UserFriendlyException(requestDeposit.ResponseMessage);
        }


        [AbpAuthorize(AppPermissions.Pages_Deposits_Cancel)]
        public async Task Cancel(CancelDepositDto request)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.TransCode == request.TransCode);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");

            if (deposit.Status != CommonConst.DepositStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            deposit.Status = CommonConst.DepositStatus.Canceled;
            //deposit.Description = request.TransNote;
            deposit.ApprovedDate = DateTime.Now;
            deposit.ApproverId = AbpSession.UserId ?? 0;
            await _depositRepository.UpdateAsync(deposit);
        }

        public async Task<FileDto> GetDepositsToExcel(GetAllDepositsForExcelInput input)
        {
            var user = await _lookupUserRepository.GetAsync(AbpSession.UserId ?? 0);
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DepositStatus) input.StatusFilter
                : default;

            List<long> listUserByNvkd = new List<long>();
            if (input.SaleLeadFilter.HasValue)
            {
                var lsSale = await _lookupUserRepository.GetAll()
                    .Where(x => x.UserSaleLeadId == input.SaleLeadFilter.Value).ToListAsync();
                if (lsSale.Any())
                    listUserByNvkd = lsSale.Select(x => x.Id).ToList();
            }

            var filteredDeposits = _depositRepository.GetAll()
                .Include(e => e.UserFk)
                //.Include(e => e.UserSaleFk)
                .Include(e => e.BankFk)
                .Include(e => e.ApproverFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter) ||
                         e.ExtraInfo.Contains(input.Filter))
                .WhereIf(input.MinApprovedDateFilter != null,
                    e => e.CreationTime >= input.MinApprovedDateFilter || e.ApprovedDate >= input.MinApprovedDateFilter)
                .WhereIf(input.MaxApprovedDateFilter != null,
                    e => e.CreationTime <= input.MaxApprovedDateFilter || e.ApprovedDate <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Pending,
                    e => e.CreationTime >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Pending,
                    e => e.CreationTime <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Approved,
                    e => e.ApprovedDate >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Approved,
                    e => e.ApprovedDate <= input.MaxApprovedDateFilter)
                .WhereIf(
                    input.MinApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Canceled,
                    e => e.LastModificationTime >= input.MinApprovedDateFilter)
                .WhereIf(
                    input.MaxApprovedDateFilter != null &&
                    input.StatusFilter == (int) CommonConst.DepositStatus.Canceled,
                    e => e.LastModificationTime <= input.MaxApprovedDateFilter)
                .WhereIf(input.UserId != null, e => e.UserId == (long) input.UserId)
                .WhereIf(input.ApproverId != null, e => e.ApproverId == input.ApproverId)
                .WhereIf(input.BankId != null, e => e.BankId == input.BankId)
                .WhereIf(input.TransCodeFilter != null, e => e.TransCode.Contains(input.TransCodeFilter))
                .WhereIf(input.TransCodeBankFilter != null, e => e.TransCodeBank.Contains(input.TransCodeBankFilter))
                .WhereIf(input.SaleLeadFilter.HasValue && listUserByNvkd.Any(),
                    e => listUserByNvkd.Contains(e.UserSaleId.Value))
                .WhereIf(input.SaleManFilter != null, e => e.UserSaleId == input.SaleManFilter)
                .WhereIf(input.RequestCodeFilter != null, e => e.RequestCode.Contains(input.RequestCodeFilter))
                //.WhereIf(input.SaleLeadFilter != null, e => e.UserSaleFk.UserSaleLeadFk.Id == input.SaleLeadFilter)
                //.WhereIf(input.SaleManFilter != null, e => e.UserSaleFk != null && e.UserSaleFk.Id == input.SaleManFilter)
                .WhereIf(input.AgentTypeFilter.HasValue,
                    e => e.UserFk != null && e.UserFk.AgentType == input.AgentTypeFilter)
                .WhereIf(input.DepositTypeFilter != null, e => e.Type == input.DepositTypeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter),
                    e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter),
                    e => e.ApproverFk != null && e.ApproverFk.Name == input.UserName2Filter);

            if (user.AccountType == CommonConst.SystemAccountType.SaleLead)
            {
                filteredDeposits = from x in filteredDeposits
                    join y in _assignAgentRepository.GetAll() on x.UserId equals y.UserAgentId
                    join s in _lookupUserRepository.GetAll() on y.SaleUserId equals s.Id
                    where s.UserSaleLeadId == user.Id
                    select x;
            }
            else if (user.AccountType == CommonConst.SystemAccountType.Sale)
            {
                filteredDeposits = from x in filteredDeposits
                    join y in _assignAgentRepository.GetAll() on x.UserId equals y.UserAgentId
                    where y.SaleUserId == user.Id
                    select x;
            }
            else if (user.AccountType != CommonConst.SystemAccountType.System)
            {
                filteredDeposits = filteredDeposits.Where(c => c.UserId == user.Id);
            }

            filteredDeposits.OrderBy(x => x.CreationTime);

            var deposits = from o in filteredDeposits
                join o1 in _lookupUserRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookupBankRepository.GetAll() on o.BankId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookupUserRepository.GetAll() on o.ApproverId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _assignAgentRepository.GetAll() on s1.Id equals o4.UserAgentId into j4
                from assign in j4.DefaultIfEmpty()
                join sm in _lookupUserRepository.GetAll() on assign.SaleUserId equals sm.Id into smg
                from sale in smg.DefaultIfEmpty()
                join lm in _lookupUserRepository.GetAll() on sale.UserSaleLeadId equals lm.Id into lmg
                from leader in lmg.DefaultIfEmpty()
                select new GetDepositForViewDto()
                {
                    Deposit = new DepositDto
                    {
                        Status = o.Status,
                        Amount = o.Amount,
                        ApprovedDate = o.ApprovedDate,
                        TransCode = o.TransCode,
                        Id = o.Id,
                        Description = o.Description,
                        Type = o.Type,
                        RecipientInfo = o.RecipientInfo,
                        CreationTime = o.CreationTime,
                        TransCodeBank = o.TransCodeBank,
                        RequestCode = o.RequestCode
                    },
                    UserName = s1 == null || s1.Name == null
                        ? ""
                        : s1.AccountCode + " - " + s1.PhoneNumber + " - " + s1.FullName,
                    BankId = s2.Id,
                    BankBankName = s2 == null || s2.ShortName == null ? "" : s2.ShortName,
                    UserName2 = s3 == null ? "" : s3.UserName,
                    AgentType = s1.AgentType,
                    AgentName = s1.AccountCode + " - " + s1.UserName + " - " + s1.FullName,
                    SaleMan = sale != null ? sale.UserName + " - " + sale.PhoneNumber + " - " + sale.FullName : "",
                    SaleLeader = leader != null
                        ? leader.UserName + " - " + leader.PhoneNumber + " - " + leader.FullName
                        : "",
                };

            var depositListDtos = await deposits.ToListAsync();

            return _depositsExcelExporter.ExportToFile(depositListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Deposits)]
        public async Task<PagedResultDto<DepositUserLookupTableDto>> GetAllUserForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookupUserRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name.Contains(input.Filter) || e.AccountCode == input.Filter || e.UserName == input.Filter ||
                     e.PhoneNumber == input.Filter
            );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = userList
                .Where(x => x.AccountType != CommonConst.SystemAccountType.System &&
                            x.AccountType != CommonConst.SystemAccountType.Staff
                            && x.AccountType != CommonConst.SystemAccountType.StaffApi).Select(user =>
                    new DepositUserLookupTableDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        DisplayName = user.AccountCode + "-" + user.PhoneNumber + "-" + user.FullName.ToString()
                    }).ToList();

            return new PagedResultDto<DepositUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<PagedResultDto<DepositUserLookupTableDto>> GetAllSaleForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookupUserRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name.Contains(input.Filter) || e.AccountCode.Contains(input.Filter) ||
                     e.UserName.Contains(input.Filter) ||
                     e.PhoneNumber.Contains(input.Filter)
            );

            query = query.Where(c =>
                c.AccountType == CommonConst.SystemAccountType.Sale ||
                c.AccountType == CommonConst.SystemAccountType.SaleLead);
            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = userList.Select(user =>
                new DepositUserLookupTableDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    DisplayName = user.AccountCode + "-" + user.PhoneNumber + "-" + user.FullName.ToString()
                }).ToList();

            return new PagedResultDto<DepositUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_Deposits)]
        public async Task<List<DepositBankLookupTableDto>> GetAllBankForTableDropdown()
        {
            return await _lookupBankRepository.GetAll()
                .Select(bank => new DepositBankLookupTableDto
                {
                    Id = bank.Id,
                    DisplayName = bank == null || bank.ShortName == null ? "" : bank.ShortName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Deposits)]
        public async Task<List<DepositUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookupUserRepository.GetAll()
                .Select(user => new DepositUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        public async Task<ResponseMessages> DepositRequest(DepositRequestDto input)
        {
            var bank = await _lookupBankRepository.FirstOrDefaultAsync(x => x.Id == input.BankId);
            if (bank == null)
                throw new UserFriendlyException("Ngân hàng không hợp lệ");
            var deposit = input.ConvertTo<Deposit>();
            deposit.UserId = AbpSession.UserId ?? 0;
            if (_appSession.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                var accountInfo = GetAccountInfo();
                if (accountInfo == null || accountInfo.NetworkInfo == null)
                    throw new UserFriendlyException("Tài khoản nhận không hợp lệ");
                deposit.UserId = accountInfo.NetworkInfo.Id;
            }

            var checkDes = await UserManager.GetUserByIdAsync(deposit.UserId);
            if (checkDes.IsAccountSystem())
                throw new UserFriendlyException("Tài khoản nhận không hợp lệ");

            var linkFullName = string.Empty;
            var assignAgent = await _assignAgentRepository.FirstOrDefaultAsync(c => c.UserAgentId == deposit.UserId);
            if (assignAgent != null)
            {
                var checkSale = await _lookupUserRepository.GetAsync(assignAgent.SaleUserId);
                linkFullName = $"{checkSale.UserName} - {checkSale.PhoneNumber} - {checkSale.FullName}";
            }

            // if (input.RequestCode == null)
            // {
            //     var randRequestCode =  await GetRandomRequestCode();
            //     deposit.RequestCode = randRequestCode.Payload.ToString();
            // }

            deposit.RequestCode = await GeRequestCode();
            deposit.TransCode = await _commonManger.GetIncrementCodeAsync("D");
            deposit.Type = CommonConst.DepositType.Deposit;
            await _depositRepository.InsertAsync(deposit);
            await Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"Begin send bot");
                    string message = "";
                    if (string.IsNullOrEmpty(linkFullName))
                        message = L("Bot_Send_RequestDeposit", checkDes.AccountCode + "-" + checkDes.FullName,
                            bank.BankName + "-" + bank.BankAccountCode,
                            deposit.Amount.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                            //deposit.Description,
                            $"Mã nạp: {deposit.RequestCode}",
                            "https://sandbox-topup.gmobile.vn/App/Deposits");

                    else
                        message = L("Bot_Send_RequestDepositInSale", checkDes.AccountCode + "-" + checkDes.FullName,
                            bank.BankName + "-" + bank.BankAccountCode,
                            linkFullName,
                            deposit.Amount.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                            //deposit.Description,
                            $"Mã nạp: {deposit.RequestCode}",
                            "https://sandbox-topup.gmobile.vn/App/Deposits");

                    await _appNotifier.PublishTeleMessage(new SendTeleMessageRequest
                    {
                        Message = message,
                        Module = "WEB",
                        Title = "Thông báo Đại lý nạp tiền vào tài khoản",
                        BotType = (byte) CommonConst.BotType.Deposit,
                        MessageType = (byte) CommonConst.BotMessageType.Message,
                    });
                    _logger.LogInformation($"Done send bot:Message:{message}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Send bot deposit request eror:{e}");
                }
            }).ConfigureAwait(false);

            return new ResponseMessages
            {
                Payload = deposit.TransCode,
                ResponseCode = "01",
                ExtraInfo = _commonManger.EncryptQueryParameters(
                    $"/Transactions/TransactionInfo?code=01&transCode={deposit.TransCode}&message={L("Message_RequestDeposit_Success")}&transType={CommonConst.TransactionType.Deposit}"),
                ResponseMessage = deposit.RequestCode + " - Nap tien - " + checkDes.AccountCode,
            };
        }

        public async Task<List<DepositRequestItemDto>> GetDepositRequest(GetTopRequestDeposit input)
        {
            var items = _depositRepository.GetAllIncluding(x => x.BankFk).OrderByDescending(x => x.CreationTime)
                .Where(x => x.UserId == AbpSession.UserId && x.Type == CommonConst.DepositType.Deposit)
                .Take(input.Total);
            return await items.Select(x => new DepositRequestItemDto
            {
                Amount = x.Amount,
                Description = x.Description,
                Status = x.Status,
                BankName = x.BankFk.BankName,
                TransCode = x.TransCode,
                RequestCode = x.RequestCode,
                CreatetionTime = x.CreationTime
            }).ToListAsync();
        }

        public async Task<DepositDto> GetDeposit(string transcode, long? userId = null)
        {
            var query = _depositRepository.GetAll().Where(x => x.TransCode == transcode)
                .WhereIf(userId != null, x => x.CreatorUserId == userId);
            var item = await query.FirstOrDefaultAsync();
            return item.ConvertTo<DepositDto>();
        }

        public async Task<decimal> GetLimitAvailability(long userId)
        {
            try
            {
                var user = await UserManager.GetUserByIdAsync(userId);
                if (user == null)
                    return 0;

                DateTime date = DateTime.Now;
                var saleLimit = _saleLimitDebtRepository.GetAll()
                    .Where(c => c.UserId == userId
                                && c.Status == CommonConst.DebtLimitAmountStatus.Active
                                && c.CreationTime <= date)
                    .OrderByDescending(c => c.CreationTime)
                    .FirstOrDefault();

                if (saleLimit == null) return 0;

                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                {
                    AccountCode = user.AccountCode,
                    CurrencyCode = "DEBT"
                });
                var balanceView = balance.Result;


                return (saleLimit != null ? saleLimit.LimitAmount : 0) - balanceView;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{userId} GetLimitAvailability eror:{ex}");
                return 0;
            }
        }

        public async Task<GetDepositForEditOutput> CheckTranscodeBank(string transCode)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.TransCodeBank == transCode);
            var output = new GetDepositForEditOutput {Deposit = ObjectMapper.Map<CreateOrEditDepositDto>(deposit)};

            return output;
        }

        public async Task<ResponseMessages> GetRandomRequestCode()
        {
            var rand = new Random();
            var randRequestCode = "NP" + rand.Next(000000, 999999).ToString("000000");

            var rs = new ResponseMessages("01");
            rs.Payload = randRequestCode;

            return rs;
        }

        private async Task<string> GeRequestCode()
        {
            var rand = new Random();
            var randRequestCode = "NP" + rand.Next(000000, 999999).ToString("000000");
            var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.RequestCode == randRequestCode);
            if (deposit == null) return randRequestCode;
            return await GeRequestCode();
        }
    }
}