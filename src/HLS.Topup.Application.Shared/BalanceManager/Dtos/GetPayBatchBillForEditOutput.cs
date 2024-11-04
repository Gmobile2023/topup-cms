using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetPayBatchBillForEditOutput
    {
		public CreateOrEditPayBatchBillDto PayBatchBill { get; set; }

		public string ProductName { get; set;}

        public string CategoryName { get; set; }


    }
}