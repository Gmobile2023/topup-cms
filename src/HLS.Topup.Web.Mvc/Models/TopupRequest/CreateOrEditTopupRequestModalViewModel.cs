using System.Collections.Generic;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Topup.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HLS.Topup.Web.Models.TopupRequest
{
    public class CreateOrEditTopupRequestModalViewModel
    {
        public CreateOrEditTopupRequestDto TopupRequest { get; set; }

        public List<decimal> CardValues { get; set; }
        public List<SelectListItem> Services { get; set; }
        public List<CategoryDto> Categorys { get; set; }
        public List<CategoryDto> PinCodeCategory { get; set; }
        public List<CategoryDto> PinDataCategory { get; set; }
        public List<CategoryDto> PinGameCategory { get; set; }
        public bool IsEditMode => TopupRequest.Id.HasValue;

    }

    public class TopupListModalViewModel
    {
        public string BatchCode { get; set; }

        public string BatchType { get; set; }

        public string BatchName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }
    }

    public class HistoryModalDetailViewModel
    {
        public string BatchCode { get; set; }
        public string BatchType { get; set; }
        public string BatchName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int QuantitySuccess { get; set; }
        public decimal PriceSuccess { get; set; }
    }
}