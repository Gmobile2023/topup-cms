using HLS.Topup.Common;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using HLS.Topup.Dtos.Provider;

namespace HLS.Topup.Providers.Dtos
{
    public class CreateOrEditProviderDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ProviderConsts.MaxCodeLength, MinimumLength = ProviderConsts.MinCodeLength)]
		public string Code { get; set; }


		[Required]
		[StringLength(ProviderConsts.MaxNameLength, MinimumLength = ProviderConsts.MinNameLength)]
		public string Name { get; set; }


		[StringLength(ProviderConsts.MaxImagesLength, MinimumLength = ProviderConsts.MinImagesLength)]
		public string Images { get; set; }


		[StringLength(ProviderConsts.MaxPhoneNumberLength, MinimumLength = ProviderConsts.MinPhoneNumberLength)]
		public string PhoneNumber { get; set; }


		[StringLength(ProviderConsts.MaxEmailAddressLength, MinimumLength = ProviderConsts.MinEmailAddressLength)]
		public string EmailAddress { get; set; }


		[StringLength(ProviderConsts.MaxAddressLength, MinimumLength = ProviderConsts.MinAddressLength)]
		public string Address { get; set; }


		public CommonConst.ProviderType ProviderType { get; set; }


		public CommonConst.ProviderStatus ProviderStatus { get; set; }


		[StringLength(ProviderConsts.MaxDescriptionLength, MinimumLength = ProviderConsts.MinDescriptionLength)]
		public string Description { get; set; }
		public string TransCodeConfig { get; set; }

		public ProviderUpdateInfo ProviderUpdateInfo { get; set; }

		public bool IsSlowTrans { get; set; }

		public int TotalTransError { get; set; }
		public int TimeClose { get; set; }
		public bool IsAutoCloseFail { get; set; }
		public string IgnoreCode { get; set; }

        public int TimeScan { get; set; }
        public int TotalTransScan { get; set; }
        public int TotalTransDubious { get; set; }
        public int TotalTransErrorScan { get; set; }
        public string ParentProvider { get; set; }
        public bool IsAutoDeposit { get; set; }
        public bool IsRoundRobinAccount { get; set; }
        public decimal MinBalance { get; set; }
        public decimal MinBalanceToDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public string WorkShortCode { get; set; }

    }
}
