using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using Abp.Web.Models;
using HLS.Topup.Authorization;
using HLS.Topup.Common;
using HLS.Topup.RequestDtos;
using HLS.Topup.Topup.Dtos;
using HLS.Topup.Transactions.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Transactions
{
    //Service này cho các thằng đối tác như Ba Khía gọi
    public partial class TransactionsAppService
    {
        [AbpAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<NewMessageReponseBase<string>> Topup(CreateOrEditTopupRequestDto input)
        {
            if (string.IsNullOrEmpty(input.TransCode))
                throw new UserFriendlyException("Vui lòng nhập mã giao dịch");
            await CheckAccountIsNetwork(AbpSession.UserId ?? 0, input.PartnerCode, input.ProductCode,
                input.CategoryCode);
            var request = new TopupRequest
            {
                Amount = input.Amount,
                ReceiverInfo = input.PhoneNumber,
                ProductCode = input.ProductCode,
                ServiceCode = CommonConst.ServiceCodes.TOPUP,
                CategoryCode = input.CategoryCode,
                PartnerCode = input.PartnerCode,
                StaffAccount = input.PartnerCode,
                StaffUser = input.PartnerCode,
                TransCode = input.TransCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
            return await _transactionManager.ProcessTopupRequest(request);
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<NewMessageReponseBase<string>> PinCode(CreateOrEditPinCodeRequestDto input)
        {
            if (string.IsNullOrEmpty(input.TransCode))
                throw new UserFriendlyException("Vui lòng nhập mã giao dịch");
            await CheckAccountIsNetwork(AbpSession.UserId ?? 0, input.PartnerCode, input.ProductCode,
                input.CategoryCode);
            var request = new CardSaleRequest
            {
                CategoryCode = input.CategoryCode,
                PartnerCode = input.PartnerCode,
                StaffAccount = input.PartnerCode,
                StaffUser = input.PartnerCode,
                Quantity = input.Quantity,
                CardValue = input.Amount,
                Email = input.Email,
                ProductCode = input.ProductCode,
                ServiceCode = input.ServiceCode,
                TransCode = input.TransCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
           return await _transactionManager.ProcessPinCodeRequest(request);
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<BillPaymentInfoDto> BillQuery(BillQueryRequest input)
        {
            await CheckAccountIsNetwork(AbpSession.UserId ?? 0, input.PartnerCode, input.ProductCode,
                input.CategoryCode, true);
            input.PartnerCode = input.PartnerCode;
            input.StaffAccount = input.PartnerCode;
            input.StaffUser = input.PartnerCode;
            return await _transactionManager.ProcessBillQueryRequest(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CreatePayment)]
        public async Task<NewMessageReponseBase<string>> PayBill(PayBillRequest input)
        {
            if (string.IsNullOrEmpty(input.TransCode))
                throw new UserFriendlyException("Vui lòng nhập mã giao dịch");
            await CheckAccountIsNetwork(AbpSession.UserId ?? 0, input.PartnerCode, input.ProductCode,
                input.CategoryCode, true);
            var request = new PayBillRequest
            {
                ReceiverInfo = input.ReceiverInfo,
                Amount = input.Amount,
                CategoryCode = input.CategoryCode,
                PartnerCode = input.PartnerCode,
                StaffAccount = input.PartnerCode,
                StaffUser = input.PartnerCode,
                ProductCode = input.ProductCode,
                ServiceCode = CommonConst.ServiceCodes.PAY_BILL,
                IsSaveBill = input.IsSaveBill,
                InvoiceInfo = input.InvoiceInfo,
                TransCode = input.TransCode,
                Channel = input.Channel == 0 ? CommonConst.Channel.WEB : input.Channel
            };
            return await _transactionManager.ProcessPayBillRequest(request);

        }

        [AbpAuthorize(AppPermissions.Pages_TransferMoney)]
        public async Task<TransactionResponse> Transfer(TransferRequest input)
        {
            var rs = await TransferMoney(input);
            if (rs.ResponseCode != "01")
                throw new UserFriendlyException(int.Parse(rs.ResponseCode), rs.ResponseMessage);
            return rs;
        }

        [HttpGet]
        [DontWrapResult]
        public async Task<NewMessageReponseBase<string>> CheckTransRequest(CheckTransStatusRequest input)
        {
            return await _transactionManager.CheckTransRequest(input);
        }

        private async Task CheckAccountIsNetwork(long userParent, string accountChild, string productcode,
            string categoryCode, bool isPaybill = false)
        {
            var user = await UserManager.GetUserByAccountCodeAsync(accountChild);
            if (user == null)
                throw new UserFriendlyException("Tài khoản không tồn tại");
            if (user.ParentId == null || user.ParentId != userParent)
                throw new UserFriendlyException(
                    "Không thể thực hiện giao dịch trên tài khoản không thuộc quyền quản lý");
            if (!user.IsActive)
            {
                throw new UserFriendlyException("Tài khoản của bạn đã bị khóa");
            }

            if (string.IsNullOrEmpty(productcode) || string.IsNullOrEmpty(categoryCode))
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
    }
}
