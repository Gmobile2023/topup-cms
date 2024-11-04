using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Banks.Dtos
{
    public class GetAllBanksForExcelInput
    {
		public string Filter { get; set; }

		public string BankNameFilter { get; set; }

		public string BranchNameFilter { get; set; }

		public string BankAccountNameFilter { get; set; }

		public string BankAccountCodeFilter { get; set; }

		public int? StatusFilter { get; set; }



    }
}