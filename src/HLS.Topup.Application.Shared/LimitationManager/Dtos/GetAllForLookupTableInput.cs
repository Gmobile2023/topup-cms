using Abp.Application.Services.Dto;

namespace HLS.Topup.LimitationManager.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}