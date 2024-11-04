using Abp.Application.Services.Dto;

namespace HLS.Topup.TopupGateResponseMessage.Dto
{
    public class GetAllTopupGateRMInput : PagedAndSortedResultRequestDto
    {
        public string Provider { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}