using HLS.Topup.Authorization.Users;
using System.Collections.Generic;
using HLS.Topup.Address;
using HLS.Topup.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Sale.Exporting;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using HLS.Topup.Dtos.Sale;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace HLS.Topup.Sale
{
    [AbpAuthorize(AppPermissions.Pages_SaleMans)]
    public class SaleMansAppService : TopupAppServiceBase, ISaleMansAppService
    {
        //private readonly IRepository<SaleMan> _saleManRepository;
        private readonly ISaleMansExcelExporter _saleMansExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<City, int> _lookupCityRepository;
        private readonly ISaleManManager _saleManManager;
        private readonly IRepository<User, long> _lookup_userRepository;

        public SaleMansAppService(ISaleMansExcelExporter saleMansExcelExporter,
            IRepository<User, long> lookupUserRepository, IRepository<City, int> lookupCityRepository,
            ISaleManManager saleManManager,
            IRepository<User, long> lookup_userRepository)
        {
            _saleMansExcelExporter = saleMansExcelExporter;
            _userRepository = lookupUserRepository;
            _lookupCityRepository = lookupCityRepository;
            _saleManManager = saleManManager;
            _lookup_userRepository = lookup_userRepository;
        }

        public async Task<PagedResultDto<GetSaleManForViewDto>> GetAll(GetAllSaleMansInput input)
        {
            var filteredSaleMans = _userRepository.GetAll().Where(x =>
                    x.AccountType == CommonConst.SystemAccountType.SaleLead ||
                    x.AccountType == CommonConst.SystemAccountType.Sale)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => false || e.UserName.Contains(input.UserNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter), e => e.PhoneNumber.Contains(input.PhoneNumberFilter))
                .WhereIf(input.SaleLeaderFilter > 0,
                    e => e.UserSaleLeadId == input.SaleLeaderFilter)
                .WhereIf(input.FromDateFilter != null,
                    e => e.CreationTime >= input.FromDateFilter)
                .WhereIf(input.ToDateFilter != null,
                    e => e.CreationTime <= input.ToDateFilter)
                .WhereIf(!string.IsNullOrEmpty(input.FullNameFilter),
                    e => false || e.Name.Contains(input.FullNameFilter) || e.Surname.Contains(input.FullNameFilter))
                .WhereIf(input.SaleTypeFilter > 0,
                    e => e.AccountType == input.SaleTypeFilter)
                .WhereIf(input.Status != null,
                    e => e.IsActive == input.Status);

            var pagedAndFilteredSaleMans = filteredSaleMans
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var saleMans = from o in pagedAndFilteredSaleMans
                           join o1 in _userRepository.GetAll() on o.UserSaleLeadId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()
                           select new GetSaleManForViewDto()
                           {
                               SaleMan = new SaleManDto
                               {
                                   Gender = o.Gender,
                                   Name = o.Name,
                                   Surname = o.Surname,
                                   FullName = o.FullName,
                                   AccountCode = o.AccountCode,
                                   AccountType = o.AccountType,
                                   AgentName = o.AgentName,
                                   CreationTime = o.CreationTime,
                                   UserName = o.UserName,
                                   PhoneNumber = o.PhoneNumber,
                                   Id = o.Id,
                                   IsActive = o.IsActive,
                                   UserSaleLeadId = o.UserSaleLeadId,
                                   SaleLeadName = s1 != null ? s1.UserName + " - " + s1.PhoneNumber + " - " + s1.FullName : ""
                               },
                           };

            var totalCount = await filteredSaleMans.CountAsync();

            return new PagedResultDto<GetSaleManForViewDto>(
                totalCount,
                await saleMans.ToListAsync()
            );
        }

        public async Task<SaleManDto> GetSaleManForView(long id)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == id);

            var output = user?.ConvertTo<SaleManDto>();

            if (output.UserSaleLeadId > 0)
            {
                var saleLead = await _lookup_userRepository.GetAsync(output.UserSaleLeadId.Value);
                output.SaleLeadName = saleLead.FullName;
            }

            if (output.CreatorUserId > 0)
            {
                var creator = await _lookup_userRepository.GetAsync(output.CreatorUserId.Value);
                output.CreatorName = creator.UserName + " - " + creator.FullName;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SaleMans_Edit)]
        public async Task<CreateOrUpdateSaleDto> GetSaleManForEdit(EntityDto input)
        {
            var saleMan = await _saleManManager.GetSaleInfo(input.Id);

            var output = saleMan?.ConvertTo<CreateOrUpdateSaleDto>();

            if (output.SaleLeadUserId > 0)
            {
                var saleLead = await _lookup_userRepository.GetAsync(output.SaleLeadUserId.Value);
                output.SaleLeadName = saleLead.UserName + " - " + saleLead.PhoneNumber + " - " + saleLead.FullName;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrUpdateSaleDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SaleMans_Create)]
        protected virtual async Task Create(CreateOrUpdateSaleDto input)
        {
            await _saleManManager.CreateSale(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleMans_Edit)]
        protected virtual async Task Update(CreateOrUpdateSaleDto input)
        {
            await _saleManManager.UpdateSale(input);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleMans_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _userRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSaleMansToExcel(GetAllSaleMansInput input)
        {
            var filteredSaleMans = _userRepository.GetAll().Where(x =>
                    x.AccountType == CommonConst.SystemAccountType.SaleLead ||
                    x.AccountType == CommonConst.SystemAccountType.Sale)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => false || e.UserName.Contains(input.UserNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter), e => e.PhoneNumber.Contains(input.PhoneNumberFilter))
                .WhereIf(input.SaleLeaderFilter > 0,
                    e => e.UserSaleLeadId == input.SaleLeaderFilter)
                .WhereIf(input.FromDateFilter != null,
                    e => e.CreationTime >= input.FromDateFilter)
                .WhereIf(input.ToDateFilter != null,
                    e => e.CreationTime <= input.ToDateFilter);

            var query = from o in filteredSaleMans
                join o1 in _userRepository.GetAll() on o.UserSaleLeadId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetSaleManForViewDto()
                {
                    SaleMan = new SaleManDto
                    {
                        Gender = o.Gender,
                        Name = o.Name,
                        Surname = o.Surname,
                        FullName = o.FullName,
                        AccountCode = o.AccountCode,
                        AccountType = o.AccountType,
                        AgentName = o.AgentName,
                        CreationTime = o.CreationTime,
                        UserName = o.UserName,
                        PhoneNumber = o.PhoneNumber,
                        Id = o.Id,
                        IsActive = o.IsActive,
                        UserSaleLeadId = o.UserSaleLeadId,
                        SaleLeadName = s1 != null ? s1.AccountCode + " - " + s1.PhoneNumber + " - " + s1.FullName : ""
                    },
                };


            var saleManListDtos = await query.ToListAsync();

            return _saleMansExcelExporter.ExportToFile(saleManListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleMans)]
        public async Task<List<SaleManUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _userRepository.GetAll()
                .Where(x => x.AccountType == CommonConst.SystemAccountType.SaleLead)
                .Select(user => new SaleManUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null
                        ? ""
                        : user.AccountCode + "-" + user.FullName + "-" + user.PhoneNumber
                }).ToListAsync();
        }

        public async Task<List<AddressSaleItemDto>> GetCities(long? userId = null,
            long? saleLeadId = null)
        {
            return await _saleManManager.GetListCitySale(userId, saleLeadId);
        }

        public async Task<List<AddressSaleItemDto>> GetWards(int? districtId = null, long? userId = null,
            long? saleLeadId = null)
        {
            return await _saleManManager.GetListWardSale(districtId, userId, saleLeadId);
        }

        public async Task<List<AddressSaleItemDto>> GetDistricts(int? cityId = null, long? userId = null,
            long? saleLeadId = null)
        {
            return await _saleManManager.GetListDistrictSale(cityId, userId, saleLeadId);
        }

        public async Task<AddressSaleSelected> GetAddressSelected(long? userId = null, long? saleLeadId = null,
            int? cityId = null, int? districtId = null, int? wardId = null)
        {
            return await _saleManManager.GetAddressSelected(userId, saleLeadId, cityId, districtId, wardId);
        }
    }
}