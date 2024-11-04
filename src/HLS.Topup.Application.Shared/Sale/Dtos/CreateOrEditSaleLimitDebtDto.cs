using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class CreateOrEditSaleLimitDebtDto : EntityDto<int?>
    {

		public decimal LimitAmount { get; set; }


		public int DebtAge { get; set; }


		public CommonConst.DebtLimitAmountStatus Status { get; set; }


		[StringLength(SaleLimitDebtConsts.MaxDescriptionLength, MinimumLength = SaleLimitDebtConsts.MinDescriptionLength)]
		public string Description { get; set; }


		 public long UserId { get; set; }


    }
}
