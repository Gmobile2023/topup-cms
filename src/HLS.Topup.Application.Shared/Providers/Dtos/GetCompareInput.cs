using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Providers.Dtos
{
    public class GetCompareInput : PagedAndSortedResultRequestDto
    {
        public DateTime? FromTransDate { get; set; }
        public DateTime? ToTransDate { get; set; }
        public DateTime? FromCompareDate { get; set; }
        public DateTime? ToCompareDate { get; set; }
        public string ProviderCode { get; set; }
    }

    public class GetCompareReponseInput : PagedAndSortedResultRequestDto
    {
        public DateTime TransDate { get; set; }
        public string ProviderCode { get; set; }
    }

    public class GetCompareReponseDetailInput : PagedAndSortedResultRequestDto
    {
        public string KeyCode { get; set; }
        public string ProviderCode { get; set; }
        public int CompareType { get; set; }
    }

    public class GetCompareRefundDetailInput : PagedAndSortedResultRequestDto
    {
        public string KeyCode { get; set; }
        public string ProviderCode { get; set; }

        public int RefundInt { get; set; }
    }

    public class GetCompareRefundInput : PagedAndSortedResultRequestDto
    {
        public DateTime FromDateTrans { get; set; }
        public DateTime ToDateTrans { get; set; }
        public string ProviderCode { get; set; }
    }

    public class GetCompareRefundSingleInput
    {
        public string KeyCode { get; set; }
        public string ProviderCode { get; set; }
    }

    public class RefundCompareAmountInput
    {
        public string KeyCode { get; set; }
        public string ProviderCode { get; set; }
    }

    public class ReportCheckCompareInput
    {
        public string TransDate { get; set; }
        public string ProviderCode { get; set; }
    }

    public class RefundCompareSelectInput
    {
        public List<string> TransCodes { get; set; }
        public string KeyCode { get; set; }
    }
}
