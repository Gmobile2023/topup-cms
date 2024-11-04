using System;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;

namespace HLS.Topup.AgentsManage.Dtos
{
    public class GetAllAgentsInput : PagedAndSortedResultRequestDto
    {
        [CanBeNull] public string Filter { get; set; }

        public DateTime? FromDateFilter { get; set; }

        public DateTime? ToDateFilter { get; set; }

        public int? AgentTypeFilter { get; set; }

        public int ? AgentId { get; set; }

        public long? SaleLeadFilter { get; set; }
        
        public long? ManagerFilter { get; set; }
        
        [CanBeNull] public string ExhibitFilter { get; set; }
        
        public int? Province { get; set; }
        
        public int? District { get; set; }
        
        public int? Village { get; set; }
        
        public int? Status { get; set; }
        
        public int? IsMapSale { get; set; }
    }
}