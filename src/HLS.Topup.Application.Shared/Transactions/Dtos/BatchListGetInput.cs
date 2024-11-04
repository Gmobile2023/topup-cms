using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Transactions.Dtos
{
    public class BatchListGetInput  : PagedAndSortedResultRequestDto
    {
        public string BatchCode { get; set; }
        public string BatchType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }

    public class BatchSingleInput
    {
        public string BatchCode { get; set; }        
    }

    public class BatchDetailGetInput : PagedAndSortedResultRequestDto
    {
        public string BatchCode { get; set; }
        public int Status { get; set; }
        public int BatchStatus { get; set; }
    }
    
    public class BatchListToExcelGetInput
    {
        public string BatchCode { get; set; }
        public string BatchType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Status { get; set; }
    }
}
