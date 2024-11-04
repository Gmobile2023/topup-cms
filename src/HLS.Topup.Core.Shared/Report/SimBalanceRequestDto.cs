using System;
using ServiceStack;
using HLS.Topup.Common;

namespace HLS.Topup.Report
{
    [Route("/api/v1/report/sim/balance_histories", "GET")]
    public class SimBalanceHistoriesRequest:PaggingBaseDto
    {
        public string SimNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Serial { get; set; }
        public string TransCode { get; set; }
    }
    [Route("/api/v1/report/sim/balance", "GET")]
    public class SimBalanceDateRequest:PaggingBaseDto
    {
        public string SimNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Serial { get; set; }
        public string TransCode { get; set; }
    }
}