using System.Collections.Generic;
using HLS.Topup.LimitationManager.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.LimitProducts
{
    public class LimitProductsViewModel
    {
		public string FilterText { get; set; }
        
        public List<LimitProductServiceLookupTableDto> LimitProductServiceList { get; set; }
    }
}