using HLS.Topup.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/stock/stockTrans/list")]
    public class CardStockTransListRequest : PaggingBaseDto
    {
        public string Provider { get; set; }
        public string BatchCode { get; set; }
        public string TransCode { get; set; }
        public string TransCodeProvider { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public byte Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
