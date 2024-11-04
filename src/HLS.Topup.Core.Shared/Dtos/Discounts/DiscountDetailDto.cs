using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Discounts
{
    public class DiscountDetailDto
    {
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }
        public decimal? ProductValue { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? DiscountId { get; set; }
        public int DiscountDetailId { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? FixAmount { get; set; }

        public int Order { get; set; }
        public long? UserId { get; set; }

        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Description { get; set; }
        public CommonConst.CategoryStatus Status { get; set; }
        public string ServiceName { get; set; }
    }

    public class ProductDiscountDto
    {
        public const string CacheKey = "PayGate_ProductDiscount";
        public decimal ProductValue { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? DiscountValue { get; set; } //phần trăm ck 6%
        public decimal DiscountAmount { get; set; } //Số tiền chiết khấu
        public decimal PaymentAmount { get; set; } //Giá bán
        public decimal? FixAmount { get; set; } //Số tiền tối đa: 10k
        public int Order { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int DiscountId { get; set; }
        public int DiscountDetailId { get; set; }

        public bool IsDiscount { get; set; } //=true>show  DiscountValue , show FixAmount
        //Nếu là thanh toán hóa đơn: DiscountValue% tối đa FixAmount
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public bool IsSaveCache { get; set; }
        public string NextDiscount { get; set; }
        public string ExpireTime { get; set; }
        public string DiscountCode { get; set; }
        public string CreatedDate { get; set; }
        public string ApprovedDate { get; set; }
    }

    public class ProductDiscountCache : ProductDiscountDto
    {
        public long? UserId { get; set; }
        public string AccountCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreationTime { get; set; }
        public int ProductId { get; set; }
        public CommonConst.DiscountStatus Status { get; set; }
        public CommonConst.ProductStatus ProductStatus { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
    }
}