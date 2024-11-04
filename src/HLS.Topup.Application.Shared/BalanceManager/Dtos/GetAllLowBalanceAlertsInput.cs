using Abp.Application.Services.Dto;

namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetAllLowBalanceAlertsInput : PagedAndSortedResultRequestDto
    {
        public int? TenantId { get; set; }
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsRun { get; set; }
    }
}