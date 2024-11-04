using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Accounts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLS.Topup.Web.Areas.App.Models.Reports
{

    public class ReportAccountDetailViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AgentCode { get; set; }
        public string AgentInfo { get; set; }

    }

    public class ReportTransferViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }
        public string AccountType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AgentCode { get; set; }
        public string AgentInfo { get; set; }
        public string ServiceCode { get; set; }

    }
    public class ReportServiceViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }
        public List<ComboboxItemDto> Services { get; set; }

        public List<AgentTypeDto> AgentTypes { get; set; }

        public string AccountType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AgentCode { get; set; }
        public string AgentInfo { get; set; }
        public string SaleCode { get; set; }
        public string SaleInfo { get; set; }
        public string ServiceCode { get; set; }
        public string ProviderCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public int Status { get; set; }

    }

    public class ReportServiceProviderViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }
        public List<ComboboxItemDto> Services { get; set; }

        public List<AgentTypeDto> AgentTypes { get; set; }
        public string AccountType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AgentCode { get; set; }
        public string AgentInfo { get; set; }      
    }

    public class ReportServiceTotalViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }
        public List<ComboboxItemDto> Services { get; set; }

    }

    public class ReportAgentBalanceViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }

    public class ReportRevenueAgentViewModel
    {
        public List<ComboboxItemDto> Citys { get; set; }

        public List<ComboboxItemDto> Services { get; set; }

        public List<AgentTypeDto> AgentTypes { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Status { get; set; }

    }

    public class ReportRevenueCityViewModel
    {
        public List<ComboboxItemDto> Citys { get; set; }

        public List<ComboboxItemDto> Services { get; set; }

        public List<AgentTypeDto> AgentTypes { get; set; }

    }

    public class ReportTotalSaleAgentViewModel
    {
        public List<ComboboxItemDto> Citys { get; set; }

        public List<ComboboxItemDto> Services { get; set; }

        public List<AgentTypeDto> AgentTypes { get; set; }

    }

    public class ReportRevenueActiveViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Status { get; set; }

    }

    public class AccountDebtDetailViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SaleCode { get; set; }
        public string SaleInfo { get; set; }
        public string Type { get; set; }

    }

    public class ComaprePartnerEmailViewModel
    {
        public string FromDateText { get; set; }

        public string ToDateText { get; set; }

        public string AgentCode { get; set; }
    }

    public class ReportCommissionViewModel
    {     
        public List<ComboboxItemDto> Services { get; set; }

        public string AccountType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AgentCode { get; set; }
        public string AgentInfo { get; set; }       
        public string ServiceCode { get; set; }      
        public int Status { get; set; }

    }

    public class ReportCommissionTotalViewModel
    {
        public List<ComboboxItemDto> Services { get; set; }

    }


    public class ReportComparePartnerViewModel
    {
        public List<AgentTypeDto> AgentTypes { get; set; }       
    }
}
