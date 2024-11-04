using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public int CategoryId { get; set; }
    }
}