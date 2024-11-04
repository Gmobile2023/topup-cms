using HLS.Topup.Address;
using System.Collections.Generic;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Address.Exporting;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Address
{
    [AbpAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsAppService : TopupAppServiceBase, IDistrictsAppService
    {
        private readonly IRepository<District> _districtRepository;
        private readonly IDistrictsExcelExporter _districtsExcelExporter;
        private readonly IRepository<City, int> _lookup_cityRepository;


        public DistrictsAppService(IRepository<District> districtRepository,
            IDistrictsExcelExporter districtsExcelExporter, IRepository<City, int> lookup_cityRepository)
        {
            _districtRepository = districtRepository;
            _districtsExcelExporter = districtsExcelExporter;
            _lookup_cityRepository = lookup_cityRepository;
        }

        public async Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DistrictStatus) input.StatusFilter
                : default;

            var filteredDistricts = _districtRepository.GetAll()
                .Include(e => e.CityFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.DistrictCode.Contains(input.Filter) || e.DistrictName.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictCodeFilter),
                    e => e.DistrictCode == input.DistrictCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter),
                    e => e.DistrictName == input.DistrictNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CityCityNameFilter),
                    e => e.CityFk != null && e.CityFk.CityName == input.CityCityNameFilter);

            var pagedAndFilteredDistricts = filteredDistricts
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.CityFk.CityName)
                .PageBy(input);

            var districts = from o in pagedAndFilteredDistricts
                join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetDistrictForViewDto()
                {
                    District = new DistrictDto
                    {
                        DistrictCode = o.DistrictCode,
                        DistrictName = o.DistrictName,
                        Status = o.Status,
                        Id = o.Id
                    },
                    CityCityName = s1 == null || s1.CityName == null ? "" : s1.CityName.ToString()
                };

            var totalCount = await filteredDistricts.CountAsync();

            return new PagedResultDto<GetDistrictForViewDto>(
                totalCount,
                await districts.ToListAsync()
            );
        }

        public async Task<GetDistrictForViewDto> GetDistrictForView(int id)
        {
            var district = await _districtRepository.GetAsync(id);

            var output = new GetDistrictForViewDto {District = ObjectMapper.Map<DistrictDto>(district)};

            if (output.District.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int) output.District.CityId);
                output.CityCityName = _lookupCity?.CityName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        public async Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto input)
        {
            var district = await _districtRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDistrictForEditOutput {District = ObjectMapper.Map<CreateOrEditDistrictDto>(district)};

            if (output.District.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int) output.District.CityId);
                output.CityCityName = _lookupCity?.CityName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDistrictDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Districts_Create)]
        protected virtual async Task Create(CreateOrEditDistrictDto input)
        {
            var district = ObjectMapper.Map<District>(input);


            if (AbpSession.TenantId != null)
            {
                district.TenantId = (int?) AbpSession.TenantId;
            }


            await _districtRepository.InsertAsync(district);
        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        protected virtual async Task Update(CreateOrEditDistrictDto input)
        {
            var district = await _districtRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, district);
        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _districtRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DistrictStatus) input.StatusFilter
                : default;

            var filteredDistricts = _districtRepository.GetAll()
                .Include(e => e.CityFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.DistrictCode.Contains(input.Filter) || e.DistrictName.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictCodeFilter),
                    e => e.DistrictCode == input.DistrictCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter),
                    e => e.DistrictName == input.DistrictNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CityCityNameFilter),
                    e => e.CityFk != null && e.CityFk.CityName == input.CityCityNameFilter);

            var query = (from o in filteredDistricts
                join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetDistrictForViewDto()
                {
                    District = new DistrictDto
                    {
                        DistrictCode = o.DistrictCode,
                        DistrictName = o.DistrictName,
                        Status = o.Status,
                        Id = o.Id
                    },
                    CityCityName = s1 == null || s1.CityName == null ? "" : s1.CityName.ToString()
                });


            var districtListDtos = await query.ToListAsync();

            return _districtsExcelExporter.ExportToFile(districtListDtos);
        }
        
        [AbpAuthorize(AppPermissions.Pages_Districts)]
        public async Task<List<DistrictCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new DistrictCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city == null || city.CityName == null ? "" : city.CityName.ToString()
                }).ToListAsync();
        }
        
        [AbpAuthorize(AppPermissions.Pages_Districts)]
        public async Task<List<DistrictCityLookupTableDto>> GetDistrictByCity(int cityId)
        {
            return await _districtRepository.GetAll().Where(x => x.CityId == cityId)
                .Select(district => new DistrictCityLookupTableDto
                {
                    Id = district.Id,
                    DisplayName = district == null || district.DistrictName == null ? "" : district.DistrictName.ToString()
                }).ToListAsync();
        }
    }
}
