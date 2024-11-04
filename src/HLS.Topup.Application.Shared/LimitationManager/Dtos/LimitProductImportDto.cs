namespace HLS.Topup.LimitationManager.Dtos
{
    public class LimitProductImportDto
    {
        public string ProductCode { get; set; }
        
        public int? LimitQuantity { get; set; }
        
        public decimal? LimitAmount { get; set; }
    }

    public class LimitProductImport
    {
        public string ServiceName { get; set; }
        
        public string ServiceCode { get; set; }
        
        public string CategoryName { get; set; }
        
        public string CategoryCode { get; set; }
        
        public long ProductId { get; set; }
        
        public string ProductType { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductCode { get; set; }

        public decimal ProductValue { get; set; }
        
        public int? LimitQuantity { get; set; }
        
        public decimal? LimitAmount { get; set; }
    }
}