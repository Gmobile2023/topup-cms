using HLS.Topup.Services.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Services
{
    public class CreateOrEditServiceModalViewModel
    {
       public CreateOrEditServiceDto Service { get; set; }

	   
       
	   public bool IsEditMode => Service.Id.HasValue;
    }
}