using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.IO.Extensions;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DiscountManager;
using HLS.Topup.Dto;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using HLS.Topup.Web.Models.Transaction;
using HLS.Topup.Web.TagHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;
using static HLS.Topup.Common.CommonConst;

namespace HLS.Topup.Web.Controllers
{
    public class CommonController : TopupControllerBase
    {
        private readonly IViewRender _viewRender;
        private readonly ICommonManger _commonManger;
        private readonly UserManager _userManager;
        private readonly ICommonLookupAppService _commonLookup;
        private readonly IDiscountManger _discountManger;
        private readonly TopupAppSession _topupAppSession;
        private readonly ITransactionsAppService _transactionsAppService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly ILogger<CommonController> _logger;

        public CommonController(IViewRender viewRender,
            ICommonManger commonManger,
            IDiscountManger discountManger,
            UserManager userManager,
            IWebHostEnvironment env,
            ITransactionsAppService transactionsAppService,
            ICommonLookupAppService commonLookup,
            TopupAppSession topupAppSession,
            ICommonLookupAppService commonLookupAppService, ILogger<CommonController> logger)
        {
            _viewRender = viewRender;
            _commonManger = commonManger;
            _discountManger = discountManger;
            _topupAppSession = topupAppSession;
            _userManager = userManager;
            _commonLookup = commonLookup;
            _transactionsAppService = transactionsAppService;
            _appConfiguration = env.GetAppConfiguration();
            _commonLookupAppService = commonLookupAppService;
            _logger = logger;
        }

        public async Task<bool> GetAgentVerify()
        {
            try
            {
                var agentProfile = await _userManager.GetAgentProfile(AbpSession.UserId ?? 0);
                var agent = await _userManager.GetUserByIdAsync(agentProfile.UserId);
                return agent.IsVerifyAccount;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAgentVerify error:{e}");
                return false;
            }
        }

        public async Task<JsonResult> GetTransInfo(decimal amount)
        {
            decimal balance = 0;
            decimal limitBalance = 0;
            var accountType = _topupAppSession.AccountType;
            var userInfo = _userManager.GetAccountInfo();
            if (userInfo.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                userInfo.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
                    {AccountCode = userInfo.NetworkInfo.AccountCode, CurrencyCode = "VND"}));
                limitBalance = (await _transactionsAppService.GetLimitAmountBalance(new GetAvailableLimitAccount
                    {AccountCode = _topupAppSession.AccountCode}));
            }
            else
            {
                balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
                    {AccountCode = userInfo.UserInfo.AccountCode, CurrencyCode = "VND"}));
                limitBalance = balance;
            }

            return Json(new
            {
                isActive = userInfo.UserInfo.IsActive,
                accountType,
                balance,
                limitBalance,
                amount = amount
            });
        }

        public async Task<JsonResult> GetPayInfo(string type, Models.PayInfoModel model)
        {
            decimal balance = 0;
            decimal limitBalance = 0;
            var accountType = _topupAppSession.AccountType;
            var userInfo = _userManager.GetAccountInfo();
            if (userInfo.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                userInfo.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
                    {AccountCode = userInfo.NetworkInfo.AccountCode, CurrencyCode = "VND"}));
                limitBalance = (await _transactionsAppService.GetLimitAmountBalance(new GetAvailableLimitAccount
                    {AccountCode = _topupAppSession.AccountCode}));
            }
            else
            {
                balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
                    {AccountCode = userInfo.UserInfo.AccountCode, CurrencyCode = "VND"}));
                limitBalance = balance;
            }

            if (type == "TOPUP" || type == "TOPUP_DATA")
            {
                #region TOPUP

                var cate = (await _commonManger.GetCategories(type, model.CardTelco))
                    .FirstOrDefault();

                var proDiscount =
                    await _discountManger.GeProductDiscountAccount(model.ProductCode,
                        _userManager.GetAccountInfo().NetworkInfo.AccountCode);
                if (proDiscount == null)
                    throw new UserFriendlyException("Sản phẩm không tồn tại");

                var viewModel = new Models.TopupInfoDto
                {
                    CategoryCode = model.CardTelco,
                    CategoryName = cate == null ? "" : cate.CategoryName,
                    PhoneNumber = model.PhoneNumber,
                    CardValue = proDiscount.ProductValue,
                    Quantity = 1,
                    FixAmount = proDiscount.FixAmount ?? 0,
                    Discount = proDiscount.DiscountAmount,
                    TotalAmount = proDiscount.PaymentAmount,
                    Description = proDiscount.Description,
                    ServiceCode = type,
                };
                viewModel.Amount = viewModel.Quantity * viewModel.CardValue;
                return Json(new
                {
                    Data = viewModel,
                    Content = await _viewRender.RenderAsync("~/Views/Common/_PayInfo_TopUp.cshtml", viewModel),
                    accountType,
                    balance,
                    limitBalance,
                    amount = viewModel.Amount
                });

                #endregion
            }
            else if (type == "TOPUPLIST")
            {
                #region TOPUPLIST

                var proDiscount =
                    await _discountManger.GetProductDiscounts(string.Empty,
                        _userManager.GetAccountInfo().NetworkInfo.AccountCode);

                var viewModel = new Models.TopupInfoDto
                {
                    ListNumbers = model.ListNumbers
                };
                viewModel.Amount = model.ListNumbers.Sum(x => x.CardPrice);
                foreach (var item in model.ListNumbers)
                {
                    var proCode = item.CategoryCode + "_" + (item.CardPrice / 1000);
                    var p = proDiscount.FirstOrDefault(x => x.ProductCode == proCode);
                    item.Discount = p?.DiscountAmount ?? 0;
                }

                //todo chỗ này xem lại
                // viewModel.TotalAmount =
                //     model.ListNumbers.Sum(x => x.CardPrice - (x.CardPrice * (x.Discount / 100) + x.FixAmount));
                // viewModel.Discount = model.ListNumbers.Sum(x => x.CardPrice * (x.Discount / 100));
                return Json(new
                {
                    Data = viewModel,
                    Content = await _viewRender.RenderAsync("~/Views/Common/_PayInfo_TopUpList.cshtml", viewModel),
                    accountType,
                    balance,
                    limitBalance,
                    amount = viewModel.Amount
                });

                #endregion
            }
            else if (type == ServiceCodes.PIN_CODE || type == ServiceCodes.PIN_DATA || type == ServiceCodes.PIN_GAME)
            {
                #region PINCODE

                var cate = await _commonManger.GetCategory(model.CardTelco);
                var proDiscount =
                    await _discountManger.GeProductDiscountAccount(model.ProductCode,
                        _userManager.GetAccountInfo().NetworkInfo.AccountCode);
                if (proDiscount == null)
                    throw new UserFriendlyException("Sản phẩm không tồn tại");

                var viewModel = new Models.TopupInfoDto
                {
                    CategoryCode = model.CardTelco,
                    CategoryName = cate == null ? "" : cate.CategoryName,
                    PhoneNumber = model.PhoneNumber,
                    CardValue = proDiscount.ProductValue,
                    Quantity = model.Quantity
                };

                viewModel.Amount = viewModel.Quantity * viewModel.CardValue;
                viewModel.Discount = proDiscount.DiscountAmount * viewModel.Quantity;
                viewModel.TotalAmount = proDiscount.PaymentAmount * viewModel.Quantity;
                viewModel.Email = model.Email;

                return Json(new
                {
                    Data = viewModel,
                    Content = await _viewRender.RenderAsync("~/Views/Common/_PayInfo_PinCode.cshtml", viewModel),
                    accountType,
                    balance,
                    limitBalance,
                    amount = viewModel.Amount
                });

                #endregion
            }

            return Json(new {Content = "", accountType, balance, limitBalance});
        }

        public async Task<JsonResult> GetPayInfoDeposit(string type, Models.DepositInfoModel model)
        {
            if (type == "DEPOSIT")
            {
                return Json(new
                {
                    Data = model,
                    Content = await _viewRender.RenderAsync("~/Views/Common/_PayInfo_Deposit.cshtml", model)
                });
            }

            if (type == "TRANSFER")
            {
                var desUser = await _commonLookup.GetUserInfoQuery(new GetUserInfoRequest() {UserName = model.Account});
                model.BankName = desUser.PhoneNumber + " " + desUser.FullName;
                return Json(new
                {
                    Data = model,
                    Content = await _viewRender.RenderAsync("~/Views/Common/_PayInfo_Transfer.cshtml", model)
                });
            }

            return Json(new {Content = ""});
        }

        public async Task<JsonResult> GetTransactionPrint(string transcode)
        {
            string content = "";
            var model = new TransactionDetailsInfoModel();
            var user = _userManager.GetAccountInfo();
            var agentProfile = _userManager.GetAgentProfile(user.UserInfo.Id);

            var city = "";
            if (agentProfile.Result.CityId.HasValue)
            {
                city = ", " + _commonLookupAppService.GetCityById((int) agentProfile.Result.CityId).Result.CityName;
            }

            var district = "";
            if (agentProfile.Result.DistrictId.HasValue)
            {
                district = ", " + _commonLookupAppService.GetDistrictsById((int) agentProfile.Result.DistrictId).Result
                    .DistrictName;
            }

            var ward = "";
            if (agentProfile.Result.WardId.HasValue)
            {
                ward = ", " + _commonLookupAppService.GetWardsById((int) agentProfile.Result.WardId).Result.WardName;
            }

            model.TransactionInfo = await _transactionsAppService.GetTransactionByCode(transcode);
            if (model.TransactionInfo == null)
            {
                return Json(new
                {
                    Content = string.Empty
                });
            }

            if ((user.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                 user.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi) &&
                user.UserInfo.AccountCode != model.TransactionInfo.StaffAccount)
            {
                return Json(new
                {
                    Content = string.Empty
                });
            }

            if (user.NetworkInfo.AccountCode != model.TransactionInfo.PartnerCode)
            {
                return Json(new
                {
                    Content = string.Empty
                });
            }

            model.Transcode = transcode;
            model.Network = user.NetworkInfo;
            model.Address = agentProfile.Result.Address + ward + district + city;
            model.Vendors = await _transactionsAppService.GetVendors();
            model.Items = await _transactionsAppService.GetTransactionDetail(transcode);
            if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.TOPUP)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/TOPUP.cshtml", model);
            }
            else if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/TOPUP_DATA.cshtml", model);
            }
            else if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.PAY_BILL)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/PAY_BILL.cshtml", model);
            }
            else if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.PIN_DATA)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/PIN_DATA.cshtml", model);
            }
            else if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.PIN_CODE)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/PIN_CODE.cshtml", model);
            }
            else if (model.TransactionInfo.ServiceCode == CommonConst.ServiceCodes.PIN_GAME)
            {
                content = await _viewRender.RenderAsync("~/Views/Common/Print/PIN_GAME.cshtml", model);
            }

            return Json(new
            {
                Data = model,
                Content = content
            });
        }

        public async Task<JsonResult> GetProvince()
        {
            var data = await _commonLookup.GetProvinces();
            return Json(data);
        }

        public async Task<JsonResult> GetDistrict(int? provinceId)
        {
            var data = await _commonLookup.GetDistricts(provinceId, false);
            return Json(data);
        }

        public async Task<JsonResult> GetWard(int? districtId)
        {
            var data = await _commonLookup.GetWards(districtId, false);
            return Json(data);
        }

        public async Task<JsonResult> DetectImageFile(IFormFile file)
        {
            if (file.Length <= 0)
                throw new UserFriendlyException("File not found");
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            var img = Convert.ToBase64String(fileBytes);
            return await DetectImage(img);
        }

        public async Task<JsonResult> DetectImage(string image)
        {
            try
            {
                var json = (new {image = image}).ToJson();
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var host = _appConfiguration["sol:host"].ToString();
                var contentType = _appConfiguration["sol:contentType"].ToString();
                var apiKey = _appConfiguration["sol:apiKey"].ToString();
                var recognitionUrl = _appConfiguration["sol:recognition"].ToString();
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(host),
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("api-key", apiKey);
                var response = await client.PostAsync(recognitionUrl, data);

                var result = await response.Content.ReadAsStringAsync();
                return Json(result);
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> TopupCodeZipFile(string transcode)
        {
            return await _transactionsAppService.ZipCards(transcode);
        }

        [ResponseCache(VaryByHeader = "User-Agent", Duration = 300)]
        public virtual ActionResult GetScripts()
        {
            //Return they JS we got from the ABP end point via Flurl
            return new JavaScriptResult(OzCrGetScriptsHelper.GetScripts(Request.HttpContext, true));
        }
        private class JavaScriptResult : ContentResult
        {
            public JavaScriptResult(string script)
            {
                this.Content = script;
                this.ContentType = "application/javascript";
            }
        }
    }
}
