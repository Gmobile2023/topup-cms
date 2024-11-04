using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.Dtos
{
    public class ServiceConfigurationUserLookupTableDto
    {
		public long Id { get; set; }

		public string DisplayName { get; set; }

        public string Phone { get; set; }

        public string AccountCode { get; set; }
    }
}