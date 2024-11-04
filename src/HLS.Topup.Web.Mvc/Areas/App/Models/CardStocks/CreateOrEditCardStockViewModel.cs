using HLS.Topup.StockManagement.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HLS.Topup.Web.Areas.App.Models.CardStocks
{
    public class CreateOrEditCardStockModalViewModel
    {
        public CreateOrEditCardStockDto CardStock { get; set; }

        public List<decimal> CardValues { get; set; } 
        
        public List<CardVendorLookupTableDto> CategoryList { get; set; }
        public List<CommonLookupTableDto> ServicesCard { get; set; }
        public List<CommonLookupTableDto> StockCodes { get; set; }
        public bool IsEditMode  { get; set; }
    }
    
    public class TransferCardStockModalViewModel
    { 
        public List<CommonLookupTableDto> StockCodes { get; set; }
        public string SrcStockCode { get; set; }
        public string DesStockCode { get; set; }
        
        public List<CommonLookupTableDto> ServicesCard { get; set; }
        public string ServiceCode { get; set; }
        public List<CardVendorLookupTableDto> VendorList { get; set; }
        public string VendorCode { get; set; }
        public List<CardBatchLookupTableDto> BatchList { get; set; }
        public string BatchCode { get; set; }
        public List<CommonLookupTableDto> CardValues { get; set; }
        public int? CardValue { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        
        
        public bool IsEditMode  { get; set; }
    }
}