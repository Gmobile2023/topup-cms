using Abp.Application.Services.Dto;

namespace HLS.Topup.Services.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}