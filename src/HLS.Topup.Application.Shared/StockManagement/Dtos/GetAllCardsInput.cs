using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetAllCardsInput : PagedAndSortedResultRequestDto
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
}