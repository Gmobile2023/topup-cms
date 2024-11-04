using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Hangfire;
using HLS.Topup.Common;
using HLS.Topup.Compare;
using HLS.Topup.Dto;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Transactions.Dtos;
using HLS.Topup.Transactions.Exporting;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Transactions
{
    public partial class TransactionsAppService
    {
        public async Task<FileDto> GetTransactionHistoryToExcel(GetAllTopupRequestsForExcelInput input)
        {
            var accountInfo = GetAccountInfo();
            if (!input.IsAdmin)
            {
                input.PartnerCodeFilter = accountInfo.NetworkInfo.AccountCode;
                input.StaffAccount = accountInfo.UserInfo.AccountCode;
                input.StaffUser = accountInfo.UserInfo.UserName;
            }


            if (input.ServiceCodes != null)
                input.ServiceCodes = input.ServiceCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
            else input.ServiceCodes = new List<string>();

            if (input.CategoryCodes != null)
                input.CategoryCodes = input.CategoryCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
            else input.CategoryCodes = new List<string>();

            if (input.ProductCodes != null)
                input.ProductCodes = input.ProductCodes.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
            else input.ProductCodes = new List<string>();


            input.ItemPerPageConfig = int.Parse(_appConfiguration["App:ExportPerPage"]);
            input.TenantId = AbpSession.TenantId;
            BackgroundJob.Enqueue<ExportTransactionManagementJob>((x) =>
                x.Execute(new ExportTransactionManagementJobArgs
                {
                    Request = input,
                    User = AbpSession.ToUserIdentifier(),
                    AccountInfo = accountInfo
                }));
            return null;
        }


        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task ProcessSyncStatusTransactionJob(UserIdentifier user, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    await _appNotifierNow.SendMessageAsync(user, "Dữ liệu không hợp lệ.",
                   Abp.Notifications.NotificationSeverity.Error);
                }
                var list = data.FromJson<List<TransItemDto>>();
                var status = new[] { "1", "2", "3", "4", "6", "7", "8", "9", "20" };
                foreach (var item in list)
                {
                    if (!status.Contains(item.Status))
                        continue;

                    try
                    {
                        if (item.Status != "20")
                        {
                            await UpdateStatus(new RequestDtos.TopupsUpdateStatusRequest()
                            {
                                TransCode = item.TransCode,
                                Status = Convert.ToByte(item.Status)
                            });
                        }
                        else
                        {
                            var transaction = GetTransactionByCode(item.TransCode).Result;
                            if (transaction != null)
                            {
                                if (transaction.Status == CommonConst.TopupStatus.Paid
                                    || transaction.Status == CommonConst.TopupStatus.WaitForResult
                                    || transaction.Status == CommonConst.TopupStatus.InProcessing
                                    || transaction.Status == CommonConst.TopupStatus.ProcessTimeout)
                                {
                                    await RefundTransactionRequest(new Topup.Dtos.RefundTransDto()
                                    {
                                        TransCode = item.TransCode,
                                        TransRef = transaction.TransRef,
                                        PartnerCode = transaction.PartnerCode,
                                        PaymentAmount = transaction.PaymentAmount
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        _logger.LogError($"TransCode= {item.TransCode}|Status= {item.Status}|ProcessSyncStatusTransactionJob_Exception= {exx.Message}|{exx.InnerException}|{exx.StackTrace}");
                    }
                }

                await _appNotifierNow.SendMessageAsync(user, "Chuyển đổi trạng thái giao dịch bằng file hoàn tất.",
                   Abp.Notifications.NotificationSeverity.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcessSyncStatusTransactionJob_Exception= {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                await _appNotifierNow.SendMessageAsync(user, "Xử lý cập nhật trạng thái trong file thất bại.",
                  Abp.Notifications.NotificationSeverity.Error);
            }
        }
    }
}
