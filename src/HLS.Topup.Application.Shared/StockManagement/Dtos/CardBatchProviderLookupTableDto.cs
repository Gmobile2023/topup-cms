using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    public class CardBatchProviderLookupTableDto
    {
		public string Id { get; set; }

		public string DisplayName { get; set; }
    }
    public class CardBatchVendorLookupTableDto
    {
		public string Id { get; set; }

		public string DisplayName { get; set; }
    }
}