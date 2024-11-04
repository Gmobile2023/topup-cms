using Abp.Application.Services.Dto;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class GetAgentNetworkInput : PagedAndSortedResultRequestDto
    {
        public string AccountCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class GetUserStaffInput : PagedAndSortedResultRequestDto
    {
        public string AccountCode { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Search { get; set; }
        public bool? IsActive { get; set; }
    }

    public class GetUserStaffInfoInput
    {
        public long UserId { get; set; }
    }

    public class GetTotalUserStaffInput
    {

    }
}
