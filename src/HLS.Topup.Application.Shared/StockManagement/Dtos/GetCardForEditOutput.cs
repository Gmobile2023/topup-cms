using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetCardForEditOutput
    {
		public CreateOrEditCardDto Card { get; set; }
   
    }
}