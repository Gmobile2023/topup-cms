using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using System.Collections.Generic;


namespace HLS.Topup.Categories
{
    public interface ICategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(int id);

		Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input);

		
		Task<List<CategoryCategoryLookupTableDto>> GetAllCategoryForTableDropdown();
		
		Task<List<CategoryServiceLookupTableDto>> GetAllServiceForTableDropdown();
		
    }
}