using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace HLS.Topup.FeeManager.Dtos
{
    public class GetAllFeesInput : PagedAndSortedResultRequestDto
    {
        public string NameFilter { get; set; }
        public string CodeFilter { get; set; }

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

    public class GetFeeDetailTableInput : PagedAndSortedResultRequestDto
    {
        public int? FeeId { get; set; }
        public List<int?> CateIds { get; set; }
        
        public List<int?> ProductIds { get; set; }
    }
}