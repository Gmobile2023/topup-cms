using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.CardBatchs
{
    public class CardBatchViewModel : GetCardBatchForViewDto
    {
        
        public string ProviderName { get; set; }
        public string CategoryName { get; set; }

        public List<CardBatchProviderLookupTableDto> ProviderList { get; set; }

        // public List<CardBatchVendorLookupTableDto> VendorList { get; set; }
    }
}