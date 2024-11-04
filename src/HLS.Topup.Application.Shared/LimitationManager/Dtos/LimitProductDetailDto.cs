namespace HLS.Topup.LimitationManager.Dtos
{
    public class LimitProductDetailDto
    {
        public int? LimitQuantity { get; set; }
        
        public decimal? LimitAmount { get; set; } 
        
        public int? ProductId { get; set; } 
        
        public string ProductName { get; set; }
        
        public string ProductType { get; set; }
        
        public string ServiceName { get; set; }
    }
}