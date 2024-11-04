namespace HLS.Topup.DiscountManager.Dtos
{
    public class GetProductDiscountDto
    {
        public string AccountCode { get; set; }
        public string ProductCode { get; set; }
        public string TransCode { get; set; }
    }
    public class GetProductDiscountUserDto
    {
        public string ProductCode { get; set; }
    }
}
