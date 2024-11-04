using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Sale.Dtos
{
    public class GetAllSaleLimitDebtsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? StatusFilter { get; set; }

		 public string UserNameFilter { get; set; }

         public int ? SaleLeaderId { get; set; }

        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
    }
}