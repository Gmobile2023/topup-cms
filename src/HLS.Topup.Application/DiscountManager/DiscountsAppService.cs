using System;
using HLS.Topup.Authorization.Users;
using System.Collections.Generic;
using HLS.Topup.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.DiscountManager.Exporting;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Services;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using ServiceStack;

namespace HLS.Topup.DiscountManager
{
    [AbpAuthorize(AppPermissions.Pages_Discounts)]
    public class DiscountsAppService : TopupAppServiceBase, IDiscountsAppService
    {
        private readonly IRepository<Discount> _discountRepository;
        private readonly IDiscountsExcelExporter _discountsExcelExporter;
        private readonly IRepository<User, long> _lookupUserRepository;

        private readonly ICommonManger _commonManger;

        //private readonly Logger _logger = LogManager.GetLogger("DiscountsAppService");
        private readonly ILogger<DiscountsAppService> _logger;
        private readonly IRepository<DiscountDetail> _discountDetailRepository;
        private readonly IDiscountManger _discountManger;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly ICategoryManager _categoryManager;
        private readonly IRepository<Product> _productRepository;


        public DiscountsAppService(IRepository<Discount> discountRepository,
            IDiscountsExcelExporter discountsExcelExporter, IRepository<User, long> lookupUserRepository,
            ICommonManger commonManger, IRepository<DiscountDetail> discountDetailRepository,
            IDiscountManger discountManger, IRepository<Category, int> lookupCategoryRepository,
            ILogger<DiscountsAppService> logger,
            IRepository<Service, int> lookup_serviceRepository,
            ICategoryManager categoryManager,
            IRepository<Product> productRepository)
        {
            _discountRepository = discountRepository;
            _discountsExcelExporter = discountsExcelExporter;
            _lookupUserRepository = lookupUserRepository;
            _commonManger = commonManger;
            _discountDetailRepository = discountDetailRepository;
            _discountManger = discountManger;
            _lookup_categoryRepository = lookupCategoryRepository;
            _logger = logger;
            _lookup_serviceRepository = lookup_serviceRepository;
            _categoryManager = categoryManager;
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<GetDiscountForViewDto>> GetAll(GetAllDiscountsInput input)
        {
            try
            {
                var statusFilter = input.StatusFilter.HasValue
                    ? (CommonConst.DiscountStatus)input.StatusFilter
                    : default;
                var agentTypeFilter = input.AgentTypeFilter.HasValue
                    ? (CommonConst.AgentType)input.AgentTypeFilter
                    : default;

                var filteredDiscounts = _discountRepository.GetAll()
                    //.Include(e => e.UserFk)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                        e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                             e.Desciptions.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                    .WhereIf(input.FromCreationTimeFilter != null, e => e.CreationTime >= input.FromCreationTimeFilter)
                    .WhereIf(input.ToCreationTimeFilter != null, e => e.CreationTime <= input.ToCreationTimeFilter)
                    .WhereIf(input.FromApprovedTimeFilter != null, e => e.DateApproved >= input.FromApprovedTimeFilter)
                    .WhereIf(input.ToApprovedTimeFilter != null, e => e.DateApproved <= input.ToApprovedTimeFilter)
                    .WhereIf(input.FromAppliedTimeFilter != null, e => e.FromDate >= input.FromAppliedTimeFilter)
                    .WhereIf(input.ToAppliedTimeFilter != null, e => e.ToDate <= input.ToAppliedTimeFilter)
                    // .WhereIf(input.ProductTypeFilter != null,
                    //     e => e.ProductType.Contains(input.ProductTypeFilter.ToString()))
                    // .WhereIf(input.ProductFilter != null, e => e.ProductList.Contains(input.ProductFilter.ToString()))
                    //.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                    .WhereIf(input.AgentTypeFilter.HasValue && input.AgentTypeFilter > -1,
                        e => e.AgentType == agentTypeFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                        e => e.UserFk != null && e.UserFk.AccountCode == input.UserNameFilter).WhereIf(
                        input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.Pending
                        || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.Approved
                        || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.Cancel,
                        e => e.Status == statusFilter);

                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.Applying)
                    filteredDiscounts = filteredDiscounts.Where(x =>
                        x.Status == CommonConst.DiscountStatus.Approved && x.FromDate <= DateTime.Now &&
                        x.ToDate >= DateTime.Now);

                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.NotApply)
                    filteredDiscounts = filteredDiscounts.Where(x =>
                        x.Status == CommonConst.DiscountStatus.Approved && x.FromDate > DateTime.Now);

                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.DiscountStatus.StopApply)
                    filteredDiscounts = filteredDiscounts.Where(x => x.Status == CommonConst.DiscountStatus.Approved && x.ToDate < DateTime.Now);

                var pagedAndFilteredDiscounts = filteredDiscounts
                    .OrderByDescending(x => x.Id)
                    .PageBy(input);

                var discounts = from o in pagedAndFilteredDiscounts
                                join c in _lookupUserRepository.GetAll() on o.CreatorUserId equals c.Id
                                join m in _lookupUserRepository.GetAll() on o.ApproverId equals m.Id into app
                                from approval in app.DefaultIfEmpty()
                                join u in _lookupUserRepository.GetAll() on o.UserId equals u.Id into userApp
                                from user in userApp.DefaultIfEmpty()
                                select new GetDiscountForViewDto()
                                {
                                    Discount = new DiscountDto
                                    {
                                        Code = o.Code,
                                        Name = o.Name,
                                        FromDate = o.FromDate,
                                        ToDate = o.ToDate,
                                        DateApproved = o.DateApproved,
                                        Status = o.Status,
                                        AgentType = o.AgentType,
                                        UserId = o.UserId,
                                        Id = o.Id
                                    },
                                    UserName = user != null
                                        ? user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName
                                        : "",
                                    CreatedDate = o.CreationTime,
                                    Createtor = c.UserName,
                                    Approver = approval != null ? approval.UserName : null
                                };

                var totalCount = await filteredDiscounts.CountAsync();
                var lst = await discounts.ToListAsync();
                foreach (var item in lst)
                {
                    item.Discount = ConvertStatus(item.Discount);
                }

                return new PagedResultDto<GetDiscountForViewDto>(
                    totalCount,
                    lst
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private DiscountDto ConvertStatus(DiscountDto item)
        {
            if (item.Status == CommonConst.DiscountStatus.Pending)
            {
                item.StatusName = L("Enum_DiscountStatus_0");
                item.Status = CommonConst.DiscountStatus.Pending;
            }
            else
            {
                if (item.Status == CommonConst.DiscountStatus.Approved)
                {
                    if (item.FromDate <= DateTime.Now && item.ToDate >= DateTime.Now)
                    {
                        item.StatusName = L("Enum_DiscountStatus_2");
                        item.Status = CommonConst.DiscountStatus.Applying;
                    }

                    if (item.FromDate > DateTime.Now)
                    {
                        item.Status = CommonConst.DiscountStatus.NotApply;
                        item.StatusName = L("Enum_DiscountStatus_4");
                    }

                    if (item.ToDate < DateTime.Now)
                    {
                        item.Status = CommonConst.DiscountStatus.StopApply;
                        item.StatusName = L("Enum_DiscountStatus_5");
                    }
                }

                if (item.Status == CommonConst.DiscountStatus.Cancel)
                {
                    item.Status = CommonConst.DiscountStatus.Cancel;
                    item.StatusName = L("Enum_DiscountStatus_3");
                }

                if (item.Status == CommonConst.DiscountStatus.StopApply)
                {
                    item.Status = CommonConst.DiscountStatus.StopApply;
                    item.StatusName = L("Enum_DiscountStatus_5");
                }
            }

            return item;
        }

        public async Task<GetDiscountForViewDto> GetDiscountForView(int id)
        {
            var discount = await _discountRepository.GetAsync(id);

            var output = new GetDiscountForViewDto { Discount = ObjectMapper.Map<DiscountDto>(discount) };

            if (output.Discount.UserId != null)
            {
                var _lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long)output.Discount.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Edit)]
        public async Task<GetDiscountForEditOutput> GetDiscountForEdit(EntityDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountForEditOutput { Discount = ObjectMapper.Map<CreateOrEditDiscountDto>(discount) };

            if (output.Discount.UserId != null)
            {
                var lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long)output.Discount.UserId);
                output.UserName = lookupUser.AccountCode + " - " + lookupUser.PhoneNumber + " - " + lookupUser.FullName;
            }

            if (discount != null && discount.ApproverId != null)
            {
                var lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long)discount.ApproverId);
                output.UserApproved = lookupUser.UserName;
            }

            if (discount != null && discount.CreatorUserId != null)
            {
                var lookupUser = await _lookupUserRepository.FirstOrDefaultAsync((long)discount.CreatorUserId);
                output.UserCreated = lookupUser.UserName;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Discounts_Create)]
        protected virtual async Task Create(CreateOrEditDiscountDto input)
        {
            try
            {
                if (input.DiscountDetail == null || !input.DiscountDetail.Any() ||
                    !input.DiscountDetail.Any(x => x.DiscountValue != null || x.FixAmount != null))
                    throw new UserFriendlyException("Vui lòng nhập thông tin chiết khấu");
                if (input.FromDate >= input.ToDate)
                    throw new UserFriendlyException("Thời gian áp dụng chiết khấu không hợp lệ");

                //discount.ToDate = input.ToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                if (input.DiscountDetail.Any(x => x.DiscountValue > 100))
                    throw new UserFriendlyException("Chiết khấu không vượt quá 100%");

                if (!input.ListUserId.Any() || input.ListUserId == null)
                {
                    var discount = ObjectMapper.Map<Discount>(input);
                    if (AbpSession.TenantId != null)
                    {
                        discount.TenantId = AbpSession.TenantId;
                    }

                    discount.Code = await _commonManger.GetIncrementCodeAsync("D");
                    discount.Status = CommonConst.DiscountStatus.Pending;
                    var discountId = await _discountRepository.InsertAndGetIdAsync(discount);
                    foreach (var item in input.DiscountDetail.Where(x =>
                        x.DiscountValue != null || x.FixAmount != null))
                    {
                        await _discountDetailRepository.InsertAsync(new DiscountDetail
                        {
                            DiscountId = discountId,
                            DiscountValue = item.DiscountValue,
                            ProductId = item.ProductId,
                            FixAmount = item.FixAmount
                        });
                    }
                }
                else
                {
                    foreach (var singleId in input.ListUserId)
                    {
                        var discount = ObjectMapper.Map<Discount>(input);
                        if (AbpSession.TenantId != null)
                        {
                            discount.TenantId = AbpSession.TenantId;
                        }

                        discount.Code = await _commonManger.GetIncrementCodeAsync("D");
                        discount.Status = CommonConst.DiscountStatus.Pending;
                        discount.UserId = singleId;
                        var discountId = await _discountRepository.InsertAndGetIdAsync(discount);
                        foreach (var item in input.DiscountDetail.Where(x =>
                            x.DiscountValue != null || x.FixAmount != null))
                        {
                            await _discountDetailRepository.InsertAsync(new DiscountDetail
                            {
                                DiscountId = discountId,
                                DiscountValue = item.DiscountValue,
                                ProductId = item.ProductId,
                                FixAmount = item.FixAmount
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

        [AbpAuthorize(AppPermissions.Pages_Discounts_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync((int)input.Id);
            if (discount == null)
                throw new UserFriendlyException("Thông tin chiết khấu không tồn tại");
            if (input.DiscountDetail == null || !input.DiscountDetail.Any() ||
                !input.DiscountDetail.Any(x => x.DiscountValue != null || x.FixAmount != null))
                throw new UserFriendlyException("Vui lòng nhập chiết khấu");
            if (input.FromDate >= input.ToDate)
                throw new UserFriendlyException("Thời gian áp dụng chiết khấu không hợp lệ");
            if (input.DiscountDetail.Any(x => x.DiscountValue > 100))
                throw new UserFriendlyException("Chiết khấu không vượt quá 100%");
            if (discount.Status != CommonConst.DiscountStatus.Pending)
                throw new UserFriendlyException("Trạng thái không thể update");
            if (!string.IsNullOrEmpty(input.Name) && input.Name != discount.Name)
                discount.Name = input.Name;
            if (input.FromDate != discount.FromDate)
                discount.FromDate = input.FromDate;
            if (input.ToDate != discount.ToDate)
                discount.ToDate = input.ToDate;
            if (input.AgentType != discount.AgentType)
                discount.AgentType = input.AgentType;
            if (input.UserId != discount.UserId)
                discount.UserId = input.UserId;

            await _discountRepository.UpdateAsync(discount);
            var lstDetail = input.DiscountDetail;
            var discountDetail = _discountDetailRepository.GetAll().Where(x => x.DiscountId == discount.Id);
            foreach (var detail in discountDetail)
            {
                await _discountDetailRepository.DeleteAsync(detail);
            }

            foreach (var item in lstDetail.Where(x => x.DiscountValue != null || x.FixAmount != null))
            {
                await _discountDetailRepository.InsertAsync(new DiscountDetail
                {
                    DiscountId = discount.Id,
                    DiscountValue = item.DiscountValue,
                    ProductId = item.ProductId,
                    FixAmount = item.FixAmount
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Delete)]
        public async Task Delete(EntityDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync((int)input.Id);
            if (discount == null)
                throw new UserFriendlyException("Chiết khấu không tồn tại");
            if (discount.Status != CommonConst.DiscountStatus.Pending)
                throw new UserFriendlyException("Trạng thái không thể xóa");
            await _discountRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Approval)]
        public async Task Approval(EntityDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync(input.Id);
            if (discount == null)
                throw new UserFriendlyException("Chiết khấu không tồn tại");
            if (discount.Status != CommonConst.DiscountStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            discount.Status = CommonConst.DiscountStatus.Approved;
            discount.ApproverId = AbpSession.UserId ?? 0;
            discount.DateApproved = DateTime.Now;
            await _discountRepository.UpdateAsync(discount);
            // await CurrentUnitOfWork.SaveChangesAsync();
            // await _discountManger.UpdateDiscountsStop();
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync((int)input.Id);
            if (discount == null)
                throw new UserFriendlyException("Chiết khấu không tồn tại");
            if (discount.Status != CommonConst.DiscountStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            discount.Status = CommonConst.DiscountStatus.Cancel;
            discount.ApproverId = AbpSession.UserId ?? 0;
            discount.DateApproved = DateTime.Now;
            await _discountRepository.UpdateAsync(discount);
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts_Stop)]
        public async Task Stop(EntityDto input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync((int)input.Id);
            if (discount == null)
                throw new UserFriendlyException("Chiết khấu không tồn tại");
            if (discount.Status != CommonConst.DiscountStatus.Approved)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            discount.Status = CommonConst.DiscountStatus.StopApply;
            discount.ApproverId = AbpSession.UserId ?? 0;
            discount.DateApproved = DateTime.Now;
            await _discountRepository.UpdateAsync(discount);
        }

        public async Task<List<DiscountLookupTableDto>> GetCategories()
        {
            var cats = await _commonManger.GetCategories(CommonConst.ServiceCodes.PAY_BILL, null);

            var items = cats.Select(category => new DiscountLookupTableDto
            {
                Id = category.Id,
                DisplayName = category.CategoryName
            }).ToList();

            return items.ConvertTo<List<DiscountLookupTableDto>>();
        }

        public async Task<FileDto> GetDiscountsToExcel(GetAllDiscountsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DiscountStatus)input.StatusFilter
                : default;
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType)input.AgentTypeFilter
                : default;

            var filteredDiscounts = _discountRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Desciptions.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.FromCreationTimeFilter != null, e => e.CreationTime >= input.FromCreationTimeFilter)
                .WhereIf(input.ToCreationTimeFilter != null, e => e.CreationTime <= input.ToCreationTimeFilter)
                .WhereIf(input.FromApprovedTimeFilter != null, e => e.DateApproved >= input.FromApprovedTimeFilter)
                .WhereIf(input.ToApprovedTimeFilter != null, e => e.DateApproved <= input.ToApprovedTimeFilter)
                .WhereIf(input.FromAppliedTimeFilter != null, e => e.FromDate >= input.FromAppliedTimeFilter)
                .WhereIf(input.ToAppliedTimeFilter != null, e => e.ToDate <= input.ToAppliedTimeFilter)
                // .WhereIf(input.ProductTypeFilter != null,
                //     e => e.ProductType.Contains(input.ProductTypeFilter.ToString()))
                // .WhereIf(input.ProductFilter != null, e => e.ProductList.Contains(input.ProductFilter.ToString()))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.AgentTypeFilter.HasValue && input.AgentTypeFilter > -1,
                    e => e.AgentType == agentTypeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.AccountCode == input.UserNameFilter);

            var query = (from o in filteredDiscounts
                         join o1 in _lookupUserRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         select new GetDiscountForViewDto()
                         {
                             Discount = new DiscountDto
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


            var discountListDtos = await query.ToListAsync();

            return _discountsExcelExporter.ExportToFile(discountListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Discounts)]
        public async Task<List<DiscountUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookupUserRepository.GetAll()
                .Select(user => new DiscountUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<ProductCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAllIncluding(x => x.ServiceFk)
                .Where(x => x.Status == CommonConst.CategoryStatus.Active)
                //             (x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.TOPUP ||
                //              x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ||
                //              x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ||
                //              x.ServiceFk.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA))
                .Select(category => new ProductCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryCode + "-" + category.CategoryName.ToString()
                }).ToListAsync();
        }

        public async Task<PagedResultDto<DiscountDetailDto>> GetDiscountDetailsTable(GetDiscountDetailTableInput input)
        {
            var item = await _discountManger.GetDiscountDetails(input.DiscountId, input.CateIds);
            return new PagedResultDto<DiscountDetailDto>(
                item.Count,
                item
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Discounts)]
        public async Task<List<DiscountServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookup_serviceRepository.GetAll()
                .Select(service => new DiscountServiceLookupTableDto
                {
                    Id = service.Id,
                    Code = service.ServiceCode,
                    DisplayName = service == null || service.ServicesName == null ? "" : service.ServicesName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize]
        public async Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode)
        {
            return await _categoryManager.GetCategoryByServiceCode(serviceCode);
        }

        public async Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null)
        {
            var lst = new List<ProductInfoDto>();

            var lstProduct = _productRepository.GetAllIncluding(x => x.CategoryFk).Include(x => x.CategoryFk)
                .Where(x => x.CategoryFk.Status == CommonConst.CategoryStatus.Active &&
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

        public async Task<FileDto> GetDetailDiscountToExcel(GetDetailDiscountsForExcelInput input)
        {
            var discount = await _discountRepository.FirstOrDefaultAsync(input.DiscountId);
            if (discount != null)
            {
                if (input.DiscountId > 0)
                {
                    var item = await _discountManger.GetDiscountDetails(input.DiscountId);

                    return _discountsExcelExporter.DiscountDetailsExportToFile(item, discount.Name);
                }
            }

            return null;
        }

        public async Task<ResponseMessages> GetDiscountImportList(List<DiscountImportDto> dataList)
        {
            if (!dataList.Any())
            {
                return new ResponseMessages("00", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var query = from lst in dataList
                        join prd in _productRepository.GetAll() on lst.ProductCode equals prd.ProductCode
                        join cat in _lookup_categoryRepository.GetAll() on prd.CategoryFk.CategoryCode equals cat.CategoryCode
                        join srv in _lookup_serviceRepository.GetAll() on cat.ServiceFk.ServiceCode equals srv.ServiceCode
                        select new DiscountImport
                        {
                            ServiceName = srv.ServicesName,
                            ServiceCode = srv.ServiceCode,
                            CategoryName = cat.CategoryName,
                            CategoryCode = cat.CategoryCode,
                            ProductId = prd.Id,
                            ProductName = prd.ProductName,
                            ProductCode = prd.ProductCode,
                            ProductValue = prd.ProductValue ?? 0,
                            DiscountValue = lst.DiscountValue ?? null,
                            FixAmount = lst.FixAmount ?? null
                        };

            var discountImports = query as DiscountImport[] ?? query.ToArray();
            if (!discountImports.Any())
            {
                return new ResponseMessages("00", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var rs = new ResponseMessages("01");
            rs.Payload = discountImports.ToList();

            return rs;
        }
    }
}
