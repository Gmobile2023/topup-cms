using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Categories.Dtos
{
    public class GetAllCategoriesForExcelInput
    {
		public string Filter { get; set; }

		public string CategoryCodeFilter { get; set; }

		public string CategoryNameFilter { get; set; }

		public int? StatusFilter { get; set; }

		public int? TypeFilter { get; set; }


		 public string CategoryCategoryNameFilter { get; set; }

		 		 public string ServiceServicesNameFilter { get; set; }

		 
    }
}