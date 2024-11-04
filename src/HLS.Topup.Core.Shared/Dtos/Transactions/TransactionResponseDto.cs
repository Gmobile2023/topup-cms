using System;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Stock;

namespace HLS.Topup.Dtos.Transactions
{
    public class TransactionReportDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string SrcAccountCode { get; set; }
        public string DesAccountCode { get; set; }
        public decimal SrcAccountBalance { get; set; }
        public decimal DesAccountBalance { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public CommonConst.TransStatus Status { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string RevertTransCode { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string TransNote { get; set; }
        public string TransactionType { get; set; }
        public CommonConst.TransactionType TransType { get; set; }
    }

    public class BalanceHistoryResponseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string SrcAccountCode { get; set; }
        public string DesAccountCode { get; set; }
        public decimal SrcAccountBalance { get; set; }
        public decimal DesAccountBalance { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public CommonConst.TransStatus Status { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string RevertTransCode { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string TransNote { get; set; }
        public string TransactionType { get; set; }
        public CommonConst.TransactionType TransType { get; set; }
    }

    public class TransactionsHistoryResponseDTO
    {
        //public Guid Id { get; set; }
        public string TransCode { get; set; }
        public CommonConst.TransStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public CommonConst.TransactionType TransType { get; set; }
        public string TransactionType { get; set; }
        public decimal RequestPrice { get; set; }
        public decimal TopupPrice { get; set; }
        public decimal CancelPrice { get; set; }
        public decimal PaymentPrice { get; set; }
    }

    public class TopupDetailResponseDTO : TopupRequestResponseDto
    { 
        public string TopupTransCode { get; set; }
        public string CardTransCode { get; set; }
        public string CardCode { get; set; }
        public string Serial { get; set; }
        public int CardValue { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string TopupTransactionType { get; set; }

    }

    public class TopupTransctionResponseDTO : TopupRequestResponseDto
    {
        public string ReceiverInfo { get; set; }
    }

    public class LevelDiscountResponseDto
    {
        public Guid Id { get; set; }
        public string TransRef { get; set; } //Mã gd topup
        public string TransCode { get; set; } //Mã LevelDiscount
        public string PartnerTransCode { get; set; }
        public string AccountCode { get; set; }
        public string TransAccount { get; set; }
        public decimal TransDiscountRate { get; set; }
        public DateTime TransDate { get; set; }
        public int TransAmount { get; set; }
        public decimal TransDiscountAmount { get; set; }
        public decimal LevelDiscountRate { get; set; }
        public decimal LevelDiscountAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public CommonConst.LevelDiscountStatus Status { get; set; }
        public int Level { get; set; }
        public int LevelDiscountPolicyId { get; set; }
        public string RefUserName { get; set; }
        public string RefPhone { get; set; }
        public string FullTransAcount => RefUserName + " - " + RefPhone;
    }

    
    public class CardVendorDTO 
    { 
        public string Code { get; set; }
        public string Name { get; set; }

    }
    
}