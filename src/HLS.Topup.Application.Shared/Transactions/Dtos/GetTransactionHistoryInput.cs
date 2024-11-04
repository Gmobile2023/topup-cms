using System;
using System.Collections.Generic;
using Abp;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Accounts;

namespace HLS.Topup.Transactions.Dtos
{
    public class GetBalanceHistoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string ServiceCodeFilter { get; set; }
        public string AccountFilter { get; set; }
        public int StatusFilter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string UserNameFilter { get; set; }
        public byte TransType { get; set; }
        public List<string> ServiceCodes { get; set; }
    }

    public class GetAllTopupRequestsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string MobileNumberFilter { get; set; }

        public string PartnerCodeFilter { get; set; }

        public string TransRefFilter { get; set; }

        public string TransCodeFilter { get; set; }
        public string ProviderTransCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }

        public List<byte?> StatusFilter { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public List<string> ServiceCodes { get; set; }
        public List<string> CategoryCodes { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<string> ProviderCode { get; set; }
        public string Telco { get; set; }
        public string TopupTransactionType { get; set; }
        public string WorkerApp { get; set; }
        public string Serial { get; set; }
        public bool IsAdmin { get; set; }
        public CommonConst.AgentType AgentTypeFilter { get; set; }
        public CommonConst.SaleType SaleTypeFilter { get; set; }
        public string ReceiverType { get; set; }
        public string ProviderResponseCode { get; set; }
        public string ReceiverTypeResponse { get; set; }
        public string ParentProvider { get; set; }        
    }

    public class GetAllTopupDetailRequestsInput : PagedAndSortedResultRequestDto
    {
        public string TransCode { get; set; }
    }

    public class GetAllTopupRequestsForExcelInput
    {
        public int ItemPerPageConfig { get; set; }       
        public int? TenantId { get; set; }

        public string Filter { get; set; }

        public string MobileNumberFilter { get; set; }

        public string PartnerCodeFilter { get; set; }

        public string TransRefFilter { get; set; }

        public string TransCodeFilter { get; set; }
        public string ProviderTransCode { get; set; }
        public string StaffAccount { get; set; }
        public string StaffUser { get; set; }

        public List<byte?> StatusFilter { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public List<string> ServiceCodes { get; set; }
        public List<string> CategoryCodes { get; set; }
        public List<string> ProductCodes { get; set; }
        public List<string> ProviderCode { get; set; }
        public string Telco { get; set; }
        public string TopupTransactionType { get; set; }
        public string WorkerApp { get; set; }
        public string Serial { get; set; }
        public bool IsAdmin { get; set; }
        public CommonConst.AgentType AgentTypeFilter { get; set; }
        public CommonConst.SaleType SaleTypeFilter { get; set; }
        public string ReceiverType { get; set; }
        public string ProviderResponseCode { get; set; }
        public string ReceiverTypeResponse { get; set; }
        public string ParentProvider { get; set; }
    }

    public class GetListTopupDetailRequestForExcelInput : PagedAndSortedResultRequestDto
    {
        public string TransCode { get; set; }
    }


    public class GetAllOffsetTopupRequestsInput : PagedAndSortedResultRequestDto
    {
      
        public string ReceiverInfo { get; set; }

        public string PartnerCode { get; set; }

        public string OriginPartnerCode { get; set; }

        public string OriginTransCode { get; set; }

        public string TransCode { get; set; }
                 
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int Status { get; set; }
                          
    }

}
