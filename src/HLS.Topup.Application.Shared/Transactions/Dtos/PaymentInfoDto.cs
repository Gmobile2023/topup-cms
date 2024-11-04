using HLS.Topup.Dtos.Transactions;

namespace HLS.Topup.Transactions.Dtos
{
    public class TransferInfo
    {
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public decimal Balance { get; set; }
    }
    public class BalanceHistoryDto:BalanceHistoryResponseDto
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public decimal Increment { get; set; }
        public decimal Decrement { get; set; }
        public decimal BalanceAfterTrans { get; set; }
        
        public decimal DiscountAmount { get; set; }
        public int Quantity { get; set; }
        public decimal Fee { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}