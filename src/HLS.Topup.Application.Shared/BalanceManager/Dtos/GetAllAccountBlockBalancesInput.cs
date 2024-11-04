using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAllAccountBlockBalancesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TransCodeFilter { get; set; }


		 public long? UserIdFilter { get; set; }
		 public DateTime? FromCreatedTimeFilter { get; set; }
		 public DateTime? ToCreatedTimeFilter { get; set; }


    }
    public class GetAllAccountBlockBalancesDetailInput : PagedAndSortedResultRequestDto
    {
	    public string Filter { get; set; }
	    public string TransCodeFilter { get; set; }
	    public int Id { get; set; }

    }
}
