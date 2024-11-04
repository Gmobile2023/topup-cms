using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using HLS.Topup.Web.TagHelpers;

namespace HLS.Topup.Web.Views.Shared.Components.LimitAmount
{
    public class LimitAmountViewComponent : TopupViewComponent
    {
        private readonly ITransactionsAppService _transactionsAppService;
        private readonly TopupAppSession _session;

        public LimitAmountViewComponent(TopupAppSession session, ITransactionsAppService transactionsAppService)
        {
            _session = session;
            _transactionsAppService = transactionsAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Balance = (await _transactionsAppService.GetLimitAmountBalance(new GetAvailableLimitAccount
                {AccountCode = _session.AccountCode})).ToFormat("đ");
            return View("_ViewLimitAmount");
        }
    }
}
