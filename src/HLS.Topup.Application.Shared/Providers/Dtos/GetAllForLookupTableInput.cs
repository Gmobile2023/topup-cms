using Abp.Application.Services.Dto;

namespace HLS.Topup.Providers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}