using Abp.Application.Services.Dto;

namespace HLS.Topup.PostManagement.Dtos
{
    public class GetPostsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public bool? Status { get; set; }
    }
}
