using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Dto;
using HLS.Topup.StockManagement.Dtos;


namespace HLS.Topup.Providers
{
    public interface IProvidersAppService : IApplicationService
    {
        Task<PagedResultDto<GetProviderForViewDto>> GetAll(GetAllProvidersInput input);

        Task<GetProviderForViewDto> GetProviderForView(int id);

        Task<GetProviderForEditOutput> GetProviderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProviderDto input);
        Task Lock(EntityDto<string> input);
        Task UnLock(EntityDto<string> input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProvidersToExcel(GetAllProvidersForExcelInput input);
        Task<List<CommonLookupTableDto>> GetAllProvider();
    }
}
