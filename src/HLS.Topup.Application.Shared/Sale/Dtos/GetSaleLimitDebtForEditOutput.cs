using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class GetSaleLimitDebtForEditOutput
    {
		public CreateOrEditSaleLimitDebtDto SaleLimitDebt { get; set; }

		public string UserName { get; set;}


    }
}