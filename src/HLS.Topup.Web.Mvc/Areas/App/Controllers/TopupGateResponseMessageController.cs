using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using HLS.Topup.TopupGateResponseMessage;
using HLS.Topup.TopupGateResponseMessage.Dto;
using HLS.Topup.Web.Areas.App.Models.TopupGateResponseMessage;
using HLS.Topup.Web.Controllers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TopupGateResponseMessage)]
    public class TopupGateResponseMessageController : TopupControllerBase
    {
        private readonly ITopupGateResponseMessageAppService _topupGateResponseMessageAppService;

        public TopupGateResponseMessageController(
            ITopupGateResponseMessageAppService topupGateResponseMessageAppService)
        {
            _topupGateResponseMessageAppService = topupGateResponseMessageAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CreateOrEditTopupGateResponseMessageModal([CanBeNull] string provider,
            [CanBeNull] string code)
        {
            GetTopupGateRMForEditOutput getTopupGateRmForEditOutput;
            if (!string.IsNullOrEmpty(provider) && !string.IsNullOrEmpty(code))
            {
                getTopupGateRmForEditOutput =
                    await _topupGateResponseMessageAppService.GetTopupGateRMForEdit(provider, code);
            }
            else
            {
                getTopupGateRmForEditOutput = new GetTopupGateRMForEditOutput()
                {
                    TopupGateResponseMessage = new TopupGateResponseMessageDto()
                };
            }

            var model = new CreateOrEditTopupGateRMModel()
            {
                TopupGateResponseMessage = getTopupGateRmForEditOutput.TopupGateResponseMessage,
            };
            return PartialView("_CreateOrEditTopupGateResponseMessageModal", model);
        }

        public async Task<PartialViewResult> ViewTopupGateResponseMessage([CanBeNull] string provider,
            [CanBeNull] string code)
        {
            GetTopupGateRMForView getTopupGateRmForView;
            if (!string.IsNullOrEmpty(provider) && !string.IsNullOrEmpty(code))
            {
                getTopupGateRmForView =
                    await _topupGateResponseMessageAppService.GetTopupGateRMForView(provider, code);
            }
            else
            {
                getTopupGateRmForView = new GetTopupGateRMForView()
                {
                    TopupGateResponseMessage = new TopupGateResponseMessageDto(),
                };
            }

            var model = new GetTopupGateRMViewModel()
            {
                TopupGateResponseMessage = getTopupGateRmForView.TopupGateResponseMessage
            };
            return PartialView("_viewTopupGateResponseMessageModal", model);
        }
    }
}