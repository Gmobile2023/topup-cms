using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetAllCardBatchsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BatchCodeFilter { get; set; }

        public string BatchNameFilter { get; set; }

        //public string ProductCodeFilter { get; set; }
        public byte StatusFilter { get; set; }
        public string ImportTypeFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }
        public string ProviderFilter { get; set; }
        public string VendorFilter { get; set; }

 
    }
    
    public class GetAllCardBatchsForExcelInput
    {
        public string Filter { get; set; }
        public string BatchCodeFilter { get; set; }
        public string BatchNameFilter { get; set; }
        public string ProductCodeFilter { get; set; }
        public byte StatusFilter { get; set; }
        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }
        public string ProviderFilter { get; set; }
        public string VendorFilter { get; set; }
        public string ImportTypeFilter { get; set; }

    }
    
    
}