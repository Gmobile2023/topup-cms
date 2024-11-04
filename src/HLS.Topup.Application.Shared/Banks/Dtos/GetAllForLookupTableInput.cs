using Abp.Application.Services.Dto;

namespace HLS.Topup.Banks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}