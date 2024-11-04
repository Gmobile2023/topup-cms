using HLS.Topup.StockManagement.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HLS.Topup.Web.Areas.App.Models.Cards
{
    public class CardsViewModel
    {
		public string FilterText { get; set; }
        public List<CardVendorLookupTableDto> VendorList { get; set;}       
        // public List<CommonLookupTableDto> ServiceCardList { get; set;}        
        public List<CardProviderLookupTableDto> ProviderList { get; set; }
        // public List<CardBatchLookupTableDto> BatchList { get; set; }
        public List<CommonLookupTableDto> StockCodes { get; set; } 
        public List<CommonLookupTableDto> ServicesCard { get; set; }
        public List<decimal> CardValues { get; set; }
    }
}