using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Web.Areas.App.Models.PayBatchBills;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Authorization;
using HLS.Topup.BalanceManager;
using HLS.Topup.BalanceManager.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using HLS.Topup.Common;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PayBatchBills)]
    public class PayBatchBillsController : TopupControllerBase
    {
        private readonly IPayBatchBillsAppService _payBatchBillsAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;

        public PayBatchBillsController(IPayBatchBillsAppService payBatchBillsAppService,
            ICommonLookupAppService commonLookupAppService)
        {
            _payBatchBillsAppService = payBatchBillsAppService;
            _commonLookupAppService = commonLookupAppService;

        }

        public ActionResult Index()
        {
            var categorys = _commonLookupAppService.GetCategories(new HLS.Topup.Common.Dto.CategorySearchInput()
            {
                ServiceCode = "PAY_BILL"
            }).Result;

            var model = new PayBatchBillsViewModel
            {
                FilterText = "",

                Categorys = (from x in categorys.ToList()
                             select new ComboboxItemDto()
                             {
                                 Value = x.CategoryCode,
                                 DisplayText = x.CategoryName,
                             }).ToList(),
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_PayBatchBills_Create)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPayBatchBillForEditOutput getPayBatchBillForEditOutput;

            if (id.HasValue)
            {
                getPayBatchBillForEditOutput = await _payBatchBillsAppService.GetPayBatchBillForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getPayBatchBillForEditOutput = new GetPayBatchBillForEditOutput
                {
                    PayBatchBill = new CreateOrEditPayBatchBillDto()
                };
                getPayBatchBillForEditOutput.PayBatchBill.FromDate = DateTime.Now;
                getPayBatchBillForEditOutput.PayBatchBill.ToDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditPayBatchBillModalViewModel()
            {
                PayBatchBill = getPayBatchBillForEditOutput.PayBatchBill,
                ProductProductName = getPayBatchBillForEditOutput.ProductName,
                PayBatchBillProductList = await _payBatchBillsAppService.GetAllProductForTableDropdown(),
            };

            var categorys = _commonLookupAppService.GetCategories(new HLS.Topup.Common.Dto.CategorySearchInput()
            {
                ServiceCode = "PAY_BILL"
            }).Result;

            viewModel.Categorys = (from x in categorys.ToList()
                                   select new ComboboxItemDto()
                                   {
                                       Value = x.CategoryCode,
                                       DisplayText = x.CategoryName,
                                   }).ToList();

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewPayBatchBillModal(int id)
        {
            var getPayBatchBillForViewDto = await _payBatchBillsAppService.GetPayBatchBillForView(id);

            var model = new PayBatchBillViewModel()
            {
                PayBatchBill = getPayBatchBillForViewDto.PayBatchBill,
                CategoryName = getPayBatchBillForViewDto.CategoryName,
                ProductName = getPayBatchBillForViewDto.ProductName,
                UserCreated = getPayBatchBillForViewDto.UserCreated,
                UserApproval = getPayBatchBillForViewDto.UserApproval
            };

            return PartialView("_ViewPayBatchBillModal", model);
        }


    }
}