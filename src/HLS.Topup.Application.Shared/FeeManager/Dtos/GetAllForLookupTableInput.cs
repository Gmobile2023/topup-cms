using Abp.Application.Services.Dto;

namespace HLS.Topup.FeeManager.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}