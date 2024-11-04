using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Dtos.Balance;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;

namespace HLS.Topup.BalanceManager
{
    [AbpAuthorize]
    public class BalanceAccountAppService : TopupAppServiceBase, IBalanceAccountAppService
    {
        private readonly TopupAppSession _topupAppSession;
        private readonly ITransactionManager _transactionManager;

        public BalanceAccountAppService(TopupAppSession topupAppSession, ITransactionManager transactionManager)
        {
            _topupAppSession = topupAppSession;
            _transactionManager = transactionManager;
        }

        public async Task<AccountBalanceInfo> GetBalanceAccountInfo(AccountBalanceInfoCheckRequest request)
        {
            if (string.IsNullOrEmpty(request.AccountCode))
                request.AccountCode = _topupAppSession.AccountCode;
            if (string.IsNullOrEmpty(request.CurrencyCode))
                request.CurrencyCode = "VND";
            var rs = await _transactionManager.GetBalanceAccountInfoRequest(request);
            return !rs.Success ? null : rs.Result;
        }

        public async Task<AccountBalanceInfo> GetBalanceAccountInfoById(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            var rs = await _transactionManager.GetBalanceAccountInfoRequest(new AccountBalanceInfoCheckRequest
            {
                AccountCode = user.AccountCode,
                CurrencyCode = "VND"
            });
            return !rs.Success ? null : rs.Result;
        }
    }
}
