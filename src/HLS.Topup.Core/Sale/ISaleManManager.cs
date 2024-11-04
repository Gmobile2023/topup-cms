using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Sale;

namespace HLS.Topup.Sale
{
    public interface ISaleManManager
    {
        Task CreateSale(CreateOrUpdateSaleDto input);
        Task UpdateSale(CreateOrUpdateSaleDto input);

        Task<List<AddressSaleItemDto>> GetListCitySale(long? userId = null, long? saleLeadId = null);

        Task<List<AddressSaleItemDto>> GetListDistrictSale(int? cityId = null, long? userId = null,
            long? saleLeadId = null);

        Task<List<AddressSaleItemDto>> GetListWardSale(int? districtId = null, long? userId = null,
            long? saleLeadId = null);

        Task<CreateOrUpdateSaleDto> GetSaleInfo(long userId);

        Task<List<UserInfoDto>> GetUserSaleLeader(UserInfoSearch search);

        Task<List<UserInfoDto>> GetUserSaleBySaleLeader(UserInfoSearch search);

        Task<List<UserInfoDto>> GetUserAgentBy(UserInfoSearch search);

        Task<List<UserInfoDto>> GetUserAgentStaff(UserInfoSearch search);

        Task<List<UserInfoDto>> GetAgentLocationSale(long saleId, string search);

        Task<AddressSaleSelected> GetAddressSelected(long userId);

        Task<UserInfoDto> GetUserSaleLeaderBySale(int userId);

        Task<UserInfoSaleDto> GetSaleAssignInfo(int userId);

        Task<AddressSaleSelected> GetAddressSelected(long? userId = null, long? saleLeadId = null,
            int? cityId = null, int? districtId = null, int? wardId = null);  
        
        Task<UserInfoDto> GetSaleAssignDetail(int userId);

        Task<List<UserInfoDto>> GetUserAgentLevel(UserInfoSearch search);
    }
}
