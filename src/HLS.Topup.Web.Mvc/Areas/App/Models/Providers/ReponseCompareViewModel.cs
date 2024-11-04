using Abp.Application.Services.Dto;
using HLS.Topup.Compare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLS.Topup.Web.Areas.App.Models.Providers
{
    public class CompareViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }
    }

    public class RefundViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }
    }
    public class ReponseCompareViewModel
    {
        public string Provider { get; set; }

        public DateTime TransDate { get; set; }

        public DateTime CompareDate { get; set; }

        public string TransDateSearch { get; set; }

        public string KeyCode { get; set; }

        public List<CompareReponseDto> Items { get; set; }

        public List<ComboboxItemDto> Providers { get; set; }
    }

    public class RefundCompareViewModel
    {
        public List<ComboboxItemDto> Providers { get; set; }

        public string Provider { get; set; }

        public DateTime TransDate { get; set; }

        public string TransDateSearch { get; set; }

        public string KeyCode { get; set; }

        public CompareRefunDto CompareRefunDto { get; set; }

    }
}
