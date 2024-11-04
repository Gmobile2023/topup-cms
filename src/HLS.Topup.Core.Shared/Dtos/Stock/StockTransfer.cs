namespace HLS.Topup.Dtos.Stock
{
    
    
    public class StockTransferItemInfoRespond
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal CardValue { get; set; }
        public int QuantityAvailable { get; set; }
        public int Quantity { get; set; }
    }
}