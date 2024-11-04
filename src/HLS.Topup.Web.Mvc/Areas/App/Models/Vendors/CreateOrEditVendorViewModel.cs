using HLS.Topup.Vendors.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Vendors
{
    public class CreateOrEditVendorModalViewModel
    {
       public CreateOrEditVendorDto Vendor { get; set; }

	   
       
	   public bool IsEditMode => Vendor.Id.HasValue;
    }
}