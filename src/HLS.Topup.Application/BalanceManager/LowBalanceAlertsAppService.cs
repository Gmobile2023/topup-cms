using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.BalanceManager.Dtos;
using ServiceStack;

namespace HLS.Topup.BalanceManager
{
    public class LowBalanceAlertsAppService : TopupAppServiceBase, ILowBalanceAlertsAppService
    {
        private readonly IBalanceAlertManager _balanceAlertManager;

        public LowBalanceAlertsAppService(IBalanceAlertManager balanceAlertManager)
        {
            _balanceAlertManager = balanceAlertManager;
        }

        public async Task<PagedResultDto<LowBalanceAlertDto>> GetAll(GetAllLowBalanceAlertsInput input)
        {
            try
            {
                var dataRequest = input.ConvertTo<BalanceAlertGetAllRequest>();
                var result = await _balanceAlertManager.BalanceAlertGetAllRequest(dataRequest);

                return new PagedResultDto<LowBalanceAlertDto>(
                    result.Total,
                    result.Payload.ConvertTo<List<LowBalanceAlertDto>>());
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        
        [AbpAuthorize(AppPermissions.Pages_LowBalanceAlerts_Edit)]
        public async Task<GetLowBalanceAlertForEditOutput> GetLowBalanceAlertForEdit(string accountCode)
        {
            var lowBalanceAlert = await _balanceAlertManager.BalanceAlertGetRequest(new BalanceAlertGetRequest
            {
                AccountCode = accountCode
            });

            var output = new GetLowBalanceAlertForEditOutput {LowBalanceAlert = lowBalanceAlert.ConvertTo<CreateOrEditLowBalanceAlertDto>()};
            return output;
        }
        
        public async Task CreateOrEdit(CreateOrEditLowBalanceAlertDto input)
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
        
        [AbpAuthorize(AppPermissions.Pages_LowBalanceAlerts_Create)]
        protected virtual async Task Create(CreateOrEditLowBalanceAlertDto input)
        {
            var dataRequest = input.ConvertTo<BalanceAlertAddRequest>();
            await _balanceAlertManager.BalanceAlertAddRequest(dataRequest);
        }

        [AbpAuthorize(AppPermissions.Pages_LowBalanceAlerts_Edit)]
        protected virtual async Task Update(CreateOrEditLowBalanceAlertDto input)
        {
            var dataRequest = input.ConvertTo<BalanceAlertUpdateRequest>();
            await _balanceAlertManager.BalanceAlertUpdateRequest(dataRequest);
        }
        
        public async Task<GetLowBalanceAlertForViewDto> GetLowBalanceAlertForView(string accountCode)
        {
            if (accountCode != null)
            {
                var response = await _balanceAlertManager.BalanceAlertGetRequest(new BalanceAlertGetRequest
                {
                    AccountCode = accountCode
                });
                
                var output = new GetLowBalanceAlertForViewDto {LowBalanceAlert = response.ConvertTo<LowBalanceAlertDto>()};

                return output;
            }

            return new GetLowBalanceAlertForViewDto();
        }
    }
}