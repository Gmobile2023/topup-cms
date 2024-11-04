using HLS.Topup.Common;
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLS.Topup.FeeManager.Dtos
{
    public class FeeDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime? DateApproved { get; set; }

        public CommonConst.FeeStatus Status { get; set; }

        public CommonConst.AgentType AgentType { get; set; }
        
        public string AgentName { get; set; }
        
        public string UserApproved { get; set; }
        
        public DateTime? CreationTime { get; set; }
        
        public long? UserId { get; set; }
        
        public List<int> ProductType { get; set; }
        
        public string StatusName { get; set; }
    }
}