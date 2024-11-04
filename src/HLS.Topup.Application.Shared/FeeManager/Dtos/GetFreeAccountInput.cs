namespace HLS.Topup.FeeManager.Dtos
{
    public class GetFreeAccountInput
    {
        public string AccountCode { get; set; }
        public string ProductCode { get; set; }
        public string ReceiverInfo { get; set; }
        public decimal Amount { get; set; }
    }
}
