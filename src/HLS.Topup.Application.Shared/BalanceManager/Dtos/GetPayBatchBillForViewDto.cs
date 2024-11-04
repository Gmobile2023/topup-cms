namespace HLS.Topup.BalanceManager.Dtos
{
    public class GetPayBatchBillForViewDto
    {
		public PayBatchBillDto PayBatchBill { get; set; }
		public string ProductName { get; set;}
        public string CategoryName { get; set; }
        public string UserCreated { get; set; }
        public string UserApproval { get; set; }
    }
}