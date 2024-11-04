using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using HLS.Topup.Audit;
using HLS.Topup.Auditing.Dto;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Audit;
using HLS.Topup.RequestDtos;
using ServiceStack;

namespace HLS.Topup.Auditing
{
    [AbpAuthorize(AppPermissions.Pages_AuditActivities)]
    public class AuditActivitiesAppService : TopupAppServiceBase, IAuditActivitiesAppService
    {
        private readonly IAuditManger _auditManger;
        private readonly IRepository<User, long> _lookupUserRepository;

        public AuditActivitiesAppService(IAuditManger auditManger, IRepository<User, long> lookupUserRepository)
        {
            _auditManger = auditManger;
            _lookupUserRepository = lookupUserRepository;
        }
        
        public async Task<PagedResultDto<AccountActivityHistoryDto>> GetAll(
            GetAuditAccountActivitiesInput input)
        {
            try
            {
                var userLookup = _lookupUserRepository.GetAll().ToList();
                
                var request = new GetAccountActivityHistoryRequest
                {
                    FromDate = input.FromDate,
                    ToDate = input.ToDate,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    AccountCode = input.AccountCode,
                    PhoneNumber = input.PhoneNumber,
                    Note = input.Note,
                    UserName = input.UserName,
                    AccountActivityType = input.AccountActivityType,
                    AgentType = input.AgentType,
                    AccountType = input.AgentType,
                    SearchType = SearchType.Search
                };

                var rs = await _auditManger.GetAccountActivityHistoryRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDto<AccountActivityHistoryDto>(
                        0,
                        new List<AccountActivityHistoryDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<AccountActivityHistoryDto>>();

                foreach (var item in lst)
                {
                    item.AgentName = item.AccountCode + " - " +
                                     userLookup.SingleOrDefault(x => x.AccountCode == item.AccountCode).PhoneNumber + " - " +
                                     userLookup.SingleOrDefault(x => x.AccountCode == item.AccountCode).FullName;
                }
                
                return new PagedResultDto<AccountActivityHistoryDto>(
                    totalCount,
                    lst
                );
            }
            catch (Exception e)
            {
                return new PagedResultDto<AccountActivityHistoryDto>(0, new List<AccountActivityHistoryDto>());
            }
        }
        
        public async Task<PagedResultDto<AccountActivityHistoryDto>> GetAccountActivityHistories(
            GetAuditAccountActivitiesInput input)
        {
            try
            {
                var request = new GetAccountActivityHistoryRequest
                {
                    FromDate = input.FromDate,
                    ToDate = input.ToDate,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    AccountCode = input.AccountCode,
                    PhoneNumber = input.PhoneNumber,
                    Note = input.Note,
                    UserName = input.UserName,
                    AccountActivityType = input.AccountActivityType,
                    AgentType = input.AgentType,
                    AccountType = input.AgentType,
                    SearchType = SearchType.Search
                };

                var rs = await _auditManger.GetAccountActivityHistoryRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDto<AccountActivityHistoryDto>(
                        0,
                        new List<AccountActivityHistoryDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<AccountActivityHistoryDto>>();
                return new PagedResultDto<AccountActivityHistoryDto>(
                    totalCount,
                    lst
                );
            }
            catch (Exception e)
            {
                return new PagedResultDto<AccountActivityHistoryDto>(0, new List<AccountActivityHistoryDto>());
            }
        }
    }
}
