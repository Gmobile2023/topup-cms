using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Deposits;
using HLS.Topup.Dto;
using HLS.Topup.Transactions;
using HLS.Topup.Transactions.Dtos;
using HLS.Topup.Web.Models.Transaction;
using log4net.Util;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TransactionsController : TopupControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IAccountManager _accountManager;
        private readonly ITransactionsAppService _transactionsAppService;
        private readonly IDepositsAppService _deposits;

        public TransactionsController(UserManager userManager, ITransactionsAppService transactionsAppService,
            IAccountManager accountManager,
            IDepositsAppService deposits)
        {
            _userManager = userManager;
            _transactionsAppService = transactionsAppService;
            _deposits = deposits;
            _accountManager = accountManager;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransferMoney)]
        public async Task<IActionResult> TransferMoney()
        {
            return View();
        }

        public async Task<IActionResult> TransactionInfo(TransactionInfoModel model)
        {
            model.TransInfo = new Dtos.Transactions.TopupRequestResponseDto();
            if (model.TransType == CommonConst.TransactionType.Transfer.ToString("G"))
                return View(model);
            if (model.Code != "1") return View(model);
            var user = _userManager.GetAccountInfo();

            if (model.TransType == CommonConst.TransactionType.Deposit.ToString("G"))
            {
                var check = await _deposits.GetDeposit(model.TransCode, AbpSession.UserId ?? 0);
                if (check == null)
                {
                    model.Code = "0";
                    model.Message = "Xin lỗi! Không tìm thấy thông tin giao dịch của bạn";
                    return View(model);
                }

                model.RequestCode = check.RequestCode;
                ViewBag.AccountCode = user.UserInfo.AccountCode;

                return View(model);
            }

            var data = await _transactionsAppService.GetTransactionByCode(model.TransCode);
            if (data == null)
            {
                model.Code = "0";
                model.Message = "Xin lỗi! Không tìm thấy thông tin giao dịch của bạn";
                return View(model);
            }

            if (user.UserInfo.AgentType == CommonConst.AgentType.WholesaleAgent &&
                model.TransType == CommonConst.TransactionType.PinCode.ToString("G"))
            {
                var account = await _accountManager.GetAccount(user.UserInfo.Id);
                ViewBag.MethodReceivePassFile = account.MethodReceivePassFile;
                model.TransInfo = data;
                return View("TransactionInfo_Zip", model);
            }

            if ((user.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                 user.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
                &&
                user.UserInfo.AccountCode == data.StaffAccount ||
                user.NetworkInfo.AccountCode == data.PartnerCode)
            {
                model.TransInfo = data;
                return View(model);
            }

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BalanceHistory)]
        public async Task<IActionResult> BalanceHistory()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransactionHistory)]
        public async Task<IActionResult> TransactionHistory()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TransactionHistory)]
        public async Task<IActionResult> TransactionDetail(string transcode, string serviceCode)
        {
            var model = new TransactionDetailDto
            {
                TransCode = transcode
            };
            if (serviceCode == CommonConst.ServiceCodes.PIN_CODE)
                return View("TransactionDetailBuyCard", model);
            return View("TransactionDetailBuyCard", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RequestDeposit)]
        public async Task<IActionResult> Deposit()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            ViewBag.AccountCode = user.AccountCode;
            return View();
        }
    }
}
