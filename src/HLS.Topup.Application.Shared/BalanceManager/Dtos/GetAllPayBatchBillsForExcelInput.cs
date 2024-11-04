using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAllPayBatchBillsForExcelInput
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }

        public int? StatusFilter { get; set; }

        public DateTime? MaxDateApprovedFilter { get; set; }
        public DateTime? MinDateApprovedFilter { get; set; }
        public string ProductCodeFilter { get; set; }
        public string CategoryCodeFilter { get; set; }

    }
}