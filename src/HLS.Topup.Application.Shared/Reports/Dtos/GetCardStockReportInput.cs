using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Reports.Dtos
{
    public class GetReportCardStockHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string StockCodeFilter { get; set; }
        public string ProductCodeFilter { get; set; }
        public string StockTypeFilter { get; set; }


        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }

        public int StatusFilter { get; set; }
        public string TelcoFilter { get; set; }
        public int CardValueFilter { get; set; }
    }

    public class GetReportCardStockImExPortInput : PagedAndSortedResultRequestDto
    {
        public string StoreCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GetReportCardStockImExProviderInput : PagedAndSortedResultRequestDto
    {
        public string StoreCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProviderCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GetReportCardStockAutoInput : PagedAndSortedResultRequestDto
    {       
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GetReportCardStockInventoryInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string StockCodeFilter { get; set; }

        public int StatusFilter { get; set; }
        public string TelcoFilter { get; set; }
        public int CardValueFilter { get; set; }
        public string ProductCodeFilter { get; set; }
        public string StockTypeFilter { get; set; }
    }
}
