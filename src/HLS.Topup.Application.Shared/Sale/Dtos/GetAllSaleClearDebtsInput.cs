using Abp.Application.Services.Dto;
using HLS.Topup.Common;
using System;

namespace HLS.Topup.Sale.Dtos
{
    public class GetAllSaleClearDebtsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? TypeFilter { get; set; }

        public long ? UserId { get; set; }

        public long? BankId { get; set; }

        public string BankBankNameFilter { get; set; }

        public string TransCodeFilter { get; set; }

        public string TransCodeBank { get; set; }

        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }

        public int? StatusFilter { get; set; }

    }
}