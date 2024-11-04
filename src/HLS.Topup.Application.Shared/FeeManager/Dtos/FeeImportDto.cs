namespace HLS.Topup.FeeManager.Dtos
{
    public class FeeImportDto
    {
        public string ProductCode { get; set; }
        
        public decimal? MinFee { get; set; }
        
        public decimal? AmountMinFee { get; set; }
        
        public decimal? AmountIncrease { get; set; }
        
        public decimal? SubFee { get; set; }
    }

    public class FeeImport
    {
        public string CategoryName { get; set; }
        
        public long? ProductId { get; set; }
        
        public string ProductCode { get; set; }
        
        public string ProductName { get; set; }
        
        public decimal? MinFee { get; set; }
        
        public decimal? AmountMinFee { get; set; }
        
        public decimal? AmountIncrease { get; set; }
        
        public decimal? SubFee { get; set; }
    }
}