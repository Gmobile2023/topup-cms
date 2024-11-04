using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Reports.Dtos
{
    public class ReportCommissionDetailDto
    {
        public string AgentSumCode { get; set; }
        public string AgentSumInfo { get; set; }
        public decimal CommissionAmount { get; set; }
        public string CommissionCode { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }

        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        public string TransCode { get; set; }

        public string RequestRef { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? PayDate { get; set; }

    }

    public class ReportCommissionAgentDetailDto
    {

        public string AgentCode { get; set; }

        public string AgentInfo { get; set; }

        public string RequestRef { get; set; }

        public string TransCode { get; set; }

        public string CommissionCode { get; set; }
        public decimal CommissionAmount { get; set; }

        public int StatusPayment { get; set; }

        public string StatusPaymentName { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public decimal Fee { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public int Status { get; set; }
        public string StatusName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? PayDate { get; set; }
                                    
    }

    public class ReportCommissionTotalDto
    {
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public decimal Quantity { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal Payment { get; set; }
        public decimal UnPayment { get; set; }        
    }

    public class ReportCommissionAgentTotalDto
    {
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public decimal Before { get; set; }
        public decimal AmountUp { get; set; }
        public decimal AmountDown { get; set; }
        public decimal After { get; set; }
    }
}
