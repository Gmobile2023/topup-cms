using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using HLS.Topup.Common;

namespace HLS.Topup.LimitationManager.Dtos
{
    public class GetAllLimitProductsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }
        
        public DateTime? FromDateFilter { get; set; }
        
        public DateTime? ToDateFilter { get; set; }
        
        public string ServiceFilter { get; set; }
        
        public string ProductTypeFilter { get; set; }
        
        public int? ProductFilter { get; set; }

        public CommonConst.AgentType? AgentTypeFilter { get; set; }
        
        public int? AgentFilter { get; set; }

        public int? StatusFilter { get; set; }
        
        public string UserNameFilter { get; set; }
    }
    
    public class GetLimitProductsTableInput : PagedAndSortedResultRequestDto
    {
        public int LimitProductId { get; set; }
        public List<int?> UserIds { get; set; }
    }
}