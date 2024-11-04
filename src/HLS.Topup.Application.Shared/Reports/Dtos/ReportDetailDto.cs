using System;
using HLS.Topup.Common;

namespace HLS.Topup.Reports.Dtos
{
    public class ReportDetailDto
    {
        public int Index { get; set; }
        public string TransCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal Increment { get; set; }
        public decimal Decrement { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string TransNote { get; set; }
        public string SrcAccountCode { get; set; }
        public string DesAccountCode { get; set; }
        public string Description { get; set; }

        public decimal SrcAccountBalanceAfterTrans { get; set; }

        public decimal DesAccountBalanceAfterTrans { get; set; }

        public decimal SrcAccountBalanceBeforeTrans { get; set; }

        public decimal DesAccountBalanceBeforeTrans { get; set; }

        public decimal SrcAccountBalance { get; set; }
        public decimal DesAccountBalance { get; set; }

        public CommonConst.TransactionType TransType { get; set; }        
    }

    public class ReportTotalDto
    {
        public int Index { get; set; }
        public string AccountCode { get; set; }
        public string AccountInfo { get; set; }        
        public int AgentType { get; set; }
        public decimal Credited { get; set; }
        public decimal Debit { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedDay { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ReportGroupDto
    {
        public int Index { get; set; }
        public string AccountCode { get; set; }
        public decimal Credited { get; set; }
        public decimal Debit { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedDay { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
