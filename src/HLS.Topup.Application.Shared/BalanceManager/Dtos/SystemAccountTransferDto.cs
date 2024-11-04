using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class SystemAccountTransferDto : EntityDto
    {
		public string SrcAccount { get; set; }

		public string DesAccount { get; set; }
		public string TransCode { get; set; }

		public decimal Amount { get; set; }

		public string Attachments { get; set; }

		public CommonConst.SystemTransferStatus Status { get; set; }



    }
}
