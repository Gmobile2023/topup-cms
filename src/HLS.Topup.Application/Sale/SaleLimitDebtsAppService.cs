using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Sale.Exporting;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace HLS.Topup.Sale
{
    [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts)]
    public class SaleLimitDebtsAppService : TopupAppServiceBase, ISaleLimitDebtsAppService
    {
        private readonly IRepository<SaleLimitDebt> _saleLimitDebtRepository;
        private readonly ISaleLimitDebtsExcelExporter _saleLimitDebtsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;


        public SaleLimitDebtsAppService(IRepository<SaleLimitDebt> saleLimitDebtRepository,
            ISaleLimitDebtsExcelExporter saleLimitDebtsExcelExporter, IRepository<User, long> lookup_userRepository)
        {
            _saleLimitDebtRepository = saleLimitDebtRepository;
            _saleLimitDebtsExcelExporter = saleLimitDebtsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
        }

        public async Task<PagedResultDto<GetSaleLimitDebtForViewDto>> GetAll(GetAllSaleLimitDebtsInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.DebtLimitAmountStatus)input.StatusFilter
                : default;

            if (input.ToDateFilter != null)
            {
                input.ToDateFilter = input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1);
            }
            var filteredSaleLimitDebts = _saleLimitDebtRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Description.Contains(input.Filter) || e.UserName.Contains(input.Filter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter)
                .WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.AccountCode == input.UserNameFilter)
                .WhereIf(input.SaleLeaderId > 0,
                    e => e.UserFk != null && e.UserFk.UserSaleLeadId == input.SaleLeaderId); ;

            var pagedAndFilteredSaleLimitDebts = filteredSaleLimitDebts
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var saleLimitDebts = from o in pagedAndFilteredSaleLimitDebts
                                 join u in _lookup_userRepository.GetAll() on o.UserId equals u.Id
                                 join uc in _lookup_userRepository.GetAll() on o.CreatorUserId equals uc.Id
                                 join ul in _lookup_userRepository.GetAll() on u.UserSaleLeadId equals ul.Id
                                 select new GetSaleLimitDebtForViewDto()
                                 {
                                     SaleLimitDebt = new SaleLimitDebtDto
                                     {
                                         SaleInfo = u.UserName + " - " + u.PhoneNumber + " - " + u.FullName,
                                         SaleLeaderInfo = ul.UserName + " - " + ul.PhoneNumber + " - " + ul.FullName,
                                         LimitAmount = o.LimitAmount,
                                         DebtAge = o.DebtAge,
                                         Status = o.Status,
                                         CreatedDate = o.CreationTime,
                                         UserCreated = uc.UserName,
                                         Description = o.Description,
                                         Id = o.Id
                                     },
                                     UserName = u.UserName,
                                 };

            var totalCount = await filteredSaleLimitDebts.CountAsync();

            return new PagedResultDto<GetSaleLimitDebtForViewDto>(
                totalCount,
                await saleLimitDebts.ToListAsync()
            );
        }

        public async Task<GetSaleLimitDebtForViewDto> GetSaleLimitDebtForView(int id)
        {
            var saleLimitDebt = await _saleLimitDebtRepository.GetAsync(id);

            var output = new GetSaleLimitDebtForViewDto
            { SaleLimitDebt = ObjectMapper.Map<SaleLimitDebtDto>(saleLimitDebt) };

            if (output.SaleLimitDebt.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SaleLimitDebt.UserId);
                var _userCreate = await _lookup_userRepository.FirstOrDefaultAsync((long)saleLimitDebt.CreatorUserId);
                output.UserName = _lookupUser?.Name?.ToString();
                output.SaleLimitDebt.SaleInfo = _lookupUser.UserName + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName;
                output.SaleLimitDebt.CreatedDate = saleLimitDebt.CreationTime;
                output.SaleLimitDebt.UserCreated = _userCreate != null ? _userCreate.UserName : string.Empty;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts_Edit)]
        public async Task<GetSaleLimitDebtForEditOutput> GetSaleLimitDebtForEdit(EntityDto input)
        {
            var saleLimitDebt = await _saleLimitDebtRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSaleLimitDebtForEditOutput
            { SaleLimitDebt = ObjectMapper.Map<CreateOrEditSaleLimitDebtDto>(saleLimitDebt) };

            if (output.SaleLimitDebt.UserId > 0)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.SaleLimitDebt.UserId);
                output.UserName = _lookupUser != null ? _lookupUser.AccountCode + " - " + _lookupUser.PhoneNumber + " - " + _lookupUser.FullName : string.Empty;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSaleLimitDebtDto input)
        {
            if (input.LimitAmount <= 0)
                throw new UserFriendlyException("Hạn mức thiết lập phải lớn hơn không.");
            if (input.DebtAge <= 0)
                throw new UserFriendlyException("Tuổi nợ thiết lập phải lớn hơn không.");

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts_Create)]
        protected virtual async Task Create(CreateOrEditSaleLimitDebtDto input)
        {
            var saleLimitDebt = ObjectMapper.Map<SaleLimitDebt>(input);


            if (AbpSession.TenantId != null)
            {
                saleLimitDebt.TenantId = (int?)AbpSession.TenantId;
            }

            if (input.UserId <= 0)
                throw new UserFriendlyException("Quý khách chưa chọn tài khoản.");


            var checkLimit = await _saleLimitDebtRepository.GetAll().Where(c => c.UserId == input.UserId).FirstOrDefaultAsync();
            if (checkLimit != null)
                throw new UserFriendlyException("Tài khoản đã tồn tại hạn mức.");

            await _saleLimitDebtRepository.InsertAsync(saleLimitDebt);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts_Edit)]
        protected virtual async Task Update(CreateOrEditSaleLimitDebtDto input)
        {
            var saleLimitDebt = await _saleLimitDebtRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, saleLimitDebt);
        }

        [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _saleLimitDebtRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSaleLimitDebtsToExcel(GetAllSaleLimitDebtsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
               ? (CommonConst.DebtLimitAmountStatus)input.StatusFilter
               : default;

            var filteredSaleLimitDebts = _saleLimitDebtRepository.GetAll()
              .Include(e => e.UserFk)
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.Description.Contains(input.Filter) || e.UserName.Contains(input.Filter))
              .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
              .WhereIf(input.FromDateFilter != null, e => e.CreationTime >= input.FromDateFilter)
              .WhereIf(input.ToDateFilter != null, e => e.CreationTime <= input.ToDateFilter)
              .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                  e => e.UserFk != null && e.UserFk.AccountCode == input.UserNameFilter)
              .WhereIf(input.SaleLeaderId > 0,
                  e => e.UserFk != null && e.UserFk.UserSaleLeadId == input.SaleLeaderId);

            var saleLimitDebts = await (from o in filteredSaleLimitDebts
                                        join u in _lookup_userRepository.GetAll() on o.UserId equals u.Id
                                        join uc in _lookup_userRepository.GetAll() on o.CreatorUserId equals uc.Id
                                        join ul in _lookup_userRepository.GetAll() on u.UserSaleLeadId equals ul.Id
                                        select new GetSaleLimitDebtForViewDto()
                                        {
                                            SaleLimitDebt = new SaleLimitDebtDto
                                            {
                                                SaleInfo = u.UserName + " - " + u.PhoneNumber + " - " + u.FullName,
                                                SaleLeaderInfo = ul.UserName + " - " + ul.PhoneNumber + " - " + ul.FullName,
                                                LimitAmount = o.LimitAmount,
                                                DebtAge = o.DebtAge,
                                                Status = o.Status,
                                                CreatedDate = o.CreationTime,
                                                UserCreated = uc.UserName,
                                                Description = o.Description,
                                                Id = o.Id
                                            },
                                            UserName = u.UserName,
                                        }).ToListAsync();


            return _saleLimitDebtsExcelExporter.ExportToFile(saleLimitDebts);
        }


        [AbpAuthorize(AppPermissions.Pages_SaleLimitDebts)]
        public async Task<PagedResultDto<SaleLimitDebtUserLookupTableDto>> GetAllUserForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name.Contains(input.Filter)
                 || e.AccountCode.Contains(input.Filter)
                 || e.UserName.Contains(input.Filter)
                 || e.PhoneNumber.Contains(input.Filter)
            );

            query = query.Where(c => c.AccountType == CommonConst.SystemAccountType.Sale);

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SaleLimitDebtUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new SaleLimitDebtUserLookupTableDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    DisplayName = user.AccountCode + " - " + user.PhoneNumber + " - " + user.FullName,
                });
            }

            return new PagedResultDto<SaleLimitDebtUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
