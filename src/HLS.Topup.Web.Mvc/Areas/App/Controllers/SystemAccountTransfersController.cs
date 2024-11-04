using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.SystemAccountTransfers;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.BalanceManager;
using HLS.Topup.BalanceManager.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SystemAccountTransfers)]
    public class SystemAccountTransfersController : TopupControllerBase
    {
        private readonly ISystemAccountTransfersAppService _systemAccountTransfersAppService;

        public SystemAccountTransfersController(ISystemAccountTransfersAppService systemAccountTransfersAppService)
        {
            _systemAccountTransfersAppService = systemAccountTransfersAppService;
        }

        public ActionResult Index()
        {
            var model = new SystemAccountTransfersViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_SystemAccountTransfers_Create, AppPermissions.Pages_SystemAccountTransfers_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetSystemAccountTransferForEditOutput getSystemAccountTransferForEditOutput;

				if (id.HasValue){
					getSystemAccountTransferForEditOutput = await _systemAccountTransfersAppService.GetSystemAccountTransferForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getSystemAccountTransferForEditOutput = new GetSystemAccountTransferForEditOutput{
						SystemAccountTransfer = new CreateOrEditSystemAccountTransferDto()
					};
				}

				var viewModel = new CreateOrEditSystemAccountTransferModalViewModel()
				{
					SystemAccountTransfer = getSystemAccountTransferForEditOutput.SystemAccountTransfer,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewSystemAccountTransferModal(int id)
        {
			var getSystemAccountTransferForViewDto = await _systemAccountTransfersAppService.GetSystemAccountTransferForView(id);

            var model = new SystemAccountTransferViewModel()
            {
                SystemAccountTransfer = getSystemAccountTransferForViewDto.SystemAccountTransfer
            };

            return PartialView("_ViewSystemAccountTransferModal", model);
        }


    }
}