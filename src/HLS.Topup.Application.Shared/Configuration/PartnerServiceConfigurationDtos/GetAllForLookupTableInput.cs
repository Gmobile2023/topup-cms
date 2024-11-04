using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public int CategoryId { get; set; }
    }
}