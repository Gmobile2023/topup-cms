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
    [AbpAuthorize(AppPermissions.Pages_Wards)]
    public class WardsAppService : TopupAppServiceBase, IWardsAppService
    {
        private readonly IRepository<Ward> _wardRepository;
        private readonly IWardsExcelExporter _wardsExcelExporter;
        private readonly IRepository<District, int> _lookup_districtRepository;


        public WardsAppService(IRepository<Ward> wardRepository, IWardsExcelExporter wardsExcelExporter,
            IRepository<District, int> lookup_districtRepository)
        {
            _wardRepository = wardRepository;
            _wardsExcelExporter = wardsExcelExporter;
            _lookup_districtRepository = lookup_districtRepository;
        }

        public async Task<PagedResultDto<GetWardForViewDto>> GetAll(GetAllWardsInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.WardStatus) input.StatusFilter
                : default;

            var filteredWards = _wardRepository.GetAll()
                .Include(e => e.DistrictFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.WardCode.Contains(input.Filter) || e.WardName.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.WardCodeFilter), e => e.WardCode == input.WardCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.WardNameFilter), e => e.WardName == input.WardNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictDistrictNameFilter),
                    e => e.DistrictFk != null && e.DistrictFk.DistrictName == input.DistrictDistrictNameFilter);

            var pagedAndFilteredWards = filteredWards
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.WardName)
                .PageBy(input);

            var wards = from o in pagedAndFilteredWards
                join o1 in _lookup_districtRepository.GetAll() on o.DistrictId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetWardForViewDto()
                {
                    Ward = new WardDto
                    {
                        WardCode = o.WardCode,
                        WardName = o.WardName,
                        Status = o.Status,
                        Id = o.Id
                    },
                    DistrictDistrictName = s1 == null || s1.DistrictName == null ? "" : s1.DistrictName.ToString()
                };

            var totalCount = await filteredWards.CountAsync();

            return new PagedResultDto<GetWardForViewDto>(
                totalCount,
                await wards.ToListAsync()
            );
        }

        public async Task<GetWardForViewDto> GetWardForView(int id)
        {
            var ward = await _wardRepository.GetAsync(id);

            var output = new GetWardForViewDto {Ward = ObjectMapper.Map<WardDto>(ward)};

            if (output.Ward.DistrictId != null)
            {
                var _lookupDistrict =
                    await _lookup_districtRepository.FirstOrDefaultAsync((int) output.Ward.DistrictId);
                output.DistrictDistrictName = _lookupDistrict?.DistrictName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Wards_Edit)]
        public async Task<GetWardForEditOutput> GetWardForEdit(EntityDto input)
        {
            var ward = await _wardRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWardForEditOutput {Ward = ObjectMapper.Map<CreateOrEditWardDto>(ward)};

            if (output.Ward.DistrictId != null)
            {
                var _lookupDistrict =
                    await _lookup_districtRepository.FirstOrDefaultAsync((int) output.Ward.DistrictId);
                output.DistrictDistrictName = _lookupDistrict?.DistrictName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWardDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Wards_Create)]
        protected virtual async Task Create(CreateOrEditWardDto input)
        {
            var ward = ObjectMapper.Map<Ward>(input);


            if (AbpSession.TenantId != null)
            {
                ward.TenantId = (int?) AbpSession.TenantId;
            }


            await _wardRepository.InsertAsync(ward);
        }

        [AbpAuthorize(AppPermissions.Pages_Wards_Edit)]
        protected virtual async Task Update(CreateOrEditWardDto input)
        {
            var ward = await _wardRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, ward);
        }

        [AbpAuthorize(AppPermissions.Pages_Wards_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _wardRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetWardsToExcel(GetAllWardsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.WardStatus) input.StatusFilter
                : default;

            var filteredWards = _wardRepository.GetAll()
                .Include(e => e.DistrictFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.WardCode.Contains(input.Filter) || e.WardName.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.WardCodeFilter), e => e.WardCode == input.WardCodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.WardNameFilter), e => e.WardName == input.WardNameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictDistrictNameFilter),
                    e => e.DistrictFk != null && e.DistrictFk.DistrictName == input.DistrictDistrictNameFilter);

            var query = (from o in filteredWards
                join o1 in _lookup_districtRepository.GetAll() on o.DistrictId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetWardForViewDto()
                {
                    Ward = new WardDto
                    {
                        WardCode = o.WardCode,
                        WardName = o.WardName,
                        Status = o.Status,
                        Id = o.Id
                    },
                    DistrictDistrictName = s1 == null || s1.DistrictName == null ? "" : s1.DistrictName.ToString()
                });


            var wardListDtos = await query.ToListAsync();

            return _wardsExcelExporter.ExportToFile(wardListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Wards)]
        public async Task<List<WardDistrictLookupTableDto>> GetAllDistrictForTableDropdown()
        {
            return await _lookup_districtRepository.GetAll()
                .Select(district => new WardDistrictLookupTableDto
                {
                    Id = district.Id,
                    DisplayName = district == null || district.DistrictName == null
                        ? ""
                        : district.DistrictName.ToString()
                }).ToListAsync();
        }
        
        [AbpAuthorize(AppPermissions.Pages_Wards)]
        public async Task<List<WardDistrictLookupTableDto>> GetWardByDistrict(int districtId)
        {
            return await _wardRepository.GetAll().Where(x => x.DistrictId == districtId)
                .Select(ward => new WardDistrictLookupTableDto
                {
                    Id = ward.Id,
                    DisplayName = ward == null || ward.WardName == null
                        ? ""
                        : ward.WardName.ToString()
                }).ToListAsync();
        }
        public async Task<GetWardForViewDto> GetTestWard()
        {
            System.Threading.Thread.Sleep(500000);
            var ward = await _wardRepository.GetAsync(100);

            var output = new GetWardForViewDto {Ward = ObjectMapper.Map<WardDto>(ward)};

            if (output.Ward.DistrictId != null)
            {
                var _lookupDistrict =
                    await _lookup_districtRepository.FirstOrDefaultAsync((int) output.Ward.DistrictId);
                output.DistrictDistrictName = _lookupDistrict?.DistrictName?.ToString();
            }

            return output;
        }
    }
}