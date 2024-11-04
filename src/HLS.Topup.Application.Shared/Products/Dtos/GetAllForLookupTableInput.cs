using Abp.Application.Services.Dto;

namespace HLS.Topup.Products.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}