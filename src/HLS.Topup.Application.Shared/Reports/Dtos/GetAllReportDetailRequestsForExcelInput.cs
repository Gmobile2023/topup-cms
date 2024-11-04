using System;

namespace HLS.Topup.Reports.Dtos
{
    public class GetAllReportDetailForExcelInput
    {
        public string Filter { get; set; }       
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }       
        public string AccountCodeFilter { get; set; }
        public string ServiceCodeFilter { get; set; }
        public string TransCodeFilter { get; set; }
        public int TransType { get; set; }
    }

    public class GetAllReportTotalForExcelInput
    {
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string AccountCodeFilter { get; set; }       

        public int AgentType { get; set; }
    }

    public class GetAllReportGroupForExcelInput
    {
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }        
    }

    public class GetAllReportTransDetailForExcelInput
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RequestTransCode { get; set; }
        public string ReceivedAccount { get; set; }
        public string ServiceCode { get; set; }
        public string ProviderCode { get; set; }
        public string UserProcess { get; set; }
        public int Status { get; set; }
        public string AccountCode
        {
            get; set;
        }
    }

    public class GetAllReportTotalDayForExcelInput
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }        
        public string AccountCode
        {
            get; set;
        }
    }
}