using HLS.Topup.Categories;
using System.Collections.Generic;
using HLS.Topup.Services;
using HLS.Topup.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Categories.Exporting;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Categories
{
    [AbpAuthorize(AppPermissions.Pages_Categories)]
    public class CategoriesAppService : TopupAppServiceBase, ICategoriesAppService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ICategoriesExcelExporter _categoriesExcelExporter;
        private readonly IRepository<Category, int> _lookup_categoryRepository;
        private readonly IRepository<Service, int> _lookup_serviceRepository;
        private readonly UrlExtentions _extentions;

        public CategoriesAppService(IRepository<Category> categoryRepository,
            ICategoriesExcelExporter categoriesExcelExporter, IRepository<Category, int> lookup_categoryRepository,
            IRepository<Service, int> lookup_serviceRepository, UrlExtentions extentions)
        {
            _categoryRepository = categoryRepository;
            _categoriesExcelExporter = categoriesExcelExporter;
            _lookup_categoryRepository = lookup_categoryRepository;
            _lookup_serviceRepository = lookup_serviceRepository;
            _extentions = extentions;
        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.CategoryStatus) input.StatusFilter
                : default;
            var typeFilter = input.TypeFilter.HasValue
                ? (CommonConst.CategoryType) input.TypeFilter
                : default;

            var filteredCategories = _categoryRepository.GetAll()
                .Include(e => e.ParentCategoryFk)
                .Include(e => e.ServiceFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.CategoryCode.Contains(input.Filter) || e.CategoryName.Contains(input.Filter) ||
                         e.Image.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCodeFilter),
                    e => e.CategoryCode.Contains(input.CategoryCodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter),
                    e => e.CategoryName.Contains(input.CategoryNameFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.ParentCategoryFk != null &&
                         e.ParentCategoryFk.CategoryName.Contains(input.CategoryCategoryNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                    e => e.ServiceFk != null && e.ServiceFk.ServicesName.Contains(input.ServiceServicesNameFilter));

            var pagedAndFilteredCategories = filteredCategories
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.ServiceFk.ServicesName)
                .PageBy(input);

            var categories = from o in pagedAndFilteredCategories
                join o1 in _lookup_categoryRepository.GetAll() on o.ParentCategoryId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {
                        CategoryCode = o.CategoryCode,
                        CategoryName = o.CategoryName,
                        Order = o.Order,
                        Status = o.Status,
                        Type = o.Type,
                        Id = o.Id,
                    },
                    CategoryCategoryName = s1 == null || s1.CategoryName == null ? "" : s1.CategoryName.ToString(),
                    ServiceServicesName = s2 == null || s2.ServicesName == null ? "" : s2.ServicesName.ToString()
                };

            var totalCount = await filteredCategories.CountAsync();

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                await categories.ToListAsync()
            );
        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto {Category = ObjectMapper.Map<CategoryDto>(category)};
            output.Category.Image = _extentions.GetFullPath(output.Category.Image);
            if (output.Category.ParentCategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int) output.Category.ParentCategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            if (output.Category.ServiceId != null)
            {
                var _lookupService =
                    await _lookup_serviceRepository.FirstOrDefaultAsync((int) output.Category.ServiceId);
                output.ServiceServicesName = _lookupService?.ServicesName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput {Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category)};
            output.Category.Image = _extentions.GetFullPath(output.Category.Image);
            if (output.Category.ParentCategoryId != null)
            {
                var _lookupCategory =
                    await _lookup_categoryRepository.FirstOrDefaultAsync((int) output.Category.ParentCategoryId);
                output.CategoryCategoryName = _lookupCategory?.CategoryName?.ToString();
            }

            if (output.Category.ServiceId != null)
            {
                var _lookupService =
                    await _lookup_serviceRepository.FirstOrDefaultAsync((int) output.Category.ServiceId);
                output.ServiceServicesName = _lookupService?.ServicesName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Categories_Create)]
        protected virtual async Task Create(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);
            
            if (AbpSession.TenantId != null)
            {
                category.TenantId = (int?) AbpSession.TenantId;
            }

            await _categoryRepository.InsertAsync(category);
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, category);
        }

        [AbpAuthorize(AppPermissions.Pages_Categories_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.CategoryStatus) input.StatusFilter
                : default;
            var typeFilter = input.TypeFilter.HasValue
                ? (CommonConst.CategoryType) input.TypeFilter
                : default;

            var filteredCategories = _categoryRepository.GetAll()
                .Include(e => e.ParentCategoryFk)
                .Include(e => e.ServiceFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.CategoryCode.Contains(input.Filter) || e.CategoryName.Contains(input.Filter) ||
                         e.Image.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCodeFilter),
                    e => e.CategoryCode == input.CategoryCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter),
                    e => e.CategoryName == input.CategoryNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryCategoryNameFilter),
                    e => e.ParentCategoryFk != null &&
                         e.ParentCategoryFk.CategoryName == input.CategoryCategoryNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ServiceServicesNameFilter),
                    e => e.ServiceFk != null && e.ServiceFk.ServicesName == input.ServiceServicesNameFilter);

            var query = (from o in filteredCategories
                join o1 in _lookup_categoryRepository.GetAll() on o.ParentCategoryId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_serviceRepository.GetAll() on o.ServiceId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {
                        CategoryCode = o.CategoryCode,
                        CategoryName = o.CategoryName,
                        Order = o.Order,
                        Status = o.Status,
                        Type = o.Type,
                        Id = o.Id,
                    },
                    CategoryCategoryName = s1 == null || s1.CategoryName == null ? "" : s1.CategoryName.ToString(),
                    ServiceServicesName = s2 == null || s2.ServicesName == null ? "" : s2.ServicesName.ToString()
                });
            
            var categoryListDtos = await query.ToListAsync();

            return _categoriesExcelExporter.ExportToFile(categoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Categories)]
        public async Task<List<CategoryCategoryLookupTableDto>> GetAllCategoryForTableDropdown()
        {
            return await _lookup_categoryRepository.GetAll()
                .Select(category => new CategoryCategoryLookupTableDto
                {
                    Id = category.Id,
                    DisplayName = category == null || category.CategoryName == null
                        ? ""
                        : category.CategoryName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Categories)]
        public async Task<List<CategoryServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookup_serviceRepository.GetAll()
                .Select(service => new CategoryServiceLookupTableDto
                {
                    Id = service.Id,
                    DisplayName = service == null || service.ServicesName == null ? "" : service.ServicesName.ToString()
                }).ToListAsync();
        }
    }
}
