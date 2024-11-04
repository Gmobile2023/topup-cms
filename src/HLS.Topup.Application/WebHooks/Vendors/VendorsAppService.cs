

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Vendors.Exporting;
using HLS.Topup.Vendors.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Vendors
{
	[AbpAuthorize(AppPermissions.Pages_Vendors)]
    public class VendorsAppService : TopupAppServiceBase, IVendorsAppService
    {
		 private readonly IRepository<Vendor> _vendorRepository;
		 private readonly IVendorsExcelExporter _vendorsExcelExporter;
		 

		  public VendorsAppService(IRepository<Vendor> vendorRepository, IVendorsExcelExporter vendorsExcelExporter ) 
		  {
			_vendorRepository = vendorRepository;
			_vendorsExcelExporter = vendorsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetVendorForViewDto>> GetAll(GetAllVendorsInput input)
         {
			
			var filteredVendors = _vendorRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Print_Help.Contains(input.Filter) || e.Print_Suport.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.HotLine.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Print_HelpFilter),  e => e.Print_Help == input.Print_HelpFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Print_SuportFilter),  e => e.Print_Suport == input.Print_SuportFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter),  e => e.Address == input.AddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HotLineFilter),  e => e.HotLine == input.HotLineFilter);

			var pagedAndFilteredVendors = filteredVendors
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.Name)
                .PageBy(input);

			var vendors = from o in pagedAndFilteredVendors
                         select new GetVendorForViewDto() {
							Vendor = new VendorDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Print_Help = o.Print_Help,
                                Print_Suport = o.Print_Suport,
                                Status = o.Status,
                                Address = o.Address,
                                HotLine = o.HotLine,
                                Id = o.Id
							}
						};

            var totalCount = await filteredVendors.CountAsync();

            return new PagedResultDto<GetVendorForViewDto>(
                totalCount,
                await vendors.ToListAsync()
            );
         }
		 
		 public async Task<GetVendorForViewDto> GetVendorForView(int id)
         {
            var vendor = await _vendorRepository.GetAsync(id);

            var output = new GetVendorForViewDto { Vendor = ObjectMapper.Map<VendorDto>(vendor) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Vendors_Edit)]
		 public async Task<GetVendorForEditOutput> GetVendorForEdit(EntityDto input)
         {
            var vendor = await _vendorRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetVendorForEditOutput {Vendor = ObjectMapper.Map<CreateOrEditVendorDto>(vendor)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditVendorDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Vendors_Create)]
		 protected virtual async Task Create(CreateOrEditVendorDto input)
         {
            var vendor = ObjectMapper.Map<Vendor>(input);

			
			if (AbpSession.TenantId != null)
			{
				vendor.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _vendorRepository.InsertAsync(vendor);
         }

		 [AbpAuthorize(AppPermissions.Pages_Vendors_Edit)]
		 protected virtual async Task Update(CreateOrEditVendorDto input)
         {
            var vendor = await _vendorRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, vendor);
         }

		 [AbpAuthorize(AppPermissions.Pages_Vendors_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _vendorRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetVendorsToExcel(GetAllVendorsForExcelInput input)
         {
			
			var filteredVendors = _vendorRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Print_Help.Contains(input.Filter) || e.Print_Suport.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.HotLine.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Print_HelpFilter),  e => e.Print_Help == input.Print_HelpFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Print_SuportFilter),  e => e.Print_Suport == input.Print_SuportFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter),  e => e.Address == input.AddressFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.HotLineFilter),  e => e.HotLine == input.HotLineFilter);

			var query = (from o in filteredVendors
                         select new GetVendorForViewDto() { 
							Vendor = new VendorDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Print_Help = o.Print_Help,
                                Print_Suport = o.Print_Suport,
                                Status = o.Status,
                                Address = o.Address,
                                HotLine = o.HotLine,
                                Id = o.Id
							}
						 });


            var vendorListDtos = await query.ToListAsync();

            return _vendorsExcelExporter.ExportToFile(vendorListDtos);
         }


    }
}