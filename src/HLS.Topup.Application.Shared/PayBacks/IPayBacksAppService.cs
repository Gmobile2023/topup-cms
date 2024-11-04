using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Dto;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.PayBacks
{
    public interface IPayBacksAppService : IApplicationService
    {
        Task<PagedResultDto<GetPayBackForViewDto>> GetAll(GetAllPayBacksInput input);

        Task<GetPayBackForViewDto> GetPayBacksForView(int id);

        Task<GetPayBackForEditOutput> GetPayBacksForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPayBacksDto input);

        Task Delete(EntityDto input);
        Task Approval(int id);
        Task Cancel(EntityDto input);

        Task<List<PayBacksCategoryLookupTableDto>> GetAllCategoryForTableDropdown();

        Task<List<PayBacksProviderLookupTableDto>> GetAllProviderForTableDropdown();

        Task<ResponseMessages> GetPayBacksImportList(List<PayBacksImportDto> dataList);


        Task<FileDto> GetPayBacksToExcel(GetAllPayBacksForExcelInput input);
    }
}
