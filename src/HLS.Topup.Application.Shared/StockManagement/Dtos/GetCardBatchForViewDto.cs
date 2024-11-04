namespace HLS.Topup.StockManagement.Dtos
{
    public class GetCardBatchForViewDto
    {
		public CardBatchDto CardBatch { get; set; }
        public string ProviderName { get; set; }

        public string VendorName { get; set; }

    }
}