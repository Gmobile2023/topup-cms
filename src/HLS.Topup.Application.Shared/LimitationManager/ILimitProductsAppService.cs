using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;


namespace HLS.Topup.LimitationManager
{
    public interface ILimitProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLimitProductForViewDto>> GetAll(GetAllLimitProductsInput input);

        Task<GetLimitProductForViewDto> GetLimitProductForView(int id);

		Task<GetLimitProductForEditOutput> GetLimitProductForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLimitProductDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLimitProductsToExcel(GetAllLimitProductsForExcelInput input);
		
		Task<List<LimitProductUserLookupTableDto>> GetAllUserForTableDropdown();

		Task<List<LimitProductServiceLookupTableDto>> GetAllServiceForTableDropdown();

		Task<FileDto> GetDetailLimitProductsToExcel(GetDetailLimitProductsForExcelInput input);

		Task<ResponseMessages> GetLimitProductImportList(List<LimitProductImportDto> dataList);
    }
}