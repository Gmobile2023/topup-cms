using HLS.Topup.Common;
using HLS.Topup.Dtos.Balance;
using ServiceStack;

namespace HLS.Topup.BalanceManager
{
    [Route("/api/v1/common/alarm/balance/get-all", "GET")]
    public class BalanceAlertGetAllRequest : PaggingBaseDto
    {
        public int? TenantId { get; set; }
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
    }
    
    [Route("/api/v1/common/alarm/balance", "GET")]
    public class BalanceAlertGetRequest : IReturn<LowBalanceAlertResponseDto>
    {
        public int? TenantId { get; set; }
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
    }
    
    [Route("/api/v1/common/alarm/balance/add", "POST")]
    public class BalanceAlertAddRequest
    {
        public int? TenantId { get; set; }
        public string Channel { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal MinBalance { get; set; }
        public long TeleChatId { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsRun { get; set; }
    }
    
    [Route("/api/v1/common/alarm/balance/update", "PUT")]
    public class BalanceAlertUpdateRequest
    {
        public int? TenantId { get; set; }
        public string Channel { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal MinBalance { get; set; }
        public long TeleChatId { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsRun { get; set; }
    }
}