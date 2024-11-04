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
using HLS.Topup.Sale.Exporting;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using HLS.Topup.Transactions;
using HLS.Topup.RequestDtos;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.Sale
{
    [AbpAuthorize(AppPermissions.Pages_SaleClearDebts)]
    public class SaleClearDebtsAppService : TopupAppServiceBase, ISaleClearDebtsAppService
    {
        private readonly IRepository<SaleClearDebt> _saleClearDebtRepository;
        private readonly ISaleClearDebtsExcelExporter _saleClearDebtsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Bank, int> _lookup_bankRepository;
        private readonly IRepository<SaleClearDebtHistory, int> _clearDebtHistoryRepository;
        private readonly ICommonManger _commonManger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<Deposits.Deposit> _depositRepository;
        private readonly ILogger<SaleClearDebtsAppService> _logger;

        public SaleClearDebtsAppService(IRepository<SaleClearDebt> saleClearDebtRepository,
            ISaleClearDebtsExcelExporter saleClearDebtsExcelExporter,
            IRepository<User, long> lookup_userRepository, IRepository<Bank, int> lookup_bankRepository,
            IRepository<SaleClearDebtHistory, int> clearDebtHistoryRepository,
            ICommonManger commonManger, ITransactionManager transactionManager,
            IRepository<Deposits.Deposit> depositRepository,
            ILogger<SaleClearDebtsAppService> logger)
        {
            _saleClearDebtRepository = saleClearDebtRepository;
            _saleClearDebtsExcelExporter = saleClearDebtsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_bankRepository = lookup_bankRepository;
            _commonManger = commonManger;
            _clearDebtHistoryRepository = clearDebtHistoryRepository;
            _transactionManager = transactionManager;
            _depositRepository = depositRepository;
            _logger = logger;
        }

        public async Task<PagedResultDtoReport<GetSaleClearDebtForViewDto>> GetAll(GetAllSaleClearDebtsInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                ? (CommonConst.ClearDebtType)input.TypeFilter
                : default;

            var status = input.StatusFilter.HasValue
                ? (CommonConst.ClearDebtStatus)input.StatusFilter
                : default;

            if (input.ToDateFilter != null)
            {
                input.ToDateFilter = input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1);
            }

            var filteredSaleClearDebts = _saleClearDebtRepository.GetAll()
                .Include(e => e.UserFk)
                .Include(e => e.BankFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Descriptions.Contains(input.Filter))
                .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == status)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeFilter), e => e.TransCode == input.TransCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeBank), e => e.TransCodeBank == input.TransCodeBank)

                //.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter)
                //.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter)
                .WhereIf(input.UserId > 0, e => e.UserId == input.UserId)
                .WhereIf(input.BankId > 0, e => e.BankId == input.BankId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter),
                    e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter);

            if (status == CommonConst.ClearDebtStatus.Init)
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter);
            }
            else if (status == CommonConst.ClearDebtStatus.Approval
                || status == CommonConst.ClearDebtStatus.Cancel)
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.LastModificationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.LastModificationTime <= input.ToDateFilter);
            }
            else
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter || e.LastModificationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter || e.LastModificationTime <= input.ToDateFilter);

            }

            var pagedAndFilteredSaleClearDebts = filteredSaleClearDebts
                .OrderByDescending(c => c.CreationTime)
                .PageBy(input);

            var saleClearDebts = from o in pagedAndFilteredSaleClearDebts
                                 join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()
                                 join o2 in _lookup_bankRepository.GetAll() on o.BankId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()
                                 join up in _lookup_userRepository.GetAll() on o.LastModifierUserId equals up.Id into upj
                                 from m in upj.DefaultIfEmpty()
                                 join c in _lookup_userRepository.GetAll() on o.CreatorUserId equals c.Id into cg
                                 from cu in cg.DefaultIfEmpty()
                                 select new GetSaleClearDebtForViewDto()
                                 {
                                     SaleClearDebt = new SaleClearDebtDto
                                     {
                                         Id = o.Id,
                                         TransCode = o.TransCode,
                                         Status = o.Status,
                                         Amount = o.Amount,
                                         Type = o.Type,
                                         SaleInfo = s1 != null
                                             ? s1.UserName + " - " + s1.PhoneNumber + " - " + s1.FullName
                                             : string.Empty,
                                         Descriptions = o.Descriptions,
                                         TransCodeBank = o.TransCodeBank,
                                         UserModify = m != null ? m.UserName : "",
                                         ModifyDate = o.LastModificationTime,
                                         CreationTime = o.CreationTime,
                                         UserCreated = cu != null ? cu.UserName : "",
                                     },
                                     UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     BankBankName = s2 == null || s2.ShortName == null ? "" : s2.ShortName.ToString()
                                 };

            var totalCount = await filteredSaleClearDebts.CountAsync();
            var totalAmount = await filteredSaleClearDebts.SumAsync(c => c.Amount);
            var sumData = new GetSaleClearDebtForViewDto()
                 {
                     SaleClearDebt = new SaleClearDebtDto()
                     {
                         Amount = totalAmount,
                     }
                 };

            return new PagedResultDtoReport<GetSaleClearDebtForViewDto>(
                totalCount,
                totalData: sumData,
                await saleClearDebts.ToListAsync()
            );
        }

        public async Task<GetSaleClearDebtForViewDto> GetSaleClearDebtForView(int id)
        {
            var saleClearDebt = await _saleClearDebtRepository.GetAsync(id);

            var output = new GetSaleClearDebtForViewDto
            { SaleClearDebt = ObjectMapper.Map<SaleClearDebtDto>(saleClearDebt) };
            output.SaleClearDebt.TransCode = saleClearDebt.TransCode;
            if (output.SaleClearDebt.UserId > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SaleClearDebt.UserId);
                output.SaleClearDebt.SaleInfo = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : string.Empty;
            }

            if (output.SaleClearDebt.BankId > 0)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.SaleClearDebt.BankId);
                output.BankBankName = _lookupBank?.ShortName?.ToString();
            }

            if (saleClearDebt.LastModifierUserId > 0)
            {
                var modifierUser = await _lookup_userRepository.GetAsync(saleClearDebt.LastModifierUserId.Value);
                output.SaleClearDebt.UserModify = modifierUser.UserName;
                output.SaleClearDebt.ModifyDate = saleClearDebt.LastModificationTime;
            }

            output.StatusName = L("Enum_ClearDebtStatus_" + ((int)output.SaleClearDebt.Status).ToString());
            ;

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts_Edit)]
        public async Task<GetSaleClearDebtForEditOutput> GetSaleClearDebtForEdit(EntityDto input)
        {
            var saleClearDebt = await _saleClearDebtRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSaleClearDebtForEditOutput
            { SaleClearDebt = ObjectMapper.Map<CreateOrEditSaleClearDebtDto>(saleClearDebt) };

            if (output.SaleClearDebt.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SaleClearDebt.UserId);
                output.UserName = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : string.Empty;
            }

            if (output.SaleClearDebt.BankId != null)
            {
                var _lookupBank = await _lookup_bankRepository.FirstOrDefaultAsync((int)output.SaleClearDebt.BankId);
                output.BankBankName = _lookupBank?.ShortName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSaleClearDebtDto input)
        {
            if (input.Amount <= 0)
                throw new UserFriendlyException("Số tiền phải lớn hơn 0.");
            if (input.UserId <= 0)
                throw new UserFriendlyException("Quý khách chưa chọn tài khoản sale.");

            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");

            if (input.Type == CommonConst.ClearDebtType.CashInBank)
            {
                if (string.IsNullOrEmpty(input.TransCodeBank))
                    throw new UserFriendlyException("Quý khách chưa nhập mã ngân hàng.");

                var checkTransBank = await _saleClearDebtRepository.FirstOrDefaultAsync(c => c.TransCodeBank == input.TransCodeBank);
                if (checkTransBank != null)
                    throw new UserFriendlyException("Mã giao dịch ngân hàng đã tồn tại. Quý khách vui lòng kiểm tra lại.");

                var checkTransCodeBank = await _depositRepository.FirstOrDefaultAsync(x => x.TransCodeBank == input.TransCodeBank);
                if (checkTransCodeBank != null)
                    throw new UserFriendlyException("Mã giao dịch ngân hàng đã tồn tại. Quý khách vui lòng kiểm tra lại.");
            }

            var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest()
            {
                AccountCode = user.AccountCode,
                CurrencyCode = "DEBT"
            });

            var balanceDebt = balance.Result;

            if (input.Amount > balanceDebt)
            {
                throw new UserFriendlyException($"Số tiền thanh toán {input.Amount.ToString("N0")} lớn hơn công nợ {balanceDebt.ToString("N0")}.");
            }

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts_Create)]
        protected virtual async Task Create(CreateOrEditSaleClearDebtDto input)
        {
            var saleClearDebt = ObjectMapper.Map<SaleClearDebt>(input);


            if (AbpSession.TenantId != null)
            {
                saleClearDebt.TenantId = (int?)AbpSession.TenantId;
            }

            var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
            if (checkDes.AccountType != CommonConst.SystemAccountType.Sale)
                throw new UserFriendlyException("Tài khoản nhận không hợp lệ");
            saleClearDebt.TransCode = await _commonManger.GetIncrementCodeAsync("D");
            await _saleClearDebtRepository.InsertAsync(saleClearDebt);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts_Edit)]
        protected virtual async Task Update(CreateOrEditSaleClearDebtDto input)
        {
            var saleClearDebt = await _saleClearDebtRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, saleClearDebt);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _saleClearDebtRepository.DeleteAsync(input.Id);
        }


        public async Task Cancel(string transcode)
        {
            var deposit = await _saleClearDebtRepository.FirstOrDefaultAsync(x => x.TransCode == transcode);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");

            if (deposit.Status != CommonConst.ClearDebtStatus.Init)
                throw new UserFriendlyException("Trạng thái không hợp lệ");

            deposit.Status = CommonConst.ClearDebtStatus.Cancel;
            await _saleClearDebtRepository.UpdateAsync(deposit);
        }

        //[AbpAuthorize(AppPermissions.Pages_SaleClearDebts_Approval)]
        public async Task Approval(string transcode, string note)
        {
            var deposit = await _saleClearDebtRepository.FirstOrDefaultAsync(x => x.TransCode == transcode);
            if (deposit == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");

            if (deposit.Status != CommonConst.ClearDebtStatus.Init)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            var user = await UserManager.GetUserByIdAsync(deposit.UserId);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");

            var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest()
            {
                AccountCode = user.AccountCode,
                CurrencyCode = "DEBT"
            });

            var balanceDebt = balance.Result;

            if (deposit.Amount > balanceDebt)
            {
                throw new UserFriendlyException($"Số tiền thanh toán {deposit.Amount.ToString("N0")} lớn hơn công nợ {balanceDebt.ToString("N0")}.");
            }

            deposit.LastModificationTime = DateTime.Now;
            deposit.LastModifierUserId = AbpSession.UserId ?? 0;
            deposit.ApprovalNote = note;
            TransactionResponse requestDeposit;

            requestDeposit = await _transactionManager.ClearDebtRequest(new ClearDebtRequest
            {
                AccountCode = user.AccountCode,
                Amount = deposit.Amount,
                TransRef = transcode,
                TransNote = note
            });

            if (requestDeposit.ResponseCode == "01")
            {
                deposit.LastModificationTime = DateTime.Now;
                deposit.LastModifierUserId = AbpSession.UserId ?? 0;
                deposit.Status = CommonConst.ClearDebtStatus.Approval;
                deposit.Descriptions = note;
                if (balanceDebt == deposit.Amount)
                {
                    var debtHistory = await _clearDebtHistoryRepository.GetAll().Where(c => c.UserId == deposit.UserId)
                        .FirstOrDefaultAsync();
                    if (debtHistory == null)
                    {
                        await _clearDebtHistoryRepository.InsertAsync(new SaleClearDebtHistory()
                        {
                            CreationTime = DateTime.Now,
                            StartDate = DateTime.Now.Date,
                            Amount = 0,
                            UserId = deposit.UserId,
                        });
                    }
                    else
                    {
                        debtHistory.StartDate = DateTime.Now.Date;
                        debtHistory.Amount = 0;
                        await _clearDebtHistoryRepository.UpdateAsync(debtHistory);
                    }
                }

                await _saleClearDebtRepository.UpdateAsync(deposit);
            }

            if (requestDeposit.ResponseCode != "01")
                throw new UserFriendlyException(requestDeposit.ResponseMessage);
        }

        public async Task<FileDto> GetSaleClearDebtsToExcel(GetAllSaleClearDebtsForExcelInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                  ? (CommonConst.ClearDebtType)input.TypeFilter
                  : default;

            var status = input.StatusFilter.HasValue
                ? (CommonConst.ClearDebtStatus)input.StatusFilter
                : default;

            if (input.ToDateFilter != null)
            {
                input.ToDateFilter = input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1);
            }

            var filteredSaleClearDebts = _saleClearDebtRepository.GetAll()
                .Include(e => e.UserFk)
                .Include(e => e.BankFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Descriptions.Contains(input.Filter))
                .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == status)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeFilter), e => e.TransCode == input.TransCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeBank), e => e.TransCodeBank == input.TransCodeBank)

                //.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter)
                //.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter)
                .WhereIf(input.UserId > 0, e => e.UserId == input.UserId)
                .WhereIf(input.BankId > 0, e => e.BankId == input.BankId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankBankNameFilter),
                    e => e.BankFk != null && e.BankFk.BankName == input.BankBankNameFilter);

            if (status == CommonConst.ClearDebtStatus.Init)
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter);
            }
            else if (status == CommonConst.ClearDebtStatus.Approval
                || status == CommonConst.ClearDebtStatus.Cancel)
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.LastModificationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.LastModificationTime <= input.ToDateFilter);
            }
            else
            {
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter || e.LastModificationTime >= input.FromDateFilter);
                filteredSaleClearDebts = filteredSaleClearDebts.WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter || e.LastModificationTime <= input.ToDateFilter);

            }

            var saleClearDebtListDtos = await filteredSaleClearDebts.ToListAsync();
            var saleClearDebts = (from o in saleClearDebtListDtos
                                 join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()
                                 join o2 in _lookup_bankRepository.GetAll() on o.BankId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()
                                 join up in _lookup_userRepository.GetAll() on o.LastModifierUserId equals up.Id into upj
                                 from m in upj.DefaultIfEmpty()
                                 join c in _lookup_userRepository.GetAll() on o.CreatorUserId equals c.Id into cg
                                 from cu in cg.DefaultIfEmpty()
                                 select new GetSaleClearDebtForViewDto()
                                 {
                                     SaleClearDebt = new SaleClearDebtDto
                                     {
                                         Id = o.Id,
                                         TransCode = o.TransCode,
                                         Status = o.Status,
                                         Amount = o.Amount,
                                         Type = o.Type,
                                         SaleInfo = s1 != null
                                             ? s1.UserName + " - " + s1.PhoneNumber + " - " + s1.FullName
                                             : string.Empty,
                                         Descriptions = o.Descriptions,
                                         TransCodeBank = o.TransCodeBank,
                                         UserModify = m != null ? m.UserName : "",
                                         ModifyDate = o.LastModificationTime,
                                         CreationTime = o.CreationTime,
                                         UserCreated = cu != null ? cu.UserName : "",
                                     },
                                     UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     BankBankName = s2 == null || s2.ShortName == null ? "" : s2.ShortName.ToString()
                                 }).ToList();


            return _saleClearDebtsExcelExporter.ExportToFile(saleClearDebts);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts)]
        public async Task<PagedResultDto<SaleClearDebtUserLookupTableDto>> GetAllUserForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name.Contains(input.Filter)
                     || e.AccountCode.Contains(input.Filter)
                     || e.UserName.Contains(input.Filter)
                     || e.PhoneNumber.Contains(input.Filter)
            );

            query = query.Where(c =>
                c.AccountType == CommonConst.SystemAccountType.Sale);
            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SaleClearDebtUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new SaleClearDebtUserLookupTableDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    DisplayName = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName,
                });
            }

            return new PagedResultDto<SaleClearDebtUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_SaleClearDebts)]
        public async Task<List<SaleClearDebtBankLookupTableDto>> GetAllBankForTableDropdown()
        {
            return await _lookup_bankRepository.GetAll()
                .Select(bank => new SaleClearDebtBankLookupTableDto
                {
                    Id = bank.Id,
                    DisplayName = bank == null || bank.ShortName == null ? "" : bank.ShortName.ToString()
                }).ToListAsync();
        }
    }
}
