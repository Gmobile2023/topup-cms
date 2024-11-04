using HLS.Topup.Products;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
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
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using HLS.Topup.Paybacks;
using ServiceStack;
using Microsoft.Extensions.Logging;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Categories;
using HLS.Topup.Transactions;
using Abp.UI;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;

namespace HLS.Topup.BalanceManager
{
    [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
    public class PayBatchBillsAppService : TopupAppServiceBase, IPayBatchBillsAppService
    {
        private readonly IRepository<PayBatchBill> _payBatchBillRepository;
        private readonly IRepository<PayBatchBillDetail> _payBatchBillDetailRepository;
        private readonly IPayBatchBillsExcelExporter _payBatchBillsExcelExporter;
        private readonly IRepository<Product, int> _lookup_productRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IPayBatchManageReponse _payBatchBackendService;
        private readonly IRepository<User, long> _userRepository;
        private readonly ICommonManger _commonManger;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<PayBatchBillsAppService> _logger;
        private readonly INotificationSender _appNotifier;

        public PayBatchBillsAppService(IRepository<PayBatchBill> payBatchBillRepository,
            IPayBatchBillsExcelExporter payBatchBillsExcelExporter, IRepository<Product, int> lookup_productRepository,
            IRepository<Category, int> lookup_categoryRepository,
            IPayBatchManageReponse payBatchBackendService, IRepository<User, long> userRepository,
            ICommonManger commonManger,
            IRepository<PayBatchBillDetail> payBatchBillDetailRepository, ITransactionManager transactionManager,
            ILogger<PayBatchBillsAppService> logger, INotificationSender appNotifier)
        {
            _logger = logger;
            _appNotifier = appNotifier;
            _payBatchBillRepository = payBatchBillRepository;
            _payBatchBillsExcelExporter = payBatchBillsExcelExporter;
            _lookup_categoryRepository = lookup_categoryRepository;
            _lookup_productRepository = lookup_productRepository;
            _payBatchBackendService = payBatchBackendService;
            _payBatchBillDetailRepository = payBatchBillDetailRepository;
            _userRepository = userRepository;
            _transactionManager = transactionManager;
            _commonManger = commonManger;
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
        public async Task<PagedResultDto<GetPayBatchBillForViewDto>> GetAll(GetAllPayBatchBillsInput input)
        {
            var statusFilter = (input.StatusFilter == null || input.StatusFilter == -1)
                ? CommonConst.PayBatchBillStatus.Pending
                : (CommonConst.PayBatchBillStatus) input.StatusFilter;


            var filteredPayBatchBills = _payBatchBillRepository.GetAll()
                .Include(e => e.CategoryFk)
                .Include(e => e.ProductFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.MinDateApprovedFilter != null, e => e.CreationTime >= input.MinDateApprovedFilter)
                .WhereIf(input.MaxDateApprovedFilter != null,
                    e => e.CreationTime <= input.MaxDateApprovedFilter.Value.Date.AddDays(1).AddSeconds(-1))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCodeFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryCode == input.CategoryCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter),
                    e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCodeFilter);

            var pagedAndFilteredPayBatchBills = filteredPayBatchBills
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var payBatchBills = from o in pagedAndFilteredPayBatchBills
                join c in _lookup_categoryRepository.GetAll() on o.CategoryId equals c.Id into gc
                from category in gc.DefaultIfEmpty()
                join p in _lookup_productRepository.GetAll() on o.ProductId equals p.Id into gp
                from product in gp.DefaultIfEmpty()
                join uc in _userRepository.GetAll() on o.CreatorUserId equals uc.Id into gu
                from userOrder in gu.DefaultIfEmpty()
                join ucfg in _userRepository.GetAll() on o.ApproverId equals ucfg.Id into gcfg
                from userConfirm in gcfg.DefaultIfEmpty()
                select new GetPayBatchBillForViewDto()
                {
                    PayBatchBill = new PayBatchBillDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        Status = o.Status,
                        TotalAgent = o.TotalAgent,
                        TotalTrans = o.TotalTrans,
                        TotalAmount = o.TotalAmount,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        Period = o.FromDate.ToString("dd/MM/yyyy") + " -" + o.ToDate.ToString("dd/MM/yyyy"),
                        TotalBlockBill = o.TotalBlockBill,
                        AmountPayBlock = o.AmountPayBlock,
                        CreatorUserId = o.CreatorUserId,
                        CreationTime = o.CreationTime,
                        DateApproved = o.DateApproved,
                        Description = o.Description,
                        MinBillAmount = o.MinBillAmount,
                        MaxAmountPay = o.MaxAmountPay,
                        Id = o.Id
                    },
                    CategoryName = category != null ? category.CategoryName : string.Empty,
                    ProductName = product != null ? product.ProductName : string.Empty,
                    UserCreated = userOrder != null ? userOrder.UserName : string.Empty,
                    UserApproval = userConfirm != null ? userConfirm.UserName : string.Empty,
                };

            var totalCount = await filteredPayBatchBills.CountAsync();

            return new PagedResultDto<GetPayBatchBillForViewDto>(
                totalCount,
                await payBatchBills.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
        public async Task<GetPayBatchBillForViewDto> GetPayBatchBillForView(int id)
        {
            var payBatchBill = await _payBatchBillRepository.GetAsync(id);

            var output = new GetPayBatchBillForViewDto {PayBatchBill = ObjectMapper.Map<PayBatchBillDto>(payBatchBill)};

            if (output.PayBatchBill.ProductId > 0)
            {
                var _lookupProduct =
                    await _lookup_productRepository.FirstOrDefaultAsync((int) output.PayBatchBill.ProductId);
                output.ProductName = _lookupProduct?.ProductName;
            }

            if (output.PayBatchBill.CategoryId > 0)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int) output.PayBatchBill.CategoryId);
                output.CategoryName = _lookupCategory?.CategoryName;
            }

            if (output.PayBatchBill.CreatorUserId > 0)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((int) output.PayBatchBill.CreatorUserId);
                output.UserCreated = _lookupUser?.UserName;
            }

            if (output.PayBatchBill.ApproverId > 0)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((int) output.PayBatchBill.ApproverId);
                output.UserApproval = _lookupUser?.UserName;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
        public async Task<GetPayBatchBillForEditOutput> GetPayBatchBillForEdit(EntityDto input)
        {
            var payBatchBill = await _payBatchBillRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPayBatchBillForEditOutput
                {PayBatchBill = ObjectMapper.Map<CreateOrEditPayBatchBillDto>(payBatchBill)};

            if (!string.IsNullOrEmpty(output.PayBatchBill.ProductCode))
            {
                var _lookupProduct =
                    await _lookup_productRepository.FirstOrDefaultAsync(c =>
                        c.ProductCode == output.PayBatchBill.ProductCode);
                output.ProductName = _lookupProduct?.ProductName?.ToString();
            }

            if (!string.IsNullOrEmpty(output.PayBatchBill.CategoryCode))
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync(c =>
                        c.CategoryCode == output.PayBatchBill.CategoryCode);
                output.CategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPayBatchBillDto input)
        {
            if (input.TotalBlockBill <= 0)
                throw new UserFriendlyException("Quý khách phải nhập số Block hóa đơn lớn hơn 0.");

            if (input.AmountPayBlock <= 0)
                throw new UserFriendlyException("Quý khách phải nhập số tiền trên mỗi Block lớn hơn 0");

            await Create(input);
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills_Create)]
        protected virtual async Task Create(CreateOrEditPayBatchBillDto input)
        {
            var payBill = await _payBatchBackendService.PayBatchBillGetRequest(new PayBatchBillRequest()
            {
                CategoryCode = input.CategoryCode,
                ProductCode = input.ProductCode,
                FromDate = input.FromDate.Date,
                ToDate = input.ToDate.Date,
                BlockMin = input.TotalBlockBill,
                MoneyBlock = input.AmountPayBlock,
                BonusMoneyMax = input.MaxAmountPay ?? 0,
                BillAmountMin = input.MinBillAmount ?? 0,
                Offset = 0,
                Limit = int.MaxValue,
            });

            var listAccount = payBill.Payload.ConvertTo<List<PayBatchBillItem>>();
            var agentCodes = listAccount.Select(c => c.AgentCode).ToList();
            var users = _userRepository.GetAll().Where(c => agentCodes.Contains(c.AccountCode)).ToList();
            var category = _lookup_categoryRepository.FirstOrDefault(c => c.CategoryCode == input.CategoryCode);
            var product = !string.IsNullOrEmpty(input.ProductCode)
                ? _lookup_productRepository.FirstOrDefault(c => c.ProductCode == input.ProductCode)
                : null;

            var payBatch = new PayBatchBill()
            {
                Code = await _commonManger.GetIncrementCodeAsync("CT"),
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                TotalAgent = listAccount.Count(),
                AmountPayBlock = input.AmountPayBlock,
                TotalBlockBill = input.TotalBlockBill,
                MaxAmountPay = input.MaxAmountPay,
                MinBillAmount = input.MinBillAmount,
                TotalAmount = listAccount.Sum(c => c.PayBatchMoney),
                TotalTrans = listAccount.Sum(c => c.Quantity),
                Name = input.Name,
                CategoryId = category.Id,
                CreationTime = DateTime.Now,
                CreatorUserId = AbpSession.UserId,
                TenantId = AbpSession.TenantId,
            };
            if (product != null)
                payBatch.ProductId = product.Id;

            if (payBatch.MaxAmountPay == -1 || payBatch.MaxAmountPay == 0)
                payBatch.MaxAmountPay = null;

            var list = (from item in listAccount
                join u in users on item.AgentCode equals u.AccountCode into g
                from us in g.DefaultIfEmpty()
                select new PayBatchBillItem()
                {
                    UserId = us.Id,
                    AgentCode = item.AgentCode,
                    Mobile = us != null ? us.PhoneNumber : string.Empty,
                    FullName = us != null ? us.FullName : string.Empty,
                    Quantity = item.Quantity,
                    PayAmount = item.PayAmount,
                    PayBatchMoney = item.PayBatchMoney,
                }).ToList();

            if (list.Count <= 0)
                throw new UserFriendlyException("Không có danh sách trả thưởng.");


            //using (var trans = new System.Transactions.TransactionScope())
            //{

            var id = _payBatchBillRepository.InsertAndGetId(payBatch);
            var listItems = (from x in list
                select new PayBatchBillDetail()
                {
                    PayBatchBillId = id,
                    Quantity = x.Quantity,
                    Amount = x.PayAmount,
                    Money = x.PayBatchMoney,
                    CreationTime = DateTime.Now,
                    CreatorUserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    UserId = x.UserId,
                }).ToList();

            foreach (var item in listItems)
            {
                await _payBatchBillDetailRepository.InsertAsync(item);
            }
            //    trans.Complete();


            //}
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _payBatchBillRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
        public async Task<FileDto> GetPayBatchBillsToExcel(GetAllPayBatchBillsForExcelInput input)
        {
            var statusFilter = (input.StatusFilter == null || input.StatusFilter == -1)
                ? CommonConst.PayBatchBillStatus.Pending
                : (CommonConst.PayBatchBillStatus) input.StatusFilter;

            var filteredPayBatchBills = _payBatchBillRepository.GetAll()
                .Include(e => e.CategoryFk)
                .Include(e => e.ProductFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.MinDateApprovedFilter != null, e => e.CreationTime >= input.MinDateApprovedFilter)
                .WhereIf(input.MaxDateApprovedFilter != null,
                    e => e.CreationTime <= input.MaxDateApprovedFilter.Value.Date.AddDays(1).AddSeconds(-1))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCodeFilter),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryCode == input.CategoryCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCodeFilter),
                    e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCodeFilter);

            var payBatchBills = from o in filteredPayBatchBills
                join c in _lookup_categoryRepository.GetAll() on o.CategoryId equals c.Id into gc
                from category in gc.DefaultIfEmpty()
                join p in _lookup_productRepository.GetAll() on o.ProductId equals p.Id into gp
                from product in gp.DefaultIfEmpty()
                join uc in _userRepository.GetAll() on o.CreatorUserId equals uc.Id into gu
                from userOrder in gu.DefaultIfEmpty()
                join ucfg in _userRepository.GetAll() on o.ApproverId equals ucfg.Id into gcfg
                from userConfirm in gcfg.DefaultIfEmpty()
                select new GetPayBatchBillForViewDto()
                {
                    PayBatchBill = new PayBatchBillDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        Status = o.Status,
                        TotalAgent = o.TotalAgent,
                        TotalTrans = o.TotalTrans,
                        TotalAmount = o.TotalAmount,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        Period = o.FromDate.ToString("dd/MM/yyyy") + " -" + o.ToDate.ToString("dd/MM/yyyy"),
                        TotalBlockBill = o.TotalBlockBill,
                        AmountPayBlock = o.AmountPayBlock,
                        CreatorUserId = o.CreatorUserId,
                        CreationTime = o.CreationTime,
                        DateApproved = o.DateApproved,
                        Description = o.Description,
                        MinBillAmount = o.MinBillAmount,
                        MaxAmountPay = o.MaxAmountPay,
                        Id = o.Id
                    },
                    CategoryName = category != null ? category.CategoryName : string.Empty,
                    ProductName = product != null ? product.ProductName : string.Empty,
                    UserCreated = userOrder != null ? userOrder.UserName : string.Empty,
                    UserApproval = userConfirm != null ? userConfirm.UserName : string.Empty
                };


            var payBatchBillListDtos = await payBatchBills.ToListAsync();
            return _payBatchBillsExcelExporter.ExportToFile(payBatchBillListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills)]
        public async Task<List<PayBatchBillProductLookupTableDto>> GetAllProductForTableDropdown()
        {
            return await _lookup_productRepository.GetAll()
                .Select(product => new PayBatchBillProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product == null || product.ProductName == null ? "" : product.ProductName.ToString()
                }).ToListAsync();
        }

        public async Task<PagedResultDtoReport<PayBatchBillItem>> PayBatchBillGetRequest(GetPayBatchSearchInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.CategoryCode) || input.IsSearch == 0)
                {
                    return new PagedResultDtoReport<PayBatchBillItem>(
                        0,
                        new PayBatchBillItem(),
                        new List<PayBatchBillItem>());
                }


                if (input.BlockMin <= 0)
                    throw new UserFriendlyException("Quý khách phải nhập số Block hóa đơn lớn hơn 0.");

                if (input.MoneyBlock <= 0)
                    throw new UserFriendlyException("Quý khách phải nhập số tiền trên mỗi Block lớn hơn 0");

                if (input.BillAmountMin == null)
                    input.BillAmountMin = 0;
                if (input.BonusMoneyMax == null || input.BonusMoneyMax == 0)
                    input.BonusMoneyMax = -1;

                var request = input.ConvertTo<PayBatchBillRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                var rs = await _payBatchBackendService.PayBatchBillGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<PayBatchBillItem>(
                        0,
                        new PayBatchBillItem(),
                        new List<PayBatchBillItem>()
                    );
                var lst = rs.Payload.ConvertTo<List<PayBatchBillItem>>();
                var sumList = rs.SumData.ConvertTo<List<PayBatchBillItem>>();
                var sumTotal = sumList != null && sumList.Count >= 1 ? sumList[0] : new PayBatchBillItem();
                var agentCodes = lst.Select(c => c.AgentCode).ToList();
                var users = _userRepository.GetAll().Where(c => agentCodes.Contains(c.AccountCode)).ToList();

                var list = (from item in lst
                    join u in users on item.AgentCode equals u.AccountCode into g
                    from us in g.DefaultIfEmpty()
                    select new PayBatchBillItem()
                    {
                        AgentCode = item.AgentCode,
                        Mobile = us != null ? us.PhoneNumber : string.Empty,
                        FullName = us != null ? us.FullName : string.Empty,
                        Quantity = item.Quantity,
                        PayAmount = item.PayAmount,
                        PayBatchMoney = item.PayBatchMoney,
                    }).ToList();
                return new PagedResultDtoReport<PayBatchBillItem>(
                    totalCount,
                    totalData: sumTotal,
                    list
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"PayBatchBillGetRequest error: {e}");
                return new PagedResultDtoReport<PayBatchBillItem>(
                    0,
                    new PayBatchBillItem(),
                    new List<PayBatchBillItem>());
            }
        }

        public async Task<PagedResultDtoReport<PayBatchBillItem>> GetPayBatchBillDetail(
            GetPayBatchSearchDetailInput input)
        {
            var list = _payBatchBillDetailRepository.GetAll().Where(c => c.PayBatchBillId == input.Id);
            var pagedDetail = list
                .OrderByDescending(x=>x.Description)
                .PageBy(input);

            var detailbills = from o in pagedDetail
                join u in _userRepository.GetAll() on o.UserId equals u.Id into gu
                from user in gu.DefaultIfEmpty()
                select new PayBatchBillItem()
                {
                    AgentCode = user != null ? user.AccountCode : string.Empty,
                    Mobile = user != null ? user.PhoneNumber : string.Empty,
                    FullName = user != null ? user.FullName : string.Empty,
                    Quantity = o.Quantity ?? 0,
                    PayAmount = o.Amount,
                    PayBatchMoney = o.Money ?? 0,
                    StatusName = o.Success ? "Đã trả" : "Chưa trả",
                    TransRef = o.TransRef,
                };

            var totalCount = await detailbills.CountAsync();
            var sumData = new PayBatchBillItem()
            {
                Quantity = list.Sum(c => c.Quantity ?? 0),
                PayAmount = list.Sum(c => c.Amount),
                PayBatchMoney = list.Sum(c => c.Money ?? 0),
            };

            return new PagedResultDtoReport<PayBatchBillItem>(
                totalCount,
                totalData: sumData,
                await detailbills.ToListAsync()
            );
        }

        public async Task<FileDto> GetPayBatchDetailToExcel(GetPayBatchSearchDetailInput input)
        {
            input.MaxResultCount = int.MaxValue;
            input.SkipCount = 0;
            var list = _payBatchBillDetailRepository.GetAll().Where(c => c.PayBatchBillId == input.Id);
            var pagedDetail = list
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var detailbills = from o in pagedDetail
                join u in _userRepository.GetAll() on o.UserId equals u.Id into gu
                from user in gu.DefaultIfEmpty()
                select new PayBatchBillItem()
                {
                    AgentCode = user != null ? user.AccountCode : string.Empty,
                    Mobile = user != null ? user.PhoneNumber : string.Empty,
                    FullName = user != null ? user.FullName : string.Empty,
                    Quantity = o.Quantity ?? 0,
                    PayAmount = o.Amount,
                    PayBatchMoney = o.Money ?? 0,
                    StatusName = o.Success ? "Đã trả" : "Chưa trả",
                    TransRef = o.TransRef,
                };

            var lst = await detailbills.ToListAsync();
            return _payBatchBillsExcelExporter.ExportDetailToFile(lst);
        }

        public async Task<int> CheckPayBatchBill(CheckPayBatchBillInput input)
        {
            var filteredPayBatchBills = _payBatchBillRepository.GetAll()
                .Include(e => e.CategoryFk)
                .Include(e => e.ProductFk)
                .Where(e => e.FromDate == input.FromDate.Date && e.ToDate == input.ToDate.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCode),
                    e => e.CategoryFk != null && e.CategoryFk.CategoryCode == input.CategoryCode)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCode),
                    e => e.ProductFk != null && e.ProductFk.ProductCode == input.ProductCode);

            var totalCount = await filteredPayBatchBills.CountAsync();

            return totalCount;
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills_Approval)]
        public async Task ConfirmApproval(int id)
        {
            var payBactchBill = await _payBatchBillRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (payBactchBill == null)
                throw new UserFriendlyException("Danh sách trả thưởng khuyến mại không tồn tại");
            if (payBactchBill.Status != CommonConst.PayBatchBillStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");

            payBactchBill.Status = CommonConst.PayBatchBillStatus.Processing;
            await _payBatchBillRepository.UpdateAsync(payBactchBill);
            await CurrentUnitOfWork.SaveChangesAsync();

            var pagedDetails =
                await _payBatchBillDetailRepository.GetAllListAsync(c => c.PayBatchBillId == id && c.Success == false);
            var detailbills = (from o in pagedDetails
                join user in _userRepository.GetAll() on o.UserId equals user.Id
                select new PayBatchBillItem()
                {
                    UserId = user.Id,
                    AgentCode = user.AccountCode,
                    Mobile = user.PhoneNumber,
                    FullName = user.FullName,
                    Quantity = o.Quantity ?? 0,
                    PayAmount = o.Amount,
                    PayBatchMoney = o.Money ?? 0,
                    TransRef = o.TransRef,
                }).ToList();

            var model = new PaybatchRequest();
            model.Accounts = new List<HLS.Topup.Dtos.PayBacks.PaybatchAccount>();
            model.TransRef = payBactchBill.Code;
            model.CurrencyCode = "VND";

            foreach (var item in detailbills)
            {
                model.Accounts.Add(new HLS.Topup.Dtos.PayBacks.PaybatchAccount()
                {
                    AccountCode = item.AgentCode,
                    Amount = item.PayBatchMoney,
                });
            }

            var results = await _transactionManager.PayBacksRequest(model);
            if (!results.Success)
            {
                payBactchBill.Status = CommonConst.PayBatchBillStatus.Error;
                payBactchBill.ApproverId = AbpSession.UserId ?? 0;
                payBactchBill.DateApproved = DateTime.Now;
                await _payBatchBillRepository.UpdateAsync(payBactchBill);
                throw new UserFriendlyException(results.Error.Message);
            }
            payBactchBill.Status = CommonConst.PayBatchBillStatus.Approved;
            payBactchBill.ApproverId = AbpSession.UserId ?? 0;
            payBactchBill.DateApproved = DateTime.Now;
            await _payBatchBillRepository.UpdateAsync(payBactchBill);
            await ApprovalBactchDetail(pagedDetails, detailbills, results.Result, payBactchBill.Name);
        }

        [AbpAuthorize(AppPermissions.Pages_PayBatchBills_Cancel)]
        public async Task ConfirmCancel(int id)
        {
            var payBactchBill = await _payBatchBillRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (payBactchBill == null)
                throw new UserFriendlyException("Danh sách trả thưởng hóa đơn không tồn tại");
            if (payBactchBill.Status != CommonConst.PayBatchBillStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            payBactchBill.Status = CommonConst.PayBatchBillStatus.Cancel;
            payBactchBill.ApproverId = AbpSession.UserId ?? 0;
            payBactchBill.DateApproved = DateTime.Now;
            await _payBatchBillRepository.UpdateAsync(payBactchBill);
        }

        private async Task ApprovalBactchDetail(List<PayBatchBillDetail> details,
            List<PayBatchBillItem> detailbills, List<HLS.Topup.Dtos.PayBacks.PaybatchAccount> reponse, string name)
        {
            foreach (var item in details)
            {
                var account = detailbills.SingleOrDefault(c => c.UserId == item.UserId);
                if (account != null)
                {
                    var rs = reponse.FirstOrDefault(r => r.AccountCode == account.AgentCode);
                    if (rs != null)
                    {
                        item.Success = rs.Success;
                        item.LastModificationTime = DateTime.Now;
                        item.LastModifierUserId = AbpSession.UserId;
                        item.TransRef = rs.TransRef;
                        await _payBatchBillDetailRepository.UpdateAsync(item);
                        await SendNotifi(item.UserFk.AccountCode, item.Money ?? 0, item.TransRef, name);
                    }
                }
            }
        }

        private async Task SendNotifi(string accountcode, decimal amount, string transcode, string promotionName)
        {
            await Task.Run(async () =>
            {
                try
                {
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
