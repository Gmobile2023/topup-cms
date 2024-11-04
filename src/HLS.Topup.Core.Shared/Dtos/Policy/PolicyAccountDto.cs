using HLS.Topup.Dtos.Discounts;

namespace HLS.Topup.Dtos.Policy
{
    public class PolicyAccountDto : ProductDiscountDto
    {
        public int FeeId { get; set; }
        public int FeeDetailId { get; set; }
        public int? ProductId { get; set; }
        public decimal? AmountMinFee { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? AmountIncrease { get; set; }
        public decimal? SubFee { get; set; }
        public decimal FeeValue { get; set; }
        public string ShowTextFee { get; set; }
    }
}
