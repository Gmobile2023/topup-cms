namespace HLS.Topup.PayBacks.Dtos
{
    public class PayBacksDetailDto
    {
        public int PayBackId { get; set; }
        
        public long? UserId { get; set; }
        
        public decimal Amount { get; set; }
        
        public string AgentCode { get; set; }
        
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string TransCode { get; set; }
    }
}