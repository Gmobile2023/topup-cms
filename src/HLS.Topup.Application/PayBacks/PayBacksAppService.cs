using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Common;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications;
using HLS.Topup.Paybacks;
using HLS.Topup.PayBacks.Dtos;
using HLS.Topup.PayBacks.Exporting;
using HLS.Topup.Providers;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.PayBacks
{
    [AbpAuthorize(AppPermissions.Pages_PayBacks)]
    public class PayBacksAppService : TopupAppServiceBase, IPayBacksAppService
    {
        private readonly IRepository<PayBack> _payBackRepository;
        private readonly IRepository<User, long> _lookupUserRepository;
        private readonly ICommonManger _commonManger;
        private readonly IRepository<PayBackDetail> _payBackDetailRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IRepository<Provider> _lookupProviderRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly ICommonLookupAppService _lookupAppService;
        private readonly IPayBacksManager _payBacksManager;
        private readonly IPayBacksExcelExporter _payBacksExcelExporter;
        private readonly ITransactionManager _transactionManager;
        private readonly INotificationSender _appNotifier;
        private readonly ILogger<PayBacksAppService> _logger;

        public PayBacksAppService(IRepository<PayBack> payBackRepository,
            IRepository<User, long> lookupUserRepository,
            ICommonManger commonManger,
            IRepository<PayBackDetail> payBackDetailRepository,
            IRepository<Category, int> lookup_categoryRepository,
            IRepository<Provider> lookupProviderRepository,
            IRepository<User, long> lookup_userRepository,
            ICommonLookupAppService lookupAppService,
            IPayBacksManager payBacksManager,
            ITransactionManager transactionManager, IPayBacksExcelExporter payBacksExcelExporter,
            ILogger<PayBacksAppService> logger, INotificationSender appNotifier)
        {
            _payBackRepository = payBackRepository;
            _lookupUserRepository = lookupUserRepository;
            _commonManger = commonManger;
            _payBackDetailRepository = payBackDetailRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
            _lookupProviderRepository = lookupProviderRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookupAppService = lookupAppService;
            _payBacksManager = payBacksManager;
            _transactionManager = transactionManager;
            _payBacksExcelExporter = payBacksExcelExporter;
            _logger = logger;
            _appNotifier = appNotifier;
        }

        public async Task<PagedResultDto<GetPayBackForViewDto>> GetAll(GetAllPayBacksInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.PayBackStatus) input.StatusFilter
                : default;

            var filteredPayBacks = _payBackRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),
                    e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),
                    e => e.Code.Contains(input.CodeFilter))
                .WhereIf(input.FromTimeFilter != null, e => e.FromDate >= input.FromTimeFilter)
                .WhereIf(input.ToTimeFilter != null, e => e.ToDate <= input.ToTimeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var pagedAndFilteredPayBacks = filteredPayBacks
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var payBacks = from o in pagedAndFilteredPayBacks
                join o1 in _lookupUserRepository.GetAll() on o.CreatorUserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookupUserRepository.GetAll() on o.ApproverId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetPayBackForViewDto()
                {
                    PayBack = new PayBackDto
                    {
                        Id = o.Id,
                        Code = o.Code,
                        Name = o.Name,
                        Status = o.Status,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        Total = o.Total,
                        TotalAmount = o.TotalAmount,
                        DateApproved = o.DateApproved,
                        CreationTime = o.CreationTime
                    },
                    UserName = s1 == null || s1.Name == null
                        ? ""
                        : s1.UserName,
                    UserApproved = s2 == null || s2.Name == null
                        ? ""
                        : s2.UserName
                };

            var totalCount = await filteredPayBacks.CountAsync();

            return new PagedResultDto<GetPayBackForViewDto>(
                totalCount,
                await payBacks.ToListAsync()
            );
        }

        public async Task<GetPayBackForViewDto> GetPayBacksForView(int id)
        {
            var payBack = await _payBackRepository.GetAsync(id);
            var output = new GetPayBackForViewDto {PayBack = ObjectMapper.Map<PayBackDto>(payBack)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks_Edit)]
        public async Task<GetPayBackForEditOutput> GetPayBacksForEdit(EntityDto input)
        {
            var payBack = await _payBackRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetPayBackForEditOutput {PayBacks = ObjectMapper.Map<CreateOrEditPayBacksDto>(payBack)};

            output.CreationTime = payBack.CreationTime;

            if (output.PayBacks.CreatorUserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.PayBacks.CreatorUserId);
                output.UserName = _lookupUser?.FullName?.ToString();
            }

            if (output.PayBacks.ApproverId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.PayBacks.ApproverId);
                output.UserApproved = _lookupUser?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPayBacksDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PayBacks_Create)]
        protected virtual async Task Create(CreateOrEditPayBacksDto input)
        {
            try
            {
                if (input.PayBacksDetail == null || !input.PayBacksDetail.Any() ||
                    !input.PayBacksDetail.Any(x =>
                        x.UserId != null || x.Amount != null))
                    throw new UserFriendlyException("Danh sách đại lý chưa chính xác!");
                if (input.FromDate.Date > input.ToDate.Date ||
                    input.ToDate.Date >= DateTime.Now.Date)
                    throw new UserFriendlyException("Thời gian chương trình không hợp lệ!");
                var payBacks = ObjectMapper.Map<PayBack>(input);

                if (AbpSession.TenantId != null)
                {
                    payBacks.TenantId = AbpSession.TenantId;
                }

                payBacks.Code = await _commonManger.GetIncrementCodeAsync("D");
                payBacks.Status = CommonConst.PayBackStatus.Init;

                var id = await _payBackRepository.InsertAndGetIdAsync(payBacks);
                if (id <= 0)
                    throw new UserFriendlyException("Thiết lập chương trình không thành công!");

                foreach (var item in input.PayBacksDetail.Where(x =>
                    x.UserId != null || x.Amount != null))
                {
                    await _payBackDetailRepository.InsertAsync(new PayBackDetail()
                    {
                        PayBackId = payBacks.Id,
                        UserId = item.UserId,
                        Amount = item.Amount
                    });
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks_Edit)]
        protected virtual async Task Update(CreateOrEditPayBacksDto input)
        {
            try
            {
                var payBacks = await _payBackRepository.FirstOrDefaultAsync((int) input.Id);
                if (payBacks == null)
                    throw new UserFriendlyException("Danh sách trả phí không tồn tại");
                if (input.PayBacksDetail == null || !input.PayBacksDetail.Any() ||
                    !input.PayBacksDetail.Any(x => x.UserId != null || x.Amount != null))
                    throw new UserFriendlyException("Danh sách đại lý chưa chính xác!");
                if (input.FromDate >= input.ToDate ||
                    input.ToDate >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    throw new UserFriendlyException("Thời gian chương trình không hợp lệ");
                if (input.Status != CommonConst.PayBackStatus.Init)
                    throw new UserFriendlyException("Trạng thái không thể cập nhật");
                if (!string.IsNullOrEmpty(input.Name) && input.Name != payBacks.Name)
                    payBacks.Name = input.Name;
                if (input.FromDate != payBacks.FromDate)
                    payBacks.FromDate = input.FromDate;
                if (input.ToDate != payBacks.ToDate)
                    payBacks.ToDate = input.ToDate;
                if (input.Total != payBacks.Total)
                    payBacks.Total = input.Total;
                if (input.TotalAmount != payBacks.TotalAmount)
                    payBacks.TotalAmount = input.TotalAmount;

                await _payBackRepository.UpdateAsync(payBacks);
                var lstDetail = input.PayBacksDetail;
                var payBacksDetail = _payBackDetailRepository.GetAll().Where(x => x.PayBackId == payBacks.Id);
                foreach (var detail in payBacksDetail)
                {
                    await _payBackDetailRepository.DeleteAsync(detail);
                }

                foreach (var item in lstDetail.Where(x => x.UserId != null || x.Amount != null))
                {
                    await _payBackDetailRepository.InsertAsync(new PayBackDetail
                    {
                        PayBackId = payBacks.Id,
                        UserId = item.UserId,
                        Amount = item.Amount
                    });
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Banks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _payBackRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks)]
        public async Task<List<PayBacksCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAll()
                .Select(category => new PayBacksCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks)]
        public async Task<List<PayBacksProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookupProviderRepository.GetAll()
                .Select(provider => new PayBacksProviderLookupTableDto
                {
                    Id = provider.Id,
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        public async Task<ResponseMessages> GetPayBacksImportList(List<PayBacksImportDto> dataList)
        {
            if (!dataList.Any())
            {
                return new ResponseMessages("0", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
//              throw new UserFriendlyException("Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var query = from a in dataList
                join user in _lookup_userRepository.GetAll().Where(x =>
                    x.AccountType == CommonConst.SystemAccountType.MasterAgent) on a.AgentCode equals user.AccountCode
                select new PayBacksImport
                {
                    Amount = a.Amount,
                    AgentCode = user.AccountCode,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    UserId = user.Id,
                    IsActive = user.IsActive
                };
            var payBacksImports = query as PayBacksImport[] ?? query.ToArray();
            if (!payBacksImports.Any())
            {
                return new ResponseMessages("0", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
                // throw new UserFriendlyException("Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            List<string> lstNotUser = new List<string>();
            foreach (var ag in dataList)
            {
                if (payBacksImports.Any(x => x.AgentCode == ag.AgentCode))
                    continue;
                lstNotUser.Add(ag.AgentCode);
            }

            if (lstNotUser.Any())
            {
                return new ResponseMessages("0",
                    "File Import có thông tin đại lý không đúng định dạng hoặc chưa có trên hệ thống");
                //throw new UserFriendlyException("Kiểm tra lại thông tin dữ liệu không hợp lệ: " + lstNotUser.Join(";"));
            }

            var rs = new ResponseMessages("1");
            rs.Payload = payBacksImports.ToList();
            return rs;
        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks_Approval)]
        public async Task Approval(int id)
        {
            // var payBacks = await _payBackRepository.FirstOrDefaultAsync(input.Id);
            // if (payBacks == null)
            //     throw new UserFriendlyException("Danh sách trả phí khuyến mại mại không tồn tại!");
            // if (payBacks.Status != CommonConst.PayBackStatus.Init)
            //     throw new UserFriendlyException("Trạng thái không hợp lệ!");
            // payBacks.Status = CommonConst.PayBackStatus.Approval;
            // payBacks.ApproverId = AbpSession.UserId ?? 0;
            // payBacks.DateApproved = DateTime.Now;
            //
            // await _payBackRepository.UpdateAsync(payBacks);

            var payBacks = await _payBackRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (payBacks == null)
                throw new UserFriendlyException("Danh sách trả thưởng khuyến mại không tồn tại");
            if (payBacks.Status != CommonConst.PayBackStatus.Init)
                throw new UserFriendlyException("Trạng thái không hợp lệ");

            payBacks.Status = CommonConst.PayBackStatus.Processing;
            await _payBackRepository.UpdateAsync(payBacks);
            await CurrentUnitOfWork.SaveChangesAsync();


            var items = await _payBacksManager.GetPayBacksDetails(payBacks.Id);

            var model = new PaybatchRequest
            {
                Accounts = new List<HLS.Topup.Dtos.PayBacks.PaybatchAccount>(),
                TransRef = payBacks.Code,
                CurrencyCode = "VND"
            };

            foreach (var item in items)
            {
                model.Accounts.Add(new HLS.Topup.Dtos.PayBacks.PaybatchAccount()
                {
                    AccountCode = item.AgentCode,
                    Amount = item.Amount,
                });
            }

            var results = await _transactionManager.PayBacksRequest(model);
            if (!results.Success)
            {
                payBacks.Status = CommonConst.PayBackStatus.Error;
                payBacks.ApproverId = AbpSession.UserId ?? 0;
                payBacks.DateApproved = DateTime.Now;
                await _payBackRepository.UpdateAsync(payBacks);
                await CurrentUnitOfWork.SaveChangesAsync();
                throw new UserFriendlyException(results.Error.Message);
            }
            payBacks.Status = CommonConst.PayBackStatus.Approval;
            payBacks.ApproverId = AbpSession.UserId ?? 0;
            payBacks.DateApproved = DateTime.Now;
            await _payBackRepository.UpdateAsync(payBacks);

            await ApprovalBactchDetail(id, results.Result);

        }

        [AbpAuthorize(AppPermissions.Pages_PayBacks_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var payBacks = await _payBackRepository.FirstOrDefaultAsync(input.Id);
            if (payBacks == null)
                throw new UserFriendlyException("Danh sách trả phí khuyến mại không tồn tại!");
            if (payBacks.Status != CommonConst.PayBackStatus.Init)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            payBacks.Status = CommonConst.PayBackStatus.Cancel;
            payBacks.ApproverId = AbpSession.UserId ?? 0;
            payBacks.DateApproved = DateTime.Now;

            await _payBackRepository.UpdateAsync(payBacks);
        }

        public async Task<PagedResultDto<PayBacksDetailDto>> GetPayBacksDetailsTable(GetPayBacksDetailTableInput input)
        {
            if (input.PayBacksId > 0)
            {
                var item = await _payBacksManager.GetPayBacksDetails(input.PayBacksId);

                return new PagedResultDto<PayBacksDetailDto>(
                    item.Count,
                    item
                );
            }

            return new PagedResultDto<PayBacksDetailDto>(0, null);
        }

        public async Task<FileDto> GetPayBacksToExcel(GetAllPayBacksForExcelInput input)
        {
            try
            {
                var statusFilter = input.StatusFilter.HasValue
                    ? (CommonConst.PayBackStatus) input.StatusFilter
                    : default;

                var filteredPayBacks = _payBackRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),
                        e => e.Name.Contains(input.NameFilter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),
                        e => e.Code.Contains(input.CodeFilter))
                    .WhereIf(input.FromTimeFilter != null, e => e.FromDate >= input.FromTimeFilter)
                    .WhereIf(input.ToTimeFilter != null, e => e.ToDate <= input.ToTimeFilter)
                    .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

                var payBacks = from o in filteredPayBacks
                    join o1 in _lookupUserRepository.GetAll() on o.CreatorUserId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()
                    join o2 in _lookupUserRepository.GetAll() on o.ApproverId equals o2.Id into j2
                    from s2 in j2.DefaultIfEmpty()
                    select new GetPayBackForViewDto()
                    {
                        PayBack = new PayBackDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            Status = o.Status,
                            FromDate = o.FromDate,
                            ToDate = o.ToDate,
                            Total = o.Total,
                            TotalAmount = o.TotalAmount,
                            DateApproved = o.DateApproved,
                            CreationTime = o.CreationTime
                        },
                        UserName = s1 == null || s1.Name == null
                            ? ""
                            : s1.UserName,
                        UserApproved = s2 == null || s2.Name == null
                            ? ""
                            : s2.UserName,
                        TotalAgent = o.Total,
                        TotalAmount = o.TotalAmount
                    };

                var listDtos = await payBacks.ToListAsync();

                return _payBacksExcelExporter.ExportToFile(listDtos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FileDto> GetDetailPayBacksToExcel(GetDetailPayBacksForExcelInput input)
        {
            var payBacks = await _payBackRepository.FirstOrDefaultAsync(input.PayBacksId);
            if (payBacks != null)
            {
                if (input.PayBacksId > 0)
                {
                    var item = await _payBacksManager.GetPayBacksDetails(input.PayBacksId);

                    return _payBacksExcelExporter.DetailPayBacksExportToFile(item, payBacks.Name);
                }
            }

            return null;
        }

        private async Task ApprovalBactchDetail(int id, List<HLS.Topup.Dtos.PayBacks.PaybatchAccount> reponse)
        {
            try
            {
                var query = _payBackDetailRepository.GetAllIncluding(x => x.UserFk).Include(x => x.PayBackFk)
                    .Where(x => x.PayBackId == id);
                var lst = await query.ToListAsync();
                if (lst != null && lst.Any())
                {
                    foreach (var item in query)
                    {
                        var rs = reponse.FirstOrDefault(x => x.AccountCode == item.UserFk.AccountCode);
                        if (rs == null) continue;
                        item.Status = rs.Success;
                        item.TransCode = rs.TransRef;
                        item.TransNote = rs.TransNote;
                        await _payBackDetailRepository.UpdateAsync(item);
                        await SendNotifi(item.UserFk.AccountCode, item.Amount, item.TransCode, item.PayBackFk.Name);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private async Task SendNotifi(string accountcode, decimal amount, string transcode, string promotionName)
        {
            await Task.Run(async () =>
            {
                try
                {
                    // var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                    // {
                    //     AccountCode = input.PartnerCode,
                    //     CurrencyCode = "VND"
                    // });
                    var message = L("Notifi_PayBatch_Body", amount.ToFormat("đ"), promotionName,
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    await _appNotifier.PublishNotification(
                        accountcode,
                        AppNotificationNames.System,
                        new SendNotificationData
                        {
                            //TransCode = input.TransRef,
                            Amount = amount,
                            PartnerCode = accountcode,
                            ServiceCode = CommonConst.ServiceCodes.PAYBATCH,
                            TransType = CommonConst.TransactionType.PayBatch.ToString("G")
                        },
                        message,
                        L("Notifi_PayBatch_Title")
                    );
                }
                catch (Exception e)
                {
                    _logger.LogError($"SendNotifi deposit approval eror:{e}");
                }
            }).ConfigureAwait(false);
        }
    }
}
