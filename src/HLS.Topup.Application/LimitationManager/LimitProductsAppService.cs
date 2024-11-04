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
using HLS.Topup.LimitationManager.Exporting;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Services;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.LimitationManager
{
    [AbpAuthorize(AppPermissions.Pages_LimitProducts)]
    public class LimitProductsAppService : TopupAppServiceBase, ILimitProductsAppService
    {
        private readonly IRepository<LimitProduct> _limitProductRepository;
        private readonly IRepository<LimitProductDetail> _limitProductDetailRepository;
        private readonly ILimitProductsExcelExporter _limitProductsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly ICategoryManager _categoryManager;
        private readonly ICommonManger _commonManger;
        private readonly ILimitationManager _limitationManager;
        private readonly IRepository<Product> _productRepository;

        public LimitProductsAppService(IRepository<LimitProduct> limitProductRepository,
            IRepository<LimitProductDetail> limitProductDetailRepository,
            ILimitProductsExcelExporter limitProductsExcelExporter, IRepository<User, long> lookup_userRepository,
            IRepository<Service, int> lookup_serviceRepository,
            ICategoryManager categoryManager,
            ICommonManger commonManger,
            ILimitationManager limitationManager,
            IRepository<Product> productRepository,
            IRepository<Category, int> lookup_categoryRepository)
        {
            _limitProductRepository = limitProductRepository;
            _limitProductDetailRepository = limitProductDetailRepository;
            _limitProductsExcelExporter = limitProductsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_serviceRepository = lookup_serviceRepository;
            _categoryManager = categoryManager;
            _commonManger = commonManger;
            _limitationManager = limitationManager;
            _productRepository = productRepository;
            _lookup_categoryRepository = lookup_categoryRepository;
        }

        public async Task<PagedResultDto<GetLimitProductForViewDto>> GetAll(GetAllLimitProductsInput input)
        {
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.LimitProductConfigStatus) input.StatusFilter
                : default;

            var filteredLimitProducts = _limitProductRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(input.FromDateFilter != null, e => e.FromDate >= input.FromDateFilter)
                .WhereIf(input.ToDateFilter != null, e => e.ToDate <= input.ToDateFilter)
                .WhereIf(input.AgentTypeFilter != null, e => e.AgentType == input.AgentTypeFilter)
                .WhereIf(input.AgentFilter > 0, e => e.UserId == input.AgentFilter)
                .WhereIf(
                    input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Pending 
                    || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Approved 
                    || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Cancel, e => e.Status == statusFilter);
            
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Applying)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now);
                
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.NotApply)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.FromDate > DateTime.Now);
                
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.StopApply)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.ToDate < DateTime.Now);

            var pagedAndFilteredLimitProducts = filteredLimitProducts
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var limitProducts = from o in pagedAndFilteredLimitProducts
                join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.ApproverId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookup_userRepository.GetAll() on o.CreatorUserId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new GetLimitProductForViewDto()
                {
                    LimitProduct = new LimitProductDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        DateApproved = o.DateApproved,
                        AgentType = o.AgentType,
                        Status = o.Status,
                        Id = o.Id,
                    },
                    UserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString(),
                    AgentName = s1 != null ? s1.AccountCode + " - " + s1.PhoneNumber + " - " + s1.FullName : "",
                    UserApproved = s2.UserName,
                    CreationTime = o.CreationTime,
                    UserCreated = s3 == null || s3.UserName == null ? "" : s3.UserName.ToString()
                };

            var totalCount = await filteredLimitProducts.CountAsync();

            var lst = await limitProducts.ToListAsync();
            foreach (var item in lst)
            {
                item.LimitProduct = ConvertStatus(item.LimitProduct);
            }

            return new PagedResultDto<GetLimitProductForViewDto>(
                totalCount,
                lst
            );
        }

        public async Task<GetLimitProductForViewDto> GetLimitProductForView(int id)
        {
            var limitProduct = await _limitProductRepository.GetAsync(id);

            var output = new GetLimitProductForViewDto {LimitProduct = ObjectMapper.Map<LimitProductDto>(limitProduct)};

            if (output.LimitProduct.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.LimitProduct.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Edit)]
        public async Task<GetLimitProductForEditOutput> GetLimitProductForEdit(EntityDto input)
        {
            var limitProduct = await _limitProductRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLimitProductForEditOutput
            {
                LimitProduct = new CreateOrEditLimitProductDto()
                {
                    Id = limitProduct.Id,
                    Code = limitProduct.Code,
                    Name = limitProduct.Name,
                    FromDate = limitProduct.FromDate,
                    ToDate = limitProduct.ToDate,
                    Status = limitProduct.Status,
                    AgentType = limitProduct.AgentType,
                    UserId = limitProduct.UserId,
                    CreatorUserId = limitProduct.CreatorUserId,
                    ApproverId = limitProduct.ApproverId,
                    DateApproved = limitProduct.DateApproved,
                },
                CreationTime = limitProduct.CreationTime,
            };

            if (output.LimitProduct.CreatorUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.LimitProduct.CreatorUserId);
                output.UserName = _lookupUser?.UserName?.ToString();
            }
            
            if (output.LimitProduct.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long) output.LimitProduct.UserId);
                output.AgentName = _lookupUser != null
                    ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName
                    : "";
            }

            if (output.LimitProduct.ApproverId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.LimitProduct.ApproverId);
                output.UserApproved = _lookupUser?.UserName?.ToString();
            }

            return output;
        }
        
        public async Task<PagedResultDto<LimitProductDetailDto>> GetLimitProductsDetailsTable(GetLimitProductsTableInput input)
        {
            if (input.LimitProductId > 0)
            {
                var item = await _limitationManager.GetLimitProductsDetails(input.LimitProductId);

                return new PagedResultDto<LimitProductDetailDto>(
                    item.Count,
                    item
                );
            }

            return new PagedResultDto<LimitProductDetailDto>(0, null);
        }

        public async Task CreateOrEdit(CreateOrEditLimitProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Create)]
        protected virtual async Task Create(CreateOrEditLimitProductDto input)
        {
            try
            {
                if (input.LimitProductsDetail == null || !input.LimitProductsDetail.Any() ||
                    !input.LimitProductsDetail.Any(x => x.LimitProductId != null))
                    throw new UserFriendlyException("Danh sách hạn mức sản phẩm không hợp lệ!");
                if (input.FromDate >= input.ToDate || input.ToDate <= DateTime.Now)
                    throw new UserFriendlyException("Thời gian áp dụng không hợp lệ!");

                if (!input.ListUserId.Any() || input.ListUserId == null)
                {
                    var limitProducts = new LimitProduct()
                    {
                        Name = input.Name,
                        FromDate = input.FromDate,
                        ToDate = input.ToDate,
                        CreationTime = DateTime.Now,
                        UserId = input.UserId,
                        CreatorUserId = AbpSession.UserId ?? 0,
                        AgentType = input.AgentType
                    };
                    
                    if (AbpSession.TenantId != null)
                    {
                        limitProducts.TenantId = AbpSession.TenantId;
                    }
                    
                    limitProducts.Code = await _commonManger.GetIncrementCodeAsync("L");
                    limitProducts.Status = CommonConst.LimitProductConfigStatus.Pending;
                    var limitProductId = await _limitProductRepository.InsertAndGetIdAsync(limitProducts);
                    foreach (var item in input.LimitProductsDetail.Where(x => x.LimitQuantity != null || x.LimitAmount != null))
                    {
                        await _limitProductDetailRepository.InsertAsync(new LimitProductDetail()
                        {
                            LimitProductId = limitProductId,
                            LimitQuantity = item.LimitQuantity,
                            LimitAmount = item.LimitAmount,
                            ProductId = item.ProductId
                        });
                    }
                }
                else
                {
                    foreach (var singleId in input.ListUserId)
                    {
                        var limitProducts = new LimitProduct()
                        {
                            Name = input.Name,
                            FromDate = input.FromDate,
                            ToDate = input.ToDate,
                            CreationTime = DateTime.Now,
                            UserId = input.UserId,
                            CreatorUserId = AbpSession.UserId ?? 0,
                            AgentType = input.AgentType
                        };
                    
                        if (AbpSession.TenantId != null)
                        {
                            limitProducts.TenantId = AbpSession.TenantId;
                        }
                    
                        limitProducts.Code = await _commonManger.GetIncrementCodeAsync("L");
                        limitProducts.Status = CommonConst.LimitProductConfigStatus.Pending;
                        limitProducts.UserId = singleId;
                        var limitProductId = await _limitProductRepository.InsertAndGetIdAsync(limitProducts);
                        foreach (var item in input.LimitProductsDetail.Where(x => x.LimitQuantity != null || x.LimitAmount != null))
                        {
                            await _limitProductDetailRepository.InsertAsync(new LimitProductDetail()
                            {
                                LimitProductId = limitProductId,
                                LimitQuantity = item.LimitQuantity,
                                LimitAmount = item.LimitAmount,
                                ProductId = item.ProductId
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

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Edit)]
        protected virtual async Task Update(CreateOrEditLimitProductDto input)
        {
            var limitProducts = await _limitProductRepository.FirstOrDefaultAsync((int) input.Id);
            if (limitProducts == null)
                throw new UserFriendlyException("Hạn mức bán hàng không tồn tại!");
            if (input.LimitProductsDetail == null || !input.LimitProductsDetail.Any() ||
                !input.LimitProductsDetail.Any(x => x.LimitProductId != null))
                throw new UserFriendlyException("Danh sách hạn mức sản phẩm không hợp lệ!");
            if (input.FromDate >= input.ToDate)
                throw new UserFriendlyException("Thời gian áp dụng không hợp lệ!");
            if (limitProducts.Status != CommonConst.LimitProductConfigStatus.Pending)
                throw new UserFriendlyException("Trạng thái không thể cập nhật!");
            if (!string.IsNullOrEmpty(input.Name) && input.Name != limitProducts.Name)
                limitProducts.Name = input.Name;
            if (input.FromDate != limitProducts.FromDate)
                limitProducts.FromDate = input.FromDate;
            if (input.ToDate != limitProducts.ToDate)
                limitProducts.ToDate = input.ToDate;
            if (input.UserId != null && input.UserId != limitProducts.UserId)
                limitProducts.UserId = input.UserId;
            else
                limitProducts.UserId = limitProducts.UserId;

            await _limitProductRepository.UpdateAsync(limitProducts);
            var lstDetail = input.LimitProductsDetail;
            var limitProductsDetail =
                _limitProductDetailRepository.GetAll().Where(x => x.LimitProductId == limitProducts.Id);
            foreach (var detail in limitProductsDetail)
            {
                await _limitProductDetailRepository.DeleteAsync(detail);
            }

            foreach (var item in lstDetail.Where(x => x.LimitProductId != null))
            {
                await _limitProductDetailRepository.InsertAsync(new LimitProductDetail()
                {
                    LimitProductId = limitProducts.Id,
                    LimitQuantity = item.LimitQuantity,
                    LimitAmount = item.LimitAmount,
                    ProductId = item.ProductId
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _limitProductRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Approval)]
        public async Task Approval(EntityDto input)
        {
            var limitProducts = await _limitProductRepository.FirstOrDefaultAsync(input.Id);
            if (limitProducts == null)
                throw new UserFriendlyException("Hạn mức bán hàng không tồn tại!");
            if (limitProducts.Status != CommonConst.LimitProductConfigStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            limitProducts.Status = CommonConst.LimitProductConfigStatus.Approved;
            limitProducts.ApproverId = AbpSession.UserId ?? 0;
            limitProducts.DateApproved = DateTime.Now;

            await _limitProductRepository.UpdateAsync(limitProducts);
        }

        [AbpAuthorize(AppPermissions.Pages_LimitProducts_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var limitProducts = await _limitProductRepository.FirstOrDefaultAsync(input.Id);
            if (limitProducts == null)
                throw new UserFriendlyException("Hạn mức bán hàng không tồn tại!");
            if (limitProducts.Status != CommonConst.LimitProductConfigStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ!");
            limitProducts.Status = CommonConst.LimitProductConfigStatus.Cancel;
            limitProducts.ApproverId = AbpSession.UserId ?? 0;
            limitProducts.DateApproved = DateTime.Now;

            await _limitProductRepository.UpdateAsync(limitProducts);
        }

        public async Task<FileDto> GetLimitProductsToExcel(GetAllLimitProductsForExcelInput input)
        {
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.LimitProductConfigStatus) input.StatusFilter
                : default;

            var filteredLimitProducts = _limitProductRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(input.FromDateFilter != null, e => e.FromDate >= input.FromDateFilter)
                .WhereIf(input.ToDateFilter != null, e => e.ToDate <= input.ToDateFilter)
                .WhereIf(input.AgentTypeFilter != null, e => e.AgentType == input.AgentTypeFilter)
                .WhereIf(input.AgentFilter > 0, e => e.UserId == input.AgentFilter)
                .WhereIf(
                    input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Pending 
                    || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Approved 
                    || input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Cancel, e => e.Status == statusFilter);
            
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.Applying)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now);
                
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.NotApply)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.FromDate > DateTime.Now);
                
                if (input.StatusFilter > -1 && input.StatusFilter == (int)CommonConst.LimitProductConfigStatus.StopApply)
                    filteredLimitProducts = filteredLimitProducts.Where(x => x.Status == CommonConst.LimitProductConfigStatus.Approved && x.ToDate < DateTime.Now);

            var query = (from o in filteredLimitProducts
                join o1 in _lookup_userRepository.GetAll() on o.CreatorUserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.ApproverId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new GetLimitProductForViewDto()
                {
                    LimitProduct = new LimitProductDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        FromDate = o.FromDate,
                        ToDate = o.ToDate,
                        DateApproved = o.DateApproved,
                        AgentType = o.AgentType,
                        Status = o.Status,
                        Id = o.Id,
                        CreationTime = o.CreationTime
                    },
                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    AgentName = s3 != null ? s3.AccountCode + " - " + s3.PhoneNumber + " - " + s3.FullName : "",
                    UserApproved = s2.UserName
                });
            
            var limitProductListDtos = await query.ToListAsync();

            return _limitProductsExcelExporter.ExportToFile(limitProductListDtos, L("LimitProducts"));
        }


        [AbpAuthorize(AppPermissions.Pages_LimitProducts)]
        public async Task<List<LimitProductUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new LimitProductUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_LimitProducts)]
        public async Task<List<LimitProductServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookup_serviceRepository.GetAll()
                .Select(service => new LimitProductServiceLookupTableDto
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
        
        public async Task<FileDto> GetDetailLimitProductsToExcel(GetDetailLimitProductsForExcelInput input)
        {
            var limitProduct = await _limitProductRepository.FirstOrDefaultAsync(input.LimitProductId);
            if (limitProduct != null)
            {
                if (input.LimitProductId > 0)
                {
                    var item = await _limitationManager.GetLimitProductsDetails(input.LimitProductId);

                    return _limitProductsExcelExporter.DetailLimitExportToFile(item, limitProduct.Name);
                }
            }

            return null;
        }

        private LimitProductDto ConvertStatus(LimitProductDto item)
        {
            if (item.Status == CommonConst.LimitProductConfigStatus.Pending)
            {
                item.StatusName = L("Enum_LimitProductConfigStatus_0");
                item.Status = CommonConst.LimitProductConfigStatus.Pending;
            }
            else
            {
                if (item.Status == CommonConst.LimitProductConfigStatus.Approved)
                {
                    if (item.FromDate <= DateTime.Now && item.ToDate >= DateTime.Now)
                    {
                        item.StatusName = L("Enum_LimitProductConfigStatus_2");
                        item.Status = CommonConst.LimitProductConfigStatus.Applying;
                    }

                    if (item.FromDate > DateTime.Now)
                    {
                        item.Status = CommonConst.LimitProductConfigStatus.NotApply;
                        item.StatusName = L("Enum_LimitProductConfigStatus_4");
                    }

                    if (item.ToDate < DateTime.Now)
                    {
                        item.Status = CommonConst.LimitProductConfigStatus.StopApply;
                        item.StatusName = L("Enum_LimitProductConfigStatus_5");
                    }
                }

                if (item.Status == CommonConst.LimitProductConfigStatus.Cancel)
                {
                    item.Status = CommonConst.LimitProductConfigStatus.Cancel;
                    item.StatusName = L("Enum_LimitProductConfigStatus_3");
                }

                if (item.Status == CommonConst.LimitProductConfigStatus.StopApply)
                {
                    item.Status = CommonConst.LimitProductConfigStatus.StopApply;
                    item.StatusName = L("Enum_LimitProductConfigStatus_5");
                }
            }

            return item;
        }
        
        public async Task<ResponseMessages> GetLimitProductImportList(List<LimitProductImportDto> dataList)
        {
            if (!dataList.Any())
            {
                return new ResponseMessages("00", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var query = from lst in dataList
                join prd in _productRepository.GetAll() on lst.ProductCode equals prd.ProductCode
                join cat in _lookup_categoryRepository.GetAll() on prd.CategoryFk.CategoryCode equals cat.CategoryCode
                join srv in _lookup_serviceRepository.GetAll() on cat.ServiceFk.ServiceCode equals srv.ServiceCode
                select new LimitProductImport
                {
                    ServiceName = srv.ServicesName,
                    ServiceCode = srv.ServiceCode,
                    CategoryName = cat.CategoryName,
                    CategoryCode = cat.CategoryCode,
                    ProductId = prd.Id,
                    ProductType = cat.CategoryName,
                    ProductName = prd.ProductName,
                    ProductCode = prd.ProductCode,
                    ProductValue = prd.ProductValue ?? 0,
                    LimitQuantity = lst.LimitQuantity ?? null,
                    LimitAmount = lst.LimitAmount ?? null
                };
            
            var limitProductList = query as LimitProductImport[] ?? query.ToArray();
            if (!limitProductList.Any())
            {
                return new ResponseMessages("00", "Kiểm tra lại thông tin dữ liệu không hợp lệ");
            }

            var rs = new ResponseMessages("01");
            rs.Payload = limitProductList.ToList();
            
            return rs;
        }
    }
}