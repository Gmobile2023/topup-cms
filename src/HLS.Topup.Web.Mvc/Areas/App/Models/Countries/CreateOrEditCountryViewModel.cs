using HLS.Topup.Address.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
       public CreateOrEditCountryDto Country { get; set; }

	   
       
	   public bool IsEditMode => Country.Id.HasValue;
    }
}