using HLS.Topup.Authorization.Users;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.FeeManager.Exporting;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common.Dto;
using HLS.Topup.Dtos.BillFees;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.FileSystem;
using ServiceStack;

namespace HLS.Topup.FeeManager
{
    [AbpAuthorize(AppPermissions.Pages_Fees)]
    public class FeesAppService : TopupAppServiceBase, IFeesAppService
    {
        private readonly IRepository<Fee> _feeRepository;
        private readonly IFeesExcelExporter _feesExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IFeeManager _feeManager;
        private readonly ICommonManger _commonManger;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<FeeDetail> _feeDetailRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;

        public FeesAppService(IRepository<Fee> feeRepository, IFeesExcelExporter feesExcelExporter,
            IRepository<User, long> lookup_userRepository, IFeeManager feeManager, ICommonManger commonManger,
            IRepository<Product> productRepository, IRepository<FeeDetail> feeDetailRepository,
            IRepository<Category, int> lookup_categoryRepository)
        {
            _feeRepository = feeRepository;
            _feesExcelExporter = feesExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _feeManager = feeManager;
            _commonManger = commonManger;
            _productRepository = productRepository;
            _feeDetailRepository = feeDetailRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
        }

        public async Task<PagedResultDto<GetFeeForViewDto>> GetAll(GetAllFeesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.FeeStatus) input.StatusFilter
                : default;
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            var filteredFees = _feeRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(input.FromCreationTimeFilter != null, e => e.CreationTime >= input.FromCreationTimeFilter)
                .WhereIf(input.ToCreationTimeFilter != null, e => e.CreationTime <= input.ToCreationTimeFilter)
                .WhereIf(input.FromApprovedTimeFilter != null, e => e.DateApproved >= input.FromApprovedTimeFilter)
                .WhereIf(input.ToApprovedTimeFilter != null, e => e.DateApproved <= input.ToApprovedTimeFilter)
                .WhereIf(input.FromAppliedTimeFilter != null, e => e.FromDate >= input.FromAppliedTimeFilter)
                .WhereIf(input.ToAppliedTimeFilter != null, e => e.ToDate <= input.ToAppliedTimeFilter)
                .WhereIf(input.ProductTypeFilter != null,
                    e => e.ProductType.Contains(input.ProductTypeFilter.ToString()))
                .WhereIf(input.ProductFilter != null, e => e.ProductList.Contains(input.ProductFilter.ToString()))
                .WhereIf(
                    input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.Pending
                    || input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.Approved
                    || input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.Cancel,
                    e => e.Status == statusFilter);

            if (input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.Applying)
                filteredFees = filteredFees.Where(x =>
                    x.Status == CommonConst.FeeStatus.Approved && x.FromDate <= DateTime.Now &&
                    x.ToDate >= DateTime.Now);

            if (input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.NotApply)
                filteredFees = filteredFees.Where(x =>
                    x.Status == CommonConst.FeeStatus.Approved && x.FromDate > DateTime.Now);

            if (input.StatusFilter > -1 && input.StatusFilter == (int) CommonConst.FeeStatus.StopApply)
                filteredFees = filteredFees.Where(x =>
                    x.Status == CommonConst.FeeStatus.Approved && x.ToDate < DateTime.Now);

            var pagedAndFilteredFees = filteredFees
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var fees = from o in pagedAndFilteredFees
                join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.ApproverId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetFeeForViewDto()
                {
                    Fee = new FeeDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        DateApproved = o.DateApproved,
                        Status = o.Status,
                        AgentType = o.AgentType,
                        AgentName = s1 != null ? s1.AccountCode + " - " + s1.PhoneNumber + " - " + s1.FullName : "",
                        CreationTime = o.CreationTime,
                        UserApproved = s2 != null ? s2.UserName : "",
                        Id = o.Id
                    },
                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                };

            var totalCount = await filteredFees.CountAsync();
            var lst = await fees.ToListAsync();
            foreach (var item in lst)
            {
                item.Fee = ConvertStatus(item.Fee);
            }

            return new PagedResultDto<GetFeeForViewDto>(
                totalCount,
                lst
            );
        }

        public async Task<GetFeeForViewDto> GetFeeForView(int id)
        {
            var fee = await _feeRepository.GetAsync(id);

            var output = new GetFeeForViewDto {Fee = ObjectMapper.Map<FeeDto>(fee)};

            if (output.Fee.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.Fee.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Edit)]
        public async Task<GetFeeForEditOutput> GetFeeForEdit(EntityDto input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFeeForEditOutput {Fee = ObjectMapper.Map<CreateOrEditFeeDto>(fee)};

            if (output.Fee.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.Fee.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFeeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Fees_Create)]
        protected virtual async Task Create(CreateOrEditFeeDto input)
        {
            try
            {
                if (input.ProductType == null || !input.ProductType.Any())
                    throw new UserFriendlyException("Vui lòng chọn loại sản phẩm!");
                if (input.FeeDetail == null || !input.FeeDetail.Any() ||
                    !input.FeeDetail.Any(x =>
                        x.MinFee != null || x.AmountMinFee != null || x.AmountIncrease != null || x.SubFee != null))
                    throw new UserFriendlyException("Vui lòng nhập thông tin chính sách thu phí!");
                if (input.FromDate >= input.ToDate)
                    throw new UserFriendlyException("Thời gian áp dụng chính sách thu phí không hợp lệ!");

                if (!input.ListUserId.Any() || input.ListUserId == null)
                {
                    var fee = ObjectMapper.Map<Fee>(input);
                    if (AbpSession.TenantId != null)
                    {
                        fee.TenantId = AbpSession.TenantId;
                    }

                    fee.Code = await _commonManger.GetIncrementCodeAsync("F");
                    fee.Status = CommonConst.FeeStatus.Pending;
                    var feeId = await _feeRepository.InsertAndGetIdAsync(fee);
                    foreach (var item in input.FeeDetail.Where(x =>
                        x.MinFee != null || x.AmountMinFee != null || x.AmountIncrease != null || x.SubFee != null))
                    {
                        await _feeDetailRepository.InsertAsync(new FeeDetail()
                        {
                            FeeId = feeId,
                            ProductId = item.ProductId,
                            MinFee = item.MinFee,
                            AmountMinFee = item.AmountMinFee,
                            AmountIncrease = item.AmountIncrease,
                            SubFee = item.SubFee
                        });
                    }
                }
                else
                {
                    foreach (var singleId in input.ListUserId)
                    {
                        var fee = ObjectMapper.Map<Fee>(input);
                        if (AbpSession.TenantId != null)
                        {
                            fee.TenantId = AbpSession.TenantId;
                        }

                        fee.Code = await _commonManger.GetIncrementCodeAsync("F");
                        fee.Status = CommonConst.FeeStatus.Pending;
                        fee.UserId = singleId;
                        var feeId = await _feeRepository.InsertAndGetIdAsync(fee);
                        foreach (var item in input.FeeDetail.Where(x =>
                            x.MinFee != null || x.AmountMinFee != null || x.AmountIncrease != null || x.SubFee != null))
                        {
                            await _feeDetailRepository.InsertAsync(new FeeDetail()
                            {
                                FeeId = feeId,
                                ProductId = item.ProductId,
                                MinFee = item.MinFee,
                                AmountMinFee = item.AmountMinFee,
                                AmountIncrease = item.AmountIncrease,
                                SubFee = item.SubFee
                            });
                        }
                    }
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Thêm mới không thành công!");
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Edit)]
        protected virtual async Task Update(CreateOrEditFeeDto input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync((int) input.Id);
            if (fee == null)
                throw new UserFriendlyException("Chính sách thu phí không tồn tại!");
            if (input.FeeDetail == null || !input.FeeDetail.Any() ||
                !input.FeeDetail.Any(x =>
                    x.MinFee != null || x.AmountMinFee != null || x.AmountIncrease != null || x.SubFee != null))
                throw new UserFriendlyException("Vui lòng nhập phí!");
            if (input.FromDate >= input.ToDate)
                throw new UserFriendlyException("Thời gian áp dụng chính sách không hợp lệ!");
            if (fee.Status != CommonConst.FeeStatus.Pending)
                throw new UserFriendlyException("Trạng thái không thể cập nhật!");
            if (!string.IsNullOrEmpty(input.Name) && input.Name != fee.Name)
                fee.Name = input.Name;
            if (input.FromDate != fee.FromDate)
                fee.FromDate = input.FromDate;
            if (input.ToDate != fee.ToDate)
                fee.ToDate = input.ToDate;
            if (input.AgentType != fee.AgentType)
                fee.AgentType = input.AgentType;
            if (input.ProductType != fee.ProductType)
                fee.ProductType = input.ProductType;
            if (input.ProductList != fee.ProductList)
                fee.ProductList = input.ProductList;
            if (input.UserId != null && input.UserId != fee.UserId)
                fee.UserId = input.UserId;
            else
                fee.UserId = fee.UserId;

            await _feeRepository.UpdateAsync(fee);
            var lstDetail = input.FeeDetail;
            var feeDetail = _feeDetailRepository.GetAll().Where(x => x.FeeId == fee.Id);
            foreach (var detail in feeDetail)
            {
                await _feeDetailRepository.DeleteAsync(detail);
            }

            foreach (var item in lstDetail.Where(x =>
                x.MinFee != null || x.AmountMinFee != null || x.AmountIncrease != null || x.SubFee != null))
            {
                await _feeDetailRepository.InsertAsync(new FeeDetail()
                {
                    FeeId = fee.Id,
                    ProductId = item.ProductId,
                    MinFee = item.MinFee,
                    AmountMinFee = item.AmountMinFee,
                    AmountIncrease = item.AmountIncrease,
                    SubFee = item.SubFee
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _feeRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Approval)]
        public async Task Approval(EntityDto input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync(input.Id);
            if (fee == null)
                throw new UserFriendlyException("Chính sách thu phí không tồn tại!");
            if (fee.Status != CommonConst.FeeStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            fee.Status = CommonConst.FeeStatus.Approved;
            fee.ApproverId = AbpSession.UserId ?? 0;
            fee.DateApproved = DateTime.Now;

            await _feeRepository.UpdateAsync(fee);
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync(input.Id);
            if (fee == null)
                throw new UserFriendlyException("Chính sách thu phí không tồn tại!");
            if (fee.Status != CommonConst.FeeStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            fee.Status = CommonConst.FeeStatus.Cancel;
            fee.ApproverId = AbpSession.UserId ?? 0;
            fee.DateApproved = DateTime.Now;

            await _feeRepository.UpdateAsync(fee);
        }

        [AbpAuthorize(AppPermissions.Pages_Fees_Stop)]
        public async Task Stop(EntityDto input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync(input.Id);
            if (fee == null)
                throw new UserFriendlyException("Chính sách thu phí không tồn tại!");
            if (fee.Status != CommonConst.FeeStatus.Approved)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            fee.Status = CommonConst.FeeStatus.StopApply;
            fee.ApproverId = AbpSession.UserId ?? 0;
            fee.DateApproved = DateTime.Now;

            await _feeRepository.UpdateAsync(fee);
        }

        public async Task<FileDto> GetFeesToExcel(GetAllFeesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.FeeStatus) input.StatusFilter
                : default;
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            var filteredFees = _feeRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.AgentTypeFilter.HasValue && input.AgentTypeFilter > -1,
                    e => e.AgentType == agentTypeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredFees
                join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetFeeForViewDto()
                {
                    Fee = new FeeDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        DateApproved = o.DateApproved,
                        Status = o.Status,
                        AgentType = o.AgentType,
                        Id = o.Id
                    },
                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                });


            var feeListDtos = await query.ToListAsync();

            return _feesExcelExporter.ExportToFile(feeListDtos);
        }

        public async Task<FileDto> GetDetailFeesToExcel(GetDetailFeesForExcelInput input)
        {
            var fee = await _feeRepository.FirstOrDefaultAsync(input.FeeId);
            if (fee != null)
            {
                if (input.ProductIds.Any(x => x.HasValue))
                {
                    var item = await _feeManager.GetFeeDetails(input.FeeId, null, input.ProductIds);

                    return _feesExcelExporter.DetailFeesExportToFile(item, fee.Name);
                }
            }

            return null;
        }

        [AbpAuthorize(AppPermissions.Pages_Fees)]
        public async Task<List<FeeUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Where(x => x.AgentType == CommonConst.AgentType.Agent ||
                            x.AgentType == CommonConst.AgentType.AgentApi ||
                            x.AgentType == CommonConst.AgentType.AgentCampany||
                            x.AgentType == CommonConst.AgentType.AgentGeneral)
                .Select(user => new FeeUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null
                        ? ""
                        : user.AccountCode.ToString() + " - " + user.PhoneNumber.ToString() + " - " +
                          user.FullName.ToString()
                }).ToListAsync();
        }

        public async Task<PagedResultDto<BillFeeDetailDto>> GetFeeDetailsTable(GetFeeDetailTableInput input)
        {
            if (input.ProductIds.Any(x => x.HasValue) || input.FeeId != null)
            {
                var item = await _feeManager.GetFeeDetails(input.FeeId, null, input.ProductIds);

                return new PagedResultDto<BillFeeDetailDto>(
                    item.Count,
                    item
                );
            }

            return new PagedResultDto<BillFeeDetailDto>(0, null);
        }

        public async Task<List<FeeLookupTableDto>> GetCategories()
        {
            var cats = await _commonManger.GetCategories(CommonConst.ServiceCodes.PAY_BILL, null);

            var items = cats.Select(category => new FeeLookupTableDto
            {
                Id = category.Id,
                DisplayName = category.CategoryName
            }).ToList();

            return items.ConvertTo<List<FeeLookupTableDto>>();
        }

        public async Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null)
        {
            var lst = new List<ProductInfoDto>();

            var lstProduct = _productRepository.GetAllIncluding(x => x.CategoryFk).Include(x => x.CategoryFk).Where(x =>
                    x.CategoryFk.Status == CommonConst.CategoryStatus.Active &&
                    x.Status == CommonConst.ProductStatus.Active)
                .WhereIf(cateIds != null && cateIds.Count > 0 && cateIds.Count(x => x.HasValue) > 0,
                    e => cateIds.Contains(e.CategoryId));

            foreach (var item in lstProduct)
            {
                var d = item.ConvertTo<ProductInfoDto>();
                d.CategoryId = item.CategoryFk.Id;
                d.CategoryCode = item.CategoryFk.CategoryCode;
                d.CategoryName = item.CategoryFk.CategoryName;
                d.ProductName = item.ProductName;
                d.ProductCode = item.ProductCode;
                lst.Add(d);
            }

            return lst.ToList();
        }

        public async Task<ResponseMessages> GetFeeImportList(List<FeeImportDto> dataList)
        {
            if (!dataList.Any())
            {
                return new ResponseMessages("0", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var query = from lst in dataList
                join prd in _productRepository.GetAll() on lst.ProductCode equals prd.ProductCode
                join cat in _lookup_categoryRepository.GetAll() on prd.CategoryFk.CategoryCode equals cat.CategoryCode
                select new FeeImport
                {
                    CategoryName = cat.CategoryName,
                    ProductId = prd.Id,
                    ProductName = prd.ProductName,
                    ProductCode = prd.ProductCode,
                    MinFee = lst.MinFee ?? null,
                    AmountMinFee = lst.AmountMinFee ?? null,
                    AmountIncrease = lst.AmountIncrease ?? null,
                    SubFee = lst.SubFee ?? null
                };

            var listFees = query as FeeImport[] ?? query.ToArray();
            if (!listFees.Any())
            {
                return new ResponseMessages("0", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var rs = new ResponseMessages("1");
            rs.Payload = listFees.ToList();

            return rs;
        }

        private FeeDto ConvertStatus(FeeDto item)
        {
            if (item.Status == CommonConst.FeeStatus.Pending)
            {
                item.StatusName = L("Fees_FeeStatus_0");
                item.Status = CommonConst.FeeStatus.Pending;
            }
            else
            {
                if (item.Status == CommonConst.FeeStatus.Approved)
                {
                    if (item.FromDate <= DateTime.Now && item.ToDate >= DateTime.Now)
                    {
                        item.StatusName = L("Fees_FeeStatus_2");
                        item.Status = CommonConst.FeeStatus.Applying;
                    }

                    if (item.FromDate > DateTime.Now)
                    {
                        item.Status = CommonConst.FeeStatus.NotApply;
                        item.StatusName = L("Fees_FeeStatus_4");
                    }

                    if (item.ToDate < DateTime.Now)
                    {
                        item.Status = CommonConst.FeeStatus.StopApply;
                        item.StatusName = L("Fees_FeeStatus_5");
                    }
                }

                if (item.Status == CommonConst.FeeStatus.Cancel)
                {
                    item.Status = CommonConst.FeeStatus.Cancel;
                    item.StatusName = L("Fees_FeeStatus_3");
                }

                if (item.Status == CommonConst.FeeStatus.StopApply)
                {
                    item.Status = CommonConst.FeeStatus.StopApply;
                    item.StatusName = L("Fees_FeeStatus_5");
                }
            }

            return item;
        }
    }
}
