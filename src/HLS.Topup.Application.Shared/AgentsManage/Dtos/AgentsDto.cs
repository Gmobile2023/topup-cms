using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using ServiceStack.DataAnnotations;

namespace HLS.Topup.AgentsManage.Dtos
{
    public class AgentsDto : EntityDto
    {
        public string AccountCode { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public CommonConst.AgentType AgentType { get; set; }

        public string ManagerName { get; set; }

        public string SaleLeadName { get; set; }

        public DateTime CreationTime { get; set; }

        public int Status { get; set; }

        public string Address { get; set; }

        public string Exhibit { get; set; }

        public bool IsMapSale { get; set; }

        public string AgentGeneral { get; set; }
    }
}