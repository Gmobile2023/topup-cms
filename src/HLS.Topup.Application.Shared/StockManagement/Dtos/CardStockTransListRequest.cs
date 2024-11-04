using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardStockTransListInput : PagedAndSortedResultRequestDto
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
