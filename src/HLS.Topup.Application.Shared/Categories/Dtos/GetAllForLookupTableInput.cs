using Abp.Application.Services.Dto;

namespace HLS.Topup.Categories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}