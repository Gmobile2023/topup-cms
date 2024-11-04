using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.CardStocks
{
    public class CardStocksViewModel
    {
		public string FilterText { get; set; }
        public List<decimal> CardValues { get; set; }
        
        public List<CardVendorLookupTableDto> VendorList { get; set; }
        public List<CommonLookupTableDto> StockCodes { get; set; }
        public List<CommonLookupTableDto> ServicesCard { get; set; }
        public List<ComboboxItemDto> Providers { get; set; }          

    }
}