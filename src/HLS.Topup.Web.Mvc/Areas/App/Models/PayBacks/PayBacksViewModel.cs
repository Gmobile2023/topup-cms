using System.Collections.Generic;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.PayBacks
{
    public class PayBacksViewModel
    {
        public List<PayBacksCategoryLookupTableDto> ProductCategory { get; set; }
        
        public List<PayBacksProviderLookupTableDto> Provider { get; set; }
    }
}