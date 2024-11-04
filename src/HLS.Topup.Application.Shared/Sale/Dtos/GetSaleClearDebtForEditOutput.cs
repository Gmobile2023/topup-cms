using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class GetSaleClearDebtForEditOutput
    {
		public CreateOrEditSaleClearDebtDto SaleClearDebt { get; set; }

		public string UserName { get; set;}

		public string BankBankName { get; set;}


    }
}