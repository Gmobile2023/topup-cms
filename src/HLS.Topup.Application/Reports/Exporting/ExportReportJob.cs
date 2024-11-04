using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Abp;
using Abp.AspNetZeroCore.Net;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Threading;
using Aspose.Cells;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.Dto;
using HLS.Topup.Notifications;
using HLS.Topup.Report;
using HLS.Topup.Reports.Dtos;
using HLS.Topup.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Reports.Exporting
{
    public class ExportReportJob : BackgroundJob<ExportReportJobArgs>,
        ITransientDependency
    {
        private readonly IAppNotifier _appNotifier;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILogger<ExportReportJob> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IReportsManager _reportsManager;
        private readonly IConfigurationRoot _appConfiguration;

        public ExportReportJob(
            IAppNotifier appNotifier,
            IUnitOfWorkManager unitOfWorkManager,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            ILogger<ExportReportJob> logger,
            IWebHostEnvironment hostingEnvironment,
            IReportsManager reportsManager)
        {
            _appNotifier = appNotifier;
            _unitOfWorkManager = unitOfWorkManager;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _logger = logger;
            _reportsManager = reportsManager;
            _hostingEnvironment = hostingEnvironment;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        public override void Execute(ExportReportJobArgs args)
        {
            switch (args.ReportType)
            {
                case ExportReportJobArgs.Type_BalanceAccount:
                    ExportBalanceDetail(args);
                    break;
                case ExportReportJobArgs.Type_BalanceAccounts:
                    ExportBalanceTotal(args);
                    break;
                case ExportReportJobArgs.Type_TotalBalance:
                    ExportBalanceGroup(args);
                    break;
                case ExportReportJobArgs.Type_CardStockHistories:
                    ExportCardStockHistories(args);
                    break;
                case ExportReportJobArgs.Type_CardStockImExPort:
                    ExportCardStockImExPort(args);
                    break;
                case ExportReportJobArgs.Type_CardStockImExProvider:
                    ExportCardStockImExProvider(args);
                    break;
                case ExportReportJobArgs.Type_CardStockInventory:
                    ExportCardStockInventory(args);
                    break;
                case ExportReportJobArgs.Type_ReportAgentBalance:
                    ExportAgentBalance(args);
                    break;
                case ExportReportJobArgs.Type_AccountDebtDetail:
                    ExportDebtDetail(args);
                    break;
                case ExportReportJobArgs.Type_TotalDebtBalance:
                    ExportTotalDebt(args);
                    break;
                case ExportReportJobArgs.Type_ReportTransferDetail:
                    ExportTransferDetail(args);
                    break;
                case ExportReportJobArgs.Type_ReportServiceDetail:
                    ExportServiceDetail(args);
                    break;
                case ExportReportJobArgs.Type_ReportServiceTotal:
                    ExportServiceTotal(args);
                    break;
                case ExportReportJobArgs.Type_ReportRefundDetail:
                    ExportRefundDetail(args);
                    break;
                case ExportReportJobArgs.Type_ReportRevenueAgent:
                    ExportRevenueAgent(args);
                    break;
                case ExportReportJobArgs.Type_ReportRevenueCity:
                    ExportRevenueCity(args);
                    break;
                case ExportReportJobArgs.Type_ReportTotalSaleAgent:
                    ExportTotalSaleAgent(args);
                    break;
                case ExportReportJobArgs.Type_ReportRevenueActive:
                    ExportAgentActive(args);
                    break;
                case ExportReportJobArgs.Type_ReportTransDetail:
                    ExportTransDetail(args);
                    break;
            }

        }

        /// <summary>
        /// Chi tiết số dư
        /// </summary>
        /// <param name="input"></param>
        private void ExportBalanceDetail(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportBalanceDetail.xlsx";
                string fileName = "Chi tiet so du tai khoan.xlsx";
                var request = input.Request.FromJson<ReportDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportDetailGetRequest(request));
                var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }

                foreach (var item in lst)
                {
                    if (request.AccountCode == item.SrcAccountCode)
                    {
                        item.Decrement = item.Amount;
                        item.BalanceAfter = item.SrcAccountBalanceAfterTrans;
                        item.BalanceBefore = item.SrcAccountBalanceBeforeTrans;
                    }

                    if (request.AccountCode == item.DesAccountCode)
                    {
                        item.Increment = item.Amount;
                        item.BalanceAfter = item.DesAccountBalanceAfterTrans;
                        item.BalanceBefore = item.DesAccountBalanceBeforeTrans;
                    }

                    item.ServiceName = getServiceName(item.ServiceCode, item.TransType == CommonConst.TransactionType.CancelPayment ?
                        "REFUND" : item.ServiceCode, request.AccountCode, item.DesAccountCode, item.SrcAccountCode);

                    if (item.TransType == CommonConst.TransactionType.Transfer)
                    {
                        if (request.AccountCode == item.SrcAccountCode)
                            item.TransNote =
                                $"Chuyển tiền tới tài khoản {item.DesAccountCode}. Nội dung: {item.Description}";
                        if (request.AccountCode == item.DesAccountCode)
                            item.TransNote = $"Nhận tiền từ tài khoản {item.SrcAccountCode}. Nội dung: {item.Description}";
                    }
                }

                var file = GetFileExcel<ReportDetailDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportBalanceTotal(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportServiceDetail.xlsx";
                string fileName = "Tong hop so du tai khoan.xlsx";
                var request = input.Request.FromJson<ReportDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportDetailGetRequest(request));
                var lst = rs.Payload.ConvertTo<List<ReportDetailDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportDetailDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportBalanceGroup(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportServiceDetail.xlsx";
                string fileName = "Tong hop so du tren he thong.xlsx";
                var request = input.Request.FromJson<BalanceGroupTotalRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.BalanceGroupTotalGetRequest(request));
                var lst = rs.Payload.ConvertTo<List<ReportGroupDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportGroupDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportCardStockHistories(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportServiceDetail.xlsx";
                string fileName = "Chi tiet ton kho the.xlsx";
                var request = input.Request.FromJson<ReportCardStockHistoriesRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportCardStockHistories(request));
                var lst = rs.Payload.ConvertTo<List<ReportCardStockHistoriesDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportCardStockHistoriesDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportCardStockImExPort(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportCardStockImExPort.xlsx";
                string fileName = "Baocao_NXT_Khothe.xlsx";
                var request = input.Request.FromJson<ReportCardStockImExPortRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportCardStockImExPort(request));
                var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportCardStockImExPortDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportCardStockImExProvider(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportCardStockImExProvider.xlsx";
                string fileName = "Baocao_NXT_Mathe_TheoNCC.xlsx";
                var request = input.Request.FromJson<ReportCardStockImExProviderRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportCardStockImExProvider(request));
                var lst = rs.Payload.ConvertTo<List<ReportCardStockImExPortDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportCardStockImExPortDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportCardStockInventory(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportCardStockInventory.xlsx";
                string fileName = "BaoCao_Tonkho_Mathe.xlsx";
                var request = input.Request.FromJson<ReportCardStockInventoryRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportCardStockInventory(request));
                var lst = rs.Payload.ConvertTo<List<ReportCardStockInventoryDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportCardStockInventoryDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportAgentBalance(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportAgentBalance.xlsx";
                string fileName = "Nhapxuatton_Tiennap.xlsx";
                var request = input.Request.FromJson<ReportAgentBalanceRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportAgentBalanceReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportAgentBalanceDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportAgentBalanceDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportDebtDetail(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportDebtDetail.xlsx";
                string fileName = "Baocao_Chitiet_Congno.xlsx";
                var request = input.Request.ConvertTo<ReportDebtDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportDebtDetailReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportDebtDetailDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportDebtDetailDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportTotalDebt(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportTotalDebt.xlsx";
                string fileName = "Tonghop_Congno.xlsx";
                var request = input.Request.FromJson<ReportTotalDebtRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportTotalDebtReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportItemTotalDebt>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportItemTotalDebt>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportTransferDetail(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportTransferDetail.xlsx";
                string fileName = "BaoCao_Chuyentien_Daily.xlsx";
                var request = input.Request.FromJson<ReportTransferDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportTransferDetailReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportTransferDetailDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportTransferDetailDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file),                    
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportServiceDetail(ExportReportJobArgs input)
        {
            try
             {
                var request = input.Request.FromJson<ReportServiceDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportServiceDetailReport(request));
                //string link = "https://ftp.daily.nhattran.com.vn/Uploads/ReportFiles/20211119//c4a79c8b-8819-40fd-b7cb-21a072ec3437.xlsx";
                AsyncHelper.RunSync(() => _appNotifier.GdUrllinkDownload(input.User,  rs.ExtraInfo));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportServiceTotal(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportServiceTotal.xlsx";
                string fileName = "Baocao_Tonghop_Xuatban_TheoSanpham.xlsx";
                var request = input.Request.FromJson<ReportServiceTotalRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportServiceTotalReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportServiceTotalDto>>();


                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportServiceTotalDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private void ExportRefundDetail(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportRefundDetail.xlsx";
                string fileName = "BaoCao_Chitiet_Hoantien.xlsx";
                var request = input.Request.FromJson<ReportRefundDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportRefundDetailReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportRefundDetailDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportRefundDetailDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }


        private void ExportRevenueAgent(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportRevenueAgent.xlsx";
                string fileName = "BaoCao_Doanhso_TheoDaiLy.xlsx";
                var request = input.Request.FromJson<ReportRevenueAgentRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportRevenueAgentReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportRevenueAgentDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportRevenueAgentDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }


        private void ExportRevenueCity(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportRevenueCity.xlsx";
                string fileName = "BaoCao_Doanhso_TheoTinh_TP.xlsx";
                var request = input.Request.FromJson<ReportRevenueCityRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportRevenueCityReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportRevenueCityDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportRevenueCityDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }


        private void ExportTotalSaleAgent(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportTotalSaleAgent.xlsx";
                string fileName = "BaoCao_Tonghop_Banthe_TheoDaily.xlsx";
                var request = input.Request.FromJson<ReportTotalSaleAgentRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportTotalSaleAgentReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportTotalSaleAgentDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportTotalSaleAgentDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }


        private void ExportAgentActive(ExportReportJobArgs input)
        {
            try
            {
                string fileSampleName = "ExportReportAgentActive.xlsx";
                string fileName = "BaoCao_DaiLy_Kichhoat.xlsx";
                var request = input.Request.FromJson<ReportRevenueActiveRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportRevenueActiveReport(request));
                var lst = rs.Payload.ConvertTo<List<ReportRevenueActiveDto>>();

                _logger.LogInformation($"Process get Excel file");
                var index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.Index = index;
                }
                var file = GetFileExcel<ReportRevenueActiveDto>(lst, fileSampleName, fileName);
                if (file == null)
                {
                    AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                        input.User,
                        new LocalizableString("ExportFileNotSuccess", TopupConsts.LocalizationSourceName),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn));
                    return;
                }

                _logger.LogInformation($"Get Excel file success");
                var zipFile = new BinaryObject
                {
                    TenantId = input.TenantId,
                    Bytes = CompressFile(file)
                };
                _logger.LogInformation($"Zip Excel file success");
                // Save zip file to object manager.
                AsyncHelper.RunSync(() => _binaryObjectManager.SaveAsync(zipFile));
                // Send notification to user.
                AsyncHelper.RunSync(() => _appNotifier.GdprDataPrepared(input.User, zipFile.Id));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }


        private void ExportTransDetail(ExportReportJobArgs input)
        {
            try
            {
                var request = input.Request.FromJson<ReporttransDetailRequest>();
                var rs = AsyncHelper.RunSync(() => _reportsManager.ReportTransDetailReport(request));                
                AsyncHelper.RunSync(() => _appNotifier.GdUrllinkDownload(input.User, rs.ExtraInfo));
                _logger.LogInformation($"Export file success");
            }
            catch (Exception e)
            {
                _logger.LogError($"Export error:{e}");
            }
        }

        private byte[] CompressFile(FileDto file)
        {
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
                    var entry = zipStream.CreateEntry(file.FileName);

                    using (var originalFileStream = new MemoryStream(fileBytes))
                    using (var zipEntryStream = entry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                    }
                }

                return outputZipFileStream.ToArray();
            }
        }

        private FileDto GetFileExcel<T>(List<T> list, string fileSampleName, string fileName)
        {
            try
            {
                var lic = new License();
                lic.SetLicense("Aspose_total_20220516.lic");
                var designer = new WorkbookDesigner();
                var path = Path.Combine(_hostingEnvironment.WebRootPath,
                    $"assets/SampleFiles/{fileSampleName}");
                designer.Workbook = new Workbook(path);
                //var i = 1;
                designer.SetDataSource("Export", list.ToList());
                var workbook = designer.Workbook;
                designer.Process(false);
                var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
                using (var memoryStream = new MemoryStream())
                {
                    workbook.Save(memoryStream, SaveFormat.Xlsx);
                    _tempFileCacheManager.SetFile(file.FileToken, memoryStream.ToArray());
                }
                return file;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private string getServiceName(string serviceCode, string transType, string accountCodeLogin, string desAccountCode, string srcAccountCode)
        {
            try
            {
                return (serviceCode == CommonConst.ServiceCodes.REFUND || transType == CommonConst.ServiceCodes.REFUND)
                            ? "Hoàn tiền"
                            : serviceCode == CommonConst.ServiceCodes.TOPUP
                                ? "Nạp tiền điện thoại"
                                : serviceCode == CommonConst.ServiceCodes.TOPUP_DATA
                                    ? "Nạp data"
                                    : serviceCode == CommonConst.ServiceCodes.PAY_BILL
                                        ? "Thanh toán hóa đơn"
                                        : serviceCode == CommonConst.ServiceCodes.PIN_DATA
                                            ? "Mua thẻ Data"
                                            : serviceCode == CommonConst.ServiceCodes.PIN_GAME
                                                ? "Mua thẻ Game"
                                                : serviceCode == CommonConst.ServiceCodes.PIN_CODE
                                                    ? "Mua mã thẻ"
                                                    : serviceCode == "CORRECT_UP"
                                                        ? "Điều chỉnh tăng"
                                                        : serviceCode == "CORRECT_DOWN"
                                                            ? "Điều chỉnh giảm"
                                                            : serviceCode == CommonConst.ServiceCodes.PAYBATCH
                                                                ? "Trả thưởng"
                                                               : serviceCode == CommonConst.ServiceCodes.PAYCOMMISSION
                                                                ? "Hoa hồng"
                                                                : serviceCode == "TRANSFER" &&
                                                                 accountCodeLogin == desAccountCode
                                                                    ? "Nhận tiền đại lý"
                                                                    : serviceCode == "TRANSFER" &&
                                                                      accountCodeLogin == srcAccountCode
                                                                        ? "Chuyển tiền đại lý"
                                                                        : serviceCode == CommonConst.ServiceCodes.DEPOSIT
                                                                            ? "Nạp tiền"
                                                                            : "";
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }
        }
    }

    public class ExportReportJobArgs
    {
        public const string Type_BalanceAccount = "BalanceAccount";
        public const string Type_BalanceAccounts = "BalanceAccounts";
        public const string Type_TotalBalance = "TotalBalance";

        public const string Type_CardStockHistories = "CardStockHistories";
        public const string Type_CardStockImExPort = "CardStockImExPort";
        public const string Type_CardStockImExProvider = "CardStockImExProvider";
        public const string Type_CardStockInventory = "CardStockInventory";
        public const string Type_ReportAgentBalance = "ReportAgentBalance";
        public const string Type_AccountDebtDetail = "AccountDebtDetail";
        public const string Type_TotalDebtBalance = "TotalDebtBalance";
        public const string Type_ReportTransferDetail = "ReportTransferDetail";

        public const string Type_ReportServiceDetail = "ReportServiceDetail";
        public const string Type_ReportServiceTotal = "ReportServiceTotal";
        public const string Type_ReportRefundDetail = "ReportRefundDetail";

        public const string Type_ReportRevenueAgent = "ReportRevenueAgent";
        public const string Type_ReportRevenueCity = "ReportRevenueCity";
        public const string Type_ReportTotalSaleAgent = "ReportTotalSaleAgent";
        public const string Type_ReportRevenueActive = "ReportRevenueActive";


        public const string Type_ReportTransDetail = "ReportTransDetail";

        public string Request { get; set; }
        public UserIdentifier User { get; set; }
        public int? TenantId { get; set; }

        public string ReportType { get; set; }
    }
}
