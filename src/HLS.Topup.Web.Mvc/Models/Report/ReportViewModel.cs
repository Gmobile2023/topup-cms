using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Web.Models.Report
{
    public class ReportViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }
        public UserProfileDto User { get; set; }
    }

    public class ReportRoseViewModel
    {        
        public string AccountCode { get; set; }
    }

    public class DashRevenueViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class DashAgentGeneralViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string AgentCodeGeneral { get; set; }
    }
}
