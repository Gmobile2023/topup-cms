using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;
using Abp.Extensions;
using HLS.Topup.Dtos.Provider;

namespace HLS.Topup.Web.Areas.App.Models.StocksAirtimes
{
    public class CreateOrEditStocksAirtimeModalViewModel
    {
       public StocksAirtimeDto StocksAirtime { get; set; }

       public List<CommonLookupTableDto> ProviderList { get; set; }
	   public bool IsEditMode => !string.IsNullOrEmpty(StocksAirtime.KeyCode);
    }
}
