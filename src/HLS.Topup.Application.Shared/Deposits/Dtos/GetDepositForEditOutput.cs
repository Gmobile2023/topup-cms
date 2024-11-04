using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Deposits.Dtos
{
    public class GetDepositForEditOutput
    {
		public CreateOrEditDepositDto Deposit { get; set; }

		public string UserName { get; set;}

		public string BankBankName { get; set;}

		public string UserName2 { get; set;}
		public string UserNameSale { get; set;}


    }
}
