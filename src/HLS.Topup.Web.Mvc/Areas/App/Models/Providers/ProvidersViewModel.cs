using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.Providers
{
    public class ProvidersViewModel
    {
		public string FilterText { get; set; }
        public List<CommonLookupTableDto> ProviderList { get; set; }
    }
}