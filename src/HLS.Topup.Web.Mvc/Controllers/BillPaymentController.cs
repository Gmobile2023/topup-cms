using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common;
using HLS.Topup.Products;
using HLS.Topup.Web.Models.BillPayment;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BillPaymentController : TopupControllerBase
    {
        private readonly ICommonManger _commonManger;
        private readonly ICommonLookupAppService _commonLookupApp;

        public BillPaymentController(ICommonManger commonManger,
            ICommonLookupAppService commonLookupApp)
        {
            _commonManger = commonManger;
            _commonLookupApp = commonLookupApp;
        }

        public async Task<IActionResult> Index()
        {
            var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.PAY_BILL);
            if (!checkService)
            {
                TempData["Type"] = CommonConst.MessageType.ServiceDisible;
                return RedirectToAction("Message", "Home");
            }

            var checkStaff = await _commonManger.CheckStaffTime(AbpSession.UserId ?? 0);
            if (!checkStaff)
            {
                TempData["Type"] = CommonConst.MessageType.StaffOutTime;
                return RedirectToAction("Message", "Home");
            }

            var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.PAY_BILL, null);
            var viewModel = new BillPaymentCategoryModel()
            {
                Categorys = cates.ConvertTo<List<CategoryDto>>()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ChooseBillService(string categoryCode)
        {
            var listProduct = await _commonLookupApp.GetProducts(new HLS.Topup.Common.Dto.ProductSearchInput()
                {CategoryCode = categoryCode});
            var viewModel = new BillPaymentCategoryModel()
            {
                Products = listProduct.ConvertTo<List<Product>>()
            };
            return View(viewModel);
        }

        // public IActionResult PaymentInfo()
        // {
        //     return View();
        // }
    }
}
