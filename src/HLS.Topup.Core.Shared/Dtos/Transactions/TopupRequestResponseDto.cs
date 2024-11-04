using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Transactions
{
    public class TopupRequestResponseDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string ReceiverInfo { get; set; }
        public byte TopupType { get; set; }
        public int Timeout { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public string StatusName { get; set; }
        public CommonConst.TopupStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Telco { get; set; }
        public DateTime EndProcessTime { get; set; }
        public DateTime? RequestDate { get; set; }
        public string PartnerCode { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public string ProviderTransCode { get; set; } //Mã ncc
        public string ProductCode { get; set; }
        public string ProductProvider { get; set; }
        public string PaymentSms { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ShortCode { get; set; }
        public string TopupCommand { get; set; }
        public string PaymentTransCode { get; set; }
        public decimal ProcessedAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal CancelAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FixAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CategoryCode { get; set; }
        public decimal Quantity { get; set; }
        public string Email { get; set; }
        public int PriorityDiscountRate { get; set; } //Phần trăm chiết khấu nhập vào để đua giá
        public int Multiples { get; set; } //Bội số. Cái này hiện chưa dùng
        public decimal PriorityFee { get; set; } //Phí ưu tiên
        public bool IsPriority => CanBePriority();
        public string Provider { get; set; }
        public string WorkerApp { get; set; }
        public string StaffAccount { get; set; }

        public CommonConst.AgentType AgentType { get; set; }
        public CommonConst.SaleType SaleType { get; set; }
        public string AgentTypeName { get; set; }
        public DateTime? ResponseDate { get; set; }

        public string ReceiverType { get; set; }

        public bool CanBePriority()
        {
            return (ServiceCode == CommonConst.ServiceCodes.TKC || ServiceCode == CommonConst.ServiceCodes.TOPUP) &&
                   (Status == CommonConst.TopupStatus.Init || Status == CommonConst.TopupStatus.Paid) &&
                   DiscountRate > 0;
        }

        public decimal ItemAmount => Amount / (Quantity == 0 ? 1 : Quantity);

        /// <summary>
        /// client use
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// client use
        /// </summary>
        public string ProviderCode { get; set; }

        /// <summary>
        /// client use
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// client use
        /// </summary>
        public string ProductName { get; set; }

        public string ExtraInfo { get; set; }
        public InvoicePayBillDto Invoice { get; set; }

        public string CustomerSupportNote { get; set; }

        public string UserManualNote { get; set; }

        public decimal? Fee { get; set; }

        public string Description { get; set; }
        public int TotalTime { get; set; }

        public string ProviderResponseCode { get; set; }
        public string ReceiverTypeResponse { get; set; }
        public string ParentProvider { get; set; }
        
    }


    public class InvoicePayBillDto
    {
        public DateTime CreatedTime { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }

        //Max KH
        public string CustomerReference { get; set; }

        public string Address { get; set; }

        //Kỳ thanh toán
        public string Period { get; set; }

        public string PhoneNumber { get; set; }

        //Mã giao dịch (TransCode core)
        public string TransCode { get; set; }

        //Mã giao dịch đối tác
        public string TransRef { get; set; }
        public string Description { get; set; }
        public string ExtraInfo { get; set; }
    }


    public class TransactionResponseDtoApp: TransactionResponseBase
    {
        public int Quantity { get; set; }
        public decimal ItemAmount => Amount / (Quantity == 0 ? 1 : Quantity);
    }

    public class TransactionResponseBase
    {
        public string TransType { get; set; }
        public string ReceiverInfo { get; set; }
        public decimal Amount { get; set; }
        public string StatusName { get; set; }

        public byte Status { get; set; }
        public DateTime CreatedTime { get; set; }


        public string Telco { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
       
        public string ProviderName { get; set; }
        public string ProviderCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Fee { get; set; }
        public string ExtraInfo { get; set; }
        public string Description { get; set; }

        // thanh toán hóa đơn => thông tin truy vẫn
        public InvoicePayBillDto Invoice { get; set; }

        // bank nạp tiền
        public BankResponseDto BankDto { get; set; }

        // chi tiết chuyển tiền
        public TransferInfo TransferInfo { get; set; }

        // người thực hiện
        public string StaffAccount { get; set; }
        public string StaffFullName { get; set; }

        public string StaffPhoneNumber { get; set; }

        // print -  print
        public string CustomerSupportNote { get; set; }
        public string UserManualNote { get; set; }

        public DateTime? SrcCreatedTime { get; set; }
        public string SrcAccountCode { get; set; }
        public string SrcFullName { get; set; }
        public string SrcPhoneNumber { get; set; }
    }


    public class TransactionResponseDto: TransactionResponseBase
    {
        public decimal Quantity { get; set; }
        public decimal ItemAmount => Amount / (Quantity == 0 ? 1 : Quantity);
    }

    public class BankResponseDto
    {
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountCode { get; set; }
    }

    public class TransferInfo
    {
        public string SrcAccountCode { get; set; }
        public string SrcFullName { get; set; }
        public string SrcPhoneNumber { get; set; }
        public string DesAccountCode { get; set; }
        public string DesFullName { get; set; }
        public string DesPhoneNumber { get; set; }
    }
}
