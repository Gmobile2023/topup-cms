using System;
using System.Collections.Generic;

namespace HLS.Topup.Dtos.Bill
{
    // public class InvoiceResponseDto
    // {
    //     public string InvoiceId { get; set; }
    //
    //
    //     public string ServiceId { get; set; }
    //
    //
    //     public string InvoiceReference { get; set; }
    //
    //
    //     public string CustomerReference { get; set; }
    //
    //
    //     public decimal Amount { get; set; }
    //
    //
    //     public string CashBackAmount { get; set; }
    //
    //
    //     public string Currency { get; set; }
    //
    //
    //     public string Info { get; set; }
    //
    //
    //     public string CreationDate { get; set; }
    //
    //
    //     public string DueDate { get; set; }
    //
    //
    //     public object PayDate { get; set; }
    //
    //
    //     public object ExpirationDate { get; set; }
    //
    //
    //     public int Status { get; set; }
    //
    //
    //     public string IsPartialPaymentAllowed { get; set; }
    //
    //
    //     public List<InvoiceAttributeDto> InvoiceAttributes { get; set; }
    //
    //
    //     public bool SetCustomerReference { get; set; }
    //
    //
    //     public bool SetCreationDate { get; set; }
    //
    //
    //     public bool SetInvoiceReference { get; set; }
    //
    //
    //     public bool SetExpirationDate { get; set; }
    //
    //
    //     public bool SetInvoiceAttributes { get; set; }
    //
    //
    //     public bool SetPayDate { get; set; }
    //
    //
    //     public bool SetCurrency { get; set; }
    //
    //
    //     public bool SetStatus { get; set; }
    //
    //
    //     public bool SetDueDate { get; set; }
    //
    //
    //     public bool SetAmount { get; set; }
    // }
    //
    // public class InvoiceAttributeDto
    // {
    //     public string InvoiceId { get; set; }
    //
    //
    //     public string InvoiceAttributeTypeId { get; set; }
    //
    //
    //     public string Value { get; set; }
    //
    //
    //     public object Created { get; set; }
    // }
    public class InvoiceBillDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        //Max KH
        public string CustomerReference { get; set; }
        public string Address { get; set; }
        //Kỳ thanh toán
        public string Period { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }
    public class InvoiceResultDto
    {
        public decimal Amount { get; set; }
        public string CustomerReference { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Period { get; set; }
        public string BillType { get; set; }

        public List<PeriodDto> PeriodDetails { get; set; }
    }

    public class PeriodDto
    {
        public string Period { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public string BillType { get; set; }
    }

}
