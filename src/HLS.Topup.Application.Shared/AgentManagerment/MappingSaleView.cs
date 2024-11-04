using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.AgentManagerment
{
    public class MappingSaleView
    {
        public int AgentId { get; set; }

        public long SaleUserId { get; set; }

        public string UserSale { get; set; }

        public string UserSaleLeader { get; set; }

        public int? Id { get; set; }
    }

    public class CreateOrEditSaleAssignAgentDto : EntityDto<int?>
    {
        public int SaleUserId { get; set; }

        public long? UserAgentId { get; set; }
    }
    public class ResetOdpInput
    {
        public long UserId { get; set; }
        public int ResetCount { get; set; }
    }

}
