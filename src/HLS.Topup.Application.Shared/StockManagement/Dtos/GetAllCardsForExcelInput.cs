using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetAllCardsForExcelInput
    {
	      public string Filter { get; set; }
           public string BatchCodeFilter { get; set; }
           
           public string SerialFilter { get; set; }
           
           public DateTime? MaxExpiredDateFilter { get; set; }
           public DateTime? MinExpiredDateFilter { get; set; }
           
           public string ProviderCodeFilter { get; set; }
           public string StockCodeFilter { get; set; }
           
           public string ServiceCodeFilter { get; set; }
           public string CategoryCodeFilter { get; set; }
                   
           public int? MaxCardValueFilter { get; set; }
           public int? MinCardValueFilter { get; set; }
           
           //public string CardCodeFilter { get; set; }
    
           public DateTime? MaxImportedDateFilter { get; set; }
           public DateTime? MinImportedDateFilter { get; set; }
   
           public DateTime? MaxExportedDateFilter { get; set; }
           public DateTime? MinExportedDateFilter { get; set; }
    
           public byte StatusFilter { get; set; }
    }

    public class CardStockTransForExcelInput
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