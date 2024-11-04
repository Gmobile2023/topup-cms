using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAllSystemAccountTransfersForExcelInput
    {
		public string Filter { get; set; }

		public string SrcAccountFilter { get; set; }

		public string DesAccountFilter { get; set; }

		public int? StatusFilter { get; set; }
		public DateTime? FromCreatedTimeFilter { get; set; }
		public DateTime? ToCreatedTimeFilter { get; set; }




    }
}
