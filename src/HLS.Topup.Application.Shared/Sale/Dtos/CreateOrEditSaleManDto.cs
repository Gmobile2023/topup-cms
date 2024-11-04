using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class CreateOrEditSaleManDto : EntityDto<long?>
    {
	    public string UserName { get; set; }

		public CommonConst.SystemAccountType SaleType { get; set; }

		public CommonConst.SaleManStatus Status { get; set; }

		[StringLength(SaleManConsts.MaxDescriptionLength, MinimumLength = SaleManConsts.MinDescriptionLength)]
		public string Description { get; set; }

    }
}
