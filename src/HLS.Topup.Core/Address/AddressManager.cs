using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Sale;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Address
{
    public class AddressManager : TopupDomainServiceBase, IAddressManager
    {
        private readonly IRepository<City> _cityRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;

        public AddressManager(ICacheManager cacheManager, IRepository<City> cityRepository,
            IRepository<District> districtRepository, IRepository<Ward> wardRepository)
        {
            _cacheManager = cacheManager;
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
        }

        public async Task<List<City>> GetListCitiesCacheAsync()
        {
            return await _cacheManager.GetCache("SystemAddress").AsTyped<string,List<City>>().GetAsync($"GetListCity", async () =>
            {
                var city = _cityRepository.GetAll().Where(x => x.Status == CommonConst.CityStatus.Active);
                return await city.OrderBy(c => c.CityName).ToListAsync();
            });
        }

        public async Task<List<City>> GetCitiesCacheAsync(string cityName)
        {
            var list = await GetListCitiesCacheAsync();
            return list.Where(p => p.CityName.ToLower().Contains(cityName.ToLower())).ToList();
        }

        public async Task<City> GetCityCacheAsync(int cityId)
        {
            var list = await GetListCitiesCacheAsync();
            return list.FirstOrDefault(x => x.Id == cityId);
        }

        public async Task<City> GetCityCacheAsync(string citycode)
        {
            var list = await GetListCitiesCacheAsync();
            return list.FirstOrDefault(x => x.CityCode == citycode);
        }

        public async Task<List<District>> GetListDistrictAllCacheAsync()
        {
            return await _cacheManager.GetCache("SystemAddress").AsTyped<string,List<District>>().GetAsync($"GetListDistrict", async () =>
            {
                var districts = _districtRepository.GetAllIncluding(x => x.CityFk)
                    .Where(x => x.Status == CommonConst.DistrictStatus.Active).OrderBy(p => p.DistrictName);
                var districtList = await districts.ToListAsync();
                return ObjectMapper.Map<List<District>>(districtList);
            });
        }

        public async Task<List<District>> GetDistrictByCityCacheAsync(int cityId)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.Where(p => p.CityId == cityId).ToList();
        }

        public async Task<List<District>> GetDistrictByCityCacheAsync(string citycode)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.Where(p => p.CityFk.CityCode == citycode).ToList();
        }

        public async Task<District> GetDistrictByNameCacheAsync(int cityId, string districtName)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.FirstOrDefault(p => p.CityId == cityId && p.DistrictName.Contains(districtName));
        }

        public async Task<District> GetDistrictByNameCacheAsync(string citycode, string districtName)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.FirstOrDefault(p => p.CityFk.CityCode == citycode && p.DistrictName.Contains(districtName));
        }

        public async Task<District> GetDistrictByIdCacheAsync(int id)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.FirstOrDefault(p => p.Id == id);
        }

        public async Task<District> GetDistrictByCodeCacheAsync(string districtCode)
        {
            var lst = await GetListDistrictAllCacheAsync();
            return lst.FirstOrDefault(p =>
                string.Equals(p.DistrictCode, districtCode.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<List<Ward>> GetListWardsCacheAsync()
        {
            return await _cacheManager.GetCache("SystemAddress").AsTyped<string,List<Ward>>().GetAsync($"GetListWards", async () =>
            {
                var ward = _wardRepository.GetAllIncluding(x => x.DistrictFk);
                return await ward.OrderBy(c => c.WardName).ToListAsync();
            });
        }

        public async Task<Ward> GetWardByNameCacheAsync(int districtId, string nameWard)
        {
            var list = await GetListWardsCacheAsync();
            return list.FirstOrDefault(
                p => p.DistrictId == districtId && p.WardName.Contains(nameWard.Trim().ToLower()));
        }

        public async Task<Ward> GetWardByNameCacheAsync(string districtcode, string nameWard)
        {
            var list = await GetListWardsCacheAsync();
            return list.FirstOrDefault(p =>
                p.DistrictFk.DistrictCode == districtcode && p.WardName.Contains(nameWard.Trim().ToLower()));
        }

        public async Task<List<Ward>> GetWardByDistrictCacheAsync(int districtId)
        {
            var lst = await GetListWardsCacheAsync();
            return lst.Where(p => p.DistrictId == districtId).ToList();
        }

        public async Task<List<Ward>> GetWardByDistrictCacheAsync(string districtcode)
        {
            var lst = await GetListWardsCacheAsync();
            return lst.Where(p => p.DistrictFk.DistrictCode == districtcode).ToList();
        }

        public async Task<Ward> GetWardByIdCacheAsync(int wardId)
        {
            var list = await GetListWardsCacheAsync();
            return list.FirstOrDefault(p => p.Id == wardId);
        }

        public async Task<Ward> GetWardByCodeCacheAsync(string wardcode)
        {
            var list = await GetListWardsCacheAsync();
            return list.FirstOrDefault(p => p.WardCode.ToLower() == wardcode.Trim().ToLower());
        }

        public async Task<List<AddressSaleDto>> GetAddressFullSale(List<int> wardIds)
        {
            var query = from w in await GetListWardsCacheAsync()
                join d in await GetListDistrictAllCacheAsync() on w.DistrictId equals d.Id
                join c in await GetListCitiesCacheAsync() on d.CityId equals c.Id
                select new AddressSaleDto
                {
                    CityId = c.Id,
                    DistrictId = d.Id,
                    WardId = w.Id
                };
            return query.Where(x=>wardIds.Contains(x.WardId)).ToList();
        }
    }
}
