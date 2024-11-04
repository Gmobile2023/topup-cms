using HLS.Topup.Common;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.BalanceManager.Exporting;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.BalanceManager
{
    [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers)]
    public class SystemAccountTransfersAppService : TopupAppServiceBase, ISystemAccountTransfersAppService
    {
        private readonly IRepository<SystemAccountTransfer> _systemAccountTransferRepository;
        private readonly ISystemAccountTransfersExcelExporter _systemAccountTransfersExcelExporter;
        private readonly ICommonManger _commonManger;
        private readonly ITransactionManager _transactionManager;

        public SystemAccountTransfersAppService(IRepository<SystemAccountTransfer> systemAccountTransferRepository,
            ISystemAccountTransfersExcelExporter systemAccountTransfersExcelExporter, ICommonManger commonManger,
            ITransactionManager transactionManager)
        {
            _systemAccountTransferRepository = systemAccountTransferRepository;
            _systemAccountTransfersExcelExporter = systemAccountTransfersExcelExporter;
            _commonManger = commonManger;
            _transactionManager = transactionManager;
        }

        public async Task<PagedResultDto<GetSystemAccountTransferForViewDto>> GetAll(
            GetAllSystemAccountTransfersInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.SystemTransferStatus) input.StatusFilter
                : default;

            var filteredSystemAccountTransfers = _systemAccountTransferRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.SrcAccount.Contains(input.Filter) || e.DesAccount.Contains(input.Filter) ||
                         e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SrcAccountFilter),
                    e => e.SrcAccount == input.SrcAccountFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DesAccountFilter),
                    e => e.DesAccount == input.DesAccountFilter)
                .WhereIf(input.FromCreatedTimeFilter != null && input.FromCreatedTimeFilter != DateTime.MinValue,
                    e => e.CreationTime >= input.FromCreatedTimeFilter)
                .WhereIf(input.ToCreatedTimeFilter != null && input.ToCreatedTimeFilter != DateTime.MinValue,
                    e => e.CreationTime <= input.ToCreatedTimeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var pagedAndFilteredSystemAccountTransfers = filteredSystemAccountTransfers
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var systemAccountTransfers = from o in pagedAndFilteredSystemAccountTransfers
                join u in UserManager.Users on o.CreatorUserId equals u.Id
                join u1 in UserManager.Users on o.ApproverId equals u1.Id into temp
                from a in temp.DefaultIfEmpty()
                select new GetSystemAccountTransferForViewDto()
                {
                    SystemAccountTransfer = new SystemAccountTransferDto
                    {
                        SrcAccount = o.SrcAccount,
                        DesAccount = o.DesAccount,
                        Amount = o.Amount,
                        Attachments = o.Attachments,
                        Status = o.Status,
                        Id = o.Id,
                        TransCode = o.TransCode
                    },
                    DateCreated = o.CreationTime,
                    DateApproved = o.DateApproved,
                    UserApproved = a != null ? a.UserName : "",
                    UserCreated = u.UserName
                };

            var totalCount = await filteredSystemAccountTransfers.CountAsync();

            return new PagedResultDto<GetSystemAccountTransferForViewDto>(
                totalCount,
                await systemAccountTransfers.ToListAsync()
            );
        }

        public async Task<GetSystemAccountTransferForViewDto> GetSystemAccountTransferForView(int id)
        {
            var systemAccountTransfer = await _systemAccountTransferRepository.GetAsync(id);

            var output = new GetSystemAccountTransferForViewDto
                {SystemAccountTransfer = ObjectMapper.Map<SystemAccountTransferDto>(systemAccountTransfer)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Edit)]
        public async Task<GetSystemAccountTransferForEditOutput> GetSystemAccountTransferForEdit(EntityDto input)
        {
            var systemAccountTransfer = await _systemAccountTransferRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSystemAccountTransferForEditOutput
                {SystemAccountTransfer = ObjectMapper.Map<CreateOrEditSystemAccountTransferDto>(systemAccountTransfer)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSystemAccountTransferDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Create)]
        protected virtual async Task Create(CreateOrEditSystemAccountTransferDto input)
        {
            if (input.Amount <= 0)
                throw new UserFriendlyException("Số tiền không hợp lệ");
            var code = await _commonManger.GetIncrementCodeAsync("D");
            var systemAccountTransfer = ObjectMapper.Map<SystemAccountTransfer>(input);
            if (AbpSession.TenantId != null)
            {
                systemAccountTransfer.TenantId = AbpSession.TenantId;
            }
            if(input.SrcAccount==input.DesAccount)
                throw new UserFriendlyException("Không thể thực hiện chuyển tiền cho cùng một tài khoản");

            systemAccountTransfer.Status = CommonConst.SystemTransferStatus.Pending;
            systemAccountTransfer.TransCode = code;

            await _systemAccountTransferRepository.InsertAsync(systemAccountTransfer);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Edit)]
        protected virtual async Task Update(CreateOrEditSystemAccountTransferDto input)
        {
            var systemAccountTransfer = await _systemAccountTransferRepository.FirstOrDefaultAsync((int) input.Id);
            if (systemAccountTransfer == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (!string.IsNullOrEmpty(input.Attachments) && input.Attachments != systemAccountTransfer.Attachments)
                systemAccountTransfer.Attachments = input.Attachments;
            if (input.Amount > 0 && input.Amount != systemAccountTransfer.Amount)
                systemAccountTransfer.Amount = input.Amount;
            if (!string.IsNullOrEmpty(input.SrcAccount) && input.SrcAccount != systemAccountTransfer.SrcAccount)
                systemAccountTransfer.SrcAccount = input.SrcAccount;
            if (!string.IsNullOrEmpty(input.DesAccount) && input.DesAccount != systemAccountTransfer.DesAccount)
                systemAccountTransfer.DesAccount = input.DesAccount;
            await _systemAccountTransferRepository.UpdateAsync(systemAccountTransfer);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Delete)]
        public async Task Delete(EntityDto input)
        {
            var item = await _systemAccountTransferRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (item.Status != CommonConst.SystemTransferStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            await _systemAccountTransferRepository.DeleteAsync(item);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Approval)]
        public async Task Approval(EntityDto input)
        {
            var item = await _systemAccountTransferRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (item.Status != CommonConst.SystemTransferStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");

            item.Status = CommonConst.SystemTransferStatus.Processing;
            await _systemAccountTransferRepository.UpdateAsync(item);
            await CurrentUnitOfWork.SaveChangesAsync();

            var response = await _transactionManager.TransferSystemRequest(new TransferSystemRequest
            {
                Amount = item.Amount,
                CurrencyCode = "VND",
                DesAccount = item.DesAccount,
                SrcAccount = item.SrcAccount,
                TransNote = item.Description,
                TransRef = item.TransCode
            });
            if (response.ResponseCode == "01")
            {
                item.ApproverId = AbpSession.UserId ?? 0;
                item.DateApproved = DateTime.Now;
                item.Status = CommonConst.SystemTransferStatus.Approved;
            }

            await _systemAccountTransferRepository.UpdateAsync(item);
            if (response.ResponseCode != "01")
                throw new UserFriendlyException(response.ResponseMessage);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemAccountTransfers_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var item = await _systemAccountTransferRepository.FirstOrDefaultAsync((int) input.Id);
            if (item == null)
                throw new UserFriendlyException("Giao dịch không tồn tại");
            if (item.Status != CommonConst.SystemTransferStatus.Pending)
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            item.Status = CommonConst.SystemTransferStatus.Cancel;
            item.ApproverId = AbpSession.UserId ?? 0;
            item.DateApproved = DateTime.Now;
            await _systemAccountTransferRepository.UpdateAsync(item);
        }

        public async Task<FileDto> GetSystemAccountTransfersToExcel(GetAllSystemAccountTransfersForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.SystemTransferStatus) input.StatusFilter
                : default;

            var filteredSystemAccountTransfers = _systemAccountTransferRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.SrcAccount.Contains(input.Filter) || e.DesAccount.Contains(input.Filter) ||
                         e.TransCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SrcAccountFilter),
                    e => e.SrcAccount == input.SrcAccountFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DesAccountFilter),
                    e => e.DesAccount == input.DesAccountFilter)
                .WhereIf(input.FromCreatedTimeFilter != null && input.FromCreatedTimeFilter != DateTime.MinValue,
                    e => e.CreationTime >= input.FromCreatedTimeFilter)
                .WhereIf(input.ToCreatedTimeFilter != null && input.ToCreatedTimeFilter != DateTime.MinValue,
                    e => e.CreationTime <= input.ToCreatedTimeFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter);

            var query = from o in filteredSystemAccountTransfers
                join u in UserManager.Users on o.CreatorUserId equals u.Id
                join u1 in UserManager.Users on o.ApproverId equals u1.Id into temp
                from a in temp.DefaultIfEmpty()
                select new GetSystemAccountTransferForViewDto()
                {
                    SystemAccountTransfer = new SystemAccountTransferDto
                    {
                        SrcAccount = o.SrcAccount,
                        DesAccount = o.DesAccount,
                        Amount = o.Amount,
                        Attachments = o.Attachments,
                        Status = o.Status,
                        Id = o.Id,
                        TransCode = o.TransCode
                    },
                    DateCreated = o.CreationTime,
                    DateApproved = o.DateApproved,
                    UserApproved = a != null ? a.UserName : "",
                    UserCreated = u.UserName
                };


            var systemAccountTransferListDtos = await query.ToListAsync();

            return _systemAccountTransfersExcelExporter.ExportToFile(systemAccountTransferListDtos);
        }
    }
}
