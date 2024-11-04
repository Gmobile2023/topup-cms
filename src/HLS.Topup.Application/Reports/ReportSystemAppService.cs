using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Hangfire;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Bill;
using HLS.Topup.Dtos.Transactions;
using HLS.Topup.Products;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using HLS.Topup.Reports.Exporting;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions;
using Microsoft.Extensions.Logging;
using ServiceStack;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using HLS.Topup.Configuration;
using static HLS.Topup.Report.ReportComparePartnerExportInfo;

namespace HLS.Topup.Reports
{
    [AbpAuthorize]
    public partial class ReportSystemAppService : TopupAppServiceBase, IReportSystemAppService
    {
        private readonly ILogger<ReportSystemAppService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IReportExcelExporter _excelExporter;
        private readonly ITransactionsAppService _tranService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<Product, int> _lookup_productRepository;
        private readonly IRepository<UserProfile> _profileRepository;
        private readonly TopupAppSession _session;
        private readonly IReportsManager _reportsManager;
        private readonly IRepository<User, long> _userRepository;

        public ReportSystemAppService(
            ITransactionManager transactionManager,
            IReportExcelExporter excelExporter,
            TopupAppSession session,
            IReportsManager reportsManager,
            IRepository<UserProfile> profileRepository,
            IRepository<Product, int> lookup_productRepository,
            IWebHostEnvironment env,
            IRepository<User, long> userRepository,
            ILogger<ReportSystemAppService> logger,
            ITransactionsAppService tranService)
        {
            _transactionManager = transactionManager;
            _excelExporter = excelExporter;
            _profileRepository = profileRepository;
            _session = session;
            _reportsManager = reportsManager;
            _logger = logger;
            _tranService = tranService;
            _lookup_productRepository = lookup_productRepository;
            _appConfiguration = env.GetAppConfiguration();
            _userRepository = userRepository;
        }

        public async Task<PagedResultDtoReport<ReportDetailDto>> GetReportDetailList(GetReportDetailInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.AccountCodeFilter) &&
                    _session.AccountType != CommonConst.SystemAccountType.Staff &&
                    _session.AccountType != CommonConst.SystemAccountType.System)
                {
                    input.AccountCodeFilter =
                        _session.AccountCode;
                }

                if (string.IsNullOrEmpty(input.AccountCodeFilter))
                {
                    return new PagedResultDtoReport<ReportDetailDto>(
                        0,
                        new ReportDetailDto(),
                        new List<ReportDetailDto>());
                }

                if (string.IsNullOrEmpty(input.AccountCodeFilter))
                {
                    return new PagedResultDtoReport<ReportDetailDto>(
                        0,
                        new ReportDetailDto(),
                        new List<ReportDetailDto>());
                }

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDateFilter ?? DateTime.Now,
                    ToDate = input.ToDateFilter ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportDetailDto>(0,
                       new ReportDetailDto(),
                       new List<ReportDetailDto>(), warning: msg);


                var request = new ReportDetailRequest
                {
                    TransCode = input.TransCodeFilter,
                    Filter = input.Filter,
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    AccountCode = input.AccountCodeFilter,
                    ServiceCode = input.ServiceCode,
                    CategoryCode = input.CategoryCode,
                };
                var rs = await _reportsManager.ReportDetailGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportDetailDto>(
                        0,
                        new ReportDetailDto(),
                        new List<ReportDetailDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();
                var sumList = rs.SumData.ConvertTo<List<ReportDetailDto>>();
                var sumTotal = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportDetailDto();

                return new PagedResultDtoReport<ReportDetailDto>(
                    totalCount,
                    totalData: sumTotal,
                    lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportDetailList error: {e}");
                return new PagedResultDtoReport<ReportDetailDto>(
                    0,
                    new ReportDetailDto(),
                    new List<ReportDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportDetailDto>> GetReportIndexList(GetReportDetailInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.AccountCodeFilter))
                {
                    return new PagedResultDtoReport<ReportDetailDto>(
                        0,
                        new ReportDetailDto(),
                        new List<ReportDetailDto>());
                }

                if (input.ToDateFilter != null)
                    input.ToDateFilter = input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1);

                var request = new BalanceHistoriesRequest
                {
                    TransCode = input.TransCodeFilter,
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    AccountTrans = input.AccountCodeFilter,
                };

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDateFilter ?? DateTime.Now,
                    ToDate = input.ToDateFilter ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportDetailDto>(0,
                       new ReportDetailDto(),
                       new List<ReportDetailDto>(), warning: msg);

                var rs = await _transactionManager.BalanceHistoriesGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportDetailDto>(0,
                     new ReportDetailDto(),
                     new List<ReportDetailDto>());

                var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();
                foreach (var item in lst)
                {
                    if (input.AccountCodeFilter == item.SrcAccountCode)
                    {
                        item.Decrement = item.Amount;
                        item.BalanceAfter = item.SrcAccountBalance;
                        item.BalanceBefore = item.SrcAccountBalance + item.Amount;
                    }

                    if (input.AccountCodeFilter == item.DesAccountCode)
                    {
                        item.Increment = item.Amount;
                        item.BalanceBefore = item.DesAccountBalance - item.Amount;
                        item.BalanceAfter = item.DesAccountBalance;
                    }

                    item.BalanceBefore = item.BalanceAfter - item.Increment + item.Decrement;
                    item.ServiceName = L($"Enum_TransType_{item.TransType}");

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

                return new PagedResultDtoReport<ReportDetailDto>(
                    totalCount, new ReportDetailDto(), lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportDetailList error: {e}");
                return new PagedResultDtoReport<ReportDetailDto>(0,
                    new ReportDetailDto(),
                    new List<ReportDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportTotalDto>> GetReportTotalList(GetReportTotalInput input)
        {
            try
            {
                var request = new BalanceTotalRequest
                {
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    AccountCode = input.AccountCodeFilter,
                    AgentType = input.AgentType,
                    SearchType = SearchType.Search,
                };

                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDateFilter ?? DateTime.Now,
                    ToDate = input.ToDateFilter ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportTotalDto>(0,
                    new ReportTotalDto(),
                    new List<ReportTotalDto>(), warning: msg);

                var rs = await _reportsManager.BalanceTotalGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportTotalDto>(0,
                        new ReportTotalDto(),
                        new List<ReportTotalDto>()
                    );


                var sumList = rs.SumData.ConvertTo<List<ReportTotalDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportTotalDto();
                var lst = rs.Payload.ConvertTo<List<ReportTotalDto>>();

                return new PagedResultDtoReport<ReportTotalDto>(
                    totalCount,
                    totalData: sumData,
                    lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTotalList error: {e}");
                return new PagedResultDtoReport<ReportTotalDto>(
                    0,
                    new ReportTotalDto(),
                    new List<ReportTotalDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportGroupDto>> GetReportGroupList(GetReportGroupInput input)
        {
            try
            {
                var request = new BalanceGroupTotalRequest
                {
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                };

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDateFilter ?? DateTime.Now,
                    ToDate = input.ToDateFilter ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportGroupDto>(0,
                       new ReportGroupDto(),
                       new List<ReportGroupDto>(), warning: msg
                   );

                var rs = await _reportsManager.BalanceGroupTotalGetRequest(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportGroupDto>(
                        0,
                        new ReportGroupDto(),
                        new List<ReportGroupDto>()
                    );


                var lst = rs.Payload.ConvertTo<List<ReportGroupDto>>();
                var sumList = rs.SumData.ConvertTo<List<ReportGroupDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportGroupDto();
                return new PagedResultDtoReport<ReportGroupDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportGroupList error: {e}");
                return new PagedResultDtoReport<ReportGroupDto>(
                    0,
                    new ReportGroupDto(),
                    new List<ReportGroupDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportTransDetailDto>> GetReportTransDetailList(
            GetReportTransDetailInput input)
        {
            try
            {

                if (_session.AccountType == CommonConst.SystemAccountType.StaffApi) //nếu Nv của đại lý API vào thì lấy lsgd của thằng cha
                {
                    var accountInfo = GetAccountInfo();
                    input.AccountCode = accountInfo.NetworkInfo.AccountCode;
                }
                else
                {
                    input.AccountCode = !string.IsNullOrEmpty(input.AccountCode)
                        ? input.AccountCode
                        : _session.AccountCode;
                }
                var request = input.ConvertTo<ReporttransDetailRequest>();
                request.Limit = input.MaxResultCount;
                request.Offset = input.SkipCount;

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportTransDetailDto>(0,
                    new ReportTransDetailDto(),
                    new List<ReportTransDetailDto>(), warning: msg);

                var rs = await _reportsManager.ReportTransDetailReport(request);

                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportTransDetailDto>(0,
                        new ReportTransDetailDto(),
                        new List<ReportTransDetailDto>()
                    );

                var lst = rs.Payload.ConvertTo<List<ReportTransDetailDto>>();
                lst.ForEach(x =>
                {
                    x.Print = (x.Status == ReportStatus.Success &&
                                          (
                                              x.ServiceCode == CommonConst.ServiceCodes.TOPUP
                                              || x.ServiceCode == CommonConst.ServiceCodes.PIN_DATA
                                              || x.ServiceCode == CommonConst.ServiceCodes.PAY_BILL
                                              || x.ServiceCode == CommonConst.ServiceCodes.PIN_CODE
                                              || x.ServiceCode == CommonConst.ServiceCodes.PIN_GAME
                                              || x.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA
                                          ))
                                     ? 1
                                     : 0;

                });
                var sumList = rs.SumData.ConvertTo<List<ReportTransDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportTransDetailDto();
                return new PagedResultDtoReport<ReportTransDetailDto>(
                    totalCount, sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTransDetailList error: {e}");
                return new PagedResultDtoReport<ReportTransDetailDto>(0,
                    new ReportTransDetailDto(),
                    new List<ReportTransDetailDto>());
            }
        }

        private async Task<TransactionResponseDto> wrapProductCode(TransactionResponseDto item)
        {
            if (!string.IsNullOrEmpty(item.ProductCode))
            {
                var product =
                    await _lookup_productRepository.FirstOrDefaultAsync(x => x.ProductCode == item.ProductCode);
                if (product != null)
                {
                    item.ProductCode = product.ProductCode;
                    item.ProductName = product.ProductName;
                    item.CustomerSupportNote = product.CustomerSupportNote;
                    item.UserManualNote = product.UserManualNote;
                    item.Description = product.Description;
                }
            }

            return item;
        }

        public async Task<TransactionResponseDtoApp> GetReportTransInfoRequest(TransRequestByTransCodeInput input)
        {
            try
            {
                var request = input.ConvertTo<TransDetailByTransCodeRequest>();
                request.Type = input.TransType;

                var rs = await _reportsManager.ReportTransDetailQuery(request);
                var data = new TransactionResponseDto();

                if (rs == null) return null;
                if (rs.TransType == "DEPOSIT")
                {
                    #region .Chi tiết nạp tiền

                    data.ReceiverInfo =
                        rs.AccountCode + (!string.IsNullOrEmpty(rs.AccountInfo) ? " - " + rs.AccountInfo : "");
                    data.Amount = Convert.ToDecimal(rs.TotalPrice);
                    data.Status = (byte)rs.Status;
                    data.StatusName = rs.Status == ReportStatus.Success ? "Thành công"
                        : rs.Status == ReportStatus.TimeOut ? "Chưa có KQ"
                        : rs.Status == ReportStatus.Error ? "Lỗi"
                        : "Chưa có KQ";
                    data.CreatedTime = rs.CreatedTime;
                    data.TransCode = rs.TransCode;
                    data.TransRef = rs.TransCode;
                    data.ServiceCode = rs.TransType;
                    data.ServiceName = "Nạp tiền tài khoản";
                    data.ProductCode = rs.ServiceCode;
                    data.ProductName = rs.ServiceName;
                    data.CategoryCode = rs.ServiceCode;
                    data.CategoryName = rs.ServiceName;
                    data.TransType = rs.TransType;
                    data.Quantity = 1;
                    data.Fee = Convert.ToDecimal(rs.Fee);
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.DiscountAmount = Convert.ToDecimal(Math.Abs(rs.Discount));
                    data.Amount = Convert.ToDecimal(Math.Abs(rs.Amount));
                    data.StaffAccount = rs.AccountCode;
                    var s = (rs.AccountInfo ?? "").Split('-');
                    data.StaffFullName = s.Length >= 2 ? s[1] : string.Empty;
                    data.StaffPhoneNumber = s.Length >= 1 ? s[0] : string.Empty;
                    data.Description = rs.TransNote;
                    if (!string.IsNullOrEmpty(rs.ExtraInfo))
                        data.BankDto = rs.ExtraInfo.FromJson<BankResponseDto>();
                    else
                        data.BankDto = new BankResponseDto();

                    return (await wrapProductCode(data)).ConvertTo<TransactionResponseDtoApp>();

                    #endregion
                }
                else if (rs.TransType == "TRANSFER")
                {
                    var accountInfo = GetAccountInfo();
                    data.ReceiverInfo = rs.AccountCode;
                    data.Amount = Convert.ToDecimal(rs.TotalPrice);
                    data.TransType = rs.TransType;
                    data.StatusName = rs.Status == ReportStatus.Success ? "Thành công"
                        : rs.Status == ReportStatus.TimeOut ? "Chưa có KQ"
                        : rs.Status == ReportStatus.Error ? "Lỗi"
                        : "Chưa có KQ";
                    data.Status = (byte)rs.Status;
                    data.CreatedTime = rs.CreatedTime;
                    data.TransCode = rs.TransCode;
                    data.TransRef = rs.TransCode;
                    data.ServiceCode = rs.ServiceCode;
                    data.ServiceName = accountInfo.UserInfo.AccountCode == rs.AccountCode
                        ? "Nhận tiền đại lý"
                        : "Chuyển tiền đại lý";
                    data.ProductCode = rs.ServiceCode;
                    data.ProductName = accountInfo.UserInfo.AccountCode == rs.AccountCode
                        ? "Nhận tiền đại lý"
                        : "Chuyển tiền đại lý";
                    data.CategoryCode = rs.ServiceCode;
                    data.CategoryName = accountInfo.UserInfo.AccountCode == rs.AccountCode
                        ? "Nhận tiền đại lý"
                        : "Chuyển tiền đại lý";
                    data.PaymentAmount = Convert.ToDecimal(rs.TotalPrice);
                    data.Description = rs.TransNote;
                    data.Quantity = 1;
                    data.Fee = Convert.ToDecimal(rs.Fee);
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.DiscountAmount = Convert.ToDecimal(Math.Abs(rs.Discount));
                    data.Amount = Convert.ToDecimal(Math.Abs(rs.Amount));
                    data.Description = rs.TransNote;
                    var s = (rs.PerformInfo ?? "").Split('-');
                    var a = (rs.AccountInfo ?? "").Split('-');
                    data.TransferInfo = new HLS.Topup.Dtos.Transactions.TransferInfo()
                    {
                        SrcAccountCode = rs.PerformAccount,
                        SrcFullName = s.Length >= 2 ? s[1] : string.Empty,
                        SrcPhoneNumber = s[0],
                        DesAccountCode = rs.AccountCode,
                        DesFullName = a.Length >= 2 ? a[1] : string.Empty,
                        DesPhoneNumber = a[0],
                    };
                    data.StaffAccount = rs.PerformAccount;
                    data.StaffFullName = s.Length >= 2 ? s[1] : "";
                    data.StaffPhoneNumber = s[1];
                    return (await wrapProductCode(data)).ConvertTo<TransactionResponseDtoApp>();
                }
                else if (rs.TransType == "CORRECT_DOWN" || rs.TransType == "CORRECT_UP")
                {
                    var accountInfo = GetAccountInfo();
                    data.ReceiverInfo = rs.AccountCode;
                    data.Amount = Convert.ToDecimal(rs.TotalPrice);
                    data.StatusName = rs.Status == ReportStatus.Success ? "Thành công"
                        : rs.Status == ReportStatus.TimeOut ? "Chưa có KQ"
                        : rs.Status == ReportStatus.Error ? "Lỗi"
                        : "Chưa có KQ";
                    data.Status = 1;
                    data.CreatedTime = rs.CreatedTime;
                    data.TransCode = rs.TransCode;
                    data.TransRef = rs.TransCode;
                    data.TransType = rs.TransType;
                    data.ServiceCode = rs.ServiceCode;
                    data.ServiceName = rs.ServiceName;
                    data.ProductCode = rs.ServiceCode;
                    data.ProductName = rs.ServiceName;
                    data.CategoryCode = rs.ServiceCode;
                    data.CategoryName = rs.ServiceName;
                    data.PaymentAmount = Convert.ToDecimal(rs.TotalPrice);
                    data.Quantity = 1;
                    data.Fee = Convert.ToDecimal(rs.Fee);
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.DiscountAmount = Convert.ToDecimal(Math.Abs(rs.Discount));
                    data.Amount = Convert.ToDecimal(Math.Abs(rs.Amount));
                    data.StaffAccount = rs.PerformAccount;
                    var s = (rs.PerformInfo ?? "").Split('-');
                    data.StaffFullName = s.Length >= 2 ? s[1] : "";
                    data.StaffPhoneNumber = s[1];
                    data.Description = rs.TransNote;
                    return (await wrapProductCode(data)).ConvertTo<TransactionResponseDtoApp>();
                }
                else if (rs.TransType == "REFUND" || rs.TransType == "PAYCOMMISSION")
                {
                    var accountInfo = GetAccountInfo();
                    if (!string.IsNullOrEmpty(rs.ExtraInfo))
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
                    if (rs.TransType == "REFUND")
                        data.TransCode = rs.PaidTransCode;
                    else data.TransCode = rs.TransCode;

                    data.TransRef = rs.RequestTransSouce;
                    data.TransType = rs.TransType;
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.Status = 1;
                    data.Quantity = 1;
                    data.Fee = Convert.ToDecimal(Math.Abs(rs.Fee));
                    data.StatusName = rs.Status == ReportStatus.Success ? "Thành công"
                        : rs.Status == ReportStatus.TimeOut ? "Chưa có KQ"
                        : rs.Status == ReportStatus.Error ? "Lỗi"
                        : "Chưa có KQ";
                    data.StaffAccount = rs.AccountCode;
                    data.CreatedTime = rs.CreatedTime;
                    data.ReceiverInfo = rs.ReceivedAccount;
                    var a = (rs.AccountInfo ?? "").Split('-');
                    data.StaffFullName = a.Length >= 2 ? a[1] : "";
                    data.StaffPhoneNumber = a[1];
                    data.ProductCode = rs.ProductCode;
                    data.ProductName = rs.ProductName;
                    data.CategoryCode = rs.CategoryCode;
                    data.CategoryName = rs.CategoryName;
                    data.ServiceCode = rs.ServiceCode;
                    data.ServiceName = rs.ServiceName;
                    data.ProviderCode = rs.ProvidersCode;
                    data.ProviderName = rs.ProvidersInfo;
                    data.ReceiverInfo = rs.ReceivedAccount;
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.DiscountAmount = Convert.ToDecimal(Math.Abs(rs.Discount));
                    data.Amount = Convert.ToDecimal(Math.Abs(rs.Amount));
                    data.SrcCreatedTime = rs.CreatedTime;
                    data.SrcAccountCode = rs.PerformAccount;
                    var s = (rs.PerformInfo ?? "").Split('-');
                    data.SrcFullName = s.Length >= 2 ? s[1] : "";
                    data.SrcPhoneNumber = s[1];
                    data.Description = rs.TransNote;
                    data.Invoice = !string.IsNullOrEmpty(rs.ExtraInfo)
                        ? rs.ExtraInfo.FromJson<InvoicePayBillDto>()
                        : null;
                    return (await wrapProductCode(data)).ConvertTo<TransactionResponseDtoApp>();
                }
                else
                {
                    if (!string.IsNullOrEmpty(rs.ExtraInfo))
                    {
                        data.Invoice = rs.ExtraInfo.FromJson<InvoicePayBillDto>();
                    }

                    data.ServiceCode = rs.ServiceCode;
                    data.ServiceName = rs.ServiceCode == CommonConst.ServiceCodes.TOPUP ? "Nạp tiền điện thoại"
                        : rs.ServiceCode == CommonConst.ServiceCodes.TOPUP_DATA ? "Nạp data"
                        : rs.ServiceCode == CommonConst.ServiceCodes.PAY_BILL ? "Thanh toán hóa đơn"
                        : rs.ServiceCode == CommonConst.ServiceCodes.PIN_DATA ? "Mua thẻ Data"
                        : rs.ServiceCode == CommonConst.ServiceCodes.PIN_GAME ? "Mua thẻ Game"
                        : rs.ServiceCode == CommonConst.ServiceCodes.PIN_CODE ? "Mua mã thẻ" : "";
                    data.ProviderCode = rs.ProvidersCode;
                    data.ProviderName = rs.ProvidersInfo;
                    data.ProductCode = rs.ProductCode;
                    data.ProductName = rs.ProductName;
                    data.CategoryCode = rs.CategoryCode;
                    data.CategoryName = rs.CategoryName;
                    data.TransCode = string.IsNullOrEmpty(rs.RequestRef) ? rs.TransCode : rs.RequestRef;
                    data.Description = rs.TransNote;
                    data.Quantity = rs.Quantity;
                    data.ReceiverInfo = rs.ReceivedAccount;
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

                    data.TransType = rs.TransType;
                    data.StatusName = rs.Status == ReportStatus.Success ? "Thành công"
                        : rs.Status == ReportStatus.TimeOut ? "Chưa có KQ"
                        : rs.Status == ReportStatus.Error ? "Lỗi"
                        : "Chưa có KQ";
                    data.Status = (byte)rs.Status;
                    data.StaffAccount = rs.AccountCode;
                    var a = (rs.AccountInfo ?? "").Split('-');
                    data.CreatedTime = rs.CreatedTime;
                    data.StaffFullName = a.Length >= 2 ? a[1] : "";
                    data.StaffPhoneNumber = a[0];
                    data.PaymentAmount = Convert.ToDecimal(Math.Abs(rs.PaidAmount ?? 0));
                    data.DiscountAmount = Convert.ToDecimal(Math.Abs(rs.Discount));
                    data.Amount = Convert.ToDecimal(Math.Abs(rs.Amount));
                    data.Fee = Convert.ToDecimal(rs.Fee);
                    data.TransRef = data.TransCode;
                    return (await wrapProductCode(data)).ConvertTo<TransactionResponseDtoApp>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"GetTransInfoRequest error: {e}");
                return null;
            }
        }

        public async Task<PagedResultDtoReport<ReportItemTotalDay>> GetReportTotalDayList(GetReportTotalDayInput input)
        {
            try
            {
                var accountInfo = GetAccountInfo();
                input.AccountCode = accountInfo.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi
                    ? accountInfo.NetworkInfo.AccountCode
                    : accountInfo.UserInfo.AccountCode;
                var request = input.ConvertTo<ReportTotalDayRequest>();
                request.Limit = input.MaxResultCount;
                request.Offset = input.SkipCount;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportItemTotalDay>(0,
                        new ReportItemTotalDay(),
                        new List<ReportItemTotalDay>(),
                        warning: msg
                    );

                var rs = await _reportsManager.ReportTotalDayReport(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportItemTotalDay>(
                        0,
                        new ReportItemTotalDay(),
                        new List<ReportItemTotalDay>()
                    );

                var lst = rs.Payload.ConvertTo<List<ReportItemTotalDay>>();
                var sumList = rs.SumData.ConvertTo<List<ReportItemTotalDay>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportItemTotalDay();
                return new PagedResultDtoReport<ReportItemTotalDay>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTotalDayList error: {e}");
                return new PagedResultDtoReport<ReportItemTotalDay>(
                    0,
                    new ReportItemTotalDay(),
                    new List<ReportItemTotalDay>());
            }
        }

        public async Task<PagedResultDtoReport<ReportDebtDetailDto>> GetReportDebtDetailList(
            GetReportdebtDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportDebtDetailRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportDebtDetailDto>(0,
                    new ReportDebtDetailDto(),
                    new List<ReportDebtDetailDto>(),
                    warning: msg);

                var rs = await _reportsManager.ReportDebtDetailReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportDebtDetailDto>(0, new ReportDebtDetailDto(),
                        new List<ReportDebtDetailDto>());


                var sumList = rs.SumData.ConvertTo<List<ReportDebtDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportDebtDetailDto();
                var lst = rs.Payload.ConvertTo<List<ReportDebtDetailDto>>();
                foreach (var item in lst)
                {
                    if (item.ServiceCode == "SaleDeposit") item.ServiceName = "Công nợ phát sinh";
                    else if (item.ServiceCode == "ClearDebt") item.ServiceName = "Thanh toán công nợ";
                }

                return new PagedResultDtoReport<ReportDebtDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportDebtDetailList error: {e}");
                return new PagedResultDtoReport<ReportDebtDetailDto>(0,
                    new ReportDebtDetailDto(),
                    new List<ReportDebtDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportItemTotalDebt>> GetReportTotalDebtList(
            GetReportTotalDebtInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportTotalDebtRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportItemTotalDebt>(0,
                       new ReportItemTotalDebt(),
                       new List<ReportItemTotalDebt>(),
                       warning: msg
                   );

                var rs = await _reportsManager.ReportTotalDebtReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportItemTotalDebt>(
                        0,
                        new ReportItemTotalDebt(),
                        new List<ReportItemTotalDebt>()
                    );


                var sumList = rs.SumData.ConvertTo<List<ReportItemTotalDebt>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportItemTotalDebt();

                var lst = rs.Payload.ConvertTo<List<ReportItemTotalDebt>>();
                foreach (var x in lst)
                {
                    var linkDebt = string.Format("{0}|{1}|{2}|{3}|ClearDebt",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        x.SaleCode, "");
                    var linkDeposit = string.Format("{0}|{1}|{2}|{3}|SALE|1",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        string.Empty,
                        x.SaleCode);

                    x.LinkPayDebt = "/App/Reports/AccountDebtDetail?key=" + linkDebt;
                    x.LinkDebt = "/App/Deposits?key=" + linkDeposit;
                }

                return new PagedResultDtoReport<ReportItemTotalDebt>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTotalDebtList error: {e}");
                return new PagedResultDtoReport<ReportItemTotalDebt>(
                    0,
                    new ReportItemTotalDebt(),
                    new List<ReportItemTotalDebt>());
            }
        }

        public async Task<PagedResultDtoReport<ReportRefundDetailDto>> GetReportRefundDetailList(
            GetReportRefundDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRefundDetailRequest>();
                request.Limit = input.MaxResultCount;
                request.Offset = input.SkipCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRefundDetailDto>(0,
                    new ReportRefundDetailDto(),
                    new List<ReportRefundDetailDto>(),
                    warning: msg);

                var rs = await _reportsManager.ReportRefundDetailReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRefundDetailDto>(0, new ReportRefundDetailDto(),
                        new List<ReportRefundDetailDto>());


                var sumList = rs.SumData.ConvertTo<List<ReportRefundDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRefundDetailDto();

                var lst = rs.Payload.ConvertTo<List<ReportRefundDetailDto>>();
                return new PagedResultDtoReport<ReportRefundDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportRefundDetailList error: {e}");
                return new PagedResultDtoReport<ReportRefundDetailDto>(0,
                    new ReportRefundDetailDto(),
                    new List<ReportRefundDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportServiceDetailDto>> GetReportServiceDetailList(
            GetReportServiceDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportServiceDetailRequest>();
                request.ReceiverType = input.ReceiverType;
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;                
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportServiceDetailDto>(0,
                    new ReportServiceDetailDto(),
                    new List<ReportServiceDetailDto>(), warning: msg);

                var rs = await _reportsManager.ReportServiceDetailReport(request);
                var totalCount = rs.Total;
                var sumList = rs.SumData.ConvertTo<List<ReportServiceDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportServiceDetailDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportServiceDetailDto>(0, new ReportServiceDetailDto(),
                        new List<ReportServiceDetailDto>());

                var lst = rs.Payload.ConvertTo<List<ReportServiceDetailDto>>();

                return new PagedResultDtoReport<ReportServiceDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportServiceDetailList error: {e}");
                return new PagedResultDtoReport<ReportServiceDetailDto>(0,
                    new ReportServiceDetailDto(),
                    new List<ReportServiceDetailDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportTransferDetailDto>> GetReportTransferDetailList(
            GetReportTransferDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportTransferDetailRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportTransferDetailDto>(0,
                   new ReportTransferDetailDto(),
                   new List<ReportTransferDetailDto>(), warning: msg);

                var rs = await _reportsManager.ReportTransferDetailReport(request);
                var totalCount = rs.Total;
                var sumList = rs.SumData.ConvertTo<List<ReportTransferDetailDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportTransferDetailDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportTransferDetailDto>(0, new ReportTransferDetailDto(),
                        new List<ReportTransferDetailDto>());
                var lst = rs.Payload.ConvertTo<List<ReportTransferDetailDto>>();

                return new PagedResultDtoReport<ReportTransferDetailDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTransferDetailList error: {e}");
                return new PagedResultDtoReport<ReportTransferDetailDto>(
                    0,
                    new ReportTransferDetailDto(),
                    new List<ReportTransferDetailDto>());
            }
        }


        public async Task<PagedResultDtoReport<ReportServiceTotalDto>> GetReportServiceTotalList(
            GetReportServiceTotalInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportServiceTotalRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string data = null;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref data))
                    return new PagedResultDtoReport<ReportServiceTotalDto>(0,
                     new ReportServiceTotalDto(),
                     new List<ReportServiceTotalDto>(), warning: data);

                var rs = await _reportsManager.ReportServiceTotalReport(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportServiceTotalDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportServiceTotalDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportServiceTotalDto>(0, new ReportServiceTotalDto(),
                        new List<ReportServiceTotalDto>());

                var lst = rs.Payload.ConvertTo<List<ReportServiceTotalDto>>();
                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        c.ProductCode, "", "", c.ServiceCode
                    );
                    c.Link = "/App/Reports/ReportServiceDetail?key=" + link;
                });
                return new PagedResultDtoReport<ReportServiceTotalDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportServiceTotalList error: {e}");
                return new PagedResultDtoReport<ReportServiceTotalDto>(
                    0,
                    new ReportServiceTotalDto(),
                    new List<ReportServiceTotalDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportServiceProviderDto>> GetReportServiceProviderList(
           GetReportServiceProviderInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportServiceProviderRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                request.ReceiverType = input.ReceiverType;
                request.ProviderReceiverType = input.ProviderReceiverType;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportServiceProviderDto>(0,
                   new ReportServiceProviderDto(),
                   new List<ReportServiceProviderDto>(), warning: msg);

                var rs = await _reportsManager.ReportServiceProviderReport(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportServiceProviderDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportServiceProviderDto();

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportServiceProviderDto>(0, new ReportServiceProviderDto(),
                        new List<ReportServiceProviderDto>());

                var lst = rs.Payload.ConvertTo<List<ReportServiceProviderDto>>();
                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        c.ProductCode, "", "", c.ServiceCode, "", "", "", c.ProviderCode
                    );
                    c.Link = "/App/Reports/ReportServiceDetail?key=" + link;
                });
                return new PagedResultDtoReport<ReportServiceProviderDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportServiceProviderList error: {e}");
                return new PagedResultDtoReport<ReportServiceProviderDto>(
                    0,
                    new ReportServiceProviderDto(),
                    new List<ReportServiceProviderDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportAgentBalanceDto>> GetReportAgentBalanceList(
            GetReportAgentBalanceInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportAgentBalanceRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                request.AgentType = input.AgentType;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportAgentBalanceDto>(0,
                     new ReportAgentBalanceDto(),
                     new List<ReportAgentBalanceDto>(), warning: msg);

                var rs = await _reportsManager.ReportAgentBalanceReport(request);
                var totalCount = rs.Total;

                var sumList = rs.SumData.ConvertTo<List<ReportAgentBalanceDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportAgentBalanceDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportAgentBalanceDto>(0, new ReportAgentBalanceDto(),
                        new List<ReportAgentBalanceDto>());

                var lst = rs.Payload.ConvertTo<List<ReportAgentBalanceDto>>();
                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        c.AgentCode
                    );

                    c.Link = "/App/Reports/BalanceAccount?key=" + link;
                });

                return new PagedResultDtoReport<ReportAgentBalanceDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportAgentBalanceList error: {e}");
                return new PagedResultDtoReport<ReportAgentBalanceDto>(
                    0,
                    new ReportAgentBalanceDto(),
                    new List<ReportAgentBalanceDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportRevenueAgentDto>> GetReportRevenueAgentList(
            GetReportRevenueAgentInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRevenueAgentRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRevenueAgentDto>(0,
                     new ReportRevenueAgentDto(),
                     new List<ReportRevenueAgentDto>(), warning: msg);

                var rs = await _reportsManager.ReportRevenueAgentReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRevenueAgentDto>(0, new ReportRevenueAgentDto(),
                        new List<ReportRevenueAgentDto>());

                var lst = rs.Payload.ConvertTo<List<ReportRevenueAgentDto>>();
                var sumList = rs.SumData.ConvertTo<List<ReportRevenueAgentDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRevenueAgentDto();

                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        "",
                        c.AgentCode,
                        c.SaleCode,
                        input.ServiceCode, c.CityId, c.DistrictId, c.WardId
                    );
                    c.Link = "/App/Reports/ReportServiceDetail?key=" + link;
                });

                return new PagedResultDtoReport<ReportRevenueAgentDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportRevenueAgentList error: {e}");
                return new PagedResultDtoReport<ReportRevenueAgentDto>(0,
                    new ReportRevenueAgentDto(),
                    new List<ReportRevenueAgentDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportRevenueCityDto>> GetReportRevenueCityList(
            GetReportRevenueCityInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRevenueCityRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRevenueCityDto>(0,
                      new ReportRevenueCityDto(),
                      new List<ReportRevenueCityDto>(), warning: msg);

                var rs = await _reportsManager.ReportRevenueCityReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRevenueCityDto>(0, new ReportRevenueCityDto(),
                        new List<ReportRevenueCityDto>());

                var sumList = rs.SumData.ConvertTo<List<ReportRevenueCityDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRevenueCityDto();
                var lst = rs.Payload.ConvertTo<List<ReportRevenueCityDto>>();
                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        string.Empty, string.Empty, input.UserSaleCode, input.ServiceCode,
                        c.CityId, c.DistrictId, c.WardId
                    );
                    c.LinkDetail = "/App/Reports/ReportServiceDetail?key=" + link;
                    c.LinkAgent = "/App/Reports/ReportRevenueAgent?key=" + link;
                });
                return new PagedResultDtoReport<ReportRevenueCityDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportRevenueCityList error: {e}");
                return new PagedResultDtoReport<ReportRevenueCityDto>(0,
                    new ReportRevenueCityDto(),
                    new List<ReportRevenueCityDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportTotalSaleAgentDto>> GetReportTotalSaleAgentList(
            GetReportTotalSaleAgentInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportTotalSaleAgentRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportTotalSaleAgentDto>(0,
                    new ReportTotalSaleAgentDto(),
                    new List<ReportTotalSaleAgentDto>(), warning: msg);

                var rs = await _reportsManager.ReportTotalSaleAgentReport(request);
                var totalCount = rs.Total;

                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportTotalSaleAgentDto>(0, new ReportTotalSaleAgentDto(),
                        new List<ReportTotalSaleAgentDto>());
                var sumList = rs.SumData.ConvertTo<List<ReportTotalSaleAgentDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportTotalSaleAgentDto();
                var lst = rs.Payload.ConvertTo<List<ReportTotalSaleAgentDto>>();
                lst.ForEach(c =>
                {
                    var link = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        "",
                        c.AgentCode,
                        string.Empty,
                        input.ServiceCode,
                        c.CityId, c.DistrictId, c.WardId);
                    c.Link = "/App/Reports/ReportServiceDetail?key=" + link;
                });

                return new PagedResultDtoReport<ReportTotalSaleAgentDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTotalSaleAgentList error: {e}");
                return new PagedResultDtoReport<ReportTotalSaleAgentDto>(0,
                    new ReportTotalSaleAgentDto(),
                    new List<ReportTotalSaleAgentDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportRevenueActiveDto>> GetReportRevenueActiveList(
            GetReportRevenueActiveInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRevenueActiveRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                if (request.AgentType == 99) request.AgentType = 0;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRevenueActiveDto>(0,
                   new ReportRevenueActiveDto(),
                   new List<ReportRevenueActiveDto>(), warning: msg);

                var rs = await _reportsManager.ReportRevenueActiveReport(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRevenueActiveDto>(0, new ReportRevenueActiveDto(),
                        new List<ReportRevenueActiveDto>());

                var lst = rs.Payload.ConvertTo<List<ReportRevenueActiveDto>>();

                var sumList = rs.SumData.ConvertTo<List<ReportRevenueActiveDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRevenueActiveDto();
                lst.ForEach(c =>
                {
                    var linkDetail = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        1,
                        "",
                        c.AgentCode,
                        input.UserSaleCode,
                        "",
                        c.CityId, c.DistrictId, c.WardId
                    );

                    c.LinkDetail = "/App/Reports/ReportServiceDetail?key=" + linkDetail;
                    var linkDeposit = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                        input.FromDate.Value.ToString("yyyy-MM-dd"),
                        input.ToDate.Value.ToString("yyyy-MM-dd"),
                        c.AgentCode,
                        input.UserSaleCode,
                        "SAlE",
                        1
                    );
                    c.LinkDeposit = "/App/Deposits?key=" + linkDeposit;
                });
                return new PagedResultDtoReport<ReportRevenueActiveDto>(
                    totalCount,
                    totalData: sumData,
                    lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportRevenueActiveList error: {e}");
                return new PagedResultDtoReport<ReportRevenueActiveDto>(0,
                    new ReportRevenueActiveDto(),
                    new List<ReportRevenueActiveDto>());
            }
        }


        public async Task<PagedResultDtoReport<ReportRevenueDashboardDay>> GetReportRevenueDashboardList(
            GetReportRevenueDashboardInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRevenueDashBoardDayRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;

                if (_session.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    var accountInfo = GetAccountInfo();
                    request.LoginCode = accountInfo.NetworkInfo.AccountCode;
                    request.AccountType = (int)CommonConst.SystemAccountType.MasterAgent;

                }
                else
                {
                    request.LoginCode = _session.AccountCode;
                    request.AccountType = (int)_session.AccountType;
                }

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRevenueDashboardDay>(0,
                    new ReportRevenueDashboardDay(),
                    new List<ReportRevenueDashboardDay>(), warning: msg);

                var rs = await _reportsManager.ReportRevenueDashboardDayList(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRevenueDashboardDay>(0, new ReportRevenueDashboardDay(),
                        new List<ReportRevenueDashboardDay>());

                var lst = rs.Payload.ConvertTo<List<ReportRevenueDashboardDay>>();

                var sumList = rs.SumData.ConvertTo<List<ReportRevenueDashboardDay>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRevenueDashboardDay();
                return new PagedResultDtoReport<ReportRevenueDashboardDay>(
                    totalCount,
                    totalData: sumData,
                    lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportRevenueDashboardList error: {e}");
                return new PagedResultDtoReport<ReportRevenueDashboardDay>(
                    0,
                    new ReportRevenueDashboardDay(),
                    new List<ReportRevenueDashboardDay>());
            }
        }


        public async Task<List<DashRevenueItem>> GetDashboardListRevenue(
            GetDashboardRevenueInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportRevenueDashBoardDayRequest>();
                request.Limit = int.MaxValue;
                request.Offset = 0;

                if (_session.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    var accountInfo = GetAccountInfo();
                    request.LoginCode = accountInfo.NetworkInfo.AccountCode;
                    request.AccountType = (int)CommonConst.SystemAccountType.MasterAgent;

                }
                else
                {
                    request.LoginCode = _session.AccountCode;
                    request.AccountType = (int)_session.AccountType;
                }

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new List<DashRevenueItem>();

                var rs = await _reportsManager.ReportRevenueDashboardDayList(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new List<DashRevenueItem>();

                var lst = rs.Payload.ConvertTo<List<ReportRevenueDashboardDay>>();
                var lit = (from item in lst.OrderBy(c => c.CreatedDay)
                           select new DashRevenueItem
                           {
                               y = item.Revenue,
                               d = item.Discount,
                               indexLabel = item.Revenue < 1000000
                                   ? Math.Round(item.Revenue / 1000, 0) + "k"
                                   : Math.Round(item.Revenue / 1000000, 1) + "tr",
                               label = item.CreatedDay.ToString("dd/MM"),
                           }).ToList();

                foreach (var item in lit)
                {
                    if (item.y > 0)
                        item.toolTipContent =
                            $"{item.label}: {item.indexLabel} - {Convert.ToDouble(item.y).ToString("N0")}";
                }

                return lit;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDashboardListRevenue error: {e}");
                return new List<DashRevenueItem>();
            }
        }


        public async Task<FileDto> GetReportRevenueDashboardListToExcel(GetReportRevenueDashboardInput input)
        {
            var request = input.ConvertTo<ReportRevenueDashBoardDayRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            if (_session.AccountType == CommonConst.SystemAccountType.StaffApi)
            {
                var accountInfo = GetAccountInfo();
                request.LoginCode = accountInfo.NetworkInfo.AccountCode;
                request.AccountType = (int)CommonConst.SystemAccountType.MasterAgent;

            }
            else
            {
                request.LoginCode = _session.AccountCode;
                request.AccountType = (int)_session.AccountType;
            }

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportRevenueDashboardDayList(request);
            var lst = rs.Payload.ConvertTo<List<ReportRevenueDashboardDay>>();

            return _excelExporter.ExportRevenueDashboardListExportToFile(lst);
        }

        public async Task<FileDto> GetReportRefundDetailListToExcel(GetReportRefundDetailInput input)
        {
            var request = input.ConvertTo<ReportRefundDetailRequest>();
            request.Offset = 0;
            request.SearchType = SearchType.Export;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            request.Limit = int.MaxValue;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportRefundDetailReport(request);
            if (rs.ExtraInfo == "Downloadlink")
            {
                return new FileDto()
                {
                    FileName = rs.ExtraInfo,
                    FilePath = rs.ResponseMessage
                };
            }
            else
            {

                var lst = rs.Payload.ConvertTo<List<ReportRefundDetailDto>>();
                return _excelExporter.ReportRefundDetailExportToFile(lst);
            }
        }

        public async Task<FileDto> GetReportTransferDetailListToExcel(GetReportTransferDetailInput input)
        {
            var request = input.ConvertTo<ReportTransferDetailRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTransferDetailReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportTransferDetailDto>>();
            return _excelExporter.ReportTransferDetailExportToFile(lst);
        }

        public async Task<FileDto> GetReportServiceDetailListToExcel(GetReportServiceDetailInput input)
        {
            var request = input.ConvertTo<ReportServiceDetailRequest>();
            request.ReceiverType = input.ReceiverType;
            request.Limit = 1;
            request.Offset = 0;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            request.Limit = int.MaxValue;
            request.SearchType = SearchType.Export;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportServiceDetailReport(request);
            if (rs.ExtraInfo == "Downloadlink")
            {
                return new FileDto()
                {
                    FileName = rs.ExtraInfo,
                    FilePath = rs.ResponseMessage,
                };
            }
            else
            {
                var lst = rs.Payload.ConvertTo<List<ReportServiceDetailDto>>();
                return _excelExporter.ReportServiceDetailExportToFile(lst);
            }
        }

        public async Task<FileDto> GetReportServiceTotalListToExcel(GetReportServiceTotalInput input)
        {
            var request = input.ConvertTo<ReportServiceTotalRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportServiceTotalReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportServiceTotalDto>>();
            return _excelExporter.ReportServiceTotalExportToFile(lst);
        }

        public async Task<FileDto> GetReportServiceProviderListToExcel(GetReportServiceProviderInput input)
        {
            var request = input.ConvertTo<ReportServiceProviderRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportServiceProviderReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportServiceProviderDto>>();
            return _excelExporter.ReportServiceProviderExportToFile(lst);
        }

        public async Task<FileDto> GetReportAgentBalanceListToExcel(GetReportAgentBalanceInput input)
        {
            var request = input.ConvertTo<ReportAgentBalanceRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            request.AgentType = input.AgentType;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportAgentBalanceReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportAgentBalanceDto>>();
            lst.ForEach(c =>
            {
                var link = string.Format("{0}|{1}|{2}",
                    input.FromDate.Value.ToString("yyyy-MM-dd"),
                    input.ToDate.Value.ToString("yyyy-MM-dd"),
                    c.AgentCode);
                c.Link = "/App/Reports/BalanceAccount?key=" + link;
            });

            return _excelExporter.ReportAgentBalanceExportToFile(lst);
        }

        public async Task<FileDto> GetReportDebtDetailListToExcel(GetReportdebtDetailInput input)
        {
            var request = input.ConvertTo<ReportDebtDetailRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportDebtDetailReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportDebtDetailDto>>();
            foreach (var item in lst)
            {
                if (item.ServiceCode == "SaleDeposit") item.ServiceName = "Công nợ phát sinh";
                else if (item.ServiceCode == "ClearDebt") item.ServiceName = "Thanh toán công nợ";
            }

            return _excelExporter.ReportDebtDetailExportToFile(lst);
        }

        public async Task<FileDto> GetReportDetailListToExcel(GetAllReportDetailForExcelInput input)
        {
            var request = new ReportDetailRequest
            {
                TransCode = input.TransCodeFilter,
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                AccountCode = input.AccountCodeFilter,
                AccountType = 0,
            };
            request.Limit = int.MaxValue;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportDetailGetRequest(request);
            if (rs.ExtraInfo == "Downloadlink")
            {
                return new FileDto()
                {
                    FileName = rs.ExtraInfo,
                    FilePath = rs.ResponseMessage,
                };
            }
            else
            {

                var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();
                return _excelExporter.ReportDetailExportToFile(lst);
            }
        }

        public async Task<FileDto> GetReportIndexListToExcel(GetAllReportDetailForExcelInput input)
        {
            if (input.ToDateFilter != null)
                input.ToDateFilter = input.ToDateFilter.Value.Date.AddDays(1).AddSeconds(-1);

            var request = new BalanceHistoriesRequest
            {
                TransCode = input.Filter,
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter?.AddHours(23).AddMinutes(59).AddSeconds(59),
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                AccountTrans = input.AccountCodeFilter,
                TransType = (CommonConst.TransactionType)input.TransType
            };
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _transactionManager.BalanceHistoriesGetRequest(request);
            var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();
            foreach (var item in lst)
            {
                if (input.AccountCodeFilter == item.SrcAccountCode)
                {
                    item.Decrement = item.Amount;
                    item.BalanceAfter = item.SrcAccountBalance;
                    item.BalanceBefore = item.SrcAccountBalance + item.Amount;
                }

                if (input.AccountCodeFilter == item.DesAccountCode)
                {
                    item.Increment = item.Amount;
                    item.BalanceAfter = item.DesAccountBalance;
                    item.BalanceBefore = item.DesAccountBalance - item.Amount;
                }

                item.ServiceName = L($"Enum_TransType_{item.TransType}");


                if (item.TransType == CommonConst.TransactionType.Transfer)
                {
                    if (_session.AccountCode == item.SrcAccountCode)
                        item.TransNote =
                            $"Chuyển tiền tới tài khoản {item.DesAccountCode}. Nội dung: {item.Description}";
                    if (_session.AccountCode == item.DesAccountCode)
                        item.TransNote = $"Nhận tiền từ tài khoản {item.SrcAccountCode}. Nội dung: {item.Description}";
                }
            }

            return _excelExporter.ReportDetailExportToFile(lst);
        }

        public async Task<FileDto> GetReportTotalListToExcel(GetAllReportTotalForExcelInput input)
        {
            var request = new BalanceTotalRequest
            {
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter,
                Limit = int.MaxValue,
                Offset = 0,
                AccountCode = input.AccountCodeFilter,
                AgentType = input.AgentType,
                SearchType = SearchType.Export,
            };
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.BalanceTotalGetRequest(request);
            var lst = rs.Payload.ConvertTo<List<ReportTotalDto>>();
            return _excelExporter.ReportTotalExportToFile(lst);
        }

        public async Task<FileDto> GetReportGroupListToExcel(GetAllReportGroupForExcelInput input)
        {
            var request = new BalanceGroupTotalRequest
            {
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Search,
            };

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.BalanceGroupTotalGetRequest(request);
            var lst = rs.Payload.ConvertTo<List<ReportGroupDto>>();
            return _excelExporter.ReportGroupExportToFile(lst);
        }

        public async Task<FileDto> GetReportTransDetailListToExcel(GetAllReportTransDetailForExcelInput input)
        {
            var request = input.ConvertTo<ReporttransDetailRequest>();
            request.Limit = 1;
            request.Offset = 0;
            request.SearchType = SearchType.Export;

            if (_session.AccountType == CommonConst.SystemAccountType.StaffApi) //nếu Nv của đại lý API vào thì lấy lsgd của thằng cha
            {
                var accountInfo = GetAccountInfo();
                request.AccountCode = accountInfo.NetworkInfo.AccountCode;
            }
            else
            {
                request.AccountCode = _session.AccountCode;
            }
            request.Limit = int.MaxValue;
            request.SearchType = SearchType.Export;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTransDetailReport(request);

            if (rs.ExtraInfo == "Downloadlink")
            {
                return new FileDto()
                {
                    FileName = rs.ExtraInfo,
                    FilePath = rs.ResponseMessage
                };
            }
            else
            {
                var lst = rs.Payload.ConvertTo<List<ReportTransDetailDto>>();
                return _excelExporter.ReportTransDetailExportToFile(lst);
            }
        }

        public async Task<FileDto> GetReportTotalDayListToExcel(GetAllReportTotalDayForExcelInput input)
        {
            var accountInfo = GetAccountInfo();
            input.AccountCode = accountInfo.UserInfo.AccountType == CommonConst.SystemAccountType.StaffApi
                ? accountInfo.NetworkInfo.AccountCode
                : accountInfo.UserInfo.AccountCode;
            var request = input.ConvertTo<ReportTotalDayRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTotalDayReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportItemTotalDay>>();
            return _excelExporter.ReportTotalDayExportToFile(lst);
        }

        public async Task<FileDto> GetReportTotalDebtListToExcel(GetReportTotalDebtInput input)
        {
            var request = input.ConvertTo<ReportTotalDebtRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTotalDebtReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportItemTotalDebt>>();
            return _excelExporter.ReportTotalDebtExportToFile(lst);
        }

        public async Task<FileDto> GetReportRevenueAgentListToExcel(GetReportRevenueAgentInput input)
        {
            var request = input.ConvertTo<ReportRevenueAgentRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportRevenueAgentReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportRevenueAgentDto>>();
            return _excelExporter.ReportRevenueAgentExportToFile(lst);
        }

        public async Task<FileDto> GetReportRevenueCityListToExcel(GetReportRevenueCityInput input)
        {
            var request = input.ConvertTo<ReportRevenueCityRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportRevenueCityReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportRevenueCityDto>>();
            return _excelExporter.ReportRevenueCityExportToFile(lst);
        }

        public async Task<FileDto> GetReportTotalSaleAgentListToExcel(GetReportTotalSaleAgentInput input)
        {
            var request = input.ConvertTo<ReportTotalSaleAgentRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTotalSaleAgentReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportTotalSaleAgentDto>>();

            return _excelExporter.ReportTotalSaleAgentExportToFile(lst);
        }

        public async Task<FileDto> GetReportRevenueActiveListToExcel(GetReportRevenueActiveInput input)
        {
            var request = input.ConvertTo<ReportRevenueActiveRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            if (request.AgentType == 99) request.AgentType = 0;

            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportRevenueActiveReport(request);
            var lst = rs.Payload.ConvertTo<List<ReportRevenueActiveDto>>();

            return _excelExporter.ReportRevenueActiveExportToFile(lst);
        }

        public async Task<GetRevenueInDayDto> GetRevenueInDayRequest(GetRevenueInDayInput input)
        {
            return await _reportsManager.GetRevenueInDayRequest(new GetRevenueInDayRequest
            { AccountCode = _session.AccountCode });
        }

        #region CardReport

        public async Task<PagedResultDtoReport<ReportCardStockHistoriesDto>> GetReportCardStockHistories(
            GetReportCardStockHistoriesInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.StockCodeFilter) || string.IsNullOrEmpty(input.ProductCodeFilter))
                    return new PagedResultDtoReport<ReportCardStockHistoriesDto>(
                        0,
                        new ReportCardStockHistoriesDto(),
                        new List<ReportCardStockHistoriesDto>(),
                        warning: "Quý khách vui lòng chọn thông tin kho và sản phẩm để tìm kiếm !"
                    );
                var request = new ReportCardStockHistoriesRequest
                {
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    Telco = input.TelcoFilter,
                    CardValue = input.CardValueFilter,
                    StockCode = input.StockCodeFilter,
                    ProductCode = input.ProductCodeFilter
                };

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCardStockHistoriesDto>(
                         0, new ReportCardStockHistoriesDto(),
                         new List<ReportCardStockHistoriesDto>(), warning: msg
                     );

                var rs = await _reportsManager.ReportCardStockHistories(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCardStockHistoriesDto>(
                        0, new ReportCardStockHistoriesDto(),
                        new List<ReportCardStockHistoriesDto>(), warning: msg
                    );
                var lst = rs.Payload.ConvertTo<List<ReportCardStockHistoriesDto>>();
                return new PagedResultDtoReport<ReportCardStockHistoriesDto>(
                    totalCount, new ReportCardStockHistoriesDto(), lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCardStockHistories error: {e}");
                return new PagedResultDtoReport<ReportCardStockHistoriesDto>(0,
                    new ReportCardStockHistoriesDto(),
                    new List<ReportCardStockHistoriesDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportCardStockImExPortDto>> GetReportCardStockImExPort(
            GetReportCardStockImExPortInput input)
        {
            try
            {
                var request = new ReportCardStockImExPortRequest
                {
                    StoreCode = input.StoreCode,
                    FromDate = input.FromDate,
                    ToDate = input.ToDate,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    ProductCode = input.ProductCode,
                    CategoryCode = input.CategoryCode,
                    ServiceCode = input.ServiceCode,
                };

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                         0, new ReportCardStockImExPortDto(),
                         new List<ReportCardStockImExPortDto>(), warning: msg
                     );

                var rs = await _reportsManager.ReportCardStockImExPort(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                        0,
                        new ReportCardStockImExPortDto(),
                        new List<ReportCardStockImExPortDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();

                var sumList = rs.SumData.ConvertTo<List<ReportCardStockImExPortDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCardStockImExPortDto();

                return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCardStockImExPort error: {e}");
                return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                    0,
                    new ReportCardStockImExPortDto(),
                    new List<ReportCardStockImExPortDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportCardStockImExPortDto>> GetReportCardStockImExProvider(
            GetReportCardStockImExProviderInput input)
        {
            try
            {
                var request = new ReportCardStockImExProviderRequest
                {
                    StoreCode = input.StoreCode,
                    FromDate = input.FromDate,
                    ToDate = input.ToDate,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    ProductCode = input.ProductCode,
                    CategoryCode = input.CategoryCode,
                    ServiceCode = input.ServiceCode,
                    ProviderCode = input.ProviderCode,
                };

                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                        0, new ReportCardStockImExPortDto(),
                        new List<ReportCardStockImExPortDto>(), warning: msg
                    );

                var rs = await _reportsManager.ReportCardStockImExProvider(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                        0, new ReportCardStockImExPortDto(),
                        new List<ReportCardStockImExPortDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();

                var sumList = rs.SumData.ConvertTo<List<ReportCardStockImExPortDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportCardStockImExPortDto();

                return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCardStockImExPort error: {e}");
                return new PagedResultDtoReport<ReportCardStockImExPortDto>(
                    0,
                    new ReportCardStockImExPortDto(),
                    new List<ReportCardStockImExPortDto>());
            }
        }

        public async Task<PagedResultDtoReport<ReportCardStockInventoryDto>> GetReportCardStockInventory(
            GetReportCardStockInventoryInput input)
        {
            try
            {
                var request = new ReportCardStockInventoryRequest
                {
                    FromDate = input.FromDateFilter,
                    ToDate = input.ToDateFilter,
                    Limit = input.MaxResultCount,
                    Offset = input.SkipCount,
                    SearchType = SearchType.Search,
                    Telco = input.TelcoFilter,
                    CardValue = input.CardValueFilter,
                    StockCode = input.StockCodeFilter,
                    ProductCode = input.ProductCodeFilter
                };
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportCardStockInventoryDto>(
                      0, new ReportCardStockInventoryDto(),
                      new List<ReportCardStockInventoryDto>(), warning: msg
                  );

                var rs = await _reportsManager.ReportCardStockInventory(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportCardStockInventoryDto>(
                        0, new ReportCardStockInventoryDto(),
                        new List<ReportCardStockInventoryDto>()
                    );
                var lst = rs.Payload.ConvertTo<List<ReportCardStockInventoryDto>>();

                return new PagedResultDtoReport<ReportCardStockInventoryDto>(
                    totalCount, new ReportCardStockInventoryDto(), lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportCardStockHistories error: {e}");
                return new PagedResultDtoReport<ReportCardStockInventoryDto>(
                    0,
                    new ReportCardStockInventoryDto(),
                    new List<ReportCardStockInventoryDto>());
            }
        }


        public async Task<FileDto> GetReportCardStockHistoriesToExcel(GetReportCardStockHistoriesInput input)
        {
            var request = new ReportCardStockHistoriesRequest
            {
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                Telco = input.TelcoFilter,
                CardValue = input.CardValueFilter,
                StockCode = input.StockCodeFilter,
                ProductCode = input.ProductCodeFilter
            };

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCardStockHistories(request);
            var lst = rs.Payload.ConvertTo<List<ReportCardStockHistoriesDto>>();
            return _excelExporter.ReportCardStockHistoriesToFile(lst);
        }

        public async Task<FileDto> GetReportCardStockImExPortToExcel(GetReportCardStockImExPortInput input)
        {
            var request = new ReportCardStockImExPortRequest
            {
                StoreCode = input.StoreCode,
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                CategoryCode = input.CategoryCode,
                ServiceCode = input.ServiceCode,
                ProductCode = input.ProductCode
            };

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCardStockImExPort(request);
            var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();
            return _excelExporter.ReportCardStockImExPortToFile(lst);
        }

        public async Task<FileDto> GetReportCardStockImExProviderToExcel(GetReportCardStockImExProviderInput input)
        {
            var request = new ReportCardStockImExProviderRequest
            {
                StoreCode = input.StoreCode,
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                CategoryCode = input.CategoryCode,
                ServiceCode = input.ServiceCode,
                ProductCode = input.ProductCode,
                ProviderCode = input.ProviderCode,
            };

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCardStockImExProvider(request);
            var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();
            return _excelExporter.ReportCardStockImExPortToFile(lst);
        }

        public async Task<FileDto> GetReportCardStockInventoryToExcel(GetReportCardStockInventoryInput input)
        {
            var request = new ReportCardStockInventoryRequest()
            {
                FromDate = input.FromDateFilter,
                ToDate = input.ToDateFilter,
                Limit = int.MaxValue,
                Offset = 0,
                SearchType = SearchType.Export,
                Telco = input.TelcoFilter,
                CardValue = input.CardValueFilter,
                StockCode = input.StockCodeFilter,
                ProductCode = input.ProductCodeFilter
            };

            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDateFilter ?? DateTime.Now,
                ToDate = input.ToDateFilter ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportCardStockInventory(request);
            var lst = rs.Payload.ConvertTo<List<ReportCardStockInventoryDto>>();
            return _excelExporter.ReportCardStockInventoryToFile(lst);
        }

        #endregion

        public async Task<PagedResultDtoReport<ReportRevenueCommistionDashDay>> GetReportAgentGeneralCommistionDashList(
           GetDashAgentGeneralboardInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportAgentGeneralDashRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportRevenueCommistionDashDay>(
                      0, new ReportRevenueCommistionDashDay(),
                      new List<ReportRevenueCommistionDashDay>(), warning: msg
                  );

                var rs = await _reportsManager.ReportAgentGeneralDashDayList(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportRevenueCommistionDashDay>(0, new ReportRevenueCommistionDashDay(),
                        new List<ReportRevenueCommistionDashDay>());

                var lst = rs.Payload.ConvertTo<List<ReportRevenueCommistionDashDay>>();

                var sumList = rs.SumData.ConvertTo<List<ReportRevenueCommistionDashDay>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportRevenueCommistionDashDay();
                return new PagedResultDtoReport<ReportRevenueCommistionDashDay>(
                    totalCount,
                    totalData: sumData,
                    lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportAgentGeneralCommistionDashList error: {e}");
                return new PagedResultDtoReport<ReportRevenueCommistionDashDay>(
                    0,
                    new ReportRevenueCommistionDashDay(),
                    new List<ReportRevenueCommistionDashDay>());
            }
        }

        public async Task<FileDto> GetReportAgentGeneralCommistionListToExcel(GetDashAgentGeneralboardInput input)
        {
            var request = input.ConvertTo<ReportAgentGeneralDashRequest>();
            request.Limit = int.MaxValue;
            request.Offset = 0;
            request.SearchType = SearchType.Search;
            request.AccountType = (int)_session.AccountType;
            request.LoginCode = _session.AccountCode;
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Total",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportAgentGeneralDashDayList(request);
            var lst = rs.Payload.ConvertTo<List<ReportRevenueCommistionDashDay>>();

            return _excelExporter.ExportAgentGeneralCommistionDashListExportToFile(lst);
        }

        public async Task<DashAgentGenerals> GetDashAgentGeneralCommistion(GetDashAgentGeneralInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportAgentGeneralDashRequest>();
                request.Limit = int.MaxValue;
                request.Offset = 0;
                request.AccountType = (int)_session.AccountType;
                request.LoginCode = _session.AccountCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = request.FromDate ?? DateTime.Now,
                    ToDate = request.ToDate ?? DateTime.Now,
                    ReportType = "Total",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new DashAgentGenerals()
                    {
                        Commistions = new List<DashRevenueItem>(),
                        Revenues = new List<DashRevenueItem>(),
                        Length = 0,
                    };

                var rs = await _reportsManager.ReportAgentGeneralDashDayList(request);
                var totalCount = rs.Total;
                if (rs.ResponseCode != "01")
                    return new DashAgentGenerals()
                    {
                        Commistions = new List<DashRevenueItem>(),
                        Revenues = new List<DashRevenueItem>(),
                        Length = 0,
                    };

                var lst = rs.Payload.ConvertTo<List<ReportRevenueCommistionDashDay>>();
                var litRevenues = (from item in lst.OrderBy(c => c.CreatedDay)
                                   select new DashRevenueItem
                                   {
                                       y = item.Revenue,
                                       indexLabel = item.Revenue < 1000000
                                           ? Math.Round(item.Revenue / 1000, 0) + "k"
                                           : Math.Round(item.Revenue / 1000000, 1) + "tr",
                                       label = item.CreatedDay.ToString("dd/MM"),
                                   }).ToList();

                var litCommistions = (from item in lst.OrderBy(c => c.CreatedDay)
                                      select new DashRevenueItem
                                      {
                                          y = item.Commission,
                                          indexLabel = item.Commission < 1000000
                                              ? Math.Round(item.Commission / 1000, 0) + "k"
                                              : Math.Round(item.Commission / 1000000, 1) + "tr",
                                          label = item.CreatedDay.ToString("dd/MM"),
                                      }).ToList();

                foreach (var item in litRevenues)
                {
                    if (item.y > 0)
                        item.toolTipContent =
                            $"{item.label}: {item.indexLabel} - {Convert.ToDouble(item.y).ToString("N0")}";
                }

                foreach (var item in litCommistions)
                {
                    if (item.y > 0)
                        item.toolTipContent =
                            $"{item.label}: {item.indexLabel} - {Convert.ToDouble(item.y).ToString("N0")}";
                }

                return new DashAgentGenerals()
                {
                    Commistions = litCommistions,
                    Revenues = litRevenues,
                    Length = 1,
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"GetDashboardAgentGeneral error: {e}");
                return new DashAgentGenerals()
                {
                    Commistions = new List<DashRevenueItem>(),
                    Revenues = new List<DashRevenueItem>(),
                    Length = 0,
                };
            }
        }

        public async Task<PagedResultDtoReport<ReportTopupRequestLogDto>> GetReportTopupRequestLogList(
    GetReportTopupRequestDetailInput input)
        {
            try
            {
                var request = input.ConvertTo<ReportTopupRequestLogRequest>();
                request.Offset = input.SkipCount;
                request.Limit = input.MaxResultCount;
                request.TransCode = input.TransCode;
                request.TransRef = input.RequestRef;
                request.ProviderCode = input.VenderCode;
                string msg = string.Empty;
                if (!IsValidateSearch(new ReportComparePartnerExportInfo.ValidateSearchInput()
                {
                    FromDate = input.FromDate ?? DateTime.Now,
                    ToDate = input.ToDate ?? DateTime.Now,
                    ReportType = "Detail",
                    Type = SearchType.Search.ToString()
                }, ref msg))
                    return new PagedResultDtoReport<ReportTopupRequestLogDto>(0,
                    new ReportTopupRequestLogDto(),
                    new List<ReportTopupRequestLogDto>(), warning: msg);

                var rs = await _reportsManager.ReportTopupRequestLogList(request);
                var totalCount = rs.Total;
                var sumList = rs.SumData.ConvertTo<List<ReportTopupRequestLogDto>>();
                var sumData = sumList != null && sumList.Count >= 1 ? sumList[0] : new ReportTopupRequestLogDto();
                if (rs.ResponseCode != "01")
                    return new PagedResultDtoReport<ReportTopupRequestLogDto>(0, new ReportTopupRequestLogDto(),
                        new List<ReportTopupRequestLogDto>());

                var lst = rs.Payload.ConvertTo<List<ReportTopupRequestLogDto>>();

                return new PagedResultDtoReport<ReportTopupRequestLogDto>(
                    totalCount, totalData: sumData, lst
                );
            }
            catch (Exception e)
            {
                _logger.LogError($"GetReportTopupRequestLogList error: {e}");
                return new PagedResultDtoReport<ReportTopupRequestLogDto>(0,
                    new ReportTopupRequestLogDto(),
                    new List<ReportTopupRequestLogDto>());
            }
        }

        public async Task<FileDto> GetReportTopupRequestLogListToExcel(GetReportTopupRequestDetailInput input)
        {
            var request = input.ConvertTo<ReportTopupRequestLogRequest>();
            request.Limit = 1;
            request.Offset = 0;
            request.ProviderCode = input.VenderCode;
            request.TransCode = input.TransCode;
            request.TransRef = input.RequestRef;
            request.Limit = int.MaxValue;
            request.SearchType = SearchType.Export;
            request.File = "EXCEL";
            FileDto data = null;
            if (!IsValidateExport(new ReportComparePartnerExportInfo.ValidateSearchInput()
            {
                FromDate = input.FromDate ?? DateTime.Now,
                ToDate = input.ToDate ?? DateTime.Now,
                ReportType = "Detail",
                Type = SearchType.Export.ToString()
            }, ref data))
                return data;

            var rs = await _reportsManager.ReportTopupRequestLogList(request);
            if (rs.ExtraInfo == "Downloadlink")
            {
                return new FileDto()
                {
                    FileName = rs.ExtraInfo,
                    FilePath = rs.ResponseMessage,
                };
            }
            else
            {
                var lst = rs.Payload.ConvertTo<List<ReportTopupRequestLogDto>>();
                return _excelExporter.ReportTopupRequestLogExportToFile(lst);
            }
        }
    }
}
