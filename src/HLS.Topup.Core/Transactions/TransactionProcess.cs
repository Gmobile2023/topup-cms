using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Notifications;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.Transactions.Dtos;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Transactions
{
    public partial class TransactionManager
    {
        public async Task<NewMessageReponseBase<string>> ProcessTopupRequest(TopupRequest request)
        {
            var transCode = !string.IsNullOrEmpty(request.TransCode)
                ? request.TransCode
                : await _commonManger.GetIncrementCodeAsync("P");
            request.TransCode = transCode;
            var rs = await TopupRequestAsync(request);
            if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"Begin send notifi to {request.PartnerCode}");
                        var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var message = request.ServiceCode == "TOPUP_DATA"
                            ? L("Notifi_TopupDataRequest", request.ReceiverInfo, transCode,
                                request.Amount.ToFormat("đ"), date)
                            : L("Notifi_TopupRequest", request.ReceiverInfo, transCode, request.Amount.ToFormat("đ"),
                                date);
                        await _appNotifier.PublishNotification(request.PartnerCode, AppNotificationNames.Payment,
                            request.ConvertTo<SendNotificationData>(),
                            message,
                            request.ServiceCode == "TOPUP_DATA"
                                ? L("Notifi_TopupDataRequest_Title")
                                : L("Notifi_TopupRequest_Title")
                        );
                        _logger.LogInformation(
                            $"Done send notifi to {request.PartnerCode}-Message: {message}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"SendNotifi CreateTopupRequest eror:{e}");
                    }
                }).ConfigureAwait(false);
            }

            _logger.LogInformation($"CreateTopupRequest done");
            return new NewMessageReponseBase<string>()
            {
                ResponseStatus = rs.ResponseStatus,
                Results = transCode
            };
        }

        public async Task<NewMessageReponseBase<string>> ProcessPinCodeRequest(CardSaleRequest request)
        {
            var transCode = !string.IsNullOrEmpty(request.TransCode)
                ? request.TransCode
                : await _commonManger.GetIncrementCodeAsync("P");
            request.TransCode = transCode;
            var rs = await PinCodeAsync(request);
            if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"Begin send notifi to {request.PartnerCode}");
                        var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var message = L("Notifi_PinCodeRequest", L("ServiceNameByCode_" + request.ServiceCode),
                            transCode, (request.CardValue * request.Quantity).ToFormat("đ"), date);
                        await _appNotifier.PublishNotification(request.PartnerCode,
                            AppNotificationNames.Payment,
                            request.ConvertTo<SendNotificationData>(),
                            message,
                            L("Notifi_PinCodeRequest_Title")
                        );
                        _logger.LogInformation(
                            $"Done send notifi to {request.PartnerCode}-Message: {message}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"SendNotifi CreatePinCode error:{e}");
                    }
                }).ConfigureAwait(false);
            }

            return new NewMessageReponseBase<string>()
            {
                ResponseStatus = rs.ResponseStatus,
                Results = transCode
            };
        }

        public async Task<BillPaymentInfoDto> ProcessBillQueryRequest(BillQueryRequest input)
        {
            try
            {
                var mess = "Truy vấn thông tin không thành công";
                var info = new BillPaymentInfoDto();
                input.ServiceCode = CommonConst.ServiceCodes.QUERY_BILL;
                if (input.CategoryCode != CommonConst.CategoryCodeConts.MOBILE_BILL)
                {
                    input.Amount = 0; //Chỉ điện thoại trả sau mới được thanh toán nhập số tiền
                }

                var rs = await BillQueryRequestAsync(input);
                if (rs.ResponseStatus.ErrorCode != ResponseCodeConst.Success &&
                    input.CategoryCode != CommonConst.CategoryCodeConts.MOBILE_BILL)
                {
                    if (rs.ResponseStatus != null && rs.ResponseStatus.Message != null)
                        mess = rs.ResponseStatus.Message;
                    throw new UserFriendlyException(mess);
                }

                if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
                {
                    var response = rs.Results;
                    if (response != null)
                    {
                        info.Amount =
                            input.Amount > 0
                                ? input.Amount
                                : response.Amount; //Nếu nhập số tiền thanh toán thì lấy theo số tiền KH nhập
                    }
                    else
                    {
                        info.Amount = input.Amount;
                    }

                    info.FullName = response.CustomerName;
                    info.Period = response.Period;
                    info.Address = response.Address;
                    if (rs.Results.PeriodDetails != null)
                        info.PeriodDetails = rs.Results.PeriodDetails.ConvertTo<List<BillInfoPeriod>>();
                    else info.PeriodDetails = new List<BillInfoPeriod>();
                }
                else
                {
                    info.Amount = input.Amount;
                }

                if (info.Amount <= 0 && input.IsCheckAmount)
                    throw new UserFriendlyException("Tài khoản không nợ cước hoặc số tiền không hợp lệ");


                info.CustomerReference = input.ReceiverInfo;
                //Ck
                info.PaymentAmount = info.Amount;
                var discount = await _discountManger.GeProductDiscountAccount(input.ProductCode,
                    input.PartnerCode, info.Amount);
                if (discount == null)
                    return info;
                if (discount.PaymentAmount <= 0)
                    throw new UserFriendlyException("Tài khoản không nợ cước hoặc số tiền thanh toán không hợp lệ");
                info.DisountAmount = discount.DiscountAmount;
                info.PaymentAmount = discount.PaymentAmount;
                var fee = await _feeManager.GetProductFee(input.ProductCode, input.PartnerCode,
                    info.Amount);
                info.Fee = fee.FeeValue;
                info.PaymentAmount += fee.FeeValue;
                info.ProductName = discount.ProductName;
                info.ProductCode = discount.ProductCode;

                return info;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(!string.IsNullOrEmpty(e.Message)
                    ? e.Message
                    : "Truy vẫn thông tin không thành công");
            }
        }

        public async Task<NewMessageReponseBase<string>> ProcessPayBillRequest(PayBillRequest input)
        {
            if (input.Amount <= 0)
                throw new UserFriendlyException("Số tiền thanh toán không hợp lệ");
            var code = !string.IsNullOrEmpty(input.TransCode)
                ? input.TransCode
                : await _commonManger.GetIncrementCodeAsync("P");
            if (input.InvoiceInfo == null)
                throw new UserFriendlyException("Không có thông tin hóa đơn");
            if (input.IsSaveBill)
            {
                var product = await _product.FirstOrDefaultAsync(x => x.ProductCode == input.ProductCode);
                if (product == null)
                    throw new UserFriendlyException("Sản phẩm không tồn tại");
                if (input.InvoiceInfo != null)
                {
                    input.InvoiceInfo.ProductCode = product.ProductCode;
                    input.InvoiceInfo.ProductName = product.ProductName;
                }
            }

            input.TransCode = code;
            input.ExtraInfo = input.InvoiceInfo.ToJson();
            var rs = await PayBillRequestAsync(input);
            if (rs.ResponseStatus.ErrorCode == ResponseCodeConst.Success)
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"Begin send notifi to {input.PartnerCode}");
                        var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        var message = L("Notifi_PayBillRequest", input.ReceiverInfo,
                            code, input.Amount.ToFormat("đ"), date);
                        await _appNotifier.PublishNotification(input.PartnerCode,
                            AppNotificationNames.Payment, input.ConvertTo<SendNotificationData>(),
                            message,
                            L("Notifi_PayBillRequest_Title")
                        );
                        _logger.LogInformation($"Done send notifi to {input.PartnerCode}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"SendNotifi PayBillRequest error:{e}");
                    }
                }).ConfigureAwait(false);
            }

            return new NewMessageReponseBase<string>()
            {
                ResponseStatus = rs.ResponseStatus,
                Results = code
            };
        }
    }
}
