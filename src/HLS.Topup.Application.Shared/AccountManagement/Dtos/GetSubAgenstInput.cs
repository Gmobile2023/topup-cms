using Abp.Application.Services.Dto;

namespace HLS.Topup.AccountManagement.Dtos
{
    public class GetSubAgenstInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? Status { get; set; }

    }
}
