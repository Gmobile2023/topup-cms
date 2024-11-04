using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.CardBatchs;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.StockManagement;
using HLS.Topup.StockManagement.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Common;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CardBatchs)]
    public class CardBatchsController : TopupControllerBase
    {
        private readonly ICardBatchsAppService _cardBatchsAppService;

        public CardBatchsController(ICardBatchsAppService cardBatchsAppService)
        {
            _cardBatchsAppService = cardBatchsAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new CardBatchsViewModel
            {
                FilterText = "",
                ProviderList = await _cardBatchsAppService.GetAllProviderForTableDropdown(),
                // VendorList =  await _cardBatchsAppService.GetAllVendorForTableDropdown(),           
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_CardBatchs_Create, AppPermissions.Pages_CardBatchs_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCardBatchForEditOutput getCardBatchForEditOutput;

            if (id.HasValue)
            {
                getCardBatchForEditOutput = await _cardBatchsAppService.GetCardBatchForEdit(id ?? Guid.NewGuid());
            }
            else
            {
                getCardBatchForEditOutput = new GetCardBatchForEditOutput
                {
                    CardBatch = new CreateOrEditCardBatchDto()
                    {
                        Status = CommonConst.CardPackageStatus.Active
                    }
                };
                getCardBatchForEditOutput.CardBatch.CreatedDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditCardBatchModalViewModel()
            {
                CardBatch = getCardBatchForEditOutput.CardBatch,
                ProviderName = getCardBatchForEditOutput.ProviderName,
                CategoryName = getCardBatchForEditOutput.VendorName,
                ProviderList = await _cardBatchsAppService.GetAllProviderForTableDropdown(),
                // VendorList =  await _cardBatchsAppService.GetAllVendorForTableDropdown(),                
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewCardBatchModal(Guid id)
        {
            var getCardBatchForViewDto = await _cardBatchsAppService.GetCardBatchForView(id);

            var model = new CardBatchViewModel()
            {
                CardBatch = getCardBatchForViewDto.CardBatch,
                ProviderName = getCardBatchForViewDto.ProviderName,
                // CategoryName = getCardBatchForViewDto.VendorName,
                //ProviderList = await _cardBatchsAppService.GetAllProviderForTableDropdown(),
                // VendorList =  await _cardBatchsAppService.GetAllVendorForTableDropdown(),         
            };
         

            return PartialView("_ViewCardBatchModal", model);
        }
    }
}