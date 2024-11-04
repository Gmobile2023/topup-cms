using System;
using System.Collections.Generic;
using System.Text;
using static HLS.Topup.Common.CommonConst;

namespace HLS.Topup.Dtos.Stock
{
    public class StockTransRequestDto
    {
        public string Provider { get; set; }
        public string BatchCode { get; set; }
        public string TransCode { get; set; }
        public string TransCodeProvider { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public decimal ItemValue { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public CardStockStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSyncCard { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
