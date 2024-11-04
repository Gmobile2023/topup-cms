using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace HLS.Topup.Web.Areas.App.Models.SaleClearDebts
{
    public class SaleClearDebtsViewModel
    {
		public string FilterText { get; set; }

        public List<ComboboxItemDto> Banks { get; set; }
    }
}