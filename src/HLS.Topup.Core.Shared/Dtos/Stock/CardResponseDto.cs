using System;
using System.Runtime.Serialization;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class CardResponseDto
    {
        public Guid Id { get; set; }
        public string CardCode { get; set; }
        public string Serial { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime? ExportedDate { get; set; }
        public CommonConst.CardStatus Status { get; set; }
        public DateTime? UsedDate { get; set; }
        public string ExportTransCode { get; set; }
        public int CardValue { get; set; }
        public string BatchCode { get; set; }
        public string CardTransCode { get; set; }
        public string ProductCode { get; set; }
        public string StockCode { get; set; }   
        public string StockType { get; set; }   
        public string ProviderCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        
        public string ServiceName { get; set; }
        public string CategoryName { get; set; }
        public string ProviderName { get; set; }
        
         
    }
}