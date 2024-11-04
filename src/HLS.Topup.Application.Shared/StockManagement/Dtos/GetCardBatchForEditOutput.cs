using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetCardBatchForEditOutput
    {
		public CreateOrEditCardBatchDto CardBatch { get; set; }

		public string ProviderName { get; set;}

		public string VendorName { get; set;}


    }
}