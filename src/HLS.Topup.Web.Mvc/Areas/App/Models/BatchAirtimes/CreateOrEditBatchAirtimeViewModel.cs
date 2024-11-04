using System.Collections.Generic;
using HLS.Topup.StockManagement.Dtos;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.BatchAirtimes
{
    public class CreateOrEditBatchAirtimeModalViewModel
    {
	    public BatchAirtimeDto BatchAirtime { get; set; }
	    public List<CommonLookupTableDto> ProviderList { get; set; }
	    public bool IsEditMode => !string.IsNullOrEmpty(BatchAirtime.BatchCode); 
    }
}