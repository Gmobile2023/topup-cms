using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace HLS.Topup.Web.Areas.App.Models.PayBatchBills
{
    public class PayBatchBillsViewModel
    {
		public string FilterText { get; set; }

        public List<ComboboxItemDto> Categorys { get; set; }
    }
}