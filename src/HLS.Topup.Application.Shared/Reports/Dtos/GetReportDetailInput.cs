using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using HLS.Topup.RequestDtos;
using System.Collections.Generic;

namespace HLS.Topup.Reports.Dtos
{
    public class GetReportDetailInput : PagedAndSortedResultRequestDto
    {
        private CommonConst.SystemAccountType _accountType;
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string AccountCodeFilter { get; set; }
        public string TransCodeFilter { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string PartnerCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }

        public CommonConst.AgentType AgentType { get; set; }
        public string LoginCode { get; set; }
        public int AccountType { get; set; }
    }

    public class GetReportTotalInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string AccountCodeFilter { get; set; }
        public string PartnerCode { get; set; }

        public int AgentType { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
    }

    public class GetReportGroupInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string PartnerCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }
    }
}
