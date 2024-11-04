namespace HLS.Topup.DiscountManager.Dtos
{
    public class DiscountImportDto
    {
        public string ProductCode { get; set; }
        
        public decimal? DiscountValue { get; set; }
        
        public decimal? FixAmount { get; set; }
    }

    public class DiscountImport
    {
        public string ServiceName { get; set; }
        
        public string ServiceCode { get; set; }
        
        public string CategoryName { get; set; }
        
        public string CategoryCode { get; set; }
        
        public long ProductId { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductCode { get; set; }

        public decimal ProductValue { get; set; }
        
        public decimal? FixAmount { get; set; }
        
        public decimal? DiscountValue { get; set; }
    }
}