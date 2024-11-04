using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Identity;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common;
using HLS.Topup.Products;
using HLS.Topup.Web.Models.BillPayment;
using ServiceStack;
using System.Collections.Generic;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Web.Controllers
{
    public class HomeController : TopupControllerBase
    {
        private readonly ICommonManger _commonManger;
        private readonly TopupAppSession _topupAppSession;
        private readonly UserManager _userManager;
        private readonly IConfigurationRoot _appConfiguration;

        public HomeController(ICommonManger commonManger, TopupAppSession topupAppSession, UserManager userManager,
            IWebHostEnvironment env)
        {
            _commonManger = commonManger;
            _topupAppSession = topupAppSession;
            _userManager = userManager;
            _appConfiguration = env.GetAppConfiguration();
        }

        [AbpMvcAuthorize]
        public async Task<IActionResult> Index()
        {
            var link = _appConfiguration["App:DownloadApp"];
            ViewBag.Android = link.Split("|")[1];
            ViewBag.Ios = link.Split("|")[0];
            var checkService = await _commonManger.CheckServiceActive(CommonConst.ServiceCodes.PAY_BILL);
            if (!checkService)
            {
                TempData["Type"] = CommonConst.MessageType.ServiceDisible;
                return RedirectToAction("Message", "Home");
            }

            if (_topupAppSession.AccountType == CommonConst.SystemAccountType.StaffApi ||
                _topupAppSession.AgentType == CommonConst.AgentType.AgentApi)
            {
                return RedirectToAction("Index", "Profile");
            }

            var cates = await _commonManger.GetCategories(CommonConst.ServiceCodes.PAY_BILL, null);
            var viewModel = new BillPaymentCategoryModel()
            {
                Categorys = cates.ConvertTo<List<CategoryDto>>()
            };

            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            ViewBag.Name = user.IsVerifyAccount ? _topupAppSession.AgentName : _topupAppSession.FullName;

            return View(viewModel);
        }

        public async Task<IActionResult> Message()
        {
            ViewBag.Type = TempData["Type"];
            return View();
        }
    }
}
