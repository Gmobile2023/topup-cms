using Abp.Application.Services.Dto;

namespace HLS.Topup.Notifications.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}