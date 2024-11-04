using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using HLS.Topup.Dtos.Sale;


namespace HLS.Topup.Sale
{
    public interface ISaleMansAppService : IApplicationService
    {
        Task<PagedResultDto<GetSaleManForViewDto>> GetAll(GetAllSaleMansInput input);

        Task<SaleManDto> GetSaleManForView(long id);

        Task<CreateOrUpdateSaleDto> GetSaleManForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrUpdateSaleDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSaleMansToExcel(GetAllSaleMansInput input);


        Task<List<SaleManUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<AddressSaleItemDto>> GetCities(long? userId = null,
            long? saleLeadId = null);

        Task<List<AddressSaleItemDto>> GetWards(int? districtId = null, long? userId = null,
            long? saleLeadId = null);

        Task<List<AddressSaleItemDto>> GetDistricts(int? cityId = null, long? userId = null,
            long? saleLeadId = null);

        Task<AddressSaleSelected> GetAddressSelected(long? userId = null, long? saleLeadId = null,
            int? cityId = null, int? districtId = null, int? wardId = null);
    }
}
