using HLS.Topup.StockManagement.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;


namespace HLS.Topup.Web.Areas.App.Models.Cards
{
    public class CreateOrEditCardModalViewModel
    {
        public CreateOrEditCardDto Card { get; set; }

        public string VendorVendorName { get; set; }
        public List<CardVendorLookupTableDto> VendorList { get; set; }
        public List<CardProviderLookupTableDto> ProviderList { get; set; }
        public List<CardBatchLookupTableDto> BatchList { get; set; }
        public SelectList CardValues { get; set; }

        public bool IsEditMode => Card.Id.HasValue;
    }
    public class ImportCardsFileModalModel
    { 
        public string Description { get; set; } 
        public List<CardProviderLookupTableDto> ProviderList { get; set; }
    }
    
    public class CreateCardsApiModalModel
    { 
        public string ProviderCode { get; set; } 
        public string Description { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public List<CardProviderLookupTableDto> ProviderList { get; set; }
        
    }
}