using System;
using HLS.Topup.Authorization.Users;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.BalanceManager.Exporting;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.BalanceManager
{
    [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances)]
    public class AccountBlockBalancesAppService : TopupAppServiceBase, IAccountBlockBalancesAppService
    {
        private readonly IRepository<AccountBlockBalance> _accountBlockBalanceRepository;
        private readonly IRepository<AccountBlockBalanceDetail> _accountBlockBalanceDetailRepository;
        private readonly IAccountBlockBalancesExcelExporter _accountBlockBalancesExcelExporter;
        private readonly IRepository<User, long> _lookupUserRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly ICommonManger _commonManger;

        public AccountBlockBalancesAppService(IRepository<AccountBlockBalance> accountBlockBalanceRepository,
            IAccountBlockBalancesExcelExporter accountBlockBalancesExcelExporter,
            IRepository<User, long> lookupUserRepository,
            IRepository<AccountBlockBalanceDetail> accountBlockBalanceDetailRepository,
            ITransactionManager transactionManager, ICommonManger commonManger)
        {
            _accountBlockBalanceRepository = accountBlockBalanceRepository;
            _accountBlockBalancesExcelExporter = accountBlockBalancesExcelExporter;
            _lookupUserRepository = lookupUserRepository;
            _accountBlockBalanceDetailRepository = accountBlockBalanceDetailRepository;
            _transactionManager = transactionManager;
            _commonManger = commonManger;
        }

        public async Task<PagedResultDto<GetAccountBlockBalanceForViewDto>> GetAll(
            GetAllAccountBlockBalancesInput input)
        {
            var filteredAccountBlockBalances = _accountBlockBalanceRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter) ||
                         e.UserFk.AccountCode == input.Filter || e.UserFk.UserName == input.Filter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeFilter), e => e.TransCode == input.TransCodeFilter)
                .WhereIf(input.FromCreatedTimeFilter!=null && input.FromCreatedTimeFilter!=DateTime.MinValue, e => e.CreationTime >=input.FromCreatedTimeFilter)
                .WhereIf(input.ToCreatedTimeFilter!=null && input.ToCreatedTimeFilter!=DateTime.MinValue, e => e.CreationTime <=input.ToCreatedTimeFilter)
                .WhereIf(input.UserIdFilter != null && input.UserIdFilter > 0,
                    e => e.UserId == input.UserIdFilter);

            var pagedAndFilteredAccountBlockBalances = filteredAccountBlockBalances
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var accountBlockBalances = from o in pagedAndFilteredAccountBlockBalances
                join s1 in _lookupUserRepository.GetAll() on o.UserId equals s1.Id
                join s2 in _lookupUserRepository.GetAll() on o.CreatorUserId equals s2.Id
                select new GetAccountBlockBalanceForViewDto()
                {
                    AccountBlockBalance = new AccountBlockBalanceDto
                    {
                        TransCode = o.TransCode,
                        BlockedMoney = o.BlockedMoney,
                        Description = o.Description,
                        Id = o.Id,
                        LastModificationTime = o.CreationTime
                    },
                    FullAgentName = s1.AccountCode + "-" + s1.PhoneNumber + "-" + s1.FullName,
                    AgentType = s1.AgentType,
                    UserName = s2.UserName + "-" + s2.FullName
                };

            var totalCount = await filteredAccountBlockBalances.CountAsync();

            return new PagedResultDto<GetAccountBlockBalanceForViewDto>(
                totalCount,
                await accountBlockBalances.ToListAsync()
            );
        }

        public async Task<PagedResultDto<AccountBlockBalanceDetailDto>> GetListDetail(
            GetAllAccountBlockBalancesDetailInput input)
        {
            var filteredAccountBlockBalances = _accountBlockBalanceDetailRepository
                .GetAllIncluding(x => x.AccountBlockBalanceFk).Where(x => x.AccountBlockBalanceId == input.Id);

            var pagedAndFilteredAccountBlockBalances = filteredAccountBlockBalances
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var accountBlockBalances = from o in pagedAndFilteredAccountBlockBalances
                join s2 in _lookupUserRepository.GetAll() on o.CreatorUserId equals s2.Id
                select new AccountBlockBalanceDetailDto()
                {
                    UserName = s2.UserName + "-" + s2.FullName,
                    Amount = o.Amount,
                    Attachments = o.Attachments,
                    Description = o.Description,
                    Success = o.Success,
                    Type = o.Type,
                    CreationTime = o.CreationTime,
                    TransNote = o.TransNote,
                    TransRef = o.TransRef,
                    AccountBlockBalanceId = o.AccountBlockBalanceId
                };

            var totalCount = await filteredAccountBlockBalances.CountAsync();

            return new PagedResultDto<AccountBlockBalanceDetailDto>(
                totalCount,
                await accountBlockBalances.ToListAsync()
            );
        }

        public async Task<GetAccountBlockBalanceForViewDto> GetAccountBlockBalanceForView(int id)
        {
            var accountBlockBalance = await _accountBlockBalanceRepository.GetAsync(id);

            var output = new GetAccountBlockBalanceForViewDto
                {AccountBlockBalance = ObjectMapper.Map<AccountBlockBalanceDto>(accountBlockBalance)};

            {
                var lookupUser =
                    await _lookupUserRepository.FirstOrDefaultAsync(output.AccountBlockBalance.UserId);
                output.FullAgentName =
                    lookupUser.AccountCode + "-" + lookupUser.PhoneNumber + "-" + lookupUser.FullName;
                output.AgentType = lookupUser.AgentType;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances_Edit)]
        public async Task<GetAccountBlockBalanceForEditOutput> GetAccountBlockBalanceForEdit(EntityDto input)
        {
            var accountBlockBalance = await _accountBlockBalanceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAccountBlockBalanceForEditOutput
                {AccountBlockBalance = ObjectMapper.Map<CreateOrEditAccountBlockBalanceDto>(accountBlockBalance)};

            if (output.AccountBlockBalance.UserId != null)
            {
                var _lookupUser =
                    await _lookupUserRepository.FirstOrDefaultAsync((long) output.AccountBlockBalance.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAccountBlockBalanceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances_Create)]
        protected virtual async Task Create(CreateOrEditAccountBlockBalanceDto input)
        {
            try
            {
                if (input.Amount <= 0)
                    throw new UserFriendlyException("Số tiền không hợp lệ");

                var checkDes = await UserManager.GetUserByIdAsync(input.UserId);
                if (checkDes.IsAccountSystem())
                    throw new UserFriendlyException("Tài khoản không hợp lệ");
                var accountBlockBalance = ObjectMapper.Map<AccountBlockBalance>(input);


                if (AbpSession.TenantId != null)
                {
                    accountBlockBalance.TenantId = AbpSession.TenantId;
                }

                var code = await _commonManger.GetIncrementCodeAsync("D");
                accountBlockBalance.TransCode = code;
                int id;
                var check = await _accountBlockBalanceRepository.FirstOrDefaultAsync(x => x.UserId == input.UserId);
                if (check == null)
                {
                    id = await _accountBlockBalanceRepository.InsertAndGetIdAsync(accountBlockBalance);
                }
                else
                {
                    id = check.Id;
                }

                if (id <= 0)
                    throw new UserFriendlyException("Thao tác không thành công. Vui lòng quay lại sau");

                var detail = new AccountBlockBalanceDetail
                {
                    Amount = input.Amount,
                    Type = input.Type,
                    Attachments = input.Attachments,
                    Description = input.Description,
                    AccountBlockBalanceId = id,
                    TransRef = code
                };
                await _accountBlockBalanceDetailRepository.InsertAsync(detail);
                await CurrentUnitOfWork.SaveChangesAsync();
                ApiResponseDto<AccountBalanceInfo> response;
                if (input.Type == CommonConst.BlockBalanceType.Block)
                {
                    response = await _transactionManager.BlockBalanceAsync(new BlockBalanceRequest
                    {
                        AccountCode = checkDes.AccountCode,
                        BlockAmount = input.Amount,
                        CurrencyCode = "VND",
                        TransNote = input.Description,
                        TransRef = code
                    });
                }
                else
                {
                    response = await _transactionManager.UnBlockBalanceAsync(new UnBlockBalanceRequest
                    {
                        AccountCode = checkDes.AccountCode,
                        UnBlockAmount = input.Amount,
                        CurrencyCode = "VND",
                        TransNote = input.Description,
                        TransRef = code
                    });
                }

                detail.TransNote = response.ToJson();
                detail.Success = response.ResponseCode == "1";
                await _accountBlockBalanceDetailRepository.UpdateAsync(detail);
                if (response.ResponseCode == "1")
                {
                    var payload = response.Payload;
                    try
                    {
                        if (payload != null)
                        {
                            var update = await _accountBlockBalanceRepository.FirstOrDefaultAsync(x => x.Id == id);
                            update.BlockedMoney = payload.BlockedMoney;
                            await _accountBlockBalanceRepository.UpdateAsync(update);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    throw new UserFriendlyException(response.ResponseMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances_Edit)]
        protected virtual async Task Update(CreateOrEditAccountBlockBalanceDto input)
        {
            // var accountBlockBalance = await _accountBlockBalanceRepository.FirstOrDefaultAsync((int) input.Id);
            // ObjectMapper.Map(input, accountBlockBalance);
        }

        [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances_Delete)]
        public async Task Delete(EntityDto input)
        {
            //await _accountBlockBalanceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAccountBlockBalancesToExcel(GetAllAccountBlockBalancesForExcelInput input)
        {
            var filteredAccountBlockBalances = _accountBlockBalanceRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter) ||
                         e.UserFk.AccountCode == input.Filter || e.UserFk.UserName == input.Filter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TransCodeFilter), e => e.TransCode == input.TransCodeFilter)
                .WhereIf(input.FromCreatedTimeFilter!=null && input.FromCreatedTimeFilter!=DateTime.MinValue, e => e.CreationTime >=input.FromCreatedTimeFilter)
                .WhereIf(input.ToCreatedTimeFilter!=null && input.ToCreatedTimeFilter!=DateTime.MinValue, e => e.CreationTime <=input.ToCreatedTimeFilter)
                .WhereIf(input.UserIdFilter != null && input.UserIdFilter > 0,
                    e => e.UserId == input.UserIdFilter);

            var query = from o in filteredAccountBlockBalances
                join s1 in _lookupUserRepository.GetAll() on o.UserId equals s1.Id
                join s2 in _lookupUserRepository.GetAll() on o.CreatorUserId equals s2.Id
                select new GetAccountBlockBalanceForViewDto()
                {
                    AccountBlockBalance = new AccountBlockBalanceDto
                    {
                        TransCode = o.TransCode,
                        BlockedMoney = o.BlockedMoney,
                        Description = o.Description,
                        Id = o.Id,
                        LastModificationTime = o.CreationTime
                    },
                    FullAgentName = s1.AccountCode + "-" + s1.PhoneNumber + "-" + s1.FullName,
                    AgentType = s1.AgentType,
                    UserName = s2.UserName + "-" + s2.FullName
                };


            var accountBlockBalanceListDtos = await query.ToListAsync();

            return _accountBlockBalancesExcelExporter.ExportToFile(accountBlockBalanceListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_AccountBlockBalances)]
        public async Task<List<AccountBlockBalanceUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookupUserRepository.GetAll()
                .Select(user => new AccountBlockBalanceUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }
    }
}
