using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Providers.Dtos
{
    public class ProviderDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string PhoneNumber { get; set; }
		public string TransCodeConfig { get; set; }

		public CommonConst.ProviderType ProviderType { get; set; }

		public CommonConst.ProviderStatus ProviderStatus { get; set; }
		public string ParentProvider { get; set; }
		public bool IsAutoDeposit { get; set; }
		public bool IsRoundRobinAccount { get; set; }
		public decimal MinBalance { get; set; }
		public decimal MinBalanceToDeposit { get; set; }
		public decimal DepositAmount { get; set; }


    }
}
