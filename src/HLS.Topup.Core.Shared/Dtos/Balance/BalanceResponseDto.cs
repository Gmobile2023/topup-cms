namespace HLS.Topup.Dtos.Balance
{
    public class BalanceResponseDto
    {
        public decimal SrcBalance { get; set; }
        public decimal DesBalance { get; set; }
        public string TransactionCode { get; set; }
    }
}
