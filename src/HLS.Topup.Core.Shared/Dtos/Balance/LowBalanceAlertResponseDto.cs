using System;

namespace HLS.Topup.Dtos.Balance
{
    public class LowBalanceAlertResponseDto
    {
        public int? TenantId { get; set; }
        public string Id { get; set; }
        public string Channel { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal MinBalance { get; set; }
        public long TeleChatId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsRun { get; set; }
    }
}