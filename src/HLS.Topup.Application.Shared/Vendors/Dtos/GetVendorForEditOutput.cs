using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Vendors.Dtos
{
    public class GetVendorForEditOutput
    {
		public CreateOrEditVendorDto Vendor { get; set; }


    }
}