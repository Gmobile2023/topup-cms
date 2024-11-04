
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class AccountBlockBalanceDto : EntityDto
    {
		public string TransCode { get; set; }

		public decimal BlockedMoney { get; set; }

		public string Description { get; set; }

		public DateTime? LastModificationTime { get; set; }
		 public long UserId { get; set; }


    }
}
