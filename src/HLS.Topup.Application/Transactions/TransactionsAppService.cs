using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using HLS.Topup.AccountManager;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Banks;
using HLS.Topup.Categories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Deposits;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Dtos.Reports;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Files;
using HLS.Topup.Net.Sms;
using HLS.Topup.Notifications;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Providers;
using HLS.Topup.Report;
using HLS.Topup.Reports;
using HLS.Topup.RequestDtos;
using HLS.Topup.Services;
using HLS.Topup.Storage;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Transactions.Dtos;
using HLS.Topup.Transactions.Exporting;
using HLS.Topup.Vendors;
using HLS.Topup.Vendors.Dtos;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Transactions
{
    [AbpAuthorize]
    public partial class TransactionsAppService : TopupAppServiceBase, ITransactionsAppService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IAccountManager _accountManager;
        private readonly ICommonManger _commonManger;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppNotifier _appNotifierNow;
        private readonly IUserEmailer _userEmailer;
        private readonly ISmsSender _smsSender;

        private readonly TopupAppSession _session;
        private readonly ILogger<TransactionsAppService> _logger;
        private readonly ITransactionsExcelExporter _transactionsExcelExporter;
        private readonly IRepository<Provider, int> _lookupProviderRepository;
        private readonly IRepository<Product, int> _lookupProductRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Service, int> _lookupServiceRepository;
        private readonly INotificationSender _appNotifier;
        private readonly IRepository<Deposit> _depositRepository;
        private readonly IRepository<Bank, int> _lookupBankRepository;
        private readonly IRepository<User, long> _lookupUserRepository;
        private readonly IReportsManager _reportsManager;
        private readonly ICategoryManager _categoryManager;
        private readonly TelcoHepper _telcoHepper;
        private readonly IConfigurationRoot _appConfiguration;

        public TransactionsAppService(ICommonManger commonManger, ITransactionManager transactionManager,
            IAccountManager accountManager,
            ITempFileCacheManager tempFileCacheManager,
            IUserEmailer userEmailer,
            ISmsSender smsSender,
            IRepository<Provider, int> lookupProviderRepository,
            IRepository<Product, int> lookupProductRepository,
            IRepository<Category> categoryrepository,
            IRepository<Vendor> vendorRepository,
            IRepository<Deposit> depositRepository,
            IRepository<Bank, int> lookupBankRepository,
            IRepository<User, long> lookupUserRepository,
            IReportsManager reportsManager,
            IBinaryObjectManager binaryObjectManager,
            IAppNotifier appNotifierNow,
            TopupAppSession session, ITransactionsExcelExporter transactionsExcelExporter,
            INotificationSender appNotifier, ILogger<TransactionsAppService> logger,
            IRepository<Service, int> lookupServiceRepository,
            ICategoryManager categoryManager,
            IServiceConfigurationsAppService serviceConfig, TelcoHepper telcoHepper,
            IWebHostEnvironment hostingEnvironment)
        {
            _commonManger = commonManger;
            _transactionManager = transactionManager;
            _session = session;
            _transactionsExcelExporter = transactionsExcelExporter;
            _appNotifier = appNotifier;
            _appNotifierNow = appNotifierNow;
            _logger = logger;
            _lookupProviderRepository = lookupProviderRepository;
            _lookupProductRepository = lookupProductRepository;
            _categoryRepository = categoryrepository;
            _vendorRepository = vendorRepository;
            _depositRepository = depositRepository;
            _lookupBankRepository = lookupBankRepository;
            _lookupUserRepository = lookupUserRepository;
            _reportsManager = reportsManager;
            _lookupServiceRepository = lookupServiceRepository;
            _categoryManager = categoryManager;
            _telcoHepper = telcoHepper;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
            _binaryObjectManager = binaryObjectManager;
            _accountManager = accountManager;
            _tempFileCacheManager = tempFileCacheManager;
            _userEmailer = userEmailer;
            _smsSender = smsSender;
        }

        [AbpAuthorize(AppPermissions.Pages_TransferMoney)]
        public async Task<TransactionResponse> TransferMoney(TransferRequest input)
        {
            try
            {
                var transcode = await _commonManger.GetIncrementCodeAsync("T");
                var desAccount = await UserManager.GetUserByAccountCodeAsync(input.DesAccount);
                if (desAccount == null || !desAccount.IsActive || desAccount.IsAccountSystem())
                    throw new UserFriendlyException("Tài khoản nhận không tồn tại hoặc không hợp lệ");
                var checkSrc = await UserManager.GetUserByIdAsync(_session.UserId ?? 0);
                if (checkSrc.IsAccountSystem())
                    throw new UserFriendlyException("Tài khoản không thể thực hiện giao dịch");
                if (desAccount == checkSrc)
                    throw new UserFriendlyException("Tài khoản nhận không hợp lệ");
                input.SrcAccount = _session.AccountCode;
                input.TransRef = transcode;
                input.CurrencyCode = "VND";
                input.TransNote = _session.UserName + "|" + desAccount.UserName;
                var response = await _transactionManager.TransferRequest(input);
                response.ExtraInfo =
                    $"/Transactions/TransactionInfo?code={response.ResponseCode}&account={desAccount.AccountCode + " - " + desAccount.FullName + " - " + desAccount.PhoneNumber}&amount={input.Amount}&message={response.ResponseMessage}&description={input.Description}&transtype={CommonConst.TransactionType.Transfer}";
                response.ExtraInfo = _commonManger.EncryptQueryParameters(response.ExtraInfo);
                if (response.ResponseCode == "1")
                {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            _logger.LogInformation(
                                $"Begin send notifi to {desAccount.AccountCode}-{_session.AccountCode}");
                            var balance = await GetBalance(new GetBalanceRequest
                            {
                                AccountCode = desAccount.AccountCode
                            });
                            var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            var message = L("Notifi_Recive_TransferMoney", input.Amount.ToFormat("đ"),
                                _session.AccountCode + "-" + _session.FullName,
                                balance.ToFormat("đ"),
                                input.Description, date);

                            var balanceTrans = response.Payload.ConvertTo<BalanceResponse>();
                            var src = response.Payload.ToString().Split("|")[0];
                            var srrBalance = decimal.Parse(src.Replace(".", ","));
                            var messageTrans = L("Notifi_TransferMoney", input.Amount.ToFormat("đ"),
                                desAccount.AccountCode + "-" + desAccount.FullName, srrBalance.ToFormat("đ"),
                                input.Description,
                                date);
                            await _appNotifier.PublishNotification(desAccount.AccountCode,
                                AppNotificationNames.Transfer,
                                new SendNotificationData
                                {
                                    PartnerCode = desAccount.AccountCode,
                                    Amount = input.Amount,
                                    TransCode = transcode,
                                    ServiceCode = CommonConst.ServiceCodes.TRANSFER_MONEY,
                                    TransType = CommonConst.TransactionType.Transfer.ToString("G")
                                }, message, L("Notifi_Recive_TransferMoney_Title"));
                            _logger.LogInformation($"Send notifi to {desAccount.AccountCode} done. Message:{message}");

                            await _appNotifier.PublishNotification(_session.AccountCode,
                                AppNotificationNames.Transfer,
                                new SendNotificationData
                                {
                                    PartnerCode = _session.AccountCode,
                                    Amount = input.Amount,
                                    TransCode = transcode,
                                    ServiceCode = CommonConst.ServiceCodes.TRANSFER_MONEY,
                                    TransType = CommonConst.TransactionType.Transfer.ToString("G")
                                },
                                messageTrans, L("Notifi_TransferMoney_Title"));
                            _logger.LogInformation(
                                $"Send notifi to {_session.AccountCode} done. Message:{messageTrans}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"SendNotifi Transfer error:{e}");
                        }
                    }).ConfigureAwait(false);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"TransferMoney error: {ex}");
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<decimal> GetBalance(GetBalanceRequest input)
        {
            input.CurrencyCode = "VND";
            if (string.IsNullOrEmpty(input.AccountCode))
                input.AccountCode = _session.AccountCode;
            var rs = await _transactionManager.GetBalanceRequest(input);
            return rs.Result;
        }

        public async Task<decimal> GetLimitAmountBalance(GetAvailableLimitAccount input)
        {
            if (string.IsNullOrEmpty(input.AccountCode))
                input.AccountCode = _session.AccountCode;
            var rs = await _transactionManager.GetLimitAmountBalance(input);
            if (!rs.Success)
                return 0;
            return decimal.Parse(rs.Result.ToString(CultureInfo.InvariantCulture)?.Replace(".", ","));
        }

        [AbpAuthorize(AppPermissions.Pages_BalanceHistory)]
        public async Task<PagedResultDto<BalanceHistoryDto>> GetBalanceHistories(GetBalanceHistoryInput input)
        {
            try
            {
                var request = new BalanceHistoriesRequest
                {
                    TransCode = input.Filter,
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter?.AddHours(23).AddMinutes(59).AddSeconds(59),
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    AccountTrans = _session.AccountCode,
                    TransType = (CommonConst.TransactionType)input.TransType
                };
                var rs = await _transactionManager.BalanceHistoriesGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<BalanceHistoryDto>(
                        0,
                        new List<BalanceHistoryDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<BalanceHistoryDto>>();
                foreach (var item in lst)
                {
                    if (_session.AccountCode == item.SrcAccountCode)
                    {
                        item.Decrement = item.Amount;
                        item.BalanceAfterTrans = item.SrcAccountBalance;
                    }

                    if (_session.AccountCode == item.DesAccountCode)
                    {
                        item.Increment = item.Amount;
                        item.BalanceAfterTrans = item.DesAccountBalance;
                    }

                    item.TransactionType = L($"Enum_TransType_{item.TransType}");

                    if (item.TransType == CommonConst.TransactionType.Transfer)
                    {
                        if (_session.AccountCode == item.SrcAccountCode)
                            item.TransNote =
                                $"Chuyển tiền tới tài khoản {item.DesAccountCode}. Nội dung: {item.Description}";
                        if (_session.AccountCode == item.DesAccountCode)
                            item.TransNote =
                                $"Nhận tiền từ tài khoản {item.SrcAccountCode}. Nội dung: {item.Description}";
                    }
                }

                return new PagedResultDto<BalanceHistoryDto>(
                    totalCount, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetBalanceHistories error: {e}");
                return new PagedResultDto<BalanceHistoryDto>(
                    0,
                    new List<BalanceHistoryDto>());
            }
        }

        public async Task<PagedResultDto<TopupRequestResponseDto>> GetTransactionHistories(
            GetAllTopupRequestsInput input)
        {
            try
            {
                var accountInfo = GetAccountInfo();
                if (!input.IsAdmin)
                {
                    input.PartnerCodeFilter = accountInfo.NetworkInfo.AccountCode;
                    input.StaffAccount = accountInfo.UserInfo.AccountCode;
                    input.StaffUser = accountInfo.UserInfo.UserName;
                }

                if (input.FromDate == null)
                {
                    input.FromDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd 00:00:00"));
                }

                if (input.ToDate == null)
                {
                    input.ToDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd 23:59:59"));
                }

                var request = new TopupsListRequest
                {                    
                    PartnerCode = input.PartnerCodeFilter,
                    StaffAccount = input.StaffAccount,
                    StaffUser = input.StaffUser,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    MobileNumber = input.MobileNumberFilter,
                    Status = input.StatusFilter,
                    TransCode = input.TransCodeFilter,
                    TransRef = input.TransRefFilter,
                    ProviderTransCode = input.ProviderTransCode,
                    FromDate = input.FromDate,
                    ServiceCode = input.ServiceCode,
                    Filter = input.Filter,
                    ToDate = input.ToDate,
                    CategoryCode = input.CategoryCode,
                    ProductCode = input.ProductCode,
                    ProviderCode = input.ProviderCode,
                    AgentType = input.AgentTypeFilter,
                    SaleType = input.SaleTypeFilter,
                    ReceiverType = input.ReceiverType,
                    ProviderResponseCode = input.ProviderResponseCode,
                    ReceiverTypeResponse = input.ReceiverTypeResponse,
                    ParentProvider = input.ParentProvider,
                };

                if (input.ServiceCodes != null)
                    request.ServiceCodes =
                        input.ServiceCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
                else request.ServiceCodes = new List<string>();

                if (input.CategoryCodes != null)
                    request.CategoryCodes =
                        input.CategoryCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
                else request.CategoryCodes = new List<string>();

                if (input.ProductCodes != null)
                    request.ProductCodes =
                        input.ProductCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
                else request.ProductCodes = new List<string>();

                if (input.ProviderCode != null)
                    request.ProviderCode =
                        input.ProviderCode.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
                else request.ProviderCode = new List<string>();

                var rs = await _transactionManager.TopupListRequestAsync(request);
                //var agentList = _lookupUserRepository.GetAll();

                var totalCount = rs.Total;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<TopupRequestResponseDto>(
                        0,
                        new List<TopupRequestResponseDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<TopupRequestResponseDto>>();


                foreach (var item in lst)
                {
                    item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                        : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";

                    item.ReceiverType = item.ReceiverType == "PREPAID" ? "Trả trước" :
                        item.ReceiverType == "POSTPAID" ? "Trả sau" : "";

                    item.ReceiverTypeResponse = item.ReceiverTypeResponse == "TT" ? "Trả trước" :
                      item.ReceiverTypeResponse == "TS" ? "Trả sau" : "";

                    //var agentInfo = agentList.SingleOrDefault(x => x.AccountCode == item.PartnerCode);
                    //item.AgentType = L("Enum_AgentType_" + (int)agentInfo.AgentType);
                    var startTime = item.RequestDate ?? item.CreatedTime;
                    if (item.ResponseDate != null)
                        item.TotalTime = (int)(item.ResponseDate.Value - startTime).TotalSeconds;
                }

                return new PagedResultDto<TopupRequestResponseDto>(
                    totalCount, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetTransactionHistories error: {e}");
                return new PagedResultDto<TopupRequestResponseDto>(
                    0,
                    new List<TopupRequestResponseDto>());
            }
        }

        public async Task<TopupRequestResponseDto> GetTransactionByCode(string transactionCode)
        {
            try
            {
                var item = await _transactionManager.GetDetailsRequestAsync(new GetSaleRequest
                { Filter = transactionCode });
                if (item == null)
                    return null;

                item.Invoice = !string.IsNullOrEmpty(item.ExtraInfo)
                    ? item.ExtraInfo.FromJson<InvoicePayBillDto>()
                    : null;

                item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                    : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";

                if (!string.IsNullOrEmpty(item.Provider))
                {
                    var provider = await _lookupProviderRepository.FirstOrDefaultAsync(x => x.Code == item.Provider);
                    if (provider != null)
                    {
                        item.ProviderCode = provider.Code;
                        item.ProviderName = provider.Name;
                    }
                }

                if (!string.IsNullOrEmpty(item.ProductCode))
                {
                    var product =
                        await _lookupProductRepository.FirstOrDefaultAsync(x => x.ProductCode == item.ProductCode);
                    if (product != null)
                    {
                        item.ProductCode = product.ProductCode;
                        item.ProductName = product.ProductName;
                        item.CustomerSupportNote = product.CustomerSupportNote;
                        item.UserManualNote = product.UserManualNote;
                        item.Description = product.Description;
                    }
                }

                if (!string.IsNullOrEmpty(item.CategoryCode))
                {
                    var p =
                        await _categoryRepository.FirstOrDefaultAsync(x => x.CategoryCode == item.CategoryCode);
                    if (p != null)
                    {
                        item.CategoryCode = p.CategoryCode;
                        item.CategoryName = p.CategoryName;
                    }
                }

                return item;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetTransactionHistories error: {e}");
                return null;
            }
        }

        /// <summary>
        ///  chi tiet 1 GD cho app
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<TransactionResponseDto> GetTransaction(string type, string code)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(code))
                return null;

            TransactionResponseDto data = new TransactionResponseDto();
            //TransactionResponseDto data = await _reportService.GetReportTransInfoRequest(new Reports.Dtos.TransRequestByTransCodeInput()
            //{
            //    TransCode = code,
            //    TransType = type,
            //});

            //return data;
            switch (type)
            {
                case "DEPOSIT":
                    data = await GetDepositTransaction(code);
                    break;
                case "TRANSFER":
                    data = await GetTransferTransaction(code);
                    break;
                case "CORRECT_DOWN":
                case "CORRECT_UP":
                    data = await GetCorrectTransaction(type, code);
                    break;
                case "REFUND":
                    data = await GetRefundTransaction(type, code);
                    break;
                default:
                    var trans = await GetTransactionByCode(code);
                    data = trans.ConvertTo<TransactionResponseDto>();
                    if (!string.IsNullOrEmpty(data.ExtraInfo))
                    {
                        var extra = data.ExtraInfo.FromJson<InvoiceBillDto>();
                        if (extra != null)
                        {
                            data.Invoice.FullName = extra.FullName;
                            data.Invoice.CustomerReference = extra.CustomerReference;
                            data.Invoice.Email = extra.Email;
                            data.Invoice.Address = extra.Address;
                            data.Invoice.Period = extra.Period;
                        }
                    }

                    data.TransType = type;
                    var user = await _lookupUserRepository.FirstOrDefaultAsync(x =>
                        x.AccountCode == trans.StaffAccount);
                    data.StaffAccount = user.AccountCode;
                    data.StaffPhoneNumber = user.PhoneNumber;
                    data.StaffFullName = user.FullName;
                    break;
            }


            return data;
        }

        private async Task<TransactionResponseDto> GetCorrectTransaction(string type, string code)
        {
            var accountInfo = GetAccountInfo();
            var rs = await _reportsManager.ReportTransDetailReport(new ReporttransDetailRequest()
            {
                RequestTransCode = code,
                AccountCode = accountInfo.UserInfo.AccountCode,
                Limit = 2,
                Offset = 0
            });
            if (rs.ResponseCode != "1") return null;
            var lst = rs.Payload.ConvertTo<List<HLS.Topup.Report.ReportItemDetailDto>>();
            if (!lst.Any()) return null;
            var trans = lst.FirstOrDefault();
            if (trans == null) return null;
            TransactionResponseDto data = new TransactionResponseDto();
            data.ReceiverInfo = trans.ReceivedAccount;
            data.Amount = Convert.ToDecimal(trans.TotalPrice);
            data.StatusName = "Thành công";
            data.Status = 1;
            data.CreatedTime = trans.CreatedTime;
            data.TransCode = trans.TransCode;
            data.TransRef = trans.TransCode;
            data.ServiceCode = type;
            data.ProductCode = data.ServiceCode;
            data.ProductName = data.ServiceName;
            data.CategoryCode = data.ServiceCode;
            data.CategoryName = data.ServiceName;
            data.PaymentAmount = Convert.ToDecimal(trans.TotalPrice);
            data.Description = data.ServiceName;
            data.Quantity = 1;
            data.DiscountAmount = 0;
            var s = (trans.PerformInfo ?? string.Empty).Split('-');
            data.StaffAccount = trans.PerformAccount;
            data.StaffFullName = s.Length >= 2 ? s[1] : "";
            data.StaffPhoneNumber = s[0];
            return data;
        }

        private async Task<TransactionResponseDto> GetRefundTransaction(string type, string code)
        {
            var accountInfo = GetAccountInfo();
            var rs = await _reportsManager.ReportTransDetailReport(new ReporttransDetailRequest()
            {
                AccountCode = accountInfo.UserInfo.AccountCode,
                RequestTransCode = code,
                Limit = 2,
                Offset = 0
            });
            if (rs.ResponseCode != "1") return null;
            var lst = rs.Payload.ConvertTo<List<ReportItemDetailDto>>();
            if (!lst.Any()) return null;
            var first = lst.FirstOrDefault();
            var trans = await GetTransactionByCode(first?.RequestTransSouce);
            var data = trans.ConvertTo<TransactionResponseDto>();
            if (!string.IsNullOrEmpty(data.ExtraInfo))
            {
                var extra = data.ExtraInfo.FromJson<InvoiceBillDto>();
                if (extra != null)
                {
                    data.Invoice.FullName = extra.FullName;
                    data.Invoice.CustomerReference = extra.CustomerReference;
                    data.Invoice.Email = extra.Email;
                    data.Invoice.Address = extra.Address;
                    data.Invoice.Period = extra.Period;
                }
            }

            data.TransType = type;
            data.Status = 1;
            var user = await _lookupUserRepository.FirstOrDefaultAsync(x => x.AccountCode == trans.StaffAccount);
            data.StaffAccount = user.AccountCode;
            data.StaffPhoneNumber = user.PhoneNumber;
            data.StaffFullName = user.FullName;
            return data;
        }

        private async Task<TransactionResponseDto> GetDepositTransaction(string code)
        {
            var deposit = await _depositRepository.FirstOrDefaultAsync(x => x.TransCode == code);
            if (deposit == null) return null;
            var bank = await _lookupBankRepository.FirstOrDefaultAsync(deposit.BankId ?? 0);
            var user = await _lookupUserRepository.FirstOrDefaultAsync((long)deposit.UserId);
            TransactionResponseDto data = new TransactionResponseDto();

            data.ReceiverInfo = user.AccountCode + " - " + user.FullName;
            data.Amount = deposit.Amount;
            data.Status = (byte)deposit.Status;
            data.StatusName = L("Enum_DepositStatus_" + (byte)deposit.Status);
            data.CreatedTime = deposit.CreationTime;
            data.TransCode = deposit.TransCode;
            data.TransRef = deposit.TransCode;
            data.ServiceCode = "DEPOSIT";
            data.ServiceName = "Nạp tiền tài khoản";
            data.ProductCode = data.ServiceCode;
            data.ProductName = data.ServiceName;
            data.CategoryCode = data.ServiceCode;
            data.CategoryName = data.ServiceName;
            data.PaymentAmount = deposit.Amount;
            data.Quantity = 1;
            data.DiscountAmount = 0;

            data.StaffAccount = user.AccountCode;
            data.StaffFullName = user.FullName;
            data.StaffPhoneNumber = user.PhoneNumber;

            data.Description = deposit.Description;
            data.BankDto = new BankResponseDto()
            {
                BankName = bank?.BankName,
                BranchName = bank?.BranchName,
                BankAccountName = bank?.BankAccountName,
                BankAccountCode = bank?.BankAccountCode,
            };
            return data;
        }

        private async Task<TransactionResponseDto> GetTransferTransaction(string code)
        {
            var accountInfo = GetAccountInfo();
            var rs = await _reportsManager.ReportTransDetailReport(new ReporttransDetailRequest()
            {
                RequestTransCode = code,
                AccountCode = accountInfo.UserInfo.AccountCode,
                Limit = 2,
                Offset = 0
            });

            if (rs.ResponseCode != "1") return null;
            var lst = rs.Payload.ConvertTo<List<ReportTransferDetailDto>>();
            if (!lst.Any()) return null;
            var trans = lst.FirstOrDefault();
            if (trans == null) return null;
            TransactionResponseDto data = new TransactionResponseDto();
            data.ReceiverInfo = trans.AgentReceiveCode;
            data.Amount = trans.Price;
            data.StatusName = "Thành công";
            data.Status = 1;
            data.CreatedTime = trans.CreatedTime;
            data.TransCode = trans.TransCode;
            data.TransRef = trans.TransCode;
            data.ServiceCode = "TRANSFER";
            data.ServiceName = accountInfo.UserInfo.AccountCode == trans.AgentReceiveCode
                ? "Nhận tiền đại lý"
                : "Chuyển tiền đại lý";
            data.ProductCode = data.ServiceCode;
            data.ProductName = data.ServiceName;
            data.CategoryCode = data.ServiceCode;
            data.CategoryName = data.ServiceName;
            data.PaymentAmount = trans.Price;
            data.Description = data.ServiceName;
            data.Quantity = 1;
            data.DiscountAmount = 0;
            var s = (trans.AgentTransfer ?? string.Empty).Split('-');
            var a = (trans.AgentReceiveInfo ?? string.Empty).Split('-');
            data.TransferInfo = new HLS.Topup.Dtos.Transactions.TransferInfo()
            {
                SrcAccountCode = trans.AgentTransfer,
                SrcFullName = s.Length >= 2 ? s[1] : "",
                SrcPhoneNumber = s[0],
                DesAccountCode = trans.AgentReceiveCode,
                DesFullName = a.Length >= 2 ? a[1] : "",
                DesPhoneNumber = a[0],
            };
            data.StaffAccount = trans.AgentTransfer;
            data.StaffFullName = s.Length >= 2 ? s[1] : "";
            data.StaffPhoneNumber = s[0];
            return data;
        }


        [AbpAuthorize(AppPermissions.Pages_TransactionHistory)]
        public async Task<PagedResultDto<TopupDetailResponseDTO>> GetTopupDetailRequest(
            GetAllTopupDetailRequestsInput input)
        {
            try
            {
                var request = new GetTopupDetailRequest
                {
                    TransCode = input.TransCode,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                };
                var rs = await _transactionManager.GetTopupDetailsRequest(request);
                var totalCount = rs != null ? rs.Total : 0;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<TopupDetailResponseDTO>(
                        0,
                        new List<TopupDetailResponseDTO>()
                    );
                var lst = rs.Payload.ConvertTo<List<TopupDetailResponseDTO>>();
                if (lst.Any())
                {
                    var vendors = await GetVendors();
                    foreach (var item in lst)
                    {
                        var tel = item.ProductCode.Split("_");
                        item.Telco = vendors.FirstOrDefault(x => x.Code == (tel[0]).ToString())?.Name;
                    }
                }

                return new PagedResultDto<TopupDetailResponseDTO>(totalCount, lst);
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetTransactionDetail error: {e}");
                return new PagedResultDto<TopupDetailResponseDTO>(
                    0,
                    new List<TopupDetailResponseDTO>());
            }
        }

        public async Task<decimal> GetDiscountAvailable(GetDiscountAvailableRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.AccountCode))
                    request.AccountCode = GetAccountInfo().NetworkInfo.AccountCode;
                if (request.ToDate != null)
                    request.ToDate = request.ToDate?.AddHours(23).AddMinutes(59).AddSeconds(59);
                var rs = await _transactionManager.GetDiscountAvailableRequest(request);
                if (rs.ResponseCode == "1")
                    return decimal.Parse(rs.Payload.ToString().Replace(".", ","));
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDiscountAvailable error: {e}");
                return 0;
            }
        }

        public async Task<FileDto> GetTransactionHistoryUserToExcel(GetAllTopupRequestsForExcelInput input)
        {
            var accountInfo = GetAccountInfo();
            var request = new TopupsListRequest
            {
                PartnerCode = input.PartnerCodeFilter,
                StaffAccount = input.StaffAccount,
                StaffUser = input.StaffUser,
                Limit = int.MinValue,
                Offset = 0,
                SearchType = SearchType.Search,
                MobileNumber = input.MobileNumberFilter,
                Status = input.StatusFilter,
                TransCode = input.TransCodeFilter,
                TransRef = input.TransRefFilter,
                ProviderTransCode = input.ProviderTransCode,
                FromDate = input.FromDate,
                ServiceCode = input.ServiceCode,
                Filter = input.Filter,
                ToDate = input.ToDate,
                CategoryCode = input.CategoryCode,
                ProductCode = input.ProductCode,
                ProviderCode = input.ProviderCode,
                AgentType = input.AgentTypeFilter,
                SaleType = input.SaleTypeFilter,
                ReceiverType = input.ReceiverType,
                ProviderResponseCode = input.ProviderResponseCode,
                ReceiverTypeResponse = input.ReceiverTypeResponse,
                ParentProvider = input.ParentProvider,
            };

            var rs = await _transactionManager.TopupListRequestAsync(request);
            var lst = rs.Payload.ConvertTo<List<TopupRequestResponseDto>>();
            foreach (var item in lst)
            {
                item.StatusName = L($"Enum_TopupStatus_{(byte)item.Status}");
                item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                    : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";
                var startTime = item.RequestDate ?? item.CreatedTime;
                if (item.ResponseDate != null)
                    item.TotalTime = (int)(item.ResponseDate.Value - startTime).TotalSeconds;
            }

            return _transactionsExcelExporter.ExportToFilePartner(lst);
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionHistory)]
        public async Task<FileDto> GetTransactionDetailHistoryToExcel(GetAllTopupDetailRequestsInput input)
        {
            var request = new GetTopupDetailRequest
            {
                TransCode = input.TransCode,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
            };
            var rs = await _transactionManager.GetTopupDetailsRequest(request);
            var lst = rs.Payload.ConvertTo<List<TopupDetailResponseDTO>>();
            if (lst.Any())
            {
                var vendors = await GetVendors();
                foreach (var item in lst)
                {
                    var tel = item.ProductCode.Split("_");
                    item.Telco = vendors.FirstOrDefault(x => x.Code == (tel[0]).ToString())?.Name;
                }
            }

            return _transactionsExcelExporter.TopupDetailExportToFile(lst);
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment_Topup)]
        public async Task<ResponseMessages> CreateTopupRequest(CreateOrEditTopupRequestDto input)
        {
            var account = UserManager.GetAccountInfo();
            CheckUserPaymentRequest(account, input.ProductCode, input.CategoryCode);
            var request = new TopupRequest
            {
                Amount = input.Amount,
                ReceiverInfo = input.PhoneNumber,
                ProductCode = input.ProductCode,
                ServiceCode = string.IsNullOrEmpty(input.ServiceCode)
                    ? CommonConst.ServiceCodes.TOPUP
                    : input.ServiceCode,
                CategoryCode = input.CategoryCode,
                PartnerCode = account.NetworkInfo.AccountCode,
                StaffAccount = account.UserInfo.AccountCode,
                StaffUser = account.UserInfo.UserName,
                AgentType = account.UserInfo.AgentType,
                AccountType = account.UserInfo.AccountType,
                ParentCode = account.UserInfo.ParentCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
            var rs = await _transactionManager.ProcessTopupRequest(request);
            var resultCode = rs.ResponseStatus.ErrorCode;
            var obj = new ResponseMessages
            {
                ResponseCode = resultCode,
                ResponseMessage = rs.ResponseStatus.Message,
                Payload = rs.Results,
                ExtraInfo = _commonManger.EncryptQueryParameters(
                    $"/Transactions/TransactionInfo?code={resultCode}&transCode={rs.Results}&message={rs.ResponseStatus.Message}&transType={CommonConst.TransactionType.Topup}&account={request.ReceiverInfo}&amount={request.Amount}")
            };
            return obj;
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment_Topup)]
        public async Task<ResponseMessages> CreateTopupListRequest(TopupListRequestDto input)
        {
            var account = UserManager.GetAccountInfo();
            var transCode = await _commonManger.GetIncrementCodeAsync("L");
            var data = input.ListNumbers.Select(x => new PayBatchItemDto()
            {
                Amount = (int)x.Value,
                ReceiverInfo = x.ReceiverInfo,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                CategoryCode = x.CategoryCode,
                CategoryName = x.CategoryName,
                ServiceCode = x.ServiceCode,
                ServiceName = x.ServiceName,
                PartnerCode = account.NetworkInfo.AccountCode,
                CreatedTime = DateTime.Now,
                Quantity = x.Quantity,
                DiscountAmount = x.Discount,
                PaymentAmount = x.Price,
                Fee = x.Fee,
            }).ToList();

            var rs = await _transactionManager.PayBatchRequestAsync(new PayBatchRequest
            {
                Items = data,
                PartnerCode = account.NetworkInfo.AccountCode,
                StaffAccount = account.UserInfo.AccountCode,
                StaffUser = account.UserInfo.UserName,
                BatchType = input.BatchType,
                Channel = CommonConst.Channel.WEB,
                AgentType = account.UserInfo.AgentType,
                AccountType = account.UserInfo.AccountType,
                ParentCode = account.UserInfo.ParentCode,
                TransRef = transCode,
            });

            if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        var price = input.ListNumbers.Sum(c => c.Price);
                        _logger.LogInformation($"Begin send notifi to {input.ToJson()}");
                        var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var dataSend = new SendNotificationData()
                        {
                            Amount = price,
                            StaffAccount = account.NetworkInfo.AccountCode,
                            PartnerCode = account.NetworkInfo.AccountCode,
                            TransCode = transCode,
                        };
                        var message = L("Notifi_BatchLotRequest", transCode, price.ToFormat("đ"), date);
                        await _appNotifier.PublishNotification(account.NetworkInfo.AccountCode,
                            AppNotificationNames.Payment, dataSend, message,
                            L("Notifi_BatchLotRequest_Title")
                        );
                        _logger.LogInformation(
                            $"Done send notifi to {account.NetworkInfo.AccountCode}-Message: {message}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"SendNotifi CreateTopupListRequest eror:{e}");
                    }
                }).ConfigureAwait(false);
            }

            var resultCode = rs.ResponseStatus.ErrorCode;
            var obj = new ResponseMessages
            {
                ResponseCode = resultCode,
                ResponseMessage = "", // rs.Error.Message,
                //Payload = rs.Result,
                ExtraInfo = _commonManger.EncryptQueryParameters(
                    $"/BatchTopup/Result?code={resultCode}&transCode={transCode}")
            };
            return obj;
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment_PinCode)]
        public async Task<ResponseMessages> CreatePinCodeRequest(CreateOrEditPinCodeRequestDto input)
        {
            var accountInfo = GetAccountInfo();
            CheckUserPaymentRequest(accountInfo, input.ProductCode, input.CategoryCode);
            var request = new CardSaleRequest
            {
                CategoryCode = input.CategoryCode,
                PartnerCode = accountInfo.NetworkInfo.AccountCode,
                StaffAccount = accountInfo.UserInfo.AccountCode,
                StaffUser = accountInfo.NetworkInfo.UserName,
                Quantity = input.Quantity,
                CardValue = input.Amount,
                Email = input.Email,
                ProductCode = input.ProductCode,
                ServiceCode = input.ServiceCode,
                AgentType = accountInfo.UserInfo.AgentType,
                AccountType = accountInfo.UserInfo.AccountType,
                ParentCode = accountInfo.UserInfo.ParentCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
            var rs = await _transactionManager.ProcessPinCodeRequest(request);
            var resultCode = rs.ResponseStatus.ErrorCode;
            var response = new ResponseMessages
            {
                ResponseCode = resultCode,
                ResponseMessage = rs.ResponseStatus.Message,
                Payload = rs.Results,
                ExtraInfo = _commonManger.EncryptQueryParameters(
                    $"/Transactions/TransactionInfo?code={resultCode}&transCode={rs.Results}&message={rs.ResponseStatus.Message}&transType={CommonConst.TransactionType.PinCode}")
            };
            return response;
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment_PayBill)]
        public async Task<BillPaymentInfoDto> BillQueryRequest(BillQueryRequest input)
        {
            if (input.CategoryCode == CommonConst.CategoryCodeConts.EVN_BILL)
            {
                var productCode = _telcoHepper.GetEvnProductCode(input.ReceiverInfo);
                if (productCode == null || input.CategoryCode == null)
                    throw new UserFriendlyException(
                        "Sản phẩm hiện không được hỗ trợ. Vui lòng liên hệ CSKH để biết thông tin chi tiết");
                input.ProductCode = productCode;
            }

            var accountInfo = UserManager.GetAccountInfo();
            input.PartnerCode = accountInfo.NetworkInfo.AccountCode;
            input.StaffAccount = accountInfo.UserInfo.AccountCode;
            input.StaffUser = accountInfo.NetworkInfo.UserName;
            return await _transactionManager.ProcessBillQueryRequest(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment_PayBill)]
        public async Task<ResponseMessages> PayBillRequest(PayBillRequest input)
        {
            var account = UserManager.GetAccountInfo();
            CheckUserPaymentRequest(account, input.ProductCode, input.CategoryCode, true);
            if (input.CategoryCode == CommonConst.CategoryCodeConts.EVN_BILL)
            {
                var productCode = _telcoHepper.GetEvnProductCode(input.ReceiverInfo);
                if (productCode == null || input.CategoryCode == null)
                    throw new UserFriendlyException(
                        "Sản phẩm hiện không được hỗ trợ. Vui lòng liên hệ CSKH để biết thông tin chi tiết");
                input.ProductCode = productCode;
            }

            var request = new PayBillRequest
            {
                ReceiverInfo = input.ReceiverInfo,
                Amount = input.Amount,
                CategoryCode = input.CategoryCode,
                PartnerCode = account.NetworkInfo.AccountCode,
                StaffAccount = account.UserInfo.AccountCode,
                StaffUser = account.NetworkInfo.UserName,
                ProductCode = input.ProductCode,
                ServiceCode = CommonConst.ServiceCodes.PAY_BILL,
                IsSaveBill = input.IsSaveBill,
                InvoiceInfo = input.InvoiceInfo,
                AgentType = account.UserInfo.AgentType,
                AccountType = account.UserInfo.AccountType,
                ParentCode = account.UserInfo.ParentCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
            var rs = await _transactionManager.ProcessPayBillRequest(request);
            var resultCode = rs.ResponseStatus.ErrorCode;
            var obj = new ResponseMessages
            {
                ResponseCode = resultCode,
                ResponseMessage = rs.ResponseStatus.Message,
                Payload = rs.Results,
                ExtraInfo = _commonManger.EncryptQueryParameters(
                    $"/Transactions/TransactionInfo?code={resultCode}&transCode={rs.Results}&message={rs.ResponseStatus.Message}&transType={CommonConst.TransactionType.Payment}&account={input.ReceiverInfo}&amount={input.Amount}")
            };
            return obj;
        }

        public async Task RemoveSavePayBill(RemoveSavePayBillInput request)
        {
            _logger.LogInformation($"RemoveSavePayBill request:{request.ToJson()}");
            var convert = request.ConvertTo<RemoveSavePayBillRequest>();
            convert.AccountCode = _session.AccountCode;
            var rs = await _transactionManager.RemoveSavePayBillRequest(convert);
            if (!rs.Success)
                throw new UserFriendlyException("Xóa hóa đơn không thành công hoặc Không tồn tại hóa đơn đã lưu");
        }

        public async Task<List<PayBillAccountDto>> GetSavePayBills(GetBillSaveInputDto request)
        {
            var convert = request.ConvertTo<GetSavePayBillRequest>();
            convert.AccountCode = _session.AccountCode;
            return await _transactionManager.GetSavePayBillRequest(convert);
        }

        public async Task<int> GetTotalWaitingBill(GetTotalWaitingBill request)
        {
            return await _transactionManager.GetTotalWaitingBillRequest(new GetTotalWaitingBillRequest
            { AccountCode = _session.AccountCode });
        }

        public async Task<ResponseMessages> CheckTransProvider(ProviderCheckTransStatusRequest input)
        {
            var rs = await _transactionManager.ProviderCheckTransRequest(input);
            return new ResponseMessages
            {
                ResponseCode = rs.ResponseStatus.ErrorCode,
                ResponseMessage = rs.ResponseStatus.Message,
                ExtraInfo = rs.Results.ToJson()
            };
        }

        public async Task<List<TopupDetailResponseDTO>> GetTransactionDetail(string transCode)
        {
            try
            {
                var request = new GetTopupDetailRequest
                {
                    TransCode = transCode,
                    Limit = int.MaxValue,
                    Offset = 0,
                    SearchType = SearchType.Search,
                };
                var rs = await _transactionManager.GetTopupDetailsRequest(request);
                return rs.Payload.ConvertTo<List<TopupDetailResponseDTO>>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<VendorDto>> GetVendors()
        {
            try
            {
                var vendors = await _vendorRepository.GetAll()
                    .Where(x => x.Status == 1).ToListAsync();
                return vendors.ConvertTo<List<VendorDto>>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements_Edit)]
        public async Task<ResponseMessages> UpdateStatus(TopupsUpdateStatusRequest request)
        {
            return await _transactionManager.UpdateStatusRequestAsync(request);
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements_Refund)]
        public async Task RefundTransactionRequest(RefundTransDto input)
        {
            var rs = await _transactionManager.TransactionRefundRequestAsync(new TransactionRefundRequest
            {
                TransCode = input.TransCode
            });
            if (rs.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                throw new UserFriendlyException(rs.ResponseStatus.Message);

            // await Task.Run(async () =>
            // {
            //     try
            //     {
            //         _logger.LogInformation($"Begin send notifi to: {input.PartnerCode}");
            //         var balance = rs.Results;
            //         var message = L("Notifi_Refund_Body", input.PaymentAmount.ToFormat("đ"), input.TransRef,
            //             balance.DesBalance.ToFormat("đ"), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            //         await _appNotifier.PublishNotification(
            //             input.PartnerCode,
            //             AppNotificationNames.System,
            //             new SendNotificationData
            //             {
            //                 TransCode = input.TransRef,
            //                 Amount = input.PaymentAmount,
            //                 PartnerCode = input.PartnerCode,
            //                 ServiceCode = CommonConst.ServiceCodes.REFUND,
            //                 TransType = CommonConst.TransactionType.CancelPayment.ToString("G")
            //             },
            //             message,
            //             L("Notifi_Refund_Title")
            //         );
            //         _logger.LogInformation($"Done notifi to: {input.PaymentAmount}-Message:{message}");
            //         var user = await UserManager.GetUserByAccountCodeAsync(input.PartnerCode);
            //         if (user.AgentType == CommonConst.AgentType.AgentApi)
            //         {
            //             var botMessage = L("Bot_Send_RefundToAgentApi",
            //                 user.AccountCode + "-" + user.FullName,
            //                 input.PaymentAmount.ToFormat("đ"),
            //                 balance.DesBalance.ToFormat("đ"),
            //                 DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            //                 $"Hoàn tiền cho giao dịch lỗi. Mã giao dịch:{input.TransRef}"
            //             );
            //             var userProfile = await UserManager.GetUserProfile(user.Id);
            //             if (userProfile != null && !string.IsNullOrEmpty(userProfile.ChatId))
            //             {
            //                 await _appNotifier.PublishTeleToGroupMessage(new SendTeleMessageRequest
            //                 {
            //                     Message = botMessage,
            //                     Module = "Balance",
            //                     Title = "Thông báo biến động số dư do hoàn tiền lỗi giao dịch",
            //                     BotType = (byte)BotType.Sale,
            //                     MessageType = (byte)BotMessageType.Message,
            //                     ChatId = userProfile.ChatId
            //                 });
            //             }
            //         }
            //     }
            //     catch (Exception e)
            //     {
            //         _logger.LogError($"SendNotifi error:{e}");
            //     }
            // }).ConfigureAwait(false);
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<TransactionsProviderLookupTableDto>> GetAllProviderForTableDropdown()
        {
            return await _lookupProviderRepository.GetAll()
                .Select(provider => new TransactionsProviderLookupTableDto
                {
                    Id = provider == null || provider.Code == null
                        ? ""
                        : provider.Code.ToString(),
                    DisplayName = provider == null || provider.Name == null ? "" : provider.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<TransactionsServiceLookupTableDto>> GetAllServiceForTableDropdown()
        {
            return await _lookupServiceRepository.GetAll()
                .Select(service => new TransactionsServiceLookupTableDto
                {
                    Id = service.Id,
                    Code = service.ServiceCode,
                    DisplayName = service == null || service.ServicesName == null ? "" : service.ServicesName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode)
        {
            return await _categoryManager.GetCategoryByServiceCode(serviceCode);
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<CategoryDto>> GetCategoryByServiceCodeMuti(List<string> serviceCode)
        {
            return await _categoryManager.GetCategoryByServiceCodeMuti(serviceCode);
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<ProductInfoDto>> GetProducts(List<int?> cateIds = null)
        {
            var lst = new List<ProductInfoDto>();

            var lstProduct = _lookupProductRepository.GetAllIncluding(x => x.CategoryFk).Include(x => x.CategoryFk)
                .WhereIf(cateIds != null && cateIds.Count > 0 && cateIds.Count(x => x.HasValue) > 0,
                    e => cateIds.Contains(e.CategoryId));

            foreach (var item in lstProduct)
            {
                var d = item.ConvertTo<ProductInfoDto>();
                d.CategoryId = item.CategoryFk.Id;
                d.CategoryCode = item.CategoryFk.CategoryCode;
                d.CategoryName = item.CategoryFk.CategoryName;
                d.ProductName = item.ProductName;
                d.ProductCode = item.ProductCode;
                lst.Add(d);
            }

            return lst.ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements)]
        public async Task<List<ProductInfoDto>> GetProductsMuti(List<string> cateCodes)
        {
            var lst = new List<ProductInfoDto>();

            var lstProduct = _lookupProductRepository.GetAllIncluding(x => x.CategoryFk).Include(x => x.CategoryFk)
                .WhereIf(cateCodes != null && cateCodes.Count > 0,
                    e => cateCodes.Contains(e.CategoryFk.CategoryCode));

            foreach (var item in lstProduct)
            {
                var d = item.ConvertTo<ProductInfoDto>();
                d.CategoryId = item.CategoryFk.Id;
                d.CategoryCode = item.CategoryFk.CategoryCode;
                d.CategoryName = item.CategoryFk.CategoryName;
                d.ProductName = item.ProductName;
                d.ProductCode = item.ProductCode;
                lst.Add(d);
            }

            return lst.ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements_PinCode)]
        public async Task<PagedResultDto<TopupDetailResponseDTO>> GetListTopupDetailRequest(
            GetAllTopupDetailRequestsInput input)
        {
            try
            {
                var request = new GetTopupDetailRequest
                {
                    TransCode = input.TransCode,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                };

                var rs = await _transactionManager.GetTopupDetailsRequest(request);
                var totalCount = rs != null ? rs.Total : 0;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<TopupDetailResponseDTO>(
                        0,
                        new List<TopupDetailResponseDTO>()
                    );
                var lst = rs.Payload.ConvertTo<List<TopupDetailResponseDTO>>();
                if (lst.Any())
                {
                    var vendors = await GetVendors();
                    foreach (var item in lst)
                    {
                        var tel = item.ProductCode.Split("_");
                        item.Telco = vendors.FirstOrDefault(x => x.Code == (tel[0]).ToString())?.Name;
                    }
                }

                return new PagedResultDto<TopupDetailResponseDTO>(totalCount, lst);
            }
            catch (System.Exception e)
            {
                _logger.LogError($"GetTransactionDetail error: {e}");
                return new PagedResultDto<TopupDetailResponseDTO>(
                    0,
                    new List<TopupDetailResponseDTO>());
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TransactionManagements_PinCode)]
        public async Task<FileDto> GetListTopupDetailRequestToExcel(GetListTopupDetailRequestForExcelInput input)
        {
            var request = new GetTopupDetailRequest
            {
                TransCode = input.TransCode,
                Limit = 999999999,
                SearchType = SearchType.Search,
            };

            var rs = await _transactionManager.GetTopupDetailsRequest(request);
            var totalCount = rs != null ? rs.Total : 0;
            var lst = rs.Payload.ConvertTo<List<TopupDetailResponseDTO>>();
            if (lst.Any())
            {
                var vendors = await GetVendors();
                foreach (var item in lst)
                {
                    var tel = item.ProductCode.Split("_");
                    item.Telco = vendors.FirstOrDefault(x => x.Code == (tel[0]).ToString())?.Name;
                }
            }

            return _transactionsExcelExporter.TopupDetailRequestExportToFile(lst, input.TransCode);
        }


        [AbpAuthorize]
        public async Task<NewMessageReponseBase<string>> UpdateCardCode(UpdateCardCodeRequest input)
        {
            var rs = await _transactionManager.UpdateCardCodeRequest(input);
            return rs;
        }

        private void CheckUserPaymentRequest(UserAccountInfoDto accountRequest, string productcode, string categoryCode,
            bool isPaybill = false)
        {
            if (accountRequest == null)
            {
                throw new UserFriendlyException("Tài khoản không tồn tại");
            }

            if (accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.MasterAgent &&
                accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.Staff &&
                accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.StaffApi &&
                accountRequest.UserInfo.AccountType != CommonConst.SystemAccountType.Agent)
            {
                throw new UserFriendlyException("Tài khoản không hợp lệ");
            }

            if (!accountRequest.UserInfo.IsActive)
            {
                throw new UserFriendlyException("Tài khoản của bạn đã bị khóa");
            }

            if (!accountRequest.NetworkInfo.IsActive)
            {
                throw new UserFriendlyException("Không thể thực hiện được giao dịch. Tài khoản đại lý đã bị khóa");
            }

            if (!isPaybill && (string.IsNullOrEmpty(productcode) || string.IsNullOrEmpty(categoryCode)))
            {
                throw new UserFriendlyException("Sản phẩm không tồn tại");
            }

            if (!isPaybill)
            {
                if (productcode.Split("_")[0] != categoryCode.Split("_")[0])
                {
                    throw new UserFriendlyException("Sản phẩm không hợp lệ");
                }
            }
        }

        public async Task<FileDto> ZipCards(string transCode)
        {
            var data = await GetTransactionByCode(transCode);
            if (data == null)
                throw new UserFriendlyException("Không tìm thấy thông tin giao dịch");
            if (!(data.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ||
                  data.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ||
                  data.ServiceCode == CommonConst.ServiceCodes.PIN_GAME))
            {
                _logger.LogInformation($"Download zip file error: Giao dịch không hợp lệ");
                throw new UserFriendlyException("Giao dịch không hợp lệ");
            }

            var account = await _accountManager.GetAccountByCode(data.StaffAccount);
            if (account == null || account.AgentType != CommonConst.AgentType.WholesaleAgent)
            {
                _logger.LogInformation($"Download zip file error: Tài khoản không hợp lệ");
                throw new UserFriendlyException("Tài khoản không hợp lệ");
            }

            if (string.IsNullOrEmpty(account.ValueReceivePassFile))
            {
                _logger.LogInformation($"Download zip file error: không có thông tin nhận password");
                throw new UserFriendlyException("Tài khoản không hợp lệ");
            }

            var password = new Random().Next(1, 999999).ToString("000000");

            //var fileName = $"{transCode}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            var basePath = Path.Combine("",
                $"wwwroot{Path.DirectorySeparatorChar}Uploads{Path.DirectorySeparatorChar}ZipCard");
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
            // file data tồn tại
            // if (Directory.Exists(Path.Combine(basePath, fileName)))
            // {
            //     var f = await File.ReadAllBytesAsync(Path.Combine(basePath, fileName + ".csv"));
            //     return await ZipFileByPassword(account, f, basePath, fileName + ".zip", password);
            // }
            // file data chưa có
            var cards = await GetTransactionDetail(transCode);
            if (!cards.Any())
            {
                _logger.LogInformation($"Download zip file error: không có thông tin mã thẻ");
                throw new UserFriendlyException("Giao dịch không hợp lệ");
            }

            var lst = new List<ZipFileModel>();
            var cardProduct = cards.GroupBy(x => x.ProductCode).Select(x => x.Key).ToList();
            foreach (var p in cardProduct)
            {
                var dataCards = cards.Where(x => x.ProductCode == p).ToList();
                if (!dataCards.Any()) continue;
                var csvStr = new StringBuilder();
                csvStr.AppendLine(string.Format("{0} {1} {2}", "Serial", "Ma the", "Han su dung"));
                foreach (var c in dataCards)
                {
                    csvStr.AppendLine(string.Format("{0} {1} {2}", c.Serial, c.CardCode,
                        c.ExpiredDate.ToString("dd/MM/yyyy")));
                }

                var byt = Encoding.UTF8.GetBytes(csvStr.ToString());
                lst.Add(new ZipFileModel()
                {
                    Data = byt,
                    Name = $"{p}.txt",
                });
            }
            // var csvStr = new StringBuilder();
            // csvStr.AppendLine(string.Format("{0} {1} {2}", "Serial", "Ma the", "Han su dung"));
            // foreach (var c in cards)
            // {
            //
            //     csvStr.AppendLine(string.Format("{0} {1} {2}", c.Serial, c.CardCode, c.ExpiredDate.ToString("dd/MM/yyyy")));
            // }
            // byte[] fileByte = Encoding.UTF8.GetBytes(csvStr.ToString());
            // using (var stream = System.IO.File.Create(Path.Combine(basePath, fileName + ".csv")))
            // {
            //     await stream.WriteAsync(fileByte);
            //     stream.Close();
            // }

            return await ZipFileByPassword(account, lst, $"{transCode}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip",
                password);
        }


        private async Task<FileDto> ZipFileByPassword(UserProfileDto user, List<ZipFileModel> fileContent,
            string fileName, string password)
        {
            if (user.MethodReceivePassFile == CommonConst.MethodReceivePassFile.Sms)
            {
                _logger.LogInformation($"Send ZipFileByPassword sms:{user.ValueReceivePassFile}-{password}");
                await _smsSender.SendAsync(user.ValueReceivePassFile, password, CommonConst.OtpType.PayBill);
            }
            else if (user.MethodReceivePassFile == CommonConst.MethodReceivePassFile.Email)
            {
                _logger.LogInformation($"Send ZipFileByPassword Email:{user.ValueReceivePassFile}-{password}");
                await _userEmailer.SendEmailPasswordFile(user.ValueReceivePassFile, fileName, password);
            }

            _logger.LogInformation($"Download zip file success: {fileName}-{password}");
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var outStream = new ZipOutputStream(outputZipFileStream))
                {
                    outStream.SetLevel(3);
                    outStream.Password = password;
                    foreach (var f in fileContent)
                    {
                        outStream.PutNextEntry(new ZipEntry(f.Name));
                        outStream.Write(f.Data);
                    }
                }

                var fileZip = new FileDto(fileName, MimeTypeNames.ApplicationZip);
                _tempFileCacheManager.SetFile(fileZip.FileToken, outputZipFileStream.ToArray());
                return fileZip;
            }
        }

        public async Task<PagedResultDto<SaleOffsetReponseDto>> GetOffsetTopupHistoryRequest(
            GetAllOffsetTopupRequestsInput input)
        {
            try
            {
                var request = input.ConvertTo<OffsetTopupRequest>();
                request.Limit = input.MaxResultCount;
                request.Offset = input.SkipCount;
                var rs = await _transactionManager.GetOffsetTopupListRequestAsync(request);

                var totalCount = rs.Total;
                if (rs.ResponseCode != "1")
                    return new PagedResultDto<SaleOffsetReponseDto>(
                        0,
                        new List<SaleOffsetReponseDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<SaleOffsetReponseDto>>();


                foreach (var item in lst)
                {
                    item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                        : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                        : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";
                }

                return new PagedResultDto<SaleOffsetReponseDto>(
                    totalCount, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetOffsetTopupHistoryRequest error: {e}");
                return new PagedResultDto<SaleOffsetReponseDto>(
                    0,
                    new List<SaleOffsetReponseDto>());
            }
        }

        public async Task<FileDto> GetOffsetTopupHistoryToExcel(GetAllOffsetTopupRequestsInput input)
        {
            var request = input.ConvertTo<OffsetTopupRequest>();
            request.SearchType = SearchType.Export;
            request.Limit = int.MaxValue;
            request.Offset = 0;
            var rs = await _transactionManager.GetOffsetTopupListRequestAsync(request);
            var lst = rs.Payload.ConvertTo<List<SaleOffsetReponseDto>>();
            foreach (var item in lst)
            {
                item.ServiceName = item.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                    : item.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                    : item.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";

                if (item.Status == CommonConst.SaleRequestStatus.Success)
                    item.StatusName = "Thành công";
                else if (item.Status == CommonConst.SaleRequestStatus.Failed)
                    item.StatusName = "Lỗi";
                else item.StatusName = "Chưa có kết quả";
            }

            return _transactionsExcelExporter.OffsetTopupExportToFile(lst);
        }


        [AbpAuthorize(AppPermissions.Pages_TransactionManagements_Refund)]
        public async Task OffsetBuRequest(OffsetBuRequest input)
        {
            var rs = await _transactionManager.OffsetTopupRequestAsync(input);
            if (rs.ResponseStatus.ErrorCode != ResponseCodeConst.Success)
                throw new UserFriendlyException(rs.ResponseStatus.Message);
        }

        private class ZipFileModel
        {
            public byte[] Data { get; set; }
            public string Name { get; set; }
        }
    }
}