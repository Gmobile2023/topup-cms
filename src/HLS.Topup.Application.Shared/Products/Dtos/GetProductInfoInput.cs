namespace HLS.Topup.Products.Dtos
{
    public class GetProductInfoInput
    {
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public decimal? Amount { get; set; }
    }
}