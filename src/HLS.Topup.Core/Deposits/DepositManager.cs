using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.Sale;
using HLS.Topup.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Deposits
{
    public class DepositManager : TopupDomainServiceBase, IDepositManager
    {
        private readonly IRepository<Deposit> _depositRepository;
        private readonly IRepository<Bank, int> _lookupBankRepository;
        private readonly IRepository<SaleLimitDebt, int> _saleLimitDebtRepository;
        private readonly IRepository<SaleClearDebtHistory, int> _clearDebtHistoryRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly INotificationSender _appNotifier;
        private readonly UserManager _userManager;
        // private readonly IRepository<UserProfile> _userProfileRepository;
        // private readonly IRepository<User, long> _userRepository;

        //private readonly Logger _logger = LogManager.GetLogger("DepositsAppService");
        private readonly ILogger<DepositManager> _logger;

        public DepositManager(IRepository<Deposit> depositRepository,
            IRepository<Bank, int> lookupBankRepository,
            IRepository<SaleLimitDebt, int> saleLimitDebtRepository,
            IRepository<SaleClearDebtHistory, int> clearDebtHistoryRepository,
            ITransactionManager transactionManager,
            INotificationSender appNotifier,

            UserManager userManager,
            // IRepository<UserProfile> userProfileRepository,
            // IRepository<User, long> userRepository,
            ILogger<DepositManager> logger)
        {
            _depositRepository = depositRepository;
            _lookupBankRepository = lookupBankRepository;
            _saleLimitDebtRepository = saleLimitDebtRepository;
            _clearDebtHistoryRepository = clearDebtHistoryRepository;
            _transactionManager = transactionManager;
            _appNotifier = appNotifier;
            _logger = logger;
            _userManager = userManager;
            // _userRepository = userRepository;
            // _userProfileRepository = userProfileRepository;
        }

        public async Task ApprovalDeposit(string transcode, string transCodeBank, string description, long? approvalId)
        {
            try
            {
                var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.TransCode == transcode);
                if (deposit == null)
                    throw new UserFriendlyException("Giao dịch không tồn tại");
                if (deposit.BankId != null && string.IsNullOrEmpty(transCodeBank))
                    throw new UserFriendlyException("Vui lòng nhập mã giao dịch ngân hàng!");
                if (deposit.Status != CommonConst.DepositStatus.Pending)
                    throw new UserFriendlyException("Trạng thái không hợp lệ");
                var user = await _userManager.GetUserByIdAsync(deposit.UserId);
                //var user = await _userRepository.FirstOrDefaultAsync(deposit.UserId);
                if (user == null)
                    throw new UserFriendlyException("Tài khoản không tồn tại");
                deposit.TransCodeBank = transCodeBank;
                deposit.ApprovedDate = DateTime.Now;
                deposit.ApproverId = approvalId;
                deposit.Status = CommonConst.DepositStatus.Processing;

                await _depositRepository.UpdateAsync(deposit);
                await CurrentUnitOfWork.SaveChangesAsync();

                TransactionResponse requestDeposit;
                string extraInfo = string.Empty;
                if (deposit.BankId > 0)
                {
                    var bank = await _lookupBankRepository.FirstOrDefaultAsync(deposit.BankId ?? 0);
                    if (bank != null)
                    {
                        extraInfo = (new BankResponseDto()
                        {
                            BankName = bank?.BankName,
                            BranchName = bank?.BranchName,
                            BankAccountName = bank?.BankAccountName,
                            BankAccountCode = bank?.BankAccountCode,
                        }).ToJson();
                    }
                }


                if (deposit.Type == CommonConst.DepositType.Deposit || deposit.Type == CommonConst.DepositType.Cash)
                {
                    requestDeposit = await _transactionManager.DepositRequest(new DepositRequest
                    {
                        AccountCode = user.AccountCode,
                        Amount = deposit.Amount,
                        CurrencyCode = "VND",
                        TransRef = deposit.TransCode,
                        Description = deposit.Description,
                        TransNote = deposit.Description,
                        ExtraInfo = extraInfo,
                    });
                    //deposit.ExtraInfo = requestDeposit.ToJson();
                    deposit.ExtraInfo =$"{requestDeposit.ResponseCode}|{requestDeposit.ResponseMessage}";
                    if (requestDeposit.ResponseCode == ResponseCodeConst.ResponseCode_Success)
                    {
                        deposit.Status = CommonConst.DepositStatus.Approved;
                        await Task.Run(async () =>
                        {
                            try
                            {
                                _logger.LogInformation($"Begin send notifi to: {user.AccountCode}");
                                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                                {
                                    AccountCode = user.AccountCode,
                                    CurrencyCode = "VND"
                                });
                                var message = L("Notifi_Recive_Deposit", deposit.Amount.ToFormat("đ"),
                                    balance.Result.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                await _appNotifier.PublishNotification(
                                    user.AccountCode,
                                    AppNotificationNames.Deposit,
                                    new SendNotificationData
                                    {
                                        TransCode = deposit.TransCode,
                                        Amount = deposit.Amount,
                                        PartnerCode = user.AccountCode,
                                        ServiceCode = CommonConst.ServiceCodes.DEPOSIT,
                                        TransType = CommonConst.TransactionType.Deposit.ToString("G")
                                    },
                                    message,
                                    L("Notifi_Recive_Deposit_Title")
                                );
                                _logger.LogInformation($"Done notifi to: {user.AccountCode}-Message:{message}");
                                if (user.AgentType == CommonConst.AgentType.AgentApi)
                                {
                                    var botMessage = L("Bot_Send_DepositToAgentApi",
                                        user.AccountCode + "-" + user.FullName,
                                        deposit.Amount.ToFormat("đ"), balance.Result.ToFormat("đ"),
                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), deposit.Description,
                                        deposit.TransCode);
                                    var userProfile = await _userManager.GetUserProfile(user.Id);
                                    //var userProfile = await _userProfileRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
                                    if (userProfile != null && !string.IsNullOrEmpty(userProfile.ChatId))
                                    {
                                        await _appNotifier.PublishTeleToGroupMessage(new SendTeleMessageRequest
                                        {
                                            Message = botMessage,
                                            Module = "Balance",
                                            Title = "Thông báo Nạp tiền TK",
                                            BotType = (byte) BotType.Deposit,
                                            MessageType = (byte) BotMessageType.Message,
                                            ChatId = userProfile.ChatId
                                        });
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.LogError($"SendNotifi deposit approval eror:{e}");
                            }
                        }).ConfigureAwait(false);
                    }
                }
                else if (deposit.Type == CommonConst.DepositType.SaleDeposit)
                {
                    var userSale = await _userManager.GetUserByIdAsync(deposit.UserSaleId ?? 0);
                    //var userSale = await _userRepository.FirstOrDefaultAsync(deposit.UserSaleId ?? 0);
                    if (userSale == null)
                        throw new UserFriendlyException("Tài khoản không tồn tại");

                    DateTime date = DateTime.Now;
                    var saleLimit = _saleLimitDebtRepository.GetAll()
                        .Where(c => c.UserId == (deposit.UserSaleId ?? 0)
                                    && c.Status == CommonConst.DebtLimitAmountStatus.Active
                                    && c.CreationTime <= date)
                        .OrderByDescending(c => c.CreationTime)
                        .FirstOrDefault();

                    if (saleLimit == null)
                        throw new UserFriendlyException("Tài khoản chưa có hạn mức và tuổi nợ");

                    var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                    {
                        AccountCode = userSale.AccountCode,
                        CurrencyCode = "DEBT"
                    });
                    var balanceView = balance.Result;

                    if (saleLimit.LimitAmount - balanceView < deposit.Amount)
                    {
                        throw new UserFriendlyException("Tài khoản không đủ hạn mức để nạp tiền cho đại lý.");
                    }

                    var debtHistory = await _clearDebtHistoryRepository.GetAll().Where(c => c.UserId == userSale.Id)
                        .FirstOrDefaultAsync();
                    if (debtHistory != null)
                    {
                        var checkDate = debtHistory.StartDate.AddDays(saleLimit.DebtAge).AddDays(-1);
                        var dateNow = DateTime.Now.Date;
                        if (balanceView > 0 && checkDate < dateNow)
                        {
                            throw new UserFriendlyException(
                                $"Tài khoản nhân viên còn nợ {balanceView.ToString("N0")}đ chưa được xóa hạn mức.");
                        }
                        else if (balanceView == 0) debtHistory.StartDate = dateNow;
                    }

                    requestDeposit = await _transactionManager.SaleDepositRequest(new SaleDepositRequest()
                    {
                        AccountCode = user.AccountCode,
                        SaleCode = userSale.AccountCode,
                        TransNote = deposit.Description,
                        Amount = deposit.Amount,
                        TransRef = transcode
                    });

                    if (requestDeposit.ResponseCode == ResponseCodeConst.ResponseCode_Success)
                    {
                        deposit.Status = CommonConst.DepositStatus.Approved;
                        if (debtHistory != null)
                        {
                            debtHistory.Amount += deposit.Amount;
                            await _clearDebtHistoryRepository.UpdateAsync(debtHistory);
                        }
                        else
                        {
                            await _clearDebtHistoryRepository.InsertAsync(new SaleClearDebtHistory()
                            {
                                CreationTime = DateTime.Now,
                                StartDate = DateTime.Now.Date,
                                Amount = deposit.Amount,
                                UserId = userSale.Id,
                            });
                        }
                    }
                }
                else
                {
                    //Call giao dịch điều chỉnh
                    requestDeposit = await _transactionManager.AdjustmentRequest(new AdjustmentRequest
                    {
                        AccountCode = user.AccountCode,
                        Amount = deposit.Amount,
                        CurrencyCode = "VND",
                        TransRef = deposit.TransCode,
                        AdjustmentType = deposit.Type == CommonConst.DepositType.Decrease
                            ? CommonConst.AdjustmentType.Decrease
                            : CommonConst.AdjustmentType.Increase,
                        TransNote = deposit.Description
                    });
                    deposit.ExtraInfo = requestDeposit.ToJson();
                    if (requestDeposit.ResponseCode == ResponseCodeConst.ResponseCode_Success)
                    {
                        deposit.Status = CommonConst.DepositStatus.Approved;
                        await Task.Run(async () =>
                        {
                            try
                            {
                                _logger.LogInformation($"Begin send notifi to: {user.AccountCode}");
                                var balance = await _transactionManager.GetBalanceRequest(new GetBalanceRequest
                                {
                                    AccountCode = user.AccountCode,
                                    CurrencyCode = "VND"
                                });
                                var key = deposit.Type == CommonConst.DepositType.Increase
                                    ? "Notifi_Adjustment_Increases_Body"
                                    : "Notifi_Adjustment_Decreases_Body";
                                var message = L($"{key}", deposit.Amount.ToFormat("đ"),
                                    balance.Result.ToFormat("đ"), deposit.Description,
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                await _appNotifier.PublishNotification(
                                    user.AccountCode,
                                    AppNotificationNames.System,
                                    new SendNotificationData
                                    {
                                        TransCode = deposit.TransCode,
                                        Amount = deposit.Amount,
                                        PartnerCode = user.AccountCode,
                                        TransType = deposit.Type == CommonConst.DepositType.Increase
                                            ? CommonConst.TransactionType.AdjustmentIncrease.ToString("G")
                                            : CommonConst.TransactionType.AdjustmentDecrease.ToString("G")
                                    },
                                    message,
                                    deposit.Type == CommonConst.DepositType.Increase
                                        ? L("Notifi_Adjustment_Increases_Title")
                                        : L("Notifi_Adjustment_Decreases_Title")
                                );
                                _logger.LogInformation($"Done notifi to: {user.AccountCode}-Message:{message}");
                            }
                            catch (Exception e)
                            {
                                _logger.LogError($"SendNotifi deposit approval eror:{e}");
                            }
                        }).ConfigureAwait(false);
                    }
                }

                await _depositRepository.UpdateAsync(deposit);
                await CurrentUnitOfWork.SaveChangesAsync();
                if (requestDeposit.ResponseCode != ResponseCodeConst.ResponseCode_Success)
                    throw new UserFriendlyException(requestDeposit.ResponseMessage);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Không thành công");
            }
        }
    }
}
