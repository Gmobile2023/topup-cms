using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Products.Dtos
{
    public class GetProductForEditOutput
    {
		public CreateOrEditProductDto Product { get; set; }

		public string CategoryCategoryName { get; set;}


    }
}