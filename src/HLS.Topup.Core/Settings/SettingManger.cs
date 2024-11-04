using System;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Dtos.Settings;
using HLS.Topup.LimitationManager;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.Settings
{
    public class SettingManger : TopupDomainServiceBase, ISettingManger
    {
        private readonly ICommonManger _commonManger;
        private ILogger<SettingManger> _logger;
        private readonly UserManager _userManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ILimitationManager _limitationManager;

        public SettingManger(ICommonManger commonManger,
            ILogger<SettingManger> logger, UserManager userManager, ITransactionManager transactionManager,
            ILimitationManager limitationManager)
        {
            _commonManger = commonManger;
            _logger = logger;
            _userManager = userManager;
            _transactionManager = transactionManager;
            _limitationManager = limitationManager;
        }

        // fix git, check thêm hạn mức nhân viên
        public async Task<bool> CheckAccountActivities(CheckAccountActivityInput input, long userId, string accountCode,
            UserIdentifier userIdentifier = null)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            //CheckServiceEnable|CheckActiveAccount|CheckVerifyAccount|CheckBalance|CheckTimeStaff|CheckCategory
            if (input.CheckTypes.Contains("CheckPaymentMethod"))
            {
                var payMethod = await GetPaymentVerifyMethod(input.Channel, userIdentifier);
                if (payMethod == 0)
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckPaymentMethod,
                        L("ErrorMessage_113"));
                }
            }

            if (input.CheckTypes.Contains("CheckServiceEnable") && !string.IsNullOrEmpty(input.ServiceCode))
            {
                if (!(input.ServiceCode.ToUpper().Contains("TRANSFER") ||
                      input.ServiceCode.ToUpper().Contains("DEPOSIT")))
                {
                    var checkService = await _commonManger.CheckServiceActive(input.ServiceCode);
                    if (!checkService)
                    {
                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckServiceEnable,
                            L("ErrorMessage_101"));
                    }
                }
                // nếu là nhân viên ko có vào 2 dịch vụ này
                else if ((input.ServiceCode.ToUpper().Contains("TRANSFER") ||
                          input.ServiceCode.ToUpper().Contains("DEPOSIT")) &&
                         (user.AccountType == CommonConst.SystemAccountType.Staff ||
                          user.AccountType == CommonConst.SystemAccountType.StaffApi))
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckUserDeposit,
                        L("ErrorMessage_109"));
                }
            }

            if (input.CheckTypes.Contains("CheckCategory"))
            {
                var checkService = await _commonManger.CheckCategoryActive(input.CategoryCode);
                if (!checkService)
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckCategory,
                        L("ErrorMessage_106"));
                }
            }

            if (input.CheckTypes.Contains("CheckActiveAccount") || input.CheckTypes.Contains("CheckVerifyAccount"))
            {
                var accountRequest = _userManager.GetAccountInfoById(userId);
                if (accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.MasterAgent &&
                    accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.Staff &&
                    accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.Agent)
                {
                    _logger.LogInformation("Tài khoản không hợp lệ");
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.AccountInvalid,
                        "Tài khoản không hợp lệ");
                }

                if (input.CheckTypes.Contains("CheckActiveAccount"))
                {
                    if (!accountRequest.UserInfo.IsActive)
                    {
                        _logger.LogInformation("Tài khoản của bạn đã bị khóa");
                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckActiveAccount,
                            L("ErrorMessage_102"));
                    }

                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                    {
                        if (!accountRequest.NetworkInfo.IsActive)
                        {
                            _logger.LogInformation(
                                "Không thể thực hiện được giao dịch. Tài khoản đại lý đã bị khóa");
                            throw new UserFriendlyException(
                                (int) ErrorConst.ActivityErrorCodes.CheckActiveAccountAgent,
                                L("ErrorMessage_108"));
                        }
                    }
                }

                if (input.CheckTypes.Contains("CheckVerifyAccount"))
                {
                    if (!accountRequest.NetworkInfo.IsVerifyAccount)
                    {
                        _logger.LogInformation("Tài khoản đại lý chưa xác thực");
                        if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                        {
                            throw new UserFriendlyException(
                                (int) ErrorConst.ActivityErrorCodes.CheckVerifyAccount,
                                L("ErrorMessage_107"));
                        }

                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckVerifyAccount,
                            L("ErrorMessage_103"));
                    }
                }
            }

            if (user.AccountType == CommonConst.SystemAccountType.Staff &&
                input.CheckTypes.Contains("CheckTimeStaff"))
            {
                var checkStaff = await _commonManger.CheckStaffTime(userId);
                if (!checkStaff)
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckTimeStaff,
                        L("ErrorMessage_105"));
                }
            }

            if (input.CheckTypes.Contains("CheckBalance"))
            {
                var accountRequest = _userManager.GetAccountInfoById(userId);
                if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                    accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                    {
                        var limitAmount = await _transactionManager.GetLimitAmountBalance(new GetAvailableLimitAccount
                        {
                            AccountCode = accountRequest.UserInfo.AccountCode,
                        });
                        if (!limitAmount.Success || limitAmount.Result < input.PaymentAmount)
                            throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckLimitAmount,
                                L("ErrorMessage_110"));
                    }

                    accountCode = accountRequest.NetworkInfo.AccountCode;
                }

                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                {
                    AccountCode = accountCode,
                    CurrencyCode = "VND",
                });
                if (!balance.Success || balance.Result < input.PaymentAmount)
                {
                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                        accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
                    {
                        throw new UserFriendlyException(
                            (int) ErrorConst.ActivityErrorCodes.CheckBalanceStaff,
                            L("ErrorMessage_111"));
                    }

                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckBalance,
                        L("ErrorMessage_104"));
                }
            }

            // if (input.CheckTypes.Contains("CheckLimitProduct"))
            // {
            //     var accountRequest = _userManager.GetAccountInfoById(userId);
            //     var checkLimit = await _limitationManager.CheckLimitConfigProduct(input.ProductCode,
            //         accountRequest.NetworkInfo.AccountCode, input.Quantity, input.Amount);
            //     if (!checkLimit)
            //     {
            //         throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckLimitProduct,
            //             L("ErrorMessage_112"));
            //     }
            // }

            return true;
        }

        public async Task<bool> CheckAccountActivitiesOld(CheckAccountActivityInput input, long userId,
            string accountCode)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            //CheckServiceEnable|CheckActiveAccount|CheckVerifyAccount|CheckBalance|CheckTimeStaff|CheckCategory
            if (input.CheckTypes.Contains("CheckServiceEnable") && !string.IsNullOrEmpty(input.CheckParam))
            {
                if (!(input.CheckParam.ToUpper().Contains("TRANSFER") ||
                      input.CheckParam.ToUpper().Contains("DEPOSIT")))
                {
                    var checkService = await _commonManger.CheckServiceActive(input.CheckParam);
                    if (!checkService)
                    {
                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckServiceEnable,
                            L("ErrorMessage_101"));
                    }
                }
                // nếu là nhân viên ko có vào 2 dịch vụ này
                else if ((input.CheckParam.ToUpper().Contains("TRANSFER") ||
                          input.CheckParam.ToUpper().Contains("DEPOSIT")) &&
                         (user.AccountType == CommonConst.SystemAccountType.Staff ||
                          user.AccountType == CommonConst.SystemAccountType.StaffApi))
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckUserDeposit,
                        L("ErrorMessage_109"));
                }
            }

            if (input.CheckTypes.Contains("CheckCategory"))
            {
                var checkService = await _commonManger.CheckCategoryActive(input.CheckParam);
                if (!checkService)
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckCategory,
                        L("ErrorMessage_106"));
                }
            }

            if (input.CheckTypes.Contains("CheckActiveAccount") || input.CheckTypes.Contains("CheckVerifyAccount"))
            {
                var accountRequest = _userManager.GetAccountInfoById(userId);
                if (accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.MasterAgent &&
                    accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.Staff)
                {
                    _logger.LogInformation("Tài khoản không hợp lệ");
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.AccountInvalid,
                        "Tài khoản không hợp lệ");
                }

                if (input.CheckTypes.Contains("CheckActiveAccount"))
                {
                    if (!accountRequest.UserInfo.IsActive)
                    {
                        _logger.LogInformation("Tài khoản của bạn đã bị khóa");
                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckActiveAccount,
                            L("ErrorMessage_102"));
                    }

                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                    {
                        if (!accountRequest.NetworkInfo.IsActive)
                        {
                            _logger.LogInformation(
                                "Không thể thực hiện được giao dịch. Tài khoản đại lý đã bị khóa");
                            throw new UserFriendlyException(
                                (int) ErrorConst.ActivityErrorCodes.CheckActiveAccountAgent,
                                L("ErrorMessage_108"));
                        }
                    }
                }

                if (input.CheckTypes.Contains("CheckVerifyAccount"))
                {
                    if (!accountRequest.NetworkInfo.IsVerifyAccount)
                    {
                        _logger.LogInformation("Tài khoản đại lý chưa xác thực");
                        if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                        {
                            throw new UserFriendlyException(
                                (int) ErrorConst.ActivityErrorCodes.CheckVerifyAccount,
                                L("ErrorMessage_107"));
                        }

                        throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckVerifyAccount,
                            L("ErrorMessage_103"));
                    }
                }
            }

            if (user.AccountType == CommonConst.SystemAccountType.Staff &&
                input.CheckTypes.Contains("CheckTimeStaff"))
            {
                var checkStaff = await _commonManger.CheckStaffTime(userId);
                if (!checkStaff)
                {
                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckTimeStaff,
                        L("ErrorMessage_105"));
                }
            }

            if (input.CheckTypes.Contains("CheckBalance"))
            {
                var accountRequest = _userManager.GetAccountInfoById(userId);
                if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                    accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff)
                    {
                        var limitAmount = await _transactionManager.GetLimitAmountBalance(new GetAvailableLimitAccount
                        {
                            AccountCode = accountRequest.UserInfo.AccountCode,
                        });
                        if (!limitAmount.Success || limitAmount.Result < input.Amount)
                            throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckLimitAmount,
                                L("ErrorMessage_110"));
                    }

                    accountCode = accountRequest.NetworkInfo.AccountCode;
                }

                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                {
                    AccountCode = accountCode,
                    CurrencyCode = "VND",
                });
                if (!balance.Success || balance.Result < input.Amount)
                {
                    if (accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.Staff ||
                        accountRequest.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi)
                    {
                        throw new UserFriendlyException(
                            (int) ErrorConst.ActivityErrorCodes.CheckBalanceStaff,
                            L("ErrorMessage_111"));
                    }

                    throw new UserFriendlyException((int) ErrorConst.ActivityErrorCodes.CheckBalance,
                        L("ErrorMessage_104"));
                }
            }

            return true;
        }

        public async Task ChangePaymentVerifyMethod(PaymentMethodDto input, UserIdentifier userIdentifier)
        {
            var name = input.Channel == CommonConst.Channel.WEB
                ? AppSettings.UserManagement.WebPaymentMethod
                : AppSettings.UserManagement.AppPaymentMethod;
            await SettingManager.ChangeSettingForUserAsync(
                userIdentifier,
                name,
                input.Type.ToString("D")
            );
        }

        public async Task<CommonConst.VerifyTransType> GetPaymentVerifyMethod(CommonConst.Channel channel,
            UserIdentifier userIdentifier)
        {
            try
            {
                _logger.LogInformation($"GetPaymentVerifyMethod:{channel:G}-{userIdentifier}");
                var name = channel == CommonConst.Channel.WEB
                    ? AppSettings.UserManagement.WebPaymentMethod
                    : AppSettings.UserManagement.AppPaymentMethod;
                var item = await SettingManager.GetSettingValueForUserAsync<int>(name,
                    userIdentifier);
                _logger.LogInformation($"GetPaymentVerifyMethodReturn:{item}");
                if (item == 0)
                    return 0;
                return (CommonConst.VerifyTransType) item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }
    }
}
