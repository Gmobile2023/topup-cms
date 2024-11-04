using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}