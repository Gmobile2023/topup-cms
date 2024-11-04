using HLS.Topup.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Banks.Exporting;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Banks
{
    [AbpAuthorize(AppPermissions.Pages_Banks)]
    public class BanksAppService : TopupAppServiceBase, IBanksAppService
    {
        private readonly IRepository<Bank> _bankRepository;
        private readonly IBanksExcelExporter _banksExcelExporter;
        private readonly UrlExtentions _extentions;

        public BanksAppService(IRepository<Bank> bankRepository, IBanksExcelExporter banksExcelExporter, UrlExtentions extentions)
        {
            _bankRepository = bankRepository;
            _banksExcelExporter = banksExcelExporter;
            _extentions = extentions;
        }

        public async Task<PagedResultDto<GetBankForViewDto>> GetAll(GetAllBanksInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.BankStatus) input.StatusFilter
                : default;

            var filteredBanks = _bankRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.BankName.Contains(input.Filter) || e.BranchName.Contains(input.Filter) ||
                         e.BankAccountName.Contains(input.Filter) || e.BankAccountCode.Contains(input.Filter) ||
                         e.Images.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName.Contains(input.BankNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNameFilter),
                    e => e.BranchName.Contains(input.BranchNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccountNameFilter),
                    e => e.BankAccountName.Contains(input.BankAccountNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccountCodeFilter),
                    e => e.BankAccountCode.Contains(input.BankAccountCodeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SmsPhoneNumberFilter),
                    e => e.SmsPhoneNumber.Contains(input.SmsPhoneNumberFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SmsGatewayNumberFilter),
                    e => e.SmsGatewayNumber.Contains(input.SmsGatewayNumberFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var pagedAndFilteredBanks = filteredBanks
                .OrderByDescending(x=>x.Id).ThenBy(x=>x.BankName)
                .PageBy(input);

            var banks = from o in pagedAndFilteredBanks
                select new GetBankForViewDto()
                {
                    Bank = new BankDto
                    {
                        BankName = o.BankName,
                        ShortName = o.ShortName,
                        BranchName = o.BranchName,
                        BankAccountName = o.BankAccountName,
                        BankAccountCode = o.BankAccountCode,
                        Status = o.Status,
                        Id = o.Id,
                        SmsPhoneNumber = o.SmsPhoneNumber,
                        SmsGatewayNumber = o.SmsGatewayNumber,
                        // SmsSyntax = o.SmsSyntax,
                        // NoteSyntax = o.NoteSyntax
                    }
                };

            var totalCount = await filteredBanks.CountAsync();

            return new PagedResultDto<GetBankForViewDto>(
                totalCount,
                await banks.ToListAsync()
            );
        }

        public async Task<GetBankForViewDto> GetBankForView(int id)
        {
            var bank = await _bankRepository.GetAsync(id);

            var output = new GetBankForViewDto {Bank = ObjectMapper.Map<BankDto>(bank)};
            output.Bank.Images = _extentions.GetFullPath(output.Bank.Images);
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Banks_Edit)]
        public async Task<GetBankForEditOutput> GetBankForEdit(EntityDto input)
        {
            var bank = await _bankRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBankForEditOutput {Bank = ObjectMapper.Map<CreateOrEditBankDto>(bank)};
            output.Bank.Images = _extentions.GetFullPath(output.Bank.Images);
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBankDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Banks_Create)]
        protected virtual async Task Create(CreateOrEditBankDto input)
        {
            var bank = ObjectMapper.Map<Bank>(input);


            if (AbpSession.TenantId != null)
            {
                bank.TenantId = (int?) AbpSession.TenantId;
            }


            await _bankRepository.InsertAsync(bank);
        }

        [AbpAuthorize(AppPermissions.Pages_Banks_Edit)]
        protected virtual async Task Update(CreateOrEditBankDto input)
        {
            var bank = await _bankRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, bank);
        }

        [AbpAuthorize(AppPermissions.Pages_Banks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _bankRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBanksToExcel(GetAllBanksForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.BankStatus) input.StatusFilter
                : default;

            var filteredBanks = _bankRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.BankName.Contains(input.Filter) || e.BranchName.Contains(input.Filter) ||
                         e.BankAccountName.Contains(input.Filter) || e.BankAccountCode.Contains(input.Filter) ||
                         e.Images.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName == input.BankNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNameFilter),
                    e => e.BranchName == input.BranchNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccountNameFilter),
                    e => e.BankAccountName == input.BankAccountNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BankAccountCodeFilter),
                    e => e.BankAccountCode == input.BankAccountCodeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var query = (from o in filteredBanks
                select new GetBankForViewDto()
                {
                    Bank = new BankDto
                    {
                        BankName = o.BankName,
                        ShortName = o.ShortName,
                        BranchName = o.BranchName,
                        BankAccountName = o.BankAccountName,
                        BankAccountCode = o.BankAccountCode,
                        Status = o.Status,
                        Id = o.Id,
                        SmsPhoneNumber = o.SmsPhoneNumber,
                        SmsGatewayNumber = o.SmsGatewayNumber,
                        // SmsSyntax = o.SmsSyntax,
                        // NoteSyntax = o.NoteSyntax
                    }
                });


            var bankListDtos = await query.ToListAsync();

            return _banksExcelExporter.ExportToFile(bankListDtos);
        }
    }
}
