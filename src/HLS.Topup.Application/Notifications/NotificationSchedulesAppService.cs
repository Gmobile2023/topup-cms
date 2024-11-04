using System;
using System.Collections.Generic;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Notifications.Exporting;
using HLS.Topup.Notifications.Dtos;
using HLS.Topup.Dto;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization;
using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;

namespace HLS.Topup.Notifications
{
    [AbpAuthorize(AppPermissions.Pages_NotificationSchedules)]
    public class NotificationSchedulesAppService : TopupAppServiceBase, INotificationSchedulesAppService
    {
        private readonly IRepository<NotificationSchedule> _notificationScheduleRepository;
        private readonly INotificationSchedulesExcelExporter _notificationSchedulesExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly INotificationScheduleManager _notificationSchedule;


        public NotificationSchedulesAppService(IRepository<NotificationSchedule> notificationScheduleRepository,
            INotificationSchedulesExcelExporter notificationSchedulesExcelExporter,
            IRepository<User, long> lookup_userRepository, INotificationScheduleManager notificationSchedule)
        {
            _notificationScheduleRepository = notificationScheduleRepository;
            _notificationSchedulesExcelExporter = notificationSchedulesExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _notificationSchedule = notificationSchedule;
        }

        public async Task<PagedResultDto<GetNotificationScheduleForViewDto>> GetAll(
            GetAllNotificationSchedulesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.SendNotificationStatus) input.StatusFilter
                : default;
            var accountTypeFilter = input.AccountTypeFilter.HasValue
                ? (CommonConst.SystemAccountType) input.AccountTypeFilter
                : default;
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            var filteredNotificationSchedules = _notificationScheduleRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Title.Contains(input.Filter) || e.Body.Contains(input.Filter) ||
                         e.ExtraInfo.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.AccountTypeFilter.HasValue && input.AccountTypeFilter > -1,
                    e => e.AccountType == accountTypeFilter)
                .WhereIf(input.AgentTypeFilter.HasValue && input.AgentTypeFilter > -1,
                    e => e.AgentType == agentTypeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredNotificationSchedules = filteredNotificationSchedules
                .OrderByDescending(x=>x.Id)
                .PageBy(input);

            var notificationSchedules = from o in pagedAndFilteredNotificationSchedules
                join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetNotificationScheduleForViewDto()
                {
                    NotificationSchedule = new NotificationScheduleDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        Title = o.Title,
                        DateSchedule = o.DateSchedule,
                        DateSend = o.DateSend,
                        Status = o.Status,
                        AccountType = o.AccountType,
                        AgentType = o.AgentType,
                        DateApproved = o.DateApproved,
                        Id = o.Id,
                        Body = o.Body
                    },
                    UserName = s1 == null || s1.Name == null ? "" : s1.AccountCode + "-" + s1.PhoneNumber + "-" + s1.FullName
                };

            var totalCount = await filteredNotificationSchedules.CountAsync();

            return new PagedResultDto<GetNotificationScheduleForViewDto>(
                totalCount,
                await notificationSchedules.ToListAsync()
            );
        }

        public async Task<GetNotificationScheduleForViewDto> GetNotificationScheduleForView(int id)
        {
            var notificationSchedule = await _notificationScheduleRepository.GetAsync(id);

            var output = new GetNotificationScheduleForViewDto
                {NotificationSchedule = ObjectMapper.Map<NotificationScheduleDto>(notificationSchedule)};

            if (output.NotificationSchedule.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.NotificationSchedule.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Edit)]
        public async Task<GetNotificationScheduleForEditOutput> GetNotificationScheduleForEdit(EntityDto input)
        {
            var notificationSchedule = await _notificationScheduleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetNotificationScheduleForEditOutput
                {NotificationSchedule = ObjectMapper.Map<CreateOrEditNotificationScheduleDto>(notificationSchedule)};

            if (output.NotificationSchedule.UserId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.NotificationSchedule.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditNotificationScheduleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Create)]
        protected virtual async Task Create(CreateOrEditNotificationScheduleDto input)
        {
            var notificationSchedule = ObjectMapper.Map<NotificationSchedule>(input);
            notificationSchedule.Code = "M" + new Random().Next(0, 99999999).ToString("0000000000");
            if (AbpSession.TenantId != null)
            {
                notificationSchedule.TenantId = AbpSession.TenantId;
            }

            await _notificationScheduleRepository.InsertAsync(notificationSchedule);
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Edit)]
        protected virtual async Task Update(CreateOrEditNotificationScheduleDto input)
        {
            var notificationSchedule = await _notificationScheduleRepository.FirstOrDefaultAsync((int) input.Id);
            if(notificationSchedule==null || notificationSchedule.Status!=CommonConst.SendNotificationStatus.Pending)
                throw new UserFriendlyException("Thao tác không thành công");
            ObjectMapper.Map(input, notificationSchedule);
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Delete)]
        public async Task Delete(EntityDto input)
        {
            var notificationSchedule = await _notificationScheduleRepository.FirstOrDefaultAsync((int) input.Id);
            if(notificationSchedule==null || notificationSchedule.Status!=CommonConst.SendNotificationStatus.Pending)
                throw new UserFriendlyException("Thao tác không thành công");
            await _notificationScheduleRepository.DeleteAsync(notificationSchedule);
        }

        public async Task<FileDto> GetNotificationSchedulesToExcel(GetAllNotificationSchedulesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                ? (CommonConst.SendNotificationStatus) input.StatusFilter
                : default;
            var accountTypeFilter = input.AccountTypeFilter.HasValue
                ? (CommonConst.SystemAccountType) input.AccountTypeFilter
                : default;
            var agentTypeFilter = input.AgentTypeFilter.HasValue
                ? (CommonConst.AgentType) input.AgentTypeFilter
                : default;

            var filteredNotificationSchedules = _notificationScheduleRepository.GetAll()
                .Include(e => e.UserFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) ||
                         e.Title.Contains(input.Filter) || e.Body.Contains(input.Filter) ||
                         e.ExtraInfo.Contains(input.Filter) || e.Description.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.AccountTypeFilter.HasValue && input.AccountTypeFilter > -1,
                    e => e.AccountType == accountTypeFilter)
                .WhereIf(input.AgentTypeFilter.HasValue && input.AgentTypeFilter > -1,
                    e => e.AgentType == agentTypeFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredNotificationSchedules
                join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetNotificationScheduleForViewDto()
                {
                    NotificationSchedule = new NotificationScheduleDto
                    {
                        Code = o.Code,
                        Name = o.Name,
                        Title = o.Title,
                        DateSchedule = o.DateSchedule,
                        DateSend = o.DateSend,
                        Status = o.Status,
                        AccountType = o.AccountType,
                        AgentType = o.AgentType,
                        DateApproved = o.DateApproved,
                        Id = o.Id,
                        Body = o.Body
                    },
                    UserName = s1 == null || s1.Name == null ? "" : s1.AccountCode + "-" + s1.PhoneNumber + "-" + s1.FullName
                });


            var notificationScheduleListDtos = await query.ToListAsync();

            return _notificationSchedulesExcelExporter.ExportToFile(notificationScheduleListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules)]
        public async Task<List<NotificationScheduleUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll().Where(x =>
                    x.AccountType == CommonConst.SystemAccountType.Agent ||
                    x.AccountType == CommonConst.SystemAccountType.MasterAgent)
                .Select(user => new NotificationScheduleUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null
                        ? ""
                        : user.AccountCode + "-" + user.PhoneNumber + "-" + user.FullName
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Approval)]
        public async Task Approval(EntityDto input)
        {
            var message = await _notificationScheduleRepository.FirstOrDefaultAsync(x =>
                x.Id == input.Id && x.Status == CommonConst.SendNotificationStatus.Pending);
            if (message == null)
                throw new UserFriendlyException("Thông báo không tồn tại");
            message.Status = CommonConst.SendNotificationStatus.Approved;
            message.ApproverId = AbpSession.UserId;
            message.DateApproved = DateTime.Now;
            await _notificationScheduleRepository.UpdateAsync(message);
            await CurrentUnitOfWork.SaveChangesAsync();
            await _notificationSchedule.ScheduleNotification(message);
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Cancel)]
        public async Task Cancel(EntityDto input)
        {
            var message = await _notificationScheduleRepository.FirstOrDefaultAsync(x =>
                x.Id == input.Id && x.Status == CommonConst.SendNotificationStatus.Pending);
            if (message == null)
                throw new UserFriendlyException("Thông báo không tồn tại");
            message.Status = CommonConst.SendNotificationStatus.Cancel;
            message.ApproverId = AbpSession.UserId;
            await _notificationScheduleRepository.UpdateAsync(message);
        }

        [AbpAuthorize(AppPermissions.Pages_NotificationSchedules_Send)]
        public async Task Send(EntityDto input)
        {
            await _notificationSchedule.SendNowNotification(input.Id);
        }
    }
}
