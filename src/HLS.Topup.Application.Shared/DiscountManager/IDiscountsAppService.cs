using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Products.Dtos;
using JetBrains.Annotations;


namespace HLS.Topup.DiscountManager
{
    public interface IDiscountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountForViewDto>> GetAll(GetAllDiscountsInput input);

        Task<GetDiscountForViewDto> GetDiscountForView(int id);

        Task<GetDiscountForEditOutput> GetDiscountForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDiscountDto input);

        Task Delete(EntityDto input);
        Task Approval(EntityDto input);
        Task Cancel(EntityDto input);
        Task Stop(EntityDto input);

        Task<FileDto> GetDiscountsToExcel(GetAllDiscountsForExcelInput input);

        Task<List<DiscountUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<PagedResultDto<DiscountDetailDto>> GetDiscountDetailsTable(GetDiscountDetailTableInput input);

        Task<List<ProductCategoryLookupTableDto>> GetAllCategoryForTableDropdown();

        Task<List<DiscountServiceLookupTableDto>> GetAllServiceForTableDropdown();

        Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode);

        Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null);

        Task<ResponseMessages> GetDiscountImportList(List<DiscountImportDto> dataList);
    }
}
