using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.BatchAirtimes;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.StockManagement;
using HLS.Topup.StockManagement.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BatchAirtimes)]
    public class BatchAirtimesController : TopupControllerBase
    {
        private readonly IBatchAirtimesAppService _batchAirtimesAppService;
        private readonly IStocksAirtimesAppService _stocksAirtimeAppService;

        public BatchAirtimesController(IBatchAirtimesAppService batchAirtimesAppService,
	        IStocksAirtimesAppService stocksAirtimesAppService)
        {
            _batchAirtimesAppService = batchAirtimesAppService;
            _stocksAirtimeAppService = stocksAirtimesAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new BatchAirtimesViewModel
			{
				FilterText = "",
				ProviderList = await _stocksAirtimeAppService.GetAllProvider()
			};
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BatchAirtimes_Create, AppPermissions.Pages_BatchAirtimes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(string code)
        {
	        BatchAirtimeDto dto;
	        if (!string.IsNullOrEmpty(code)){
		        dto = await _batchAirtimesAppService.GetBatchAirtimeForEdit(code);
	        }
	        else{
		        dto = new BatchAirtimeDto{
			        Status = (byte)BatchAirtimeStatus.Init,
			        Amount = 0,
			        Discount = 0,
		        };
	        }  
	        var viewModel = new CreateOrEditBatchAirtimeModalViewModel()
	        {
		        BatchAirtime = dto,
		        ProviderList = await _stocksAirtimeAppService.GetAllProvider()
	        };
            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewBatchAirtimeModal(string code)
        {
	        BatchAirtimeDto dto = await _batchAirtimesAppService.GetBatchAirtimeForView(code);

            var model = new BatchAirtimeViewModel()
            {
	            BatchAirtime = dto
            };
            return PartialView("_ViewBatchAirtimeModal", model);
        }


    }
}