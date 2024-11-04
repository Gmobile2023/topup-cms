using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Categories.Dtos
{
    public class GetCategoryForEditOutput
    {
		public CreateOrEditCategoryDto Category { get; set; }

		public string CategoryCategoryName { get; set;}

		public string ServiceServicesName { get; set;}


    }
}