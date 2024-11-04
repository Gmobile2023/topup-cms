using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.DiscountManager.Dtos
{
    public class GetAllDiscountsForExcelInput
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }

        public int? StatusFilter { get; set; }

        public int? AgentTypeFilter { get; set; }
        
        public string UserNameFilter { get; set; }
        
        public DateTime? FromCreationTimeFilter { get; set; }
        
        public DateTime? ToCreationTimeFilter { get; set; }
        
        public DateTime? FromApprovedTimeFilter { get; set; }
        
        public DateTime? ToApprovedTimeFilter { get; set; }
        
        public DateTime? FromAppliedTimeFilter { get; set; }
        
        public DateTime? ToAppliedTimeFilter { get; set; }
        
        public int? ProductTypeFilter { get; set; }
        
        public int? ProductFilter { get; set; }
    }
    
    public class GetDetailDiscountsForExcelInput
    {
        public int DiscountId { get; set; }
    }
}