using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetAllCardStocksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; } 
		public string StockCodeFilter { get; set; } 
		public byte StatusFilter { get; set; }  
		public string ServiceCodeFilter { get; set; }  
		public string CategoryCodeFilter { get; set; }  
		public string ProductCodeFilter { get; set; }  
		
		public int? MaxCardValueFilter { get; set; }
		public int? MinCardValueFilter { get; set; } 
		
    }
}