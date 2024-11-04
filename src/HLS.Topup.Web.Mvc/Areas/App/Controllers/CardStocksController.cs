using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.CardStocks;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.StockManagement;
using HLS.Topup.StockManagement.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceStack;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CardStocks)]
    public class CardStocksController : TopupControllerBase
    {
        private readonly ICardStocksAppService _cardStocksAppService;
        private readonly ICardsAppService _cardsAppService;
        private readonly ICommonLookupAppService _commonSv;

        public CardStocksController(ICardStocksAppService cardStocksAppService, ICommonLookupAppService commonSv, ICardsAppService cardsAppService)
        {
            _cardStocksAppService = cardStocksAppService;
            _cardsAppService = cardsAppService;
            _commonSv = commonSv;
        }


        public async Task<ActionResult> Index()
        {
            var model = new CardStocksViewModel
            {
                FilterText = "",
                VendorList = await _cardsAppService.GetAllVendorForTableDropdown(),
                CardValues = _cardsAppService.CardValues(),
                StockCodes = await _cardStocksAppService.StockCodes(),
                ServicesCard = await _commonSv.ServiceCardList(),
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_CardStocks_Create, AppPermissions.Pages_CardStocks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(string code, string productCode, decimal cardValue)
        {
            GetCardStockForEditOutput getCardStockForEditOutput;
            var action = "Add";
            if (!string.IsNullOrEmpty(code))
            {
                action = "Edit";
                getCardStockForEditOutput = await _cardStocksAppService.GetCardStockForEdit(code, productCode, cardValue);
            }
            else
            {
                getCardStockForEditOutput = new GetCardStockForEditOutput
                {
                    CardStock = new CreateOrEditCardStockDto()
                    {
                        Status = CommonConst.CardStockStatus.Active,
                        //StockCode = "STOCK_ACTIVE",
                        //VendorCode = "",
                        //ProductCode = "",
                    }
                };
            }

            var viewModel = new CreateOrEditCardStockModalViewModel()
            {
                CardStock = getCardStockForEditOutput.CardStock,
                CardValues = _cardsAppService.CardValues(),
                CategoryList = await _cardsAppService.GetAllVendorForTableDropdown(),
                ServicesCard = await _commonSv.ServiceCardList(),
                StockCodes = await _cardStocksAppService.StockCodes(),
                IsEditMode = action != "Add"
            };
            ViewBag.ActionType = action;
            return PartialView("_CreateOrEditModal", viewModel);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_CardStocks_EditQuantity)]
        public async Task<PartialViewResult> EditQuantityModal(string code, string productCode, decimal cardValue)
        {
            GetCardStockForEditOutput getCardStockForEditOutput;            
            getCardStockForEditOutput = await _cardStocksAppService.GetCardStockForEdit(code, productCode, cardValue);
            var viewModel = new CreateOrEditCardStockModalViewModel()
            {
                CardStock = getCardStockForEditOutput.CardStock,
                CardValues = _cardsAppService.CardValues(),
                CategoryList = await _cardsAppService.GetAllVendorForTableDropdown(),
                ServicesCard = await _commonSv.ServiceCardList(),
                StockCodes = await _cardStocksAppService.StockCodes(),
                IsEditMode = true
            };
            ViewBag.ActionType = true;
            return PartialView("_EditQuantityModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CardStocks_Edit)]
        public async Task<PartialViewResult> TransferModal(Guid? id)
        {
            var action = "Add";
            TransferCardStockDto data;
            if (id.HasValue)
            {
                action = "Edit";
                data = await _cardStocksAppService.GetTransferStock(id.Value);
            }
            else
            {
                data = new TransferCardStockDto();
            }

            var cardVal = _cardsAppService.CardValues();
            var viewModel = data.ConvertTo<TransferCardStockModalViewModel>();
            viewModel.IsEditMode = action != "Add";
            viewModel.BatchList = await _cardsAppService.GetAllCardBatchForTableDropdown();
            viewModel.VendorList = await _cardsAppService.GetAllVendorForTableDropdown();
            viewModel.ServicesCard = await _commonSv.ServiceCardList();
            viewModel.CardValues = cardVal.Select(x => (new CommonLookupTableDto()
            { Id = x.ToString(), DisplayName = x.ToString() })).ToList();
            viewModel.StockCodes = await _cardStocksAppService.StockCodes();

            ViewBag.ActionType = action;
            return PartialView("_TransferModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CardStocks_Edit)]
        public async Task<PartialViewResult> TransferCardModal(Guid? id)
        {
            var action = "Add";
            TransferCardStockDto data = new TransferCardStockDto();

            var viewModel = data.ConvertTo<TransferCardStockModalViewModel>();
            viewModel.IsEditMode = action != "Add";
            viewModel.BatchList = await _cardsAppService.GetAllCardBatchForTableDropdown();

            viewModel.ServicesCard = await _commonSv.ServiceCardList();

            viewModel.StockCodes = await _cardStocksAppService.StockCodes();

            ViewBag.ActionType = action;
            return PartialView("_TransferCardModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCardStockModal(string code, string productCode, decimal cardValue)
        {
            var getCardStockForViewDto = await _cardStocksAppService.GetCardStockForView(code, productCode, cardValue);

            var model = new CardStockViewModel()
            {
                CardStock = getCardStockForViewDto.CardStock,
            };

            return PartialView("_ViewCardStockModal", model);
        }
    }
}
