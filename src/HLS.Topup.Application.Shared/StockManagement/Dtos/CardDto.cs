using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardDto
    {
        
        public Guid Id { get; set; }
        public string Serial { get; set; }
        public string CardCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime ExportedDate { get; set; }
        public CommonConst.CardStatus Status { get; set; }
        public string BatchCode { get; set; }
        public string StockType { get; set; }
        public string StockCode { get; set; }
        public int CardValue { get; set; } 
        // nha cung cap
        public string ProviderName{ get; set; }
        public string ProviderCode { get; set; }
        // dịch vu
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        // loai sp
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
    }
}