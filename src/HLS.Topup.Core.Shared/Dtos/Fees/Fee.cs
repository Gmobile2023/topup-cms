using System;

namespace HLS.Topup.Dtos.Fees
{
    public class ProductFeeDto
    {
                public const string CacheKey = "PayGate_ProductFeeInfo";
        public int FeeId { get; set; }
        public int FeeDetailId { get; set; }

        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public long? UserId { get; set; }
        public decimal? AmountMinFee { get; set; }

        public decimal? MinFee { get; set; }

        public decimal? AmountIncrease { get; set; }

        public decimal? SubFee { get; set; }
        public decimal Amount { get; set; }
        public decimal FeeValue { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ToDate { get; set; }
    }
}
