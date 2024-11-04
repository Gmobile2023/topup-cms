using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Sale.Dtos
{
    public class GetSaleManForEditOutput
    {
		public CreateOrEditSaleManDto SaleMan { get; set; }

		public string UserName { get; set;}

		public string CityCityName { get; set;}


    }
}