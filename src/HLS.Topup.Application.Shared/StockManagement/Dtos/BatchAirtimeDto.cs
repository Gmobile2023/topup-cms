using HLS.Topup.StockManagement;

using System;
using System.Runtime.Serialization;
using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class BatchAirtimeDto : EntityDto
    {
		public string BatchCode { get; set; }
		public string Vendor { get; set; }
		[DataMember(Name = "provider")] 
		public string ProviderCode { get; set; }
		public string ProviderName { get; set; }
		[DataMember(Name = "totalValue")] 
		public decimal Airtime  { get; set; }
		public decimal Amount { get; set; }
		public float Discount { get; set; }
		[DataMember(Name = "batchStatus")] 
		public BatchAirtimeStatus Status { get; set; } 
		public string Description { get; set; }
		
		public DateTime CreatedDate { get; set; }
		public string CreatedAccount { get; set; } 
		public string CreatedAccountName { get; set; } 
		public DateTime? ModifiedDate { get; set; } 
		public string ModifiedAccount { get; set; } 
		public string ModifiedAccountName { get; set; } 
    }
    
    public class GetAllBatchAirtimesInput : PagedAndSortedResultRequestDto
    {
	    public string Filter { get; set; } 
	    public string BatchCodeFilter { get; set; } 
	    public string ProviderCodeFilter { get; set; }   
	    public byte StatusFilter { get; set; }  
	    public DateTime? FormDate { get; set; }  
	    public DateTime? ToDate { get; set; }  
		
    }
    
    public class GetAllBatchAirtimesForExcelInput 
    {
	    public string Filter { get; set; } 
	    public string BatchCodeFilter { get; set; } 
	    public string ProviderCodeFilter { get; set; }   
	    public byte StatusFilter { get; set; }  
	    public DateTime? FormDate { get; set; }  
	    public DateTime? ToDate { get; set; }  
		
    }
    
    
}