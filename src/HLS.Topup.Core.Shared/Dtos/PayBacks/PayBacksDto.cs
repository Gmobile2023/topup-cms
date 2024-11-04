namespace HLS.Topup.Dtos.PayBacks
{
    public class PaybatchAccount
    {
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public bool Success { get; set; }
        public string TransRef { get; set; }
        public string TransNote { get; set; }
    }
}