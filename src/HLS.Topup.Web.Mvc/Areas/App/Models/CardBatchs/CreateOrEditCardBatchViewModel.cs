using HLS.Topup.StockManagement.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;
using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.CardBatchs
{
    public class CreateOrEditCardBatchModalViewModel
    {
        public CreateOrEditCardBatchDto CardBatch { get; set; }

        public string ProviderName { get; set; }

        public string CategoryName { get; set; }


        public List<CardBatchProviderLookupTableDto> ProviderList { get; set; }

        // public List<CardBatchVendorLookupTableDto> VendorList { get; set; }


        public bool IsEditMode => CardBatch.Id.HasValue;
    }
}