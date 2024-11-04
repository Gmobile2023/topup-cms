using HLS.Topup.Categories.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Categories
{
    public class CreateOrEditCategoryModalViewModel
    {
       public CreateOrEditCategoryDto Category { get; set; }

	   		public string CategoryCategoryName { get; set;}

		public string ServiceServicesName { get; set;}


       public List<CategoryCategoryLookupTableDto> CategoryCategoryList { get; set;}

public List<CategoryServiceLookupTableDto> CategoryServiceList { get; set;}


	   public bool IsEditMode => Category.Id.HasValue;
    }
}