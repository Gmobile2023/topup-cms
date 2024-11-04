using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Address.Dtos
{
    public class GetDistrictForEditOutput
    {
		public CreateOrEditDistrictDto District { get; set; }

		public string CityCityName { get; set;}


    }
}