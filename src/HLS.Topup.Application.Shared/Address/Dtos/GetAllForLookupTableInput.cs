using Abp.Application.Services.Dto;

namespace HLS.Topup.Address.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}