using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using HLS.Topup.Products.Dtos;

namespace HLS.Topup.FeeManager
{
    public interface IFeesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFeeForViewDto>> GetAll(GetAllFeesInput input);

        Task<GetFeeForViewDto> GetFeeForView(int id);

		Task<GetFeeForEditOutput> GetFeeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditFeeDto input);

		Task Delete(EntityDto input);
		Task Stop(EntityDto input);
		Task Cancel(EntityDto input);
		Task Approval(EntityDto input);

		Task<FileDto> GetFeesToExcel(GetAllFeesForExcelInput input);

		Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null);
		Task<List<FeeUserLookupTableDto>> GetAllUserForTableDropdown();

		Task<List<FeeLookupTableDto>> GetCategories();

		Task<ResponseMessages> GetFeeImportList(List<FeeImportDto> dataList);
    }
}
