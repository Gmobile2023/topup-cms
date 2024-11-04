using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.Extensions.Logging;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.RequestDtos.TopupGateResponseMessage;
using HLS.Topup.TopupGateResponseMessage;
using HLS.Topup.TopupGateResponseMessage.Dto;
using HLS.Topup.TopupGateResponseMessageManager;
using ServiceStack;

namespace HLS.Topup.TopupGateResponseMessageAppService
{
    [AbpAuthorize(AppPermissions.Pages_TopupGateResponseMessage)]
    public class TopupGateResponseMessageAppService : TopupAppServiceBase, ITopupGateResponseMessageAppService
    {
        private readonly ITopupGateResponseMessageManager _topupGateResponseMessageManager;
        private readonly ILogger<TopupGateResponseMessageAppService> _logger;

        public TopupGateResponseMessageAppService(ITopupGateResponseMessageManager topupGateResponseMessageManager,
            ILogger<TopupGateResponseMessageAppService> logger)
        {
            _topupGateResponseMessageManager = topupGateResponseMessageManager;
            _logger = logger;
        }

        public async Task CreateOrEditTopupGateResponseMessage(CreateOrEditTopupGateResponse input)
        {
            try
            {
                if (input.Id != Guid.Empty)
                {
                    var request = input.ConvertTo<UpdateTopupGateResponseMessageRequest>();
                    await _topupGateResponseMessageManager.UpdateTopupGateResponseMessageManager(request);
                }
                else
                {
                    var request = input.ConvertTo<CreateTopupGateResponseMessageRequest>();
                    await _topupGateResponseMessageManager.CreateTopupGateResponseMessageManager(request);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<GetTopupGateRMForEditOutput> GetTopupGateRMForEdit(string provider, string code)
        {
            try
            {
                var request = new GetTopupGateResponseMRequest()
                {
                    Provider = provider,
                    Code = code,
                };
                var rs = await _topupGateResponseMessageManager.GetTopupGateResponseMessageManager(request);
                var output = new GetTopupGateRMForEditOutput()
                {
                    TopupGateResponseMessage = rs.Results.ConvertTo<TopupGateResponseMessageDto>()
                };
                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<TopupGateResponseMessageDto>> GetListTopupGateResponseMessage(
            GetAllTopupGateRMInput input)
        {
            try
            {
                var request = input.ConvertTo<GetListTopupGateResponseRMRequest>();
                var rs = await _topupGateResponseMessageManager.GetListTopupGateResponseMessageAsync(request);
                return new PagedResultDto<TopupGateResponseMessageDto>(
                    rs.Total,
                    rs.Payload.ConvertTo<List<TopupGateResponseMessageDto>>().OrderByDescending(x => x.AddedAtUtc)
                        .ThenBy(x => x.Provider).ThenBy(x => x.Code).ThenBy(x => x.Name).ToList()
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<GetTopupGateRMForView> GetTopupGateRMForView(string provider, string code)
        {
            try
            {
                var request = new GetTopupGateResponseMRequest()
                {
                    Provider = provider,
                    Code = code
                };
                var rs = await _topupGateResponseMessageManager.GetTopupGateResponseMessageManager(request);
                var output = new GetTopupGateRMForView()
                {
                    TopupGateResponseMessage = rs.Results,
                };
                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<object> CreateListTopupGateRM(
            CreateListTopupGateRMRequest request)
        {
            try
            {
                var rs = await _topupGateResponseMessageManager.CreateListTopupGateResponseMessageAsync(request);
                return rs;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TopupGateResponseMessage_Delete)]
        public async Task<object> DeleteTopupGateRM(string provider, string code)
        {
            try
            {
                var request = new DeleteTopupGateResponseMessageRequest()
                {
                    Provider = provider,
                    Code = code
                };

                var rs = await _topupGateResponseMessageManager.DeleteTopupGateResponseMessageManager(request);
                return rs;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}