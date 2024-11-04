using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.StocksAirtimes
{
    public class StocksAirtimesViewModel
    {
		public string FilterText { get; set; } 
        public List<CommonLookupTableDto> ProviderList { get; set; }
    }
}