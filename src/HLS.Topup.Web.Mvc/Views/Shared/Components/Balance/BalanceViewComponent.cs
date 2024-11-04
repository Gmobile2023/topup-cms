using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using HLS.Topup.Web.TagHelpers;

namespace HLS.Topup.Web.Views.Shared.Components.Balance
{
    public class BalanceViewComponent : TopupViewComponent
    {
        private readonly ITransactionsAppService _transactionsAppService;
        private readonly TopupAppSession _session;

        public BalanceViewComponent(TopupAppSession session, ITransactionsAppService transactionsAppService)
        {
            _session = session;
            _transactionsAppService = transactionsAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string accountCode)
        {
            ViewBag.Balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
            {
                AccountCode = !string.IsNullOrEmpty(accountCode) ? accountCode : _session.AccountCode,
                CurrencyCode = "VND"
            })).ToFormat("đ");
            return View("_ViewBalance");
        }
    }
}