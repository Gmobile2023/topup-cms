using Abp.Application.Services.Dto;

namespace HLS.Topup.Sale.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}