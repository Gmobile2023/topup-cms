using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Dtos.Sale;

namespace HLS.Topup.Address
{
    public interface IAddressManager
    {
        Task<List<City>> GetListCitiesCacheAsync();
        Task<List<City>> GetCitiesCacheAsync(string cityName);
        Task<City> GetCityCacheAsync(int cityId);
        Task<City> GetCityCacheAsync(string citycode);


        Task<List<District>> GetListDistrictAllCacheAsync();
        Task<List<District>> GetDistrictByCityCacheAsync(int cityId);
        Task<List<District>> GetDistrictByCityCacheAsync(string citycode);
        Task<District> GetDistrictByNameCacheAsync(int cityId, string districtName);
        Task<District> GetDistrictByNameCacheAsync(string citycode, string districtName);
        Task<District> GetDistrictByIdCacheAsync(int id);
        Task<District> GetDistrictByCodeCacheAsync(string districtCode);


        Task<List<Ward>> GetListWardsCacheAsync();
        Task<Ward> GetWardByNameCacheAsync(int districtId, string nameWard);
        Task<Ward> GetWardByNameCacheAsync(string districtcode, string nameWard);
        Task<List<Ward>> GetWardByDistrictCacheAsync(int districtId);
        Task<List<Ward>> GetWardByDistrictCacheAsync(string districtcode);
        Task<Ward> GetWardByIdCacheAsync(int wardId);
        Task<Ward> GetWardByCodeCacheAsync(string wardcode);
        Task<List<AddressSaleDto>> GetAddressFullSale(List<int> wardIds);
    }
}
