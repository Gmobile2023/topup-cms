
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using HLS.Topup.Common;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class CreateOrEditAccountBlockBalanceDto : EntityDto<int?>
    {

		//public decimal BlockMoney { get; set; }


		//public decimal UnBlockMoney { get; set; }
		public decimal Amount { get; set; }


		[StringLength(AccountBlockBalanceConsts.MaxDescriptionLength, MinimumLength = AccountBlockBalanceConsts.MinDescriptionLength)]
		public string Description { get; set; }


		 public long UserId { get; set; }
		 public virtual CommonConst.BlockBalanceType Type { get; set; }
		 public string Attachments { get; set; }



    }
}
