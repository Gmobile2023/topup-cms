using HLS.Topup.StockManagement;

using System;
using System.Runtime.Serialization;
using Abp.Application.Services.Dto;

namespace HLS.Topup.StockManagement.Dtos
{

    public class GetAllStocksAirtimesInput : PagedAndSortedResultRequestDto
    {
	    public string Filter { get; set; }
	    public byte StatusFilter { get; set; }
	    public string ProviderCodeFilter { get; set; }

    }

    public class GetAllStocksAirtimesForExcelInput
    {
	    public string Filter { get; set; }
	    public byte StatusFilter { get; set; }
	    public string ProviderCodeFilter { get; set; }

    }


}
