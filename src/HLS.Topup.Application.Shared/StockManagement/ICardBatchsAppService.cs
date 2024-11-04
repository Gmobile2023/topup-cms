using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using System.Collections.Generic;


namespace HLS.Topup.StockManagement
{
    public interface ICardBatchsAppService : IApplicationService 
    {
        Task<PagedResultDto<CardBatchDto>> GetAll(GetAllCardBatchsInput input);

        Task<GetCardBatchForViewDto> GetCardBatchForView(Guid id);

		Task<GetCardBatchForEditOutput> GetCardBatchForEdit(Guid id);

		Task CreateOrEdit(CreateOrEditCardBatchDto input);

		Task Delete(Guid id);

		Task<FileDto> GetCardBatchsToExcel(GetAllCardBatchsForExcelInput input);
 
		
		Task<List<CardBatchProviderLookupTableDto>> GetAllProviderForTableDropdown();
		Task<List<CardBatchVendorLookupTableDto>> GetAllVendorForTableDropdown();
		
		Task<List<CardBatchCategoryLookupTableDto>> GetAllCategoryForTableDropdown();
		
    }
}