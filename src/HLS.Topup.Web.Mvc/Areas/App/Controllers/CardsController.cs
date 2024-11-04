using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.Cards;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Aspose.Cells;
using Hangfire;
using HLS.Topup.Common;
using HLS.Topup.StockManagement;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.StockManagement.Importing;
using HLS.Topup.Storage;
using HLS.Topup.Topup.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Scaffolding;
using ServiceStack;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Cards)]
    public class CardsController : TopupControllerBase
    {
        private readonly ICardsAppService _cardsAppService;
        private readonly ICommonLookupAppService _commonSv;
        private readonly ICardStocksAppService _cardStocksAppService;
        private readonly ICardListExcelDataReader _cardListExcelDataReader;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IWebHostEnvironment _env;

        public CardsController(ICardsAppService cardsAppService,
            ICardStocksAppService cardStocksAppService,
            ICardListExcelDataReader cardListExcelDataReader,
            IWebHostEnvironment env,
            ICommonLookupAppService commonSv,
            IBinaryObjectManager binaryObjectManager)
        {
            _cardsAppService = cardsAppService;
            _binaryObjectManager = binaryObjectManager;
            _cardStocksAppService = cardStocksAppService;
            _cardListExcelDataReader = cardListExcelDataReader;
            _env = env;
            _commonSv = commonSv;
        }

        public async Task<ActionResult> Index()
        {
            var model = new CardsViewModel
            {
                FilterText = "",
                //BatchList = await _cardsAppService.GetAllCardBatchForTableDropdown(),
                ProviderList = await _cardsAppService.GetAllProviderForTableDropdown(),
                VendorList = await _cardsAppService.GetAllVendorForTableDropdown(),
                ServicesCard = await _commonSv.ServiceCardList(),
                StockCodes = await _cardStocksAppService.StockCodes(),
                CardValues = _cardsAppService.CardValues()
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCardForEditOutput getCardForEditOutput;

            if (id.HasValue)
            {
                getCardForEditOutput = await _cardsAppService.GetCardForEdit(id ?? Guid.NewGuid());
            }
            else
            {
                getCardForEditOutput = new GetCardForEditOutput
                {
                    Card = new CreateOrEditCardDto()
                    {
                        Status = CommonConst.CardStatus.Active
                    }
                };
                getCardForEditOutput.Card.ExpiredDate = DateTime.Now;
            }

            var cardVal = _cardsAppService.CardValues();
            var viewModel = new CreateOrEditCardModalViewModel()
            {
                Card = getCardForEditOutput.Card,

                BatchList = await _cardsAppService.GetAllCardBatchForTableDropdown(),
                ProviderList = await _cardsAppService.GetAllProviderForTableDropdown(),
                VendorList = await _cardsAppService.GetAllVendorForTableDropdown(),
                CardValues = new SelectList(cardVal.Select(x => (new { Id = x, Value = x })), "Id", "Value",
                    getCardForEditOutput.Card.CardValue),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCardModal(Guid id)
        {
            var getCardForViewDto = await _cardsAppService.GetCardForView(id);

            var model = new CardViewModel()
            {
                Card = getCardForViewDto.Card,
            };

            return PartialView("_ViewCardModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<PartialViewResult> CreateCardsApiModal()
        {
            var provider = await _cardsAppService.GetAllProviderForTableDropdown();
            var viewModel = new CreateCardsApiModalModel()
            {
                Description = "",
                ProviderCode = "",
                ProviderList = provider != null ? provider.Where(c => !c.Id.ToUpper().StartsWith("NHATTRAN")).ToList() : new List<CardProviderLookupTableDto>(),
                ExpiredDate = null,
            };
            return PartialView("_CreateCardsApiModal", viewModel);
        }


        #region ImportCardsFromExcel

        [AbpMvcAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<PartialViewResult> ImportCardsModal()
        {
            var viewModel = new ImportCardsFileModalModel()
            {
                ProviderList = await _cardsAppService.GetAllProviderForTableDropdown(),
            };
            return PartialView("_ImportCardsModal", viewModel);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<ResponseMessages> ReadFileImport()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var dataList = _cardListExcelDataReader.GetCardsFromExcel(fileBytes);
                if (dataList == null || !dataList.Any())
                {
                    return new ResponseMessages
                    {
                        ResponseCode = "00",
                        ResponseMessage = "Lỗi không load được dữ liệu file import"
                    };
                }
                return await _cardsAppService.GetCardImportList(dataList);
            }
            catch (Exception ex)
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Cards_Create, AppPermissions.Pages_Cards_Edit)]
        public async Task<ResponseMessages> ImportCardsFromJobExcel(string data, string providerCode, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(providerCode))
                    throw new UserFriendlyException(L("ProviderCode_Empty_Error"));
                if (string.IsNullOrEmpty(data))
                    throw new UserFriendlyException(L("Data_Empty_Error"));
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);
                await _binaryObjectManager.SaveAsync(fileObject);
                BackgroundJob.Enqueue<ICardsAppService>((x) => x.ImportCardsJob(fileObject.Id, providerCode, data, description, AbpSession.ToUserIdentifier()));

                return new ResponseMessages
                {
                    ResponseCode = "01",
                    ResponseMessage = L("Bắt đầu nhập file thẻ, vui lòng chờ thông báo kết quả xử lý!")
                };
            }
            catch (Exception ex)
            {
                return new ResponseMessages
                {
                    ResponseCode = "00",
                    ResponseMessage = L("Error")
                };
            }
        }


        public async Task<ActionResult> ListTransCard()
        {
            var model = new CardsViewModel
            {
                FilterText = "",
                ProviderList = await _cardsAppService.GetAllProviderForTableDropdown()
            };

            return View(model);
        }


        #endregion
    }
}