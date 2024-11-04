namespace HLS.Topup.BalanceManager.Dtos
{
    public class CreateOrEditLowBalanceAlertDto
    {
        public string Id { get; set; }
        public string Channel { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal MinBalance { get; set; }
        public long TeleChatId { get; set; }
        public bool? IsRun { get; set; }
    }
}