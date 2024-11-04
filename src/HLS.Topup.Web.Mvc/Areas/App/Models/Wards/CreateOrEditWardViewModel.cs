using HLS.Topup.Address.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Wards
{
    public class CreateOrEditWardModalViewModel
    {
       public CreateOrEditWardDto Ward { get; set; }

	   		public string DistrictDistrictName { get; set;}


       public List<WardDistrictLookupTableDto> WardDistrictList { get; set;}


	   public bool IsEditMode => Ward.Id.HasValue;
    }
}