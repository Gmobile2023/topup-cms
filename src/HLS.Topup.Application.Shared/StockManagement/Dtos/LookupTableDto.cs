using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{
    //   public class CardCategoryLookupTableDto
    //   {
    // public string Id { get; set; }
    //
    // public string DisplayName { get; set; }
    //   }
    public class CardProviderLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class CardVendorLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string Service { get; set; }
    }

    public class CardBatchLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string VendorCode { get; set; }
    }

    public class CommonLookupTableDto
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public object Payload { get; set; }
    }
}