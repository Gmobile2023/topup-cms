namespace HLS.Topup.PayBacks.Dtos
{
    public class GetPayBackForViewDto
    {
        public PayBackDto PayBack { get; set; }

        public string UserName { get; set;}
        public string UserApproved { get; set;}
        
        public int? TotalAgent { get; set;}
        
        public decimal? TotalAmount { get; set;}
    }
}