using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Common;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;

namespace HLS.Topup.Web.Views.Shared.Components.AccountInfo
{
    public class AccountInfoViewComponent : TopupViewComponent
    {
        private readonly ITransactionsAppService _transactionsAppService;
        private readonly TopupAppSession _session;

        public AccountInfoViewComponent(TopupAppSession session, ITransactionsAppService transactionsAppService)
        {
            _session = session;
            _transactionsAppService = transactionsAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var accountCode = _session.AccountCode;
            if (_session.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                accountCode = _session.ParentCode;
            }

            var model = new AccountInfoHeaderModel
            {
                Balance = (await _transactionsAppService.GetBalance(new GetBalanceRequest
                    {AccountCode = accountCode, CurrencyCode = "VND"})).ToFormat("đ"),
                AccountCode = accountCode,
                AccountType = _session.AccountType,
                StaffAccount = _session.AccountCode
            };
            return View("_AccountInfo", model);
        }
    }
}
