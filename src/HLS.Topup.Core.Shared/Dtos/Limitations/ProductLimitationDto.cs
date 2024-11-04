namespace HLS.Topup.Dtos.Limitations
{
    public class ProductLimitationDto
    {
        public int LimitProductId { get; set; }
        public int LimitProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public long? UserId { get; set; }
        public virtual decimal? LimitAmount { get; set; } //Hạn mức thanh toán
        public virtual int? LimitQuantity { get; set; } //Hạn mức số lượng
    }
    public class AccountProductLimitDto
    {
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
