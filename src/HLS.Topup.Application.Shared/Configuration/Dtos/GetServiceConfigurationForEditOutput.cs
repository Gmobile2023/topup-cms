using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Configuration.Dtos
{
    public class GetServiceConfigurationForEditOutput
    {
		public CreateOrEditServiceConfigurationDto ServiceConfiguration { get; set; }

		public string ServiceServicesName { get; set;}

		public string ProviderName { get; set;}

		public string CategoryCategoryName { get; set;}

		public string ProductProductName { get; set;}

		public string UserName { get; set;}

        public bool DispalyRate { get; set; }
    }
}