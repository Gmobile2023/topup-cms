using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.Products
{
    public interface IProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(int id);

		Task<GetProductForEditOutput> GetProductForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProductDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

		
		Task<List<ProductCategoryLookupTableDto>> GetAllCategoryForTableDropdown();
		
    }
}