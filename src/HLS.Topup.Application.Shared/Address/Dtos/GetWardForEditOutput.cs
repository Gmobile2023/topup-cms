using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class GetWardForEditOutput
    {
		public CreateOrEditWardDto Ward { get; set; }

		public string DistrictDistrictName { get; set;}


    }
}