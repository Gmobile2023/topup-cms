using HLS.Topup.Common;

namespace HLS.Topup.Dtos.BillFees
{
    public class BillFeeDetailDto
    {
        public int? FeeId { get; set; }
        
        public int? CategoryId { get; set; }
        
        public int? ProductId { get; set; }
        
        public string ProductCode { get; set; }
        
        public string ProductName { get; set; }
        
        public long? UserId { get; set; }
        
        public int Order { get; set; }
        
        public string CategoryCode { get; set; }
        
        public string CategoryName { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        public decimal? AmountMinFee { get; set; }
        
        public decimal? MinFee { get; set; }
        
        public decimal? AmountIncrease { get; set; }
        
        public decimal? SubFee { get; set; }
        
        public CommonConst.CategoryStatus Status { get; set; }
    }
    
    public class ProductFeeDetailDto
    {
        public int? FeeId { get; set; }
        
        public int? CategoryId { get; set; }
        
        public int? ProductId { get; set; }
        
        public string ProductCode { get; set; }
        
        public string ProductName { get; set; }
        
        public long? UserId { get; set; }
        
        public string CategoryCode { get; set; }
        
        public string CategoryName { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        public decimal? AmountMinFee { get; set; }
        
        public decimal? MinFee { get; set; }
        
        public decimal? AmountIncrease { get; set; }
        
        public decimal? SubFee { get; set; }
        
        public CommonConst.CategoryStatus Status { get; set; }
    }
}