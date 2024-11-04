using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class CreateOrEditPayBatchBillDto : EntityDto<int?>
    {
		public string Code { get; set; }
		[Required]
		[StringLength(PayBatchBillConsts.MaxNameLength, MinimumLength = PayBatchBillConsts.MinNameLength)]
		public string Name { get; set; }
		public CommonConst.PayBatchBillStatus Status { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public int TotalBlockBill { get; set; }


		public decimal AmountPayBlock { get; set; }


		[StringLength(PayBatchBillConsts.MaxDescriptionLength, MinimumLength = PayBatchBillConsts.MinDescriptionLength)]
		public string Description { get; set; }


		public decimal? MinBillAmount { get; set; }

		public decimal? MaxAmountPay { get; set; }

		public string CategoryCode { get; set; }

		public string ProductCode { get; set; }
    }
}
