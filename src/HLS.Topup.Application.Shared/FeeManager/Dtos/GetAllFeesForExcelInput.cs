using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace HLS.Topup.FeeManager.Dtos
{
    public class GetAllFeesForExcelInput
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }

        public int? StatusFilter { get; set; }

        public int? AgentTypeFilter { get; set; }

        public string UserNameFilter { get; set; }
    }

    public class GetDetailFeesForExcelInput
    {
        public int FeeId { get; set; }
        
        public List<int?> ProductIds { get; set; }
    }
}