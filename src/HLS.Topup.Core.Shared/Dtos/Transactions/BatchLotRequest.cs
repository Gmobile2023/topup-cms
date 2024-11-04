using ServiceStack;
using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Transactions
{
    [Route("/api/v1/gateway/batchLot_List", "GET")]
    public class BatchListGetRequest : PaggingBaseDto
    {
        public string AccountCode { get; set; }
        public bool IsStaff { get; set; }
        public string BatchCode { get; set; }
        public string BatchType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }

    [Route("/api/v1/gateway/batchLot_Detail", "GET")]
    public class BatchDetailGetRequest : PaggingBaseDto
    {
        public string AccountCode { get; set; }
        public bool IsStaff { get; set; }
        public string BatchCode { get; set; }
        public int Status { get; set; }
        public int BatchStatus { get; set; }
    }

    [Route("/api/v1/gateway/batchLot_Single", "GET")]
    public class BatchSingleGetRequest 
    {
        public string BatchCode { get; set; }
        public string AccountCode { get; set; }
        public bool IsStaff { get; set; }
    }
}

