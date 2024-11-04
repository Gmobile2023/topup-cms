using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.StocksAirtimes;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.StockManagement;
using HLS.Topup.StockManagement.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Dtos.Provider;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_StocksAirtimes)]
    public class StocksAirtimesController : TopupControllerBase
    {
        private readonly IStocksAirtimesAppService _stocksAirtimeAppService;

        public StocksAirtimesController(IStocksAirtimesAppService stocksAirtimesAppService)
        {
            _stocksAirtimeAppService = stocksAirtimesAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new StocksAirtimesViewModel
            {
                FilterText = "",
                ProviderList = await _stocksAirtimeAppService.GetAllProvider()
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StocksAirtimes_Create, AppPermissions.Pages_StocksAirtimes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(string code)
        {
            StocksAirtimeDto dto;
            if (!string.IsNullOrEmpty(code))
            {
                dto = await _stocksAirtimeAppService.GetStocksAirtimeForEdit(code);
            }
            else
            {
                dto = new StocksAirtimeDto
                {
                    Status = StocksAirtimeStatus.Active,
                    TotalAirtime = 0,
                    TotalAmount = 0,
                    MinLimitAirtime = 0,
                    MaxLimitAirtime = 0
                };
            }
            var viewModel = new CreateOrEditStocksAirtimeModalViewModel()
            {
                StocksAirtime = dto,
                ProviderList = await _stocksAirtimeAppService.GetAllProvider()
            };
            return PartialView("_CreateOrEditModal", viewModel);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_StocksAirtimes_Deposit)]
        public async Task<PartialViewResult> DepositAirtimeModal(string keyCode, string providerCode)
        {
            var item = new CreateOrEditStocksAirtimeModalViewModel()
            {
                StocksAirtime = new StocksAirtimeDto()
                {
                    KeyCode = keyCode,
                    ProviderCode = providerCode
                }
            };
            return PartialView("_DepositAirtimeModal", item);
        }

        public async Task<PartialViewResult> ViewStocksAirtimeModal(string code)
        {
            StocksAirtimeDto dto = await _stocksAirtimeAppService.GetStocksAirtimeForView(code);

            var model = new StocksAirtimeViewModel()
            {
                StocksAirtime = dto
            };
            return PartialView("_ViewStocksAirtimeModal", model);
        }


    }
}
