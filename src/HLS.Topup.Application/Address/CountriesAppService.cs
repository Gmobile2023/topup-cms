
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
	[AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : TopupAppServiceBase, ICountriesAppService
    {
		 private readonly IRepository<Country> _countryRepository;
		 private readonly ICountriesExcelExporter _countriesExcelExporter;


		  public CountriesAppService(IRepository<Country> countryRepository, ICountriesExcelExporter countriesExcelExporter )
		  {
			_countryRepository = countryRepository;
			_countriesExcelExporter = countriesExcelExporter;

		  }

		 public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (CommonConst.CountryStatus) input.StatusFilter
                        : default;

			var filteredCountries = _countryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CountryCode.Contains(input.Filter) || e.CountryName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter),  e => e.CountryCode == input.CountryCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter),  e => e.CountryName == input.CountryNameFilter)
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

			var pagedAndFilteredCountries = filteredCountries
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.CountryName)
                .PageBy(input);

			var countries = from o in pagedAndFilteredCountries
                         select new GetCountryForViewDto() {
							Country = new CountryDto
							{
                                CountryCode = o.CountryCode,
                                CountryName = o.CountryName,
                                Status = o.Status,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCountries.CountAsync();

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                await countries.ToListAsync()
            );
         }

		 public async Task<GetCountryForViewDto> GetCountryForView(int id)
         {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
		 public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

		    var output = new GetCountryForEditOutput {Country = ObjectMapper.Map<CreateOrEditCountryDto>(country)};

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCountryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
		 protected virtual async Task Create(CreateOrEditCountryDto input)
         {
            var country = ObjectMapper.Map<Country>(input);


			if (AbpSession.TenantId != null)
			{
				country.TenantId = (int?) AbpSession.TenantId;
			}


            await _countryRepository.InsertAsync(country);
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
		 protected virtual async Task Update(CreateOrEditCountryDto input)
         {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, country);
         }

		 [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _countryRepository.DeleteAsync(input.Id);
         }

		public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (CommonConst.CountryStatus) input.StatusFilter
                        : default;

			var filteredCountries = _countryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CountryCode.Contains(input.Filter) || e.CountryName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter),  e => e.CountryCode == input.CountryCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter),  e => e.CountryName == input.CountryNameFilter)
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

			var query = (from o in filteredCountries
                         select new GetCountryForViewDto() {
							Country = new CountryDto
							{
                                CountryCode = o.CountryCode,
                                CountryName = o.CountryName,
                                Status = o.Status,
                                Id = o.Id
							}
						 });


            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
         }


    }
}
